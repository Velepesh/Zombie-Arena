using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace FSG.MeshAnimator
{
    public static class MeshAnimatorURPSetup
    {
        private static ListRequest _packageListRequest;

        [InitializeOnLoadMethod]
        private static void SetupUrpShaders()
        {
            _packageListRequest = Client.List();
            EditorApplication.update += CheckForUrp;
        }

        private static void CheckForUrp()
        {
            if (!_packageListRequest.IsCompleted) return;
            EditorApplication.update -= CheckForUrp;
            if (_packageListRequest.Status != StatusCode.Success) return;
            string versionFile = null;
            try
            {
                var package = _packageListRequest.Result.FirstOrDefault(p => p.name == "com.unity.render-pipelines.universal");
                // find mesh animator hlsl path
                var meshAnimatorPath = AssetDatabase.FindAssets("t:TextAsset MeshAnimator")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(x => x.Contains("/Shaders/"))
                    .FirstOrDefault(x => x.EndsWith(".hlsl"));
                if (string.IsNullOrWhiteSpace(meshAnimatorPath)) return;
                var destDir = Path.Combine(Path.GetDirectoryName(meshAnimatorPath), "URP");
                versionFile = Path.Combine(destDir, ".version");
                // if URP is not installed, delete the URP folder
                if (package == null)
                {
                    if (Directory.Exists(destDir))
                    {
                        Directory.Delete(destDir, true);
                        AssetDatabase.Refresh();
                    }

                    return;
                }
                var packageVersionString = string.Join("-", package.version, Application.unityVersion, MeshAnimationCreator.Version);
                // copy URP shaders to mesh animator folder
                var shaderPath = AssetDatabase.GetAssetPath(Shader.Find("Universal Render Pipeline/Lit"));
                var dir = Path.GetDirectoryName(shaderPath);
                var files = Directory.GetFiles(dir, "*.shader", SearchOption.TopDirectoryOnly)
                    .Concat(Directory.GetFiles(dir, "*.hlsl", SearchOption.TopDirectoryOnly))
                    .ToList();
                if (files.Count == 0 || File.Exists(versionFile) && File.ReadAllText(versionFile) == packageVersionString)
                    return;
                if (Directory.Exists(destDir)) Directory.Delete(destDir, true);
                Directory.CreateDirectory(destDir);
                var fileNames = files.Select(Path.GetFileName).ToList();
                foreach (var file in files)
                {
                    var dest = Path.Combine(destDir, Path.GetFileName(file));
                    if (File.Exists(dest)) File.Delete(dest);
                    File.Copy(file, dest);
                    // process shader files
                    ProcessMeshAnimatorShaderFile(dest, fileNames);
                }
                AssetDatabase.Refresh();
                File.WriteAllText(versionFile, packageVersionString);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                if (!string.IsNullOrWhiteSpace(versionFile) && File.Exists(versionFile))
                    File.Delete(versionFile);
            }
        }

        private static void ProcessMeshAnimatorShaderFile(string file, List<string> allFiles)
        {
            var fileLines = File.ReadAllLines(file).ToList();
            if (file.EndsWith(".shader"))
            {
                if (fileLines.Any(x => x.Contains("Shader \"Hidden")))
                {
                    File.Delete(file);
                    return;
                }
                if (fileLines.Any(x => x.Contains("/SpatialMapping/")))
                {
                    File.Delete(file);
                    return;
                }

                var shaderLine = fileLines.FirstOrDefault(x => x.Contains("Shader \"Universal Render Pipeline/"));
                if (shaderLine != null)
                    fileLines[fileLines.IndexOf(shaderLine)] = shaderLine.Replace("Universal Render Pipeline/", "Mesh Animator/Universal Render Pipeline/");
            }
            string vertexPositionName = null;
            var hasNormal = false;
            string normalInsideIf = null;
            string normalInputName = null;
            for (int i = 0; i < fileLines.Count; i++)
            {
                var line = fileLines[i];
                if (line.IndexOf("struct Attributes", StringComparison.OrdinalIgnoreCase) < 0) continue;
                var indentation = "  ";
                var index = i;
                while (!fileLines[index + 1].Contains("}"))
                {
                    index++;
                    if (fileLines[index].Contains("POSITION;"))
                    {
                        vertexPositionName = fileLines[index].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                        indentation = new string(fileLines[index].TakeWhile(char.IsWhiteSpace).ToArray());
                    }

                    if (!hasNormal && fileLines[index].Contains(": NORMAL"))
                    {
                        hasNormal = true;
                        normalInputName = fileLines[index].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                        normalInsideIf = fileLines[index - 1].Contains("#if defined") ? Regex.Match(fileLines[index - 1], @"(?<=\().+?(?=\))").Value : null;
                    }
                }
                fileLines.Insert(index + 1, indentation + Attributes.Replace("\n", $"\n{indentation}"));
            }

            var newLinesInProp = Props.Split('\n').Length;
            for (int i = 0; i < fileLines.Count; i++)
            {
                var line = fileLines[i];
                if (!line.StartsWith("CBUFFER_START")) continue;
                fileLines.Insert(i - 1, Props);
                i += newLinesInProp + 1;
            }

            var includeNewlines = Include.Split('\n').Length;
            for (int i = 0; i < fileLines.Count; i++)
            {
                var line = fileLines[i];
                if (!line.EndsWith("(Attributes input)")) continue;
                // setup vertex pass
                var index = i + 2;
                var foundIndex = index;
                while (index + 1 < fileLines.Count &&
                       fileLines[index + 1].IndexOf("UNITY_SETUP_INSTANCE_ID", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    index++;
                    if (fileLines[index].IndexOf("UNITY_SETUP_INSTANCE_ID", StringComparison.OrdinalIgnoreCase) <= 0) continue;
                    foundIndex = index;
                    break;
                }
                while (index + 1 < fileLines.Count && fileLines[index + 1].Contains("UNITY_"))
                {
                    index++;
                    foundIndex = index;
                }
                var indentation = new string(fileLines[foundIndex].TakeWhile(char.IsWhiteSpace).ToArray());
                fileLines.Insert(foundIndex + 1, VertexPassCode(vertexPositionName, normalInputName, normalInsideIf, indentation));
                // setup #include
                index = i;
                while (index - 1 > 0 && !fileLines[index - 1].Contains("#include") && !fileLines[index - 1].Contains("#pragma"))
                    index--;
                while (fileLines[index].TrimStart().StartsWith("#"))
                    index++;
                if (index - 1 < 0 || fileLines[index].IndexOf("MeshAnimator", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    indentation = new string(fileLines[index].TakeWhile(char.IsWhiteSpace).ToArray());
                    fileLines.Insert(index, indentation + Include.Replace("\n", $"\n{indentation}"));
                    foundIndex += includeNewlines;
                }
                i = foundIndex + 1;
            }

            // replace #includes
            for (int i = 0; i < fileLines.Count; i++)
            {
                var line = fileLines[i];
                if (!line.Contains("#include")) continue;
                foreach (var otherFile in allFiles)
                {
                    if (file.EndsWith(otherFile)) continue;
                    var includeFileName = line.Split('/').Last().TrimEnd('"');
                    if (includeFileName != otherFile) continue;
                    var newInclude = line.Substring(0, line.IndexOf("#"));
                    newInclude += $"#include \"{otherFile}\"";
                    fileLines[i] = newInclude;
                }
            }

            var fullText = fileLines.Aggregate((x, y) => $"{x}\n{y}");
            fullText = fullText.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
            File.WriteAllText(file, fullText);
        }

        private const string BeginComment = "// BEGIN GENERATED MESH ANIMATOR CODE";
        private const string EndComment = "// END GENERATED MESH ANIMATOR CODE";
        private const string Attributes = BeginComment + "\nuint vertexId        : SV_VertexID;\n" + EndComment;
        private const string Include = BeginComment + "\n#include \"../MeshAnimator.hlsl\"\n" + EndComment;
        private const string Props = BeginComment + @"
TEXTURE2D_ARRAY(_AnimTextures);
SAMPLER(sampler_AnimTextures);
UNITY_INSTANCING_BUFFER_START(Props)
	UNITY_DEFINE_INSTANCED_PROP(float4, _AnimTimeInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _AnimInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _AnimScalar)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeAnimInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeAnimTimeInfo)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeAnimScalar)
	UNITY_DEFINE_INSTANCED_PROP(float4, _CrossfadeData)
UNITY_INSTANCING_BUFFER_END(Props)
" + EndComment;

        private static string VertexPassCode(string positionName,
            string normalName,
            string normalIfDefined,
            string indentation)
        {
            var normalCode = normalName != null ? $"input.{normalName}.xyz," : "float3(0, 0, 0),";
            if (normalIfDefined != null)
                normalCode = $"#if defined({normalIfDefined})\n    {normalCode}\n    #else\n    float3(0, 0, 0),\n    #endif";
            var setNormalCode = normalName != null ? $"input.{normalName}.xyz = animatedNormal;" : "";
            if (normalIfDefined != null)
                setNormalCode = $"#if defined({normalIfDefined})\n{setNormalCode}\n#endif";
            
            /*
             * 
void ApplyMeshAnimationValues_float(
	float3 vertexPosition,
	float3 vertexNormal,
	float4 animTimeInfo, 
	Texture2DArray animTextures,
	float4 animInfo, 
	float4 animScalar, 
	float4 crossfadeAnimInfo, 
	float4 crossfadeAnimTimeInfo, 
	float4 crossfadeAnimScalar, 
	float1 crossfadeData,
	float1 vertexIndex,
	SamplerState samplerState,
	out float3 outputVertexPosition,
	out float3 outputVertexNormal)
             */
            
            var code = "\n" + BeginComment + $@"
float3 animatedPosition;
float3 animatedNormal;	
ApplyMeshAnimationValues_float(
    input.{positionName}.xyz,
    {normalCode}
    UNITY_ACCESS_INSTANCED_PROP(Props, _AnimTimeInfo), 
    _AnimTextures,
    UNITY_ACCESS_INSTANCED_PROP(Props, _AnimInfo),
    UNITY_ACCESS_INSTANCED_PROP(Props, _AnimScalar), 
    UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimInfo), 
    UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimTimeInfo), 
    UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeAnimScalar), 
    UNITY_ACCESS_INSTANCED_PROP(Props, _CrossfadeData),  
    input.vertexId,
    sampler_AnimTextures,
    animatedPosition,
    animatedNormal);

input.{positionName}.xyz = animatedPosition;
{setNormalCode}
" + EndComment;
            code = code.Replace("\n", $"\n{indentation}");
            return code.TrimEnd();
        }
    }
}
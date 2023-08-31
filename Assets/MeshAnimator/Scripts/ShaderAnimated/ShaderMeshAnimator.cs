//----------------------------------------------
// Mesh Animator
// Flick Shot Games
// http://www.flickshotgames.com
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace FSG.MeshAnimator.ShaderAnimated
{
    [AddComponentMenu("Mesh Animator/GPU Shader Animated")]
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteInEditMode]
    public class ShaderMeshAnimator : MeshAnimatorBase
    {
        private static readonly int _animTimeProp = Shader.PropertyToID("_AnimTimeInfo");
        private static readonly int _animInfoProp = Shader.PropertyToID("_AnimInfo");
        private static readonly int _animScalarProp = Shader.PropertyToID("_AnimScalar");
        private static readonly int _animTexturesProp = Shader.PropertyToID("_AnimTextures");
        private static readonly int _crossfadeAnimInfoProp = Shader.PropertyToID("_CrossfadeAnimInfo");
        private static readonly int _crossfadeAnimScalarProp = Shader.PropertyToID("_CrossfadeAnimScalar");
        private static readonly int _crossfadeAnimTimeInfo = Shader.PropertyToID("_CrossfadeAnimTimeInfo");
        private static readonly int _crossfadeData = Shader.PropertyToID("_CrossfadeData");
        private static Vector4 _shaderTime { get { return Shader.GetGlobalVector("_Time"); } }

        private static Dictionary<Mesh, int> _meshCount = new Dictionary<Mesh, int>();
        private static List<Material> _materialCacheLookup = new List<Material>();
        private static HashSet<Material> _setMaterials = new HashSet<Material>();
        private static Dictionary<Mesh, Texture2DArray> _animTextures = new Dictionary<Mesh, Texture2DArray>();

        private int pixelsPerTexture = 2;
        private int textureStartIndex = 0;
        private int textureSizeX;
        private int textureSizeY;
        private MaterialPropertyBlock materialPropertyBlock;
        private Vector4 animInfo;
        private Vector4 animTimeInfo;

        public ShaderMeshAnimation defaultMeshAnimation;
        public ShaderMeshAnimation[] meshAnimations;

        public override IMeshAnimation defaultAnimation
        {
            get
            {
                return defaultMeshAnimation;
            }
            set
            {
                defaultMeshAnimation = value as ShaderMeshAnimation;
            }
        }
        public override IMeshAnimation[] animations { get { return meshAnimations; } }

        protected override void OnEnable()
        {
#if UNITY_EDITOR
            if (Baking) return;
#endif
            base.OnEnable();
            AddMeshCount(_meshCount);
            if (resetOnEnable)
            {
                GetAnimationStartData(currentAnimation, out currentFrame, out currentAnimTime);
                SetupTextureData();
                RefreshTimeData();
                RestartAnim();
            }
        }
        protected override void Start()
        {
#if UNITY_EDITOR
            if (Baking) return;
#endif
            base.Start();
            SetupTextureData();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void OnDestroy()
        {
#if UNITY_EDITOR
            if (Baking) return;
#endif
            base.OnDestroy();
            RemoveMeshCount(_meshCount);
            if (!_meshCount.ContainsKey(baseMesh) && _animTextures.ContainsKey(baseMesh))
            {
                DestroyImmediate(_animTextures[baseMesh]);
                _animTextures.Remove(baseMesh);
                _setMaterials.Clear();
            }
        }
        private void OnApplicationFocus(bool focus)
        {
            if (focus && hasExposedTransforms && currentAnimIndex >= 0)
            {
                // Ensure the currentFrame matches exactly with the shader
                // to sync exposed transforms
                RefreshTimeData();
            }
        }
        protected override bool DisplayFrame(int previousFrame)
        {
            if (currentFrame == previousFrame || currentAnimIndex < 0)
                return base.DisplayFrame(previousFrame);
            float currentStartTime = animTimeInfo.z;
            float currentEndTime = animTimeInfo.w;
            float directionalSpeed = speed * currentAnimation.playbackSpeed;
            float newEndTime = currentStartTime + currentAnimation.length / directionalSpeed;
            if (!currentAnimation.IsLoopingWrapMode && 
                directionalSpeed > 0 && currentFrame >= currentAnimation.TotalFrames ||
                directionalSpeed < 0 && currentFrame <= 0)
            {
                isPaused = true;
                if (OnAnimationFinished != null) OnAnimationFinished(currentAnimation.animationName);
                OnAnimationCompleted(true);
            }
            else if (Mathf.Abs(currentEndTime - newEndTime) > 0.001f)
            {
                float normalizedTime = CalculateShaderNormalizedTime();
                float newLength = newEndTime - currentStartTime;
                float shaderTime = _shaderTime.y;
                animTimeInfo.z = shaderTime - newLength * normalizedTime;
                animTimeInfo.w = shaderTime + newLength * (1f - normalizedTime);
                meshRenderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetVector(_animTimeProp, animTimeInfo);
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
            return base.DisplayFrame(previousFrame);
        }
        protected override void OnCurrentAnimationChanged(IMeshAnimation meshAnimation)
        {
            ShaderMeshAnimation currentAnimation = meshAnimation as ShaderMeshAnimation;
            CreatePropertyBlock();
            meshRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetVector(_animScalarProp, currentAnimation.animScalar);
            animInfo.y = currentAnimation.vertexCount;
            pixelsPerTexture = currentAnimation.textureSize.x * currentAnimation.textureSize.y;
            textureStartIndex = 1; // offset by one since default in shader is 0
            for (int i = 0; i < currentAnimIndex; i++)
            {
                textureStartIndex += meshAnimations[i].textureCount;
            }
            textureSizeX = currentAnimation.textureSize.x;
            textureSizeY = currentAnimation.textureSize.y;
            // set animation info
            animInfo.x = textureStartIndex;
            animInfo.z = textureSizeX;
            animInfo.w = (float)(currentAnimation.wrapMode > WrapMode.PingPong ? 0 : currentAnimation.wrapMode);
            materialPropertyBlock.SetVector(_animInfoProp, animInfo);
            // set time info
            float startTime = _shaderTime.y;
            animTimeInfo = new Vector4(
                0,
                currentAnimation.TotalFrames,
                startTime,
                startTime + (currentAnimation.length / (speed * currentAnimation.playbackSpeed)));
            materialPropertyBlock.SetVector(_animTimeProp, animTimeInfo);
            // set property block
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        public override void Play(string animationName, float normalizedTime = -1)
        {
            var current = currentAnimation;
            base.Play(animationName);
            RefreshTimeData();
            if (currentAnimation != null && (current == null || currentAnimation.animationName != current.animationName))
                RestartAnim();
        }

        public override void Play(int index)
        {
            var current = currentAnimation;
            base.Play(index);
            RefreshTimeData();
            if (currentAnimation != null && (current == null || currentAnimation.animationName != current.animationName))
                RestartAnim();
        }

        public override void Play()
        {
            base.Play();
            RefreshTimeData();
        }
        public override void Pause()
        {
            base.Pause();
            // sync up shader and playback time
            float shaderTime = _shaderTime.y;
            float normalizedTime = (shaderTime - animTimeInfo.z) / (animTimeInfo.w - animTimeInfo.z);
            normalizedTime = Mathf.Clamp01(normalizedTime - Mathf.Floor(normalizedTime));
            currentFrame = Mathf.Max(0, Mathf.RoundToInt(Mathf.Min(Mathf.Ceil(animTimeInfo.y * normalizedTime), animTimeInfo.y - 1)));
            currentAnimTime = currentAnimation.length * normalizedTime;
            animTimeInfo.x = currentFrame;
            animTimeInfo.y = currentFrame;
            animTimeInfo.z = shaderTime;
            animTimeInfo.w = shaderTime;
            meshRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetVector(_animTimeProp, animTimeInfo);
            // set property block
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
        public override void RestartAnim()
        {
            base.RestartAnim();
            OnCurrentAnimationChanged(currentAnimation);
        }
        public override void Crossfade(int index, float speed)
        {
            if (currentAnimIndex < 0)
            {
                Play(index);
                return;
            }
            ShaderMeshAnimation currentAnimation = this.currentAnimation as ShaderMeshAnimation;
            CreatePropertyBlock();
            var shaderTime = _shaderTime.y;
            Vector4 crossfadeData = new Vector4(shaderTime, shaderTime + speed, speed, 0f);
            meshRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetVector(_crossfadeAnimScalarProp, currentAnimation.animScalar);
            materialPropertyBlock.SetVector(_crossfadeAnimInfoProp, animInfo);
            materialPropertyBlock.SetVector(_crossfadeAnimTimeInfo, animTimeInfo);
            materialPropertyBlock.SetVector(_crossfadeData, crossfadeData);
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
            base.StartCrossfade(index, speed);
        }
        public override void PrepopulateCrossfadePool(int count) { }
        public override void SetTime(float time, bool instantUpdate = false)
        {
            base.SetTime(time, instantUpdate);
            RefreshTimeData();
        }
        public override void SetTimeNormalized(float time, bool instantUpdate = false)
        {
            base.SetTimeNormalized(time, instantUpdate);
            RefreshTimeData();
        }
        public override void SetAnimations(IMeshAnimation[] meshAnimations)
        {
            this.meshAnimations = new ShaderMeshAnimation[meshAnimations.Length];
            for (int i = 0; i < meshAnimations.Length; i++)
            {
                this.meshAnimations[i] = meshAnimations[i] as ShaderMeshAnimation;
            }
            if (meshAnimations.Length > 0 && defaultMeshAnimation == null)
                defaultMeshAnimation = this.meshAnimations[0];
        }
        public override void StoreAdditionalMeshData(Mesh mesh) { }

        private void RefreshTimeData()
        {
            if (currentAnimation == null) return;
            float normalized = currentAnimTime / currentAnimation.length;
            // set time info
            float startTime = _shaderTime.y - (currentAnimation.length * normalized);
            animTimeInfo = new Vector4(
                0,
                currentAnimation.TotalFrames,
                startTime,
                startTime + (currentAnimation.length / (speed * currentAnimation.playbackSpeed)));
            materialPropertyBlock.SetVector(_animTimeProp, animTimeInfo);
            // set property block
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
        private void SetupTextureData()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (baseMesh == null || meshAnimations == null || meshAnimations.Length == 0)
                    return;
            }
#endif
            CreatePropertyBlock();
            if (!_animTextures.ContainsKey(baseMesh))
            {
                int totalTextures = 0;
                Vector2Int texSize = Vector2Int.zero;
                for (int i = 0; i < meshAnimations.Length; i++)
                {
                    var anim = meshAnimations[i];
                    totalTextures += anim.textures.Length;
                    for (int t = 0; t < anim.textures.Length; t++)
                    {
                        if (anim.textures[t].width > texSize.x)
                            texSize.x = anim.textures[t].width;

                        if (anim.textures[t].height > texSize.y)
                            texSize.y = anim.textures[t].height;
                    }
                }
                var textureLimit = QualitySettings.masterTextureLimit;
                QualitySettings.masterTextureLimit = 0;
                var copyTextureSupport = SystemInfo.copyTextureSupport;
                Texture2DArray texture2DArray = new Texture2DArray(texSize.x, texSize.y, totalTextures, meshAnimations[0].textures[0].format, false, false);
                texture2DArray.filterMode = FilterMode.Point;
                DontDestroyOnLoad(texture2DArray);
                int index = 0;
                for (int i = 0; i < meshAnimations.Length; i++)
                {
                    var anim = meshAnimations[i];
                    for (int t = 0; t < anim.textures.Length; t++)
                    {
                        var tex = anim.textures[t];
                        if (copyTextureSupport != UnityEngine.Rendering.CopyTextureSupport.None)
                        {
                            Graphics.CopyTexture(tex, 0, 0, texture2DArray, index, 0);
                        }
                        else
                        {
                            texture2DArray.SetPixels(tex.GetPixels(0), index);
                        }
                        index++;
                    }
                    totalTextures += anim.textures.Length;
                }
                if (copyTextureSupport == UnityEngine.Rendering.CopyTextureSupport.None)
                {
                    texture2DArray.Apply(true, true);
                }
                _animTextures.Add(baseMesh, texture2DArray);
                QualitySettings.masterTextureLimit = textureLimit;
            }
            _materialCacheLookup.Clear();
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.GetSharedMaterials(_materialCacheLookup);
            for (int m = 0; m < _materialCacheLookup.Count; m++)
            {
                Material material = _materialCacheLookup[m];
                if (_setMaterials.Contains(material))
                    continue;
                material.SetTexture(_animTexturesProp, _animTextures[baseMesh]);
                _setMaterials.Add(material);
            }
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                meshRenderer = GetComponent<MeshRenderer>();
                currentAnimation = defaultMeshAnimation ?? meshAnimations[0];
                currentFrame = Mathf.Clamp(currentFrame, 0, currentAnimation.Frames.Length - 1);
                for (int i = 0; i < meshAnimations.Length; i++)
                {
                    if (meshAnimations[i] == currentAnimation)
                        currentAnimIndex = i;
                }
                if (currentAnimation != null)
                {
                    OnCurrentAnimationChanged(defaultMeshAnimation ?? meshAnimations[0]);
                    DisplayFrame(Random.Range(-2, -10000));
                }
                var materials = meshRenderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetTexture(_animTexturesProp, _animTextures[baseMesh]);
                }
                meshRenderer.sharedMaterials = materials;
                meshRenderer.GetPropertyBlock(materialPropertyBlock);
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
#endif
        }
        private void CreatePropertyBlock()
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            if (materialPropertyBlock == null)
            {
                materialPropertyBlock = new MaterialPropertyBlock();
                meshRenderer.GetPropertyBlock(materialPropertyBlock);
            }
        }

        private float CalculateShaderNormalizedTime()
        {
            // mirrors MeshAnimator.cginc GetPixelOffset
            Vector4 shaderTime = _shaderTime;
            float wrapMode = animInfo.w;	
            bool isPlayingBackwards = animTimeInfo.w < animTimeInfo.z;
            float animRunTime = !isPlayingBackwards ?
                (shaderTime.y - animTimeInfo.z) / (animTimeInfo.w - animTimeInfo.z) :
                (shaderTime.y - animTimeInfo.z) / (animTimeInfo.z - animTimeInfo.w);
            bool isNormalizedLooping = animRunTime > 0.9999;
            bool isLoopingWrapMode = wrapMode >= 2;
            bool isPingPong = wrapMode >= 4;
            bool isEvenLoop = Mathf.Floor(animRunTime) % 2 > 0.5f;
            bool invertPingPongLoop = isPingPong && isEvenLoop;
            float normalizedLoopTime = animRunTime - Mathf.Floor(animRunTime);
            float normalizedTime = isLoopingWrapMode ? normalizedLoopTime : isNormalizedLooping ? 1f : normalizedLoopTime;
            normalizedTime = invertPingPongLoop && !isPlayingBackwards ? 1f - normalizedTime : normalizedTime;
            return normalizedTime;
        }

        private int CacheKey()
        {
            _materialCacheLookup.Clear();
            meshRenderer.GetSharedMaterials(_materialCacheLookup);
            int key = 0;
            for (int i = 0; i < _materialCacheLookup.Count; i++)
            {
                key = (key << 8) + _materialCacheLookup[i].GetInstanceID();
            }
            return key;
        }
    }
}
Shader "MyShader/Emissive"
{
    Properties{
       _MainTex("Texture2D", 2D) = "white"{}
       _BumpMap("Texture2D", 2D) = "bump"{}
       _SpecMap("SpecularMap", 2D) = "black"{}
       _SpecColor("SpecularColor" Color) = (0.5, 0.5, 0.5, 1.0)
       _SpecPower("SpecularPower", Range(0,1)) = 0.5
       _EmitMap("EmitPower", Range(0,2)) = 1.0
    }

    SubShader{
       Tags{"RenderType" = "Opaque"}
       CGPROGRAM
       #pragma surface surf BlinnPhong
       #pragma exclude_renderers flash

       sampler2D _MainTex;
       sampler2D _BumpMap;
       sampler2D _SpecMap;
       float _SpecPower;
       sampler2D _EmitMap;

       struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float2 uv_SpecMap;
          float2 uv_EmitMap;
       };
       void surf(Input IN, inout SurfaceOutput o) {
           fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
           fixed4 spec = tex2D(_SpecMap, IN.uv_SpecMap);
           fixed4 emit = tex2D(_EmitMap, IN.uv_EmitMap);

           o.Albedo = tex.rgb;
           o.Normal = UnpackNormal(_BumpMap, IN.uv_BumpMap);
           o.Specular = _SpecPower;
           o.Gloss = spec.rgb;
           o.Emission = emit.rgb * _EmitPower;
       }
       ENDCG
   }
   Fallback "Diffuse"
}
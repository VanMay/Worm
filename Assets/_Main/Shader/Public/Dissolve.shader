Shader "Main/Dissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_AO ("AO", 2D) = "AO" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Dissolve ("Dissolve", Range(0,1)) = 0
		_DissolveColor ("Dissolve Color", Color) = (1,1,1,1)
		_Edge ("Edge", Float) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _AO;
		sampler2D _NoiseTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NoiseTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Dissolve;
		fixed4 _DissolveColor;
		float _Edge;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float noise = tex2D(_NoiseTex, IN.uv_NoiseTex);
			clip(noise - _Dissolve);
			float factor = (1 - smoothstep(0, _Edge, noise - _Dissolve)) * step(0.0001, _Dissolve);
			fixed4 col = lerp(c, _DissolveColor, factor);
			o.Albedo = col.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			o.Occlusion = tex2D(_AO, IN.uv_MainTex);
			o.Emission = lerp(fixed4(0,0,0,0), _DissolveColor, factor);
		}
		ENDCG
	}
	FallBack "Diffuse"
}

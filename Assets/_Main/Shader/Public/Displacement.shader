Shader "Main/Displacement" {
	Properties {
		[Header(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_AO ("AO", 2D) = "white" {}

		[Header(Tessellation)]
		[Toggle(ENABLE_TESS)] _Tess ("Enable Tessellation", Float) = 1
		_EdgeLength ("Edge Length", Range(1,64)) = 16
		_Phong ("Phong Strength", Range(0,1)) = 0.5

		[Header(Displacement)]
		[Toggle(ENABLE_DISP)] _Disp ("Enable Displacement", Float) = 1
		_Displacement ("Displacement Strength", Range(0,1)) = 1
		_DispTex ("Displacement Texture", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma shader_feature ENABLE_TESS
		#pragma shader_feature ENABLE_DISP
		#pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tess tessphong:_Phong

		#pragma target 5.0
		#include "Tessellation.cginc"

		//细分曲面
		float _EdgeLength;
		float _Phong;
		float4 tess(appdata_full v0, appdata_full v1, appdata_full v2) {
			#if ENABLE_TESS
				return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
			#else
				return 1;
			#endif
		}

		//置换贴图
		sampler2D _DispTex;
		float _Displacement;
		void disp(inout appdata_full v) {
			#if ENABLE_DISP
				float height = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
				v.vertex.xyz += v.normal * height;
			#endif
		}

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		sampler2D _NormalMap;
		sampler2D _AO;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			o.Occlusion = tex2D(_AO, IN.uv_MainTex);
		}
		ENDCG
	}
	FallBack "Diffuse"
}

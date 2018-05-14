Shader "Main/WetSurface" {
	Properties {
		[Header(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_AO ("AO", 2D) = "white" {}
		_ParalaxTex ("Parallax Texture", 2D) = "black" {}
		_Parallax ("Parallax", Range(0.005,0.08)) = 0.02

		[Header(Wet)]
		_WetLevel ("Wet Level", Range(0,1)) = 1
		_WaterLevel ("Final Water Level", Range(0, 1)) = 0.5
		_DeltaWetHeight ("Delta Wet Height", Range(0, 0.5)) = 0.3
		_WetMetallic ("Wet Metallic", Range(0,1)) = 1
		_WetSmoothness ("Wet Smoothness", Range(0,1)) = 1
		_WaterMetallic ("Water Metallic", Range(0,1)) = 1
		_WaterSmoothness ("Water Smoothness", Range(0,1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0
		#include "Tessellation.cginc"
		#include "UnityCG.cginc"

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		sampler2D _NormalMap;
		sampler2D _AO;
		sampler2D _ParalaxTex;
		float _Parallax;

		float _WaterLevel;
		float _WetLevel;
		float _DeltaWetHeight;
		float _WetMetallic;
		float _WetSmoothness;
		float _WaterMetallic;
		float _WaterSmoothness;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			//视差贴图计算
			float h = tex2D(_ParalaxTex, IN.uv_MainTex).r;
			float2 offset = ParallaxOffset(h, _Parallax, IN.viewDir);
			IN.uv_MainTex += offset;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float3 normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));

			float delta = _DeltaWetHeight;
			float waterLevel = _WaterLevel - delta;

			//湿润
			if(_WetLevel < 0.5) {
				float factor = _WetLevel / 0.5;
				float currentWaterLevel = factor * _WaterLevel;
				if(currentWaterLevel > h) {
					if(currentWaterLevel - h > delta) {
						_Metallic = _WetMetallic;
						_Glossiness = _WetSmoothness;
					}
					else {
						float ratio = (currentWaterLevel - h) / delta;
						_Metallic = lerp(_Metallic, _WetMetallic, ratio);
						_Glossiness = lerp(_Glossiness, _WetSmoothness, ratio);
					}
				}
			}
			//积水
			else {
				float factor = (_WetLevel - 0.5) / 0.5;
				float currentWaterLevel = factor * waterLevel;
				//水面
				if(currentWaterLevel > h) {
					_Metallic = _WaterMetallic;
					_Glossiness = _WaterSmoothness;
					normal = float3(0,0,1);
				}
				//湿润区
				else if(h < _WaterLevel) {
					if(_WaterLevel - h > delta) {
						_Metallic = _WetMetallic;
						_Glossiness = _WetSmoothness;
					}
					else {
						float ratio = (_WaterLevel - h) / delta;
						_Metallic = lerp(_Metallic, _WetMetallic, ratio);
						_Glossiness = lerp(_Glossiness, _WetSmoothness, ratio);
					}
				}
			}

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = normal;
			o.Occlusion = tex2D(_AO, IN.uv_MainTex);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
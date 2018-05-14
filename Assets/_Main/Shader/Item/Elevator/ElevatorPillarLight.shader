Shader "Main/Item/Elevator/ElevatorPillarLight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_LightColor ("Light Color", Color) = (1,1,1,1)
		_LightMask ("Light Mask Map", 2D) = "white" {}
		_BlinkTime ("Blink Time", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _LightMask;

		struct Input {
			float2 uv_MainTex;
			float2 uv_LightMask;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _LightColor;
		float _BlinkTime;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			uint time = _Time.y / _BlinkTime;
			uint lightArea = tex2D (_LightMask, IN.uv_LightMask).r * (time % 2 == 0 ? 1 : 0);
			o.Emission = lightArea * _LightColor;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

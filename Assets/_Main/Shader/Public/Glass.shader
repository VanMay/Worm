// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Main/Glass"
{
	Properties
	{
		[KeywordEnum(Solid, Thin)] _Type ("Type", Float) = 0
		_Color ("Color", Color) = (0,0,0,0)
		_CubeMap ("Cube Map", Cube) = "_Skybox" {}
		_MainTex ("Texture", 2D) = "white" {}
		_Refraction ("Refraction", Range(0,1)) = 1
	}
	SubShader
	{
		Tags 
		{ 
			"RenderType"="Opaque"
			"Queue"="Transparent"
		}
		LOD 100

		GrabPass { "_RefractionTex" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _TYPE_SOLID _TYPE_THIN 
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "MyLib.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
			};

			fixed4 _Color;
			samplerCUBE _CubeMap;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Refraction;
			sampler2D _RefractionTex;
			float4 _RefractionTex_TexelSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.screenPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f IN) : SV_Target
			{
				fixed4 texCol = tex2D(_MainTex, IN.uv) * _Color;
				float3 viewDir = normalize(IN.viewDir);
				float3 normal = normalize(IN.normal);
				//菲涅尔反射
				float fresnel = GetFresnel(viewDir, normal);
				float3 reflDir = reflect(-viewDir, normal);
				fixed3 reflCol = texCUBE(_CubeMap, reflDir).rgb;

				float2 screenNormal = mul(unity_WorldToCamera, normal);
				#if _TYPE_THIN
					//折射
					float2 offset = screenNormal * _RefractionTex_TexelSize * 64 * _Refraction * fresnel;
					fixed3 refrCol = tex2D(_RefractionTex, (IN.screenPos.xy + offset) / IN.screenPos.w);
					fixed3 finalColor = lerp(texCol.rgb, reflCol, fresnel) + refrCol;
				#elif _TYPE_SOLID
					//折射
					float2 offset = -screenNormal * _RefractionTex_TexelSize * 512 * _Refraction * fresnel;
					fixed3 refrCol = tex2D(_RefractionTex, (IN.screenPos.xy + offset) / IN.screenPos.w);
					fixed3 finalColor = lerp(texCol.rgb, reflCol, fresnel) + refrCol;
				#endif
				//高光
				float3 lightDir = -_WorldSpaceLightPos0;
				fixed3 specular = _LightColor0.rgb * pow(saturate(dot(reflect(lightDir, normal), viewDir)), 64) * fresnel;

				finalColor += specular; 
				return fixed4(finalColor, 1);
			}
			ENDCG
		}
	}
	FallBack "Transparent/VertexLit"
}

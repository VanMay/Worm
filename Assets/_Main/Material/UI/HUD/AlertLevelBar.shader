// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sprites Default"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Value ("Value", Range(0, 1)) = 0
		_EmptyColor ("Empty Color", Color) = (1,1,1,1)
		_StartColor ("Start Color", Color) = (1,1,1,1)
		_MiddleColor ("Middle Color", Color) = (1,1,1,1)
		_EndColor ("End Color", Color) = (1,1,1,1)
		_Mask ("Mask", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
			
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;

			float _Value;
			fixed4 _EmptyColor;
			fixed4 _StartColor;
			fixed4 _MiddleColor;
			fixed4 _EndColor;
			sampler2D _Mask;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				fixed4 valueColor;
				if (_Value < 0.5) {
					valueColor = lerp(_StartColor, _MiddleColor, _Value / 0.5);
				}
				else {
					valueColor = lerp(_MiddleColor, _EndColor, (_Value - 0.5) / 0.5);
				}

				float mask = tex2D(_Mask, IN.texcoord).r;
				if (mask < _Value) {
					c *= valueColor;
				}
				else {
					c *= _EmptyColor;
					c.rgb *= c.a;
				}

				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14201
305;565;1191;498;1005.867;330.7558;1.277313;True;True
Node;AmplifyShaderEditor.TemplateMasterNode;0;20,-57;Float;False;True;2;Float;ASEMaterialInspector;0;4;Sprites Default;0f8ba0101102bb14ebf021ddadce9b49;Sprites Default;3;One;OneMinusSrcAlpha;0;One;Zero;Off;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
ASEEND*/
//CHKSM=C595BBBE24A01152ED5E471FE75C8D165A6A7F49
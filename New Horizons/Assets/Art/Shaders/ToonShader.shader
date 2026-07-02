Shader "Custom/ToonShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Normal("Normal Map", 2D) = "black" {}
		_CellShadeDevide("Cell shade lighting", 2D) = "black" {}
		_MainCol("Color", Color) = (1,1,1,1)
		_Smoothness("Smoothness", float) = .5
		_RimAmount("Rim Light Strength", float) = 0
		_OutlineThickness("Outline Thickness", float) = 0.00035
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
	}
	SubShader
	{
		Tags {"RenderType" = "Opaque"}
		Pass
		{
			Name "UniversalForward"
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

			struct Attributes
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 positionWS : TEXCOORD1;
				float3 normalWS : TEXCOORD2;
				half4 shadowCoord : TEXCOORD3;
			};

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			TEXTURE2D(_CellShadeDevide);
			SAMPLER(sampler_CellShadeDevide);
			float4 _MainCol;

			Varyings vert(Attributes v)
			{
				Varyings o;

				VertexPositionInputs posInputs = GetVertexPositionInputs(v.positionOS.xyz);
				VertexNormalInputs normalInputs = GetVertexNormalInputs(v.normalOS);

				o.positionCS = posInputs.positionCS;
				o.positionWS = posInputs.positionWS;
				o.normalWS = normalInputs.normalWS;
				o.uv = v.uv;

				o.shadowCoord = TransformWorldToShadowCoord(posInputs.positionWS);

				return o;
			}

			half4 frag(Varyings i) : SV_Target
			{
				float4 shadowCoord = TransformWorldToShadowCoord(i.positionWS);
				Light mainLight = GetMainLight(shadowCoord);
				float3 normalWS = normalize(i.normalWS);
				float NdotL = saturate(dot(normalWS, mainLight.direction));
				float light = NdotL * mainLight.shadowAttenuation;

				half4 albedo = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);
				half4 cellLight = SAMPLE_TEXTURE2D(_CellShadeDevide,sampler_CellShadeDevide,float2(clamp(light, 0.05, 0.95), 0.5));

				return albedo * _MainCol * cellLight;
				//return i.shadowCoord;
			}

			ENDHLSL
		}



		//Outline
		Pass
		{
			Tags {"RenderType" = "Transparent"}
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 normal : NORMAL;
			};

			float _OutlineThickness;
			half4 _OutlineColor;

			v2f vert(appdata v)
			{
				v2f o;
				v.vertex += v.normal * _OutlineThickness;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.uv = v.uv;


				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				if(_OutlineColor.a <= 0)
				{
					discard;
				}
				return _OutlineColor;
			}
			ENDCG
		}

		// cast shadows:
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On
			ZTest LEqual
			ColorMask 0
			Cull Back

			HLSLPROGRAM

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment

			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

			#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"

			ENDHLSL
		}
	}
}
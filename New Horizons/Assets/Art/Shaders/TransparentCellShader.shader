Shader "Custom/TransparentCellShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_CellShadeDevide("Cell shade lighting", 2D) = "black" {}
		_MainCol("Color", Color) = (1,1,1,1)
		_Smoothness("Smoothness", float) = .5
		_RimAmount("Rim Light Strength", float) = 0
	}
	SubShader
	{
		Tags {"RenderType" = "Transparent"}
		Pass
		{
			Name "UniversalForward"
			Tags { "LightMode" = "UniversalForward" }
			Blend One One
			ZWrite Off
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
				half4 finalCol = albedo * _MainCol * cellLight;
				finalCol.a = _MainCol.a;
				return finalCol;
			}

			ENDHLSL
		}



		
	}
}
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
			Tags {"LightMode" = "UniversalForward"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc" // NEEDED FOR LightColor0 !

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
				float4 world : TEXCOORD1;
				float4 worldNormal : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D _Normal;
			sampler2D _CellShadeDevide;
			fixed4 _MainCol;
			float _Smoothness;
			float _RimAmount;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.world = mul(UNITY_MATRIX_M, v.vertex);
				o.worldNormal = normalize(mul(UNITY_MATRIX_M, float4(v.normal.xyz,0)));
				o.normal = v.normal;
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//i.normal += 2 * tex2D(_Normal, i.uv) - 1;
				fixed light = saturate(dot(i.worldNormal, _WorldSpaceLightPos0));
				fixed specular = pow(saturate(dot(normalize(_WorldSpaceCameraPos - i.world), normalize(reflect(i.worldNormal, _WorldSpaceLightPos0)))), _Smoothness);
				fixed rim = 1 - pow(saturate(dot(normalize(_WorldSpaceCameraPos), i.worldNormal)), _RimAmount);
				fixed4 albedo = tex2D(_MainTex, i.uv);
				albedo *= _LightColor0;
				//albedo += specular * _LightColor0;
				//albedo += rim * _LightColor0;

				half4 cellLight = tex2D(_CellShadeDevide, fixed2(max(min(light.x, .95), .05), .5));

				return albedo * _MainCol * cellLight;
			}
			ENDCG
		}

		//Outline
		Pass
		{
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
				return _OutlineColor;
			}
			ENDCG
		}

		// cast shadows:
		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex VSMain
			#pragma fragment PSMain

			float4 VSMain(float4 vertex:POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 PSMain(float4 vertex:SV_POSITION) : SV_TARGET
			{
				return 0;
			}

			ENDCG
		}
	}
}
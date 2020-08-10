Shader "Custom/StarFon" {
	Properties
	{
		_BackgroundColor("Background Color", Color) = (1,1,1,1)

		_Background("Background (RGB)", 2D) = "black" {}
		_BackgroundForce("Background Force", Range(0,10)) = 1
		_BackgroundSpeed("Background Speed", float) = 1

		_SpaceDust("_SpaceDust (RGB)", 2D) = "black" {}
		_SpaceDustForce("Space Dust Force", Range(0,10)) = 1
		_SpaceDustSpeed("Space Dust Speed", float) = 1

		_SmallStars("Small Stars (RGBA)" , 2D) = "black" {}
		_SmallStarsForce("SmallStars Force", Range(0,10)) = 1
		_SmallStarsSpeed("Background Speed", float) = 1

		_MediumStars("Medium Stars (RGBA)", 2D) = "black" {}
		_MediumStarsForce("MediumStars Force", Range(0,10)) = 1
		_MediumStarsSpeed("Background Speed", float) = 1

		_BigStars("Big Stars (RGBA)"   , 2D) = "black" {}
		_BigStarsForce("BigStars Force", Range(0,10)) = 1
		_BigStarsSpeed("Background Speed", float) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		
		Pass
		{
		//-----------------------------------------------------------------------------------//
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct VS_INPUT
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VS_OUTPUT
			{
				float4 vertex : SV_POSITION;
				float2 uv_Background : TEXCOORD0;
				float2 uv_SpaceDust : TEXCOORD1;
				float2 uv_SmallStars : TEXCOORD2;
				float2 uv_MediumStars : TEXCOORD3;
				float2 uv_BigStars : TEXCOORD4;
			};

			sampler2D _Background;
			float _BackgroundForce;
			float _BackgroundSpeed;

			sampler2D _SpaceDust;
			float _SpaceDustForce;
			float _SpaceDustSpeed;

			sampler2D _SmallStars;
			float _SmallStarsForce;
			float _SmallStarsSpeed;

			sampler2D _MediumStars;
			float _MediumStarsForce;
			float _MediumStarsSpeed;

			sampler2D _BigStars;
			float _BigStarsForce;
			float _BigStarsSpeed;

			fixed4 _BackgroundColor;

			float4 _Background_ST;
			float4 _SpaceDust_ST;
			float4 _SmallStars_ST;
			float4 _MediumStars_ST;
			float4 _BigStars_ST;

			VS_OUTPUT vert(VS_INPUT input)
			{
				VS_OUTPUT output;

				output.vertex = UnityObjectToClipPos(input.vertex);

				output.uv_Background = TRANSFORM_TEX(input.uv, _Background);
				output.uv_SpaceDust = TRANSFORM_TEX(input.uv, _SpaceDust);
				output.uv_SmallStars = TRANSFORM_TEX(input.uv, _SmallStars);
				output.uv_MediumStars = TRANSFORM_TEX(input.uv, _MediumStars);
				output.uv_BigStars = TRANSFORM_TEX(input.uv, _BigStars);

				return output;
			}

			fixed4 frag(VS_OUTPUT input) : SV_Target
			{
				fixed4 final;

				half4 background = tex2D(_Background, input.uv_Background	 + float2(_WorldSpaceCameraPos.x / (230 * (_BackgroundSpeed  / 10) / _Background_ST.x) , _WorldSpaceCameraPos.y / (230 * (_BackgroundSpeed  / 10) / _Background_ST.y )));
				half4 spacedust = tex2D(_SpaceDust, input.uv_SpaceDust		 + float2(_WorldSpaceCameraPos.x / (230 * (_SpaceDustSpeed   / 10) / _SpaceDust_ST.x)  , _WorldSpaceCameraPos.y / (230 * (_SpaceDustSpeed   / 10) / _SpaceDust_ST.y  )));
				half4 smallStars = tex2D(_SmallStars, input.uv_SmallStars	 + float2(_WorldSpaceCameraPos.x / (230 * (_SmallStarsSpeed  / 10) / _SmallStars_ST.x) , _WorldSpaceCameraPos.y / (230 * (_SmallStarsSpeed  / 10) / _SmallStars_ST.y )));
				half4 mediumStars = tex2D(_MediumStars, input.uv_MediumStars + float2(_WorldSpaceCameraPos.x / (230 * (_MediumStarsSpeed / 10) / _MediumStars_ST.x), _WorldSpaceCameraPos.y / (230 * (_MediumStarsSpeed / 10) / _MediumStars_ST.y)));
				half4 bigStars = tex2D(_BigStars, input.uv_BigStars			 + float2(_WorldSpaceCameraPos.x / (230 * (_BigStarsSpeed	 / 10) / _BigStars_ST.x)   , _WorldSpaceCameraPos.y / (230 * (_BigStarsSpeed	/ 10) / _BigStars_ST.y	 )));

				//half3 starRGB = smallStars.rgb * smallStars.a + mediumStars.rgb * mediumStars.a + bigStars.rgb * bigStars.a;
				half4 starRGB = smallStars * _SmallStarsForce + mediumStars * _MediumStarsForce + bigStars * _BigStarsForce + background * _BackgroundForce + spacedust * _SpaceDustForce;

				final = _BackgroundColor + starRGB;

				/*half starAlpha = smallStars.a * (2 + sin(input.uv_SmallStars.x * input.uv_SmallStars.y * 12 + _Time.w * 3)) +
					mediumStars.a * (2 + sin(input.uv_MediumStars.x * input.uv_MediumStars.y * 24 + _Time.z * 2) / 2) +
					bigStars.a;*/

				//final.Emission = background.rgb + starRGB * starAlpha;

				return final;
			}

				ENDCG
			//-----------------------------------------------------------------------------------//
		}
	}
}

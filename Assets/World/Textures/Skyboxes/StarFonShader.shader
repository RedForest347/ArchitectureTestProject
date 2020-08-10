Shader "Custom/StarFonShader" {
	Properties
	{
		_BackgroundColor("Background Color", Color) = (1,1,1,1)

		[Toggle] _BackgroundShow("Show Background", Float) = 0
		_Background("Background (RGB)", 2D) = "black" {}
		_Background_Mask("Background Mask (RGB)", 2D) = "black" {}
		_BackgroundColorForce("Background Color Force", Range(0,20)) = 1

		[Toggle] _SpaceDustShow("Show SpaceDust", Float) = 0
		_SpaceDust("SpaceDust (RGB)", 2D) = "black" {}
		_SpaceDust_Mask("SpaceDust Mask (RGB)", 2D) = "black" {}
		_SpaceDustColorForce("Space Dust Color Force", Range(0,20)) = 1

		[Toggle] _SmallStarsShow("Show SmallStars", Float) = 0
		_SmallStars("Small Stars (RGBA)" , 2D) = "black" {}
		_SmallStars_Mask("Small Stars Mask (RGBA)" , 2D) = "black" {}
		_SmallStarsColorForce("SmallStars Color Force", Range(0,20)) = 1

		[Toggle] _MediumStarsShow("Show MediumStars", Float) = 0
		_MediumStars("Medium Stars (RGBA)", 2D) = "black" {}
		_MediumStars_Mask("Medium Stars Mask (RGBA)", 2D) = "black" {}
		_MediumStarsColorForce("MediumStars Color Force", Range(0,20)) = 1

		[Toggle] _BigStarsShow("Show BigStars", Float) = 0
		_BigStars("Big Stars (RGBA)", 2D) = "black" {}
		_BigStars_Mask("Big Stars Mask (RGBA)", 2D) = "black" {}
		_BigStarsColorForce("BigStars Color Force", Range(0,20)) = 1
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		LOD 200

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
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
				float2 uv_Background_Mask : TEXCOORD1;

				float2 uv_SpaceDust : TEXCOORD2;
				float2 uv_SpaceDust_Mask : TEXCOORD3;

				float2 uv_SmallStars : TEXCOORD4;
				float2 uv_SmallStars_Mask : TEXCOORD5;

				float2 uv_MediumStars : TEXCOORD6;
				float2 uv_MediumStars_Mask : TEXCOORD7;

				float2 uv_BigStars : TEXCOORD8;
				float2 uv_BigStars_Mask : TEXCOORD9;

			};

			sampler2D _Background;
			sampler2D _Background_Mask;
			float4 _Background_ST;
			float4 _Background_Mask_ST;
			float _BackgroundColorForce;
			float _BackgroundShow;

			sampler2D _SpaceDust;
			sampler2D _SpaceDust_Mask;
			float4 _SpaceDust_ST;
			float4 _SpaceDust_Mask_ST;
			float _SpaceDustColorForce;
			float _SpaceDustShow;

			sampler2D _SmallStars;
			sampler2D _SmallStars_Mask;
			float4 _SmallStars_ST;
			float4 _SmallStars_Mask_ST;
			float _SmallStarsColorForce;
			float _SmallStarsShow;

			sampler2D _MediumStars;
			sampler2D _MediumStars_Mask;
			float4 _MediumStars_ST;
			float4 _MediumStars_Mask_ST;
			float _MediumStarsColorForce;
			float _MediumStarsShow;

			sampler2D _BigStars;
			sampler2D _BigStars_Mask;
			float4 _BigStars_ST;
			float4 _BigStars_Mask_ST;
			float _BigStarsColorForce;
			float _BigStarsShow;

			fixed4 _BackgroundColor;


			VS_OUTPUT vert(VS_INPUT input)
			{
				VS_OUTPUT output;

				output.vertex = UnityObjectToClipPos(input.vertex);

				output.uv_Background = TRANSFORM_TEX(input.uv, _Background);
				output.uv_Background_Mask = TRANSFORM_TEX(input.uv, _Background_Mask);

				output.uv_SpaceDust = TRANSFORM_TEX(input.uv, _SpaceDust);
				output.uv_SpaceDust_Mask = TRANSFORM_TEX(input.uv, _SpaceDust_Mask);

				output.uv_SmallStars = TRANSFORM_TEX(input.uv, _SmallStars);
				output.uv_SmallStars_Mask = TRANSFORM_TEX(input.uv, _SmallStars_Mask);

				output.uv_MediumStars = TRANSFORM_TEX(input.uv, _MediumStars);
				output.uv_MediumStars_Mask = TRANSFORM_TEX(input.uv, _MediumStars_Mask);

				output.uv_BigStars = TRANSFORM_TEX(input.uv, _BigStars);
				output.uv_BigStars_Mask = TRANSFORM_TEX(input.uv, _BigStars_Mask);

				return output;
			}

			/*float4 Tiling(float approximation, float2 texture_ST) {
				float a = (10 / (unity_OrthoParams.y)) + approximation / 100;
				return float4(a, a, a * texture_ST.x * -0.5 + 0.5, a * texture_ST.y * -0.5 + 0.5);
			}

			float2 Offsett(float approximation, float speed, float2 texture_ST) {
				return float2(Tiling(approximation, texture_ST).z, Tiling(approximation, texture_ST).w);
			}*/

			fixed4 frag(VS_OUTPUT input) : SV_Target
			{
				fixed4 final;

				//half4 background = tex2D(_Background, input.uv_Background * Tiling(10, _Background_ST) + Offsett(10, 70, _Background_ST));
				
				half4 background = tex2D(_Background, input.uv_Background);
				half4 spacedust = tex2D(_SpaceDust, input.uv_SpaceDust);
				half4 smallStars = tex2D(_SmallStars, input.uv_SmallStars);
				half4 mediumStars = tex2D(_MediumStars, input.uv_MediumStars);
				half4 bigStars = tex2D(_BigStars, input.uv_BigStars);				

				half4 background_mask = tex2D(_Background_Mask, input.uv_Background_Mask);
				half4 spacedust_mask = tex2D(_SpaceDust_Mask, input.uv_SpaceDust_Mask);
				half4 smallStars_mask = tex2D(_SmallStars_Mask, input.uv_SmallStars_Mask);
				half4 mediumStars_mask = tex2D(_MediumStars_Mask, input.uv_MediumStars_Mask);
				half4 bigStars_mask = tex2D(_BigStars_Mask, input.uv_BigStars_Mask);


				background = background * _BackgroundColorForce * _BackgroundShow;
				background.rgb *= background_mask.rgb;

				spacedust = spacedust * _SpaceDustColorForce * _SpaceDustShow;
				spacedust.rgb *= spacedust_mask.rgb;

				smallStars = smallStars * _SmallStarsColorForce * _SmallStarsShow;
				smallStars.rgb *= smallStars_mask.rgb;

				mediumStars = mediumStars * _MediumStarsColorForce * _MediumStarsShow;
				mediumStars.rgb *= mediumStars_mask.rgb;

				bigStars = bigStars * _BigStarsColorForce * _BigStarsShow;
				bigStars.rgb *= bigStars_mask.rgb;


				half4 starRGB = background + spacedust + smallStars + mediumStars + bigStars;
				final = _BackgroundColor + starRGB;

				final.a = 1;
				return final;
			}

			ENDCG
			//-----------------------------------------------------------------------------------//
		}
	}
}


Shader "My/SpriteFlashSingleColor"
{
	//類似外部變數的部分，可以Inspector中編輯
	Properties
	{
		//宣告一個主要的貼圖變數
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		//宣告一個rgba的變數
		_Color ("Color", Color) = (1,1,1,1)
		//變色開關 1 = 開 , 0 = 關
		SingleColor ("Single Color", Float) = 0
		//宣告一個類似布林值的變數
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}
	//一個子shader，一個shader可以有多個子shader，根據情況來使用不同的子shader
	SubShader
	{
		//子着色器使用标签来告诉渲染引擎期望何时和如何渲染对象。
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
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			float SingleColor;
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) ;
				if (SingleColor == 1)
				{
					c.rgb = _Color;
				}
				else
				{
					c *= IN.color;
				}
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}

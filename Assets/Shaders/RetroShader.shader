Shader "Unlit/RetroShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		smallSpeed("Small Speed", Range(0, 10)) = 1
		smallFrequency("Small Frequency", Range(0, 5000)) = 2500
		smallLow("Small Low", Range(0, 2)) = 0.5
		smallHigh("Small High", Range(0, 2)) = 1

		largeSpeed("Larg Speed", Range(0, 10)) = 2
		largeFrequency("Larg Frequency", Range(0, 5000)) = 500
		largeLow("Larg Low", Range(0, 2)) = 0.5
		largeHigh("Larg High", Range(0, 2)) = 1

		yOff("Y Offset", Range(0,1)) = 0
		brightness("Brightness", Range(0,1)) = 1
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float yOff;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv + float2(0, yOff), _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float smallSpeed = 1;
			float smallFrequency = 2500;
			float smallLow = 0.5;
			float smallHigh = 1;

			float largeSpeed = 1;
			float largeFrequency = 500;
			float largeLow = 0.5;
			float largeHigh = 1;

			float brightness = 1;

			fixed4 frag(v2f i) : SV_Target
			{
				float small = sin((i.uv.y + (_Time * smallSpeed))* smallFrequency) * (smallHigh - smallLow) + smallLow;
				float large = sin((i.uv.y + (_Time * largeSpeed))* largeFrequency) * (largeHigh - largeLow) + largeLow;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * small * large;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col * brightness;
			}
			ENDCG
		}
	}
}

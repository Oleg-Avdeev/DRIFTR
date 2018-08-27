Shader "Unlit/3d"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_White ("White", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				half3 worldNormal : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float _White;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 lightDirection = float4(0,-1,-0.5,0);
			
			v2f vert (float4 vertex : POSITION, float3 normal : NORMAL)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				o.worldNormal = UnityObjectToWorldNormal(normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 c = 0;
				c.rgb = i.worldNormal*0.5+0.5+_White;
				return c;
			}
			ENDCG
		}
	}
}

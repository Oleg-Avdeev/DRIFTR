Shader "Unlit/Shaded 3d"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
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

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				return o;
			}

			fixed4 _Color;
			fixed _White;
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color+_White;
			}
			ENDCG
		}
	}
}

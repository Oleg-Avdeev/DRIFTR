// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/boss 3dr" {
    Properties {
        _HueShift("HueShift", Float ) = 0
        _Frequency("Frequency", Float) = 1
    }
    SubShader {
 
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
 
            #include "UnityCG.cginc"
 
            float3 shift_col(float3 RGB, float shift)
            {
                float3 RESULT = float3(RGB);
                float VSU = cos(shift*3.14159265/180);
                float VSW = sin(shift*3.14159265/180);
               
                RESULT.x = (.299 + .701*VSU + .168*VSW)*RGB.x
                        + (.587  - .587*VSU + .330*VSW)*RGB.y
                        + (.114  - .114*VSU - .497*VSW)*RGB.z;
               
                RESULT.y = (.299 - .299*VSU - .328*VSW)*RGB.x
                        + (.587  + .413*VSU + .035*VSW)*RGB.y
                        + (.114  - .114*VSU + .292*VSW)*RGB.z;
               
                RESULT.z = (.299 - .3*VSU + 1.25*VSW)*RGB.x
                        + (.587  - .588*VSU - 1.05*VSW)*RGB.y
                        + (.114  + .886*VSU - .203*VSW)*RGB.z;
               
            return (RESULT);
            }
 
            struct v2f {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
 
            float4 _MainTex_ST;
 
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
 
            float _Frequency;
            float _HueShift;
 
            half4 frag(v2f i) : COLOR
            {
                half4 col = fixed4(1,0,0,1);
                float shift = _HueShift * sin(_Time.x * _Frequency);
                return half4( half3(shift_col(col, shift)), col.a);
            }
            ENDCG
        }
    }
    Fallback "Particles/Alpha Blended"
}
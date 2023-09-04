Shader "Unlit/MainMenu"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // normalized sine between 0 and 1.
            float4 nsin(float value)
            {
                return (sin(value) + 1.0) * 0.5;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time * 10.0;
                float r = nsin(t + 0.0);
                float g = nsin(t + 3.14/2);
                float b = nsin(t + 3.14);
                float4 col = float4(r,g,b,0.0);

                // i.uv.x - (x offset) - (shaping function) * (intensity of color) 
                col *= (smoothstep(0.0, 0.001, i.uv.x - (0.7 + cos(_Time * 1.0) / 50.0) - (nsin(i.uv.y * 15.0 + _Time * 80.0 + 0.9) * 0.08 - i.uv.y * 0.08))) * (1.0);

                return col;
            }
            ENDCG
        }
    }
}

Shader "Unlit/Grid"
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

            float distFromGrid(float2 pos)
            {
                float dist = 0.0;

                for (float i = 0.0; i < 1.0; i += 0.1)
                {
                    // vert lines
                    dist += 1.0 - smoothstep(0.0, 0.004, abs(pos[0] + pos.y*(i-0.5) - i) );
                    // horz lines
                    dist += 1.0 - smoothstep(0.0, 0.004, fmod(abs(pos[1] - i + _Time),1.0));
                }

                return dist; 
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                float4 col;
                col = float4(distFromGrid(i.uv), 0.0, 0.0, 0.0);
                return col;
            }
            ENDCG
        }
    }
}

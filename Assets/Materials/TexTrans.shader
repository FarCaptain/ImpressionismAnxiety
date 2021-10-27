Shader "Unlit/TexTrans"
{
    Properties
    {
        _MainTex ("Texture1", 2D) = "white" {}
        _MainTex2 ("Texture2", 2D) = "white" {}
        _MainTex3 ("Texture3", 2D) = "white" {}
        _Anxiety ("Anxiety", float) = 100
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
            sampler2D _MainTex2;
            sampler2D _MainTex3;
            float4 _MainTex_ST;

            float _Anxiety;

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
                fixed4 col;
                // 0~50 50~100 
                if( _Anxiety <= 50.0 )    
                    col = lerp(tex2D(_MainTex3, i.uv), tex2D(_MainTex2, i.uv), _Anxiety / 50.0);
                else
                    col = lerp(tex2D(_MainTex2, i.uv), tex2D(_MainTex, i.uv), (_Anxiety - 50.0) / 50.0);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}

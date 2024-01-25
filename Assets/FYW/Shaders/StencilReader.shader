Shader "Unlit/StencilReader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {} // Define the Albedo texture property
    }
    
    SubShader
    {
        Pass
        {
            Stencil
            {
                Ref 1
                Comp notequal
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv); // Use the Albedo texture
            }

            ENDCG
        }
    }
}

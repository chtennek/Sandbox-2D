Shader "Sandbox/MultiplyShader"
{
    Properties
    {
        _MainTex ("Image", 2D) = "white" {}
        _SubTex ("Texture Map", 2D) = "white" {}
        
        _Threshold("Threshold", Range(0, 1)) = 0.5
        _Falloff("Falloff", Range(0, 1)) = 0.1
        _Null("Null", Color) = (0, 0, 0, 1)
    }

    SubShader
    {
        Tags
        {
        "Queue" = "Transparent"
        "PreviewType" = "Plane"
        }

        Pass 
        {
            Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
         
            sampler2D _MainTex;
            sampler2D _SubTex;
            
            fixed _Threshold;
            fixed _Falloff;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 addedCol = tex2D(_SubTex, i.uv) + (_Threshold) * float4(1, 1, 1, 1);
                fixed4 clampedCol = clamp(addedCol, 0, 1);
                fixed4 appliedCol = lerp(float4(1, 1, 1, 1), clampedCol, _Falloff);
                fixed4 col = tex2D(_MainTex, i.uv) * appliedCol;
                return col;
            }

            ENDCG
        }
    }
}

Shader "Sandbox/BasicShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SubTex ("Texture 2", 2D) = "white" {}
        
        _Frequency("Frequency", Range(1, 60)) = 1
        _Magnitude("Magnitude", Range(0, 1)) = 0
        _Tween("Tween", Range(0, 1)) = 0
        _Tint ("Tint", Color) = (1, 1, 1, 1)
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
            
            fixed _Frequency;
            fixed _Magnitude;
            fixed4 _Tint;
            fixed _Tween;
            
            fixed luminance (float4 color)
            {
                return .3 * color.r + .59 * color.g + .11 * color.b;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // uv
                //float4 uvCol = float4(i.uv.x, i.uv.y, 0, 1);

                float2 uv = i.uv + float2(1, 1) * _Magnitude * sin(_Frequency * _Time.y);
                //float2 uv = i.uv; // - float2(_Time.x, 0);
                fixed4 col = lerp(tex2D(_MainTex, uv), tex2D(_SubTex, uv), _Tween);

                // Luminance
                fixed lum = luminance(col);
                fixed4 lumCol = float4(lum, lum, lum, col.a);

                return _Tint * col;
            }

            ENDCG
        }
    }
}

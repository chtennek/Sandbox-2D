Shader "Sandbox/MaskShader"
{
    Properties
    {
        _MainTex("Image", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}

        _OffsetTex("Mask Offset", 2D) = "gray" {}
        _OffsetFactor("Offset Multiplier", Range(0, 10)) = 1
        
        _Threshold("Threshold", Range(0, 1)) = 0.5
        _Smoothness("Smoothness", Range(0, 1)) = 0

        _HighPass("High Pass", Color) = (0, 0, 0, 0)
        _Edge("Edge Color", Color) = (0, 0, 0, 0)
        _LowPass("Low Pass", Color) = (0, 0, 0, 1)
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
            sampler2D _MaskTex;
            sampler2D _OffsetTex;
            float4 _MainTex_ST;
            float4 _MaskTex_ST;
            float4 _OffsetTex_ST;
            
            float _OffsetFactor;
            float _Threshold;
            float _Smoothness;

            float4 _HighPass;
            float4 _Edge;
            float4 _LowPass;

            float4 frag (v2f i) : SV_Target
            {
                float4 src = tex2D(_MainTex, (i.uv + _MainTex_ST.zw) * _MainTex_ST.xy);
                float expand = _Smoothness + 0.5 * _OffsetFactor;
                float expandedThreshold = lerp(-expand, 1 + expand, _Threshold);

                // Find if we are above/below the threshold
                float2 uv = i.uv + _MaskTex_ST.zw;
                float offset = _OffsetFactor * (tex2D(_OffsetTex, (uv + _OffsetTex_ST.zw) * _OffsetTex_ST.xy).x - 0.5);
                
                float val = tex2D(_MaskTex, uv * _MaskTex_ST.xy).x + offset - expandedThreshold;
                if (_Smoothness != 0)
                    val = clamp(val / _Smoothness, -1, 1);
                else
                {
                    val = sign(val);
                }

                // [TODO] only alpha blend what we need for performance
                float4 high = float4(0, 0, 0, 1);
                high.rgb = _HighPass.a * _HighPass.rgb + (1 - _HighPass.a) * src.rgb;

                float4 edge = float4(0, 0, 0, 1);
                edge.rgb = _Edge.a * _Edge.rgb + (1 - _Edge.a) * src.rgb;

                float4 low = float4(0, 0, 0, 1);
                low.rgb = _LowPass.a * _LowPass.rgb + (1 - _LowPass.a) * src.rgb;

                if (val == 0)
                    return edge;
                else if (val > 0)
                    return lerp(edge, high, val);
                else
                    return lerp(edge, low, -val);

                float t = (1 + val) / 2;
                float4 col = lerp(low, high, t);
                return col;
            }

            ENDCG
        }
    }
}

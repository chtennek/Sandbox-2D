Shader "Surface/Unlit"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100


        CGPROGRAM
 
		#pragma surface surf NoLighting noforwardadd //addshadow
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
        };

		fixed4 _Color;
 
		 fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
         {
             fixed4 c;
             c.rgb = s.Albedo * 0.5; 
             c.a = s.Alpha;
             return c;
         }

 
        void surf (Input IN, inout SurfaceOutput o)
        {				
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG

	}
	//FallBack "Standard"
}

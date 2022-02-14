﻿Shader "Custom/SpriteHDR"
{
    Properties
    {
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 t = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 c = t * _Color;
            o.Albedo = c.rgb;
            o.Alpha = t.a;
            if(c.a < 0.03f)
                o.Alpha = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

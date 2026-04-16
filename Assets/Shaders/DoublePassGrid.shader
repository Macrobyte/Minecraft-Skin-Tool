Shader "Custom/DoublePassGrid"
{
    Properties
    {
        _Color ("Main Color (A=Opacity)", Color) = (1,1,1,1)
        _MainTex ("Base (A=Opacity)", 2D) = "white" {}
        _GridColor ("Grid Color", Color) = (0,0,0,0)
        _GridSize ("Grid Size", Vector) = (16, 16, 0, 0)
        _GridThickness ("Grid Thickness", Float) = 0.02
    }

    SubShader
    {
        Tags { "Queue"="Geometry"}
        Cull Off
        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _MainTex_ST;
            float4 _Color;
            
            float4 _GridColor;
            float2 _GridSize;
            float _GridThickness;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 color = tex2D(_MainTex, i.uv) * _Color;
                

                // Calculate grid lines based on UV coordinates
                float2 grid = frac(i.uv * _GridSize);
                float2 distToLine = min(grid, 1.0 - grid);
                float gridLine = step(distToLine.x, _GridThickness) + step(distToLine.y, _GridThickness);


                // Combine the texture color with the grid line color
                fixed4 gridLineColor = _GridColor * gridLine;

                // Ensure the grid lines always appear, even over transparency
                fixed4 finalColor = lerp(color, gridLineColor, gridLineColor.a);
                finalColor.a = max(color.a, gridLineColor.a); // Ensure grid lines contribute to alpha

                if (finalColor.a < 0.5) discard;
                
                return finalColor;
            }

            
            ENDCG
        }
    }
}

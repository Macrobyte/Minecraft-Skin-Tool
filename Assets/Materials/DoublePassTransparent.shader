Shader "Custom/DoublePassTransparent"
{
    Properties {
        _Color ("Main Color (A=Opacity)", Color) = (1,1,1,1)
        _MainTex ("Base (A=Opacity)", 2D) = "white" {}
    }

    SubShader {
        // First pass: render front faces with opaque settings
        Tags { "Queue" = "Geometry" }
        Cull Off
        ZWrite On
        Blend One Zero

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 color = tex2D(_MainTex, i.uv) * _Color;
                if (color.a < 0.1) discard; // Adjust the threshold as needed
                return color;
            }
            ENDCG
        }
    }

    SubShader {
        // Second pass: "Hides" the transparency 
        Tags { "Queue" = "Transparent" }
        Cull Front
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 color = tex2D(_MainTex, i.uv) * _Color;
                if (color.a < 0.1) discard; // Adjust the threshold as needed
                return color;
            }
            ENDCG
        }
    }
}
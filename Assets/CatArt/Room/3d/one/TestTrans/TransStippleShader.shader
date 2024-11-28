Shader "Ocias/Diffuse (Stipple Transparency) URP" 
{
    Properties 
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Transparency ("Transparency", Range(0,1)) = 1.0
    }
    SubShader 
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 150

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"

            // sampler2D _MainTex;
            float _Transparency;
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 screenPos : TEXCOORD1;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                o.screenPos = TransformObjectToHClip(v.positionOS).xy / TransformObjectToHClip(v.positionOS).w * _ScreenParams.xy;
                return o;
            }

            float4 frag(Varyings i) : SV_Target
            {
                float4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // Dither matrix for screen-door transparency
                float4x4 thresholdMatrix = float4x4(
                    1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                    13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                     4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                    16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                );

                float2 pos = i.screenPos.xy;
                float2 modPos = fmod(pos, 4.0);
                int2 indices = int2(modPos.x, modPos.y);
                float threshold = thresholdMatrix[indices.x][indices.y];

                if (_Transparency < threshold)
                    discard;

                return float4(c.rgb, c.a);
            }
            ENDHLSL
        }
    }
    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}
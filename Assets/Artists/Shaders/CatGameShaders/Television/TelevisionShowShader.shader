Shader "Custom/Catgame/TelevisionShowShader"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap("Texture", 2D) = "white" {}
        _BaseMap2("Texture2", 2D) = "white" {} //添加第二张贴图,用于显示报错的情况
        //toggle 是否会报错
        [Toggle] _IsError("Is Error?", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BaseMap2);
            SAMPLER(sampler_BaseMap2);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                bool _IsError;
            CBUFFER_END
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                half2 uv = input.uv;
                half4 texColor1 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
                half4 texColor2 = SAMPLE_TEXTURE2D(_BaseMap2, sampler_BaseMap, uv);
                half4 texColor = _IsError ? texColor2 : texColor1;
                half3 color = texColor.rgb * _BaseColor.rgb;
                //随着时间推移，有淡入淡出的效果
                half time = _Time.y;
                half fade = saturate(sin(time - 0.5 * 3.1415926) * 0.5 + 0.5);
                //fade要从零开始
                if(_IsError)
                {
                    fade = 0;
                }
                half3 fadeColor = lerp(color, _BaseColor.rgb, fade);
                return half4(fadeColor, texColor.a);
            }
            ENDHLSL
        }
    }
}

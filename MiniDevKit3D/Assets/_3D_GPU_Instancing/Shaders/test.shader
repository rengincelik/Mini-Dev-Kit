Shader "Custom/URP_GPUInstancingTest"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,0,0,1)
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // StructuredBuffer
            StructuredBuffer<float3> _Positions;

            // Material renk
            float4 _BaseColor;

            struct Attributes
            {
                float3 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings Vert(Attributes IN, uint instanceID : SV_InstanceID)
            {
                Varyings OUT;

                float3 instancePos = _Positions[instanceID];

                float3 worldPos = TransformObjectToWorld(IN.positionOS) + instancePos;
                OUT.positionHCS = TransformWorldToHClip(worldPos);

                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                return _BaseColor;
            }

            ENDHLSL
        }
    }
}


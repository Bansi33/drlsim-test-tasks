Shader "DRL/CustomLitShader"
{
    Properties
    {
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float4 color : TEXCOORD0;
                float3 normal : NORMAL;                
            };

            struct VertexToFragmentData
            {
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD0;  
                float3 ambient : TEXCOORD1;
                float3 diffuse : TEXCOORD2;

                SHADOW_COORDS(3)    
            };

            struct InstancedData
            {
                // Tightly packed data into one vector, with parameters indicating:
                // x, y --> 2D position (3. coordinate is 0)
                // z --> size
                // w --> color ID (0 = red, 1 = green, 2 = blue)
                float4 data;
            };

#if SHADER_TARGET >= 45
            StructuredBuffer<InstancedData> _Data;
#endif

            VertexToFragmentData vert (VertexInput vertexInput, uint instanceID : SV_InstanceID)
            {
                VertexToFragmentData outputData;
#if SHADER_TARGET >= 45
                float4 vertexInstanceData = _Data[instanceID].data;
#else
                float4 vertexInstanceData = 0;
#endif
                float3 localPosition = vertexInput.vertex.xyz * vertexInstanceData.z;
                float3 worldPosition = float3(vertexInstanceData.x, 0, vertexInstanceData.y) + localPosition;

                outputData.vertex = mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0));

                float3 worldNormal = vertexInput.normal;
                float3 ndotl = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
                outputData.ambient = ShadeSH9(float4(worldNormal, 1.0f));
                outputData.diffuse = (ndotl * _LightColor0.rgb);

                float colorId = vertexInstanceData.w;
                outputData.color = float4(0,0,0,1);
                UNITY_BRANCH
                if(colorId == 0){
                    // Red.
                    outputData.color.r = 1;
                }
                else if(colorId == 1){
                    // Green.
                    outputData.color.g = 1;
                }
                else{
                    // Blue.
                    outputData.color.b = 1;
                }

                return outputData;
            }

            fixed4 frag (VertexToFragmentData fragmentInput) : SV_Target
            {
                fixed shadow = SHADOW_ATTENUATION(i);
                fixed4 albedo = fragmentInput.color;
                float3 lighting = fragmentInput.diffuse * shadow + fragmentInput.ambient;
                fixed4 output = fixed4(albedo.rgb * lighting, albedo.w);
                return output;
            }

            ENDCG
        }
    }
}

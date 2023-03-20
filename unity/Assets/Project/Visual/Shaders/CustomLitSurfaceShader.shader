// Custom Lit shader that supports indirect instancing.
Shader "DRL/CustomLitSurfaceShader"
{
    Properties
    {
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow fullforwardshadows
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:setup

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;

        struct InstancedData
        {
            // Tightly packed data into one vector, with parameters indicating:
            // x, y --> 2D position (3. coordinate is 0)
            // z --> size
            // w --> color ID (0 = red, 1 = green, 2 = blue)
            float4 data;

            // Color of the instance.
            float4 color;
        };

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        StructuredBuffer<InstancedData> _Data;
#endif

        void setup()
        {
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            float4 vertexInstanceData = _Data[unity_InstanceID].data;
            float shouldBeVisibleMultiplier = _Data[unity_InstanceID].color.a;
            float size = vertexInstanceData.z * 0.25 * shouldBeVisibleMultiplier;
            float3 worldPosition = float3(vertexInstanceData.x, 0, vertexInstanceData.y);

            unity_ObjectToWorld._11_21_31_41 = float4(size, 0, 0, 0);
            unity_ObjectToWorld._12_22_32_42 = float4(0, size, 0, 0);
            unity_ObjectToWorld._13_23_33_43 = float4(0, 0, size, 0);
            unity_ObjectToWorld._14_24_34_44 = float4(worldPosition, 1);

            unity_WorldToObject = unity_ObjectToWorld;
            unity_WorldToObject._14_24_34 *= -1;
            unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;
#endif
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            float4 fragmentInstanceColor = _Data[unity_InstanceID].color;
#else
            float4 fragmentInstanceColor = 0;
#endif
            o.Albedo = fragmentInstanceColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = fragmentInstanceColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

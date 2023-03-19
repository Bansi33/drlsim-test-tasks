using UnityEngine;

namespace DRL
{
    public static class FunctionVisualizerGPU
    {
        private struct MeshInstanceData
        {
            // Tightly packed data into one vector, with parameters indicating:
            // x, y --> 2D position (3. coordinate is 0)
            // z --> size
            // w --> color ID (0 = red, 1 = green, 2 = blue)
            public Vector4 Data;
        }

        public static void VisualizeData(FunctionVisualizationData functionVisualizationData,
            Mesh mesh, Material material, float startAngleOffset)
        {
            int numberOfInstancesToDraw = Mathf.RoundToInt(functionVisualizationData.Instances.Value);
            if(numberOfInstancesToDraw  == 0)
            {
                return;
            }

            ComputeBuffer drawingArgumentsBuffer = InitializeRenderingArgumentsBuffer(mesh, numberOfInstancesToDraw);
            
            // TODO: Transfer this to compute shader
            MeshInstanceData[] meshInstancesData = CalculateMeshInstancesData(
                numberOfInstances: numberOfInstancesToDraw, 
                startAngleOffset: startAngleOffset,
                constA: functionVisualizationData.ConstA.Value,
                constB: functionVisualizationData.ConstB.Value,
                radius: functionVisualizationData.Radius.Value,
                size: functionVisualizationData.Size.Value);

            ComputeBuffer meshInstancesDataBuffer = new ComputeBuffer(
                count: numberOfInstancesToDraw,
                stride: sizeof(float) * 4);
            meshInstancesDataBuffer.SetData(meshInstancesData);


            material.SetBuffer("_Data", meshInstancesDataBuffer);
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * functionVisualizationData.Radius.Value * 200);

            Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, drawingArgumentsBuffer);

            //meshInstancesDataBuffer.Release();
            //drawingArgumentsBuffer.Release();
        }

        private static ComputeBuffer InitializeRenderingArgumentsBuffer(Mesh mesh, int numberOfInstancesToDraw)
        {
            uint[] drawingArguments = new uint[]
            {
                (uint)mesh.GetIndexCount(0),
                (uint)numberOfInstancesToDraw,
                (uint)mesh.GetIndexStart(0),
                (uint)mesh.GetBaseVertex(0),
                (uint)0
            };

            ComputeBuffer drawingArgumentsBuffer = new ComputeBuffer(
                count: 1,
                stride: drawingArguments.Length * sizeof(uint),
                type: ComputeBufferType.IndirectArguments);
            drawingArgumentsBuffer.SetData(drawingArguments);

            return drawingArgumentsBuffer;
        }
    
        private static MeshInstanceData[] CalculateMeshInstancesData(int numberOfInstances, float startAngleOffset,
            float constA, float constB, float radius, float size)
        {
            float angleDelta = 360f / numberOfInstances;
            if (constA % 2 != 0 && constB % 2 != 0)
            {
                angleDelta /= 2;
            }

            MeshInstanceData[] meshInstancesData = new MeshInstanceData[numberOfInstances];
            for (int i = 0; i < numberOfInstances; i++)
            {
                float angleInRadians = Mathf.Deg2Rad * (angleDelta * i + startAngleOffset);
                Vector2 polarCoordinates = CalculatePolarCoordinates(angleInRadians, constA, constB);
                Vector3 cartesianCoordinates = ConvertPolarCoordinatesToCartesian(polarCoordinates);

                MeshInstanceData meshInstanceData = new MeshInstanceData();
                meshInstanceData.Data = new Vector4(cartesianCoordinates.x * radius, cartesianCoordinates.z * radius, size, 1);

                meshInstancesData[i] = meshInstanceData;
            }
            return meshInstancesData;
        }

        private static Vector2 CalculatePolarCoordinates(float angleInRadians, float constA, float constB)
        {
            return new Vector2(Mathf.Sin(angleInRadians * constA) + Mathf.Cos(angleInRadians * constB), angleInRadians);
        }

        private static Vector3 ConvertPolarCoordinatesToCartesian(Vector2 polarCoordinates)
        {
            return new Vector3(polarCoordinates.x * Mathf.Cos(polarCoordinates.y), 0, polarCoordinates.x * Mathf.Sin(polarCoordinates.y));
        }
    }
}

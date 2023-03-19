using System;
using UnityEngine;

namespace DRL
{
    public static class FunctionVisualizerCPU
    {
        private const int MAX_INSTANCES_PER_BATCH = 1023;
        private const float MAX_ANGLE = 360f;

        public static void VisualizeData(FunctionVisualizationData functionVisualizationData, Mesh mesh, 
            Material material, float startAngleOffset)
        {
            
            int numberOfInstancesToDraw = Mathf.RoundToInt(functionVisualizationData.Instances.Value);
            Matrix4x4[] matrices = CalculateFunctionTransformationMatrices(
                numberOfMatrices: numberOfInstancesToDraw,
                startAngleOffset: startAngleOffset,
                constA: functionVisualizationData.ConstA.Value,
                constB: functionVisualizationData.ConstB.Value,
                radius: functionVisualizationData.Radius.Value,
                size: functionVisualizationData.Size.Value);

            switch (functionVisualizationData.Mode)
            {
                case FunctionVisualizationType.DrawMesh:
                    DrawMeshesDirectly(mesh, material, matrices);
                    break;
                case FunctionVisualizationType.DrawMeshInstanced:
                    DrawMeshesInBatches(mesh, material, matrices);
                    break;
                default:
                    Debug.LogError($"No visualization function implemented for the provided {functionVisualizationData.Mode}.");
                    break;
            }
        }

        private static Matrix4x4[] CalculateFunctionTransformationMatrices(int numberOfMatrices, float startAngleOffset, 
            float constA, float constB, float radius, float size)
        {
            if(numberOfMatrices == 0)
            {
                return new Matrix4x4[0];
            }

            float angleDelta = MAX_ANGLE / numberOfMatrices;
            if (constA % 2 != 0 && constB % 2 != 0)
            {
                angleDelta /= 2;
            }

            Matrix4x4[] positionMatrices = new Matrix4x4[numberOfMatrices];
            for (int i = 0; i < numberOfMatrices; i++)
            {
                float angleInRadians = Mathf.Deg2Rad * (angleDelta * i + startAngleOffset);
                Vector2 polarCoordinates = CalculatePolarCoordinates(angleInRadians, constA, constB);
                Vector3 cartesianCoordinates = ConvertPolarCoordinatesToCartesian(polarCoordinates);

                positionMatrices[i] = Matrix4x4.TRS(
                    cartesianCoordinates * radius,
                    Quaternion.identity,
                    Vector3.one * size);
            }

            return positionMatrices;
        }

        private static void DrawMeshesDirectly(Mesh mesh, Material material, Matrix4x4[] matrices)
        {
            int numberOfInstancesToDraw = matrices.Length;
            for (int i = 0; i < numberOfInstancesToDraw; i++)
            {
                Graphics.DrawMesh(mesh, matrices[i], material, 0);
            }
        }

        private static void DrawMeshesInBatches(Mesh mesh, Material material, Matrix4x4[] matrices)
        {
            int numberOfInstancesToDraw = matrices.Length;
            int numberOfDrawnInstances = 0;

            while (numberOfDrawnInstances < numberOfInstancesToDraw)
            {
                int batchSize = Mathf.Min(numberOfInstancesToDraw - numberOfDrawnInstances, MAX_INSTANCES_PER_BATCH);
                
                Matrix4x4[] batchMatrices = new Matrix4x4[batchSize];
                Array.Copy(matrices, numberOfDrawnInstances, batchMatrices, 0, batchSize);
                Graphics.DrawMeshInstanced(mesh, 0, material, batchMatrices);

                numberOfDrawnInstances += batchSize;
            }
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

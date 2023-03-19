using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Abstract class that unifies functionalities for the function visualizers that calculate
    /// discrete positions on the CPU and propagate the data to the GPU for rendering.
    /// </summary>
    public abstract class FunctionVisualizerCPU : FunctionVisualizerBase
    {
        private const float MAX_ANGLE = 360f;

        [Header("References: ")]
        [SerializeField] protected Material _material = null;

        /// <summary>
        /// Function draws the provided <paramref name="mesh"/> on the world positions
        /// specified by the <paramref name="transformationMatrices"/>.
        /// </summary>
        /// <param name="mesh">Reference to the <see cref="Mesh"/> asset that will be instantiated
        /// on the specified world positions.</param>
        /// <param name="transformationMatrices">Collection of <see cref="Matrix4x4"/> structures
        /// that contain world positions, rotations and sizes of the meshes that will be instantiated.</param>
        protected abstract void DrawMeshes(Mesh mesh, Matrix4x4[] transformationMatrices);

        /// <inheritdoc/>
        protected override void VisualizeFunction(FunctionVisualizationData functionVisualizationData, Mesh mesh)
        {
            Matrix4x4[] matrices = CalculateFunctionTransformationMatrices(
                numberOfMatrices: _numberOfInstancesToDraw,
                startAngleOffset: _startOffsetAngle,
                constA: functionVisualizationData.ConstA.Value,
                constB: functionVisualizationData.ConstB.Value,
                radius: functionVisualizationData.Radius.Value,
                size: functionVisualizationData.Size.Value);

            DrawMeshes(mesh, matrices);
        }

        private Matrix4x4[] CalculateFunctionTransformationMatrices(int numberOfMatrices, float startAngleOffset,
            float constA, float constB, float radius, float size)
        {
            if (numberOfMatrices == 0)
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
                positionMatrices[i] = GetTransformationMatrix(angleInRadians, constA, constB, size, radius);
            }

            return positionMatrices;
        }

        private Matrix4x4 GetTransformationMatrix(float angleInRadians, float constA, float constB, float size, float radius)
        {
            Vector2 polarCoordinates = CalculatePolarCoordinates(angleInRadians, constA, constB);
            Vector3 cartesianCoordinates = ConvertPolarCoordinatesToCartesian(polarCoordinates);

            return Matrix4x4.TRS(
                   pos: cartesianCoordinates * radius,
                   q: Quaternion.identity,
                   s: Vector3.one * size);
        }

        private Vector2 CalculatePolarCoordinates(float angleInRadians, float constA, float constB)
        {
            return new Vector2(Mathf.Sin(angleInRadians * constA) + Mathf.Cos(angleInRadians * constB), angleInRadians);
        }

        private Vector3 ConvertPolarCoordinatesToCartesian(Vector2 polarCoordinates)
        {
            return new Vector3(polarCoordinates.x * Mathf.Cos(polarCoordinates.y), 0, polarCoordinates.x * Mathf.Sin(polarCoordinates.y));
        }
    }
}

using System;
using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Specific CPU Function Visualizer that displays the meshes by batching multiple
    /// draw calls together and then dispatching them to the GPU for rendering.
    /// </summary>
    public class FunctionVisualizerCPU_BatchedDraw : FunctionVisualizerCPU
    {
        private const int MAX_INSTANCES_PER_BATCH = 1023;

        /// <inheritdoc/>
        public override void CleanUp()
        {
        }

        /// <inheritdoc/>
        public override FunctionVisualizationType GetVisualizationType()
        {
            return FunctionVisualizationType.DrawMeshInstanced;
        }

        /// <inheritdoc/>
        protected override void DrawMeshes(Mesh mesh, Matrix4x4[] matrices)
        {
            int numberOfInstancesToDraw = matrices.Length;
            int numberOfDrawnInstances = 0;

            while (numberOfDrawnInstances < numberOfInstancesToDraw)
            {
                int batchSize = Mathf.Min(numberOfInstancesToDraw - numberOfDrawnInstances, MAX_INSTANCES_PER_BATCH);

                Matrix4x4[] batchMatrices = new Matrix4x4[batchSize];
                Array.Copy(matrices, numberOfDrawnInstances, batchMatrices, 0, batchSize);
                Graphics.DrawMeshInstanced(mesh, 0, _material, batchMatrices);

                numberOfDrawnInstances += batchSize;
            }
        }
    }
}

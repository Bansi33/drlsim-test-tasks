using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Specific CPU Function Visualizer that displays the meshes by directly executing
    /// draw orders to the GPU for each of mesh.
    /// </summary>
    public class FunctionVisualizerCPU_DirectDraw : FunctionVisualizerCPU
    {
        /// <inheritdoc/>
        public override void CleanUp()
        {
        }

        /// <inheritdoc/>
        public override FunctionVisualizationType GetVisualizationType()
        {
            return FunctionVisualizationType.DrawMesh;
        }

        /// <inheritdoc/>
        protected override void DrawMeshes(Mesh mesh, Matrix4x4[] matrices)
        {
            int numberOfInstancesToDraw = matrices.Length;
            for (int i = 0; i < numberOfInstancesToDraw; i++)
            {
                Graphics.DrawMesh(mesh, matrices[i], _material, 0);
            }
        }
    }
}

using UnityEngine;

namespace DRL
{
    public class FunctionVisualizer : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private Mesh _mesh = null;
        [SerializeField] private Material _material = null;
        [SerializeField] private Material _materialForIndirectDrawing = null;

        private float _currentStartAngleOffset = 0f;

        public void VisualizeData(FunctionVisualizationData functionVisualizationData)
        {
            if(functionVisualizationData == null)
            {
                Debug.LogError($"Can't visualize the function since the {nameof(FunctionVisualizationData)} is null.", gameObject);
                return;
            }

            int numberOfInstancesToDraw = Mathf.RoundToInt(functionVisualizationData.Instances.Value);
            if (numberOfInstancesToDraw == 0)
            {
                return;
            }

            switch (functionVisualizationData.Mode)
            {                
                case FunctionVisualizationType.DrawMesh:
                case FunctionVisualizationType.DrawMeshInstanced:
                    // Both DrawMesh and DrawMeshInstanced functions run their positions calculations
                    // on CPU and only issue draw calls to the GPU.
                    FunctionVisualizerCPU.VisualizeData(functionVisualizationData, _mesh, _material, _currentStartAngleOffset);
                    break;
                case FunctionVisualizationType.DrawMeshInstancedIndirect:
                    // Indirect drawing provides ability to do both calculation and rendering on the GPU.
                    FunctionVisualizerGPU.VisualizeData(functionVisualizationData, _mesh, _materialForIndirectDrawing, _currentStartAngleOffset);
                    break;
                default:
                    Debug.LogError($"No visualization function implemented for the provided {functionVisualizationData.Mode}.", gameObject);
                    break;
            }

            _currentStartAngleOffset += functionVisualizationData.Speed.Value * Time.deltaTime;
        }
    }
}

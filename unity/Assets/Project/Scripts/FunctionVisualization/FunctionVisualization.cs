using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Class that contains reference to all currently supported function visualizers and triggers
    /// the function visualization process on the desired one.
    /// </summary>
    public class FunctionVisualization : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private Mesh _mesh = null;
        [SerializeField] private FunctionVisualizerBase[] _functionVisualizers = null;

        private FunctionVisualizerBase _currentFunctionVisualizer = null;

        private void OnDisable()
        {
            _currentFunctionVisualizer?.CleanUp();
        }

        /// <summary>
        /// The method visualizes the polar coordinates function by spawning the <see cref="_mesh"/> asset along the discrete sample
        /// points based on the provided <paramref name="functionVisualizationData"/>.
        /// </summary>
        /// <param name="functionVisualizationData">Instance of the <see cref="FunctionVisualizationData"/> that holds the 
        /// parameters required for successfully plotting the function.</param>
        public void VisualizeData(FunctionVisualizationData functionVisualizationData)
        {
            if (_functionVisualizers == null || _functionVisualizers.Length == 0)
            {
                Debug.LogError($"Can't visualize function since no specific function visualizers have been assigned!", gameObject);
                return;
            }

            if (functionVisualizationData == null)
            {
                Debug.LogError($"Can't visualize the function since the {nameof(FunctionVisualizationData)} is null.", gameObject);
                return;
            }

            int numberOfInstancesToDraw = Mathf.RoundToInt(functionVisualizationData.Instances.Value);
            if (numberOfInstancesToDraw == 0)
            {
                return;
            }

            FunctionVisualizerBase desiredFunctionVisualizer = GetSpecificFunctionVisualizer(functionVisualizationData.Mode);
            if(_currentFunctionVisualizer != null && _currentFunctionVisualizer != desiredFunctionVisualizer)
            {
                _currentFunctionVisualizer.CleanUp();                
            }

            _currentFunctionVisualizer = desiredFunctionVisualizer;
            _currentFunctionVisualizer?.VisualizeFunctionData(functionVisualizationData, _mesh);
        }

        private FunctionVisualizerBase GetSpecificFunctionVisualizer(FunctionVisualizationType functionVisualizationType)
        {
            foreach(FunctionVisualizerBase functionVisualizer in _functionVisualizers)
            {
                if(functionVisualizer.GetVisualizationType() == functionVisualizationType)
                {
                    return functionVisualizer;
                }
            }

            Debug.LogError($"No specific function visualizer found for the provided {functionVisualizationType}.", gameObject);
            return null;
        }
    }
}

using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Abstract class for all specific function visualizers. Contains public method for visualization 
    /// of the function and implements starting angle offset calculation.
    /// </summary>
    public abstract class FunctionVisualizerBase : MonoBehaviour
    {
        /// <summary>
        /// Starting angle offset that needs to be applied to the function plotting. Used to simulate
        /// the movement of the plotted points over time.
        /// </summary>
        protected float _startOffsetAngle = 0f;
        /// <summary>
        /// How many points will need to be plotted by instantiating objects along the function.
        /// </summary>
        protected int _numberOfInstancesToDraw = 0;

        /// <summary>
        /// Function cleans up leftover references and memory allocations, after the visualization has been completed.
        /// </summary>
        public abstract void CleanUp();

        /// <summary>
        /// Function returns the <see cref="FunctionVisualizationType"/> that the visualizer implements.
        /// </summary>
        /// <returns><see cref="FunctionVisualizationType"/> that the visualizer implements.</returns>
        public abstract FunctionVisualizationType GetVisualizationType();

        /// <summary>
        /// Abstract function that every specific function visualizer needs to implement. The method should
        /// visualize the polar coordinates function by spawning the provided <paramref name="mesh"/> along the discrete sample
        /// points. Use the provided <paramref name="functionVisualizationData"/> for the calculations.
        /// </summary>
        /// <param name="functionVisualizationData">Instance of the <see cref="FunctionVisualizationData"/> that holds the 
        /// parameters required for successfully plotting the function.</param>
        /// <param name="mesh">Reference to the <see cref="Mesh"/> asset that will need to be instantiated on the
        /// discrete points of the function.</param>
        protected abstract void VisualizeFunction(FunctionVisualizationData functionVisualizationData, Mesh mesh);

        /// <summary>
        /// Function visualizes the specific function in the 3D space by instantiating the provided <paramref name="mesh"/>.
        /// The discrete points where the mesh will spawn are gained by applying calculations based on the provided 
        /// <paramref name="functionVisualizationData"/> parameters.
        /// </summary>
        /// <param name="functionVisualizationData">Instance of the <see cref="FunctionVisualizationData"/> that holds the 
        /// parameters required for successfully plotting the function.</param>
        /// <param name="mesh">Reference to the <see cref="Mesh"/> asset that will need to be instantiated on the
        /// discrete points of the function.</param>
        public virtual void VisualizeFunctionData(FunctionVisualizationData functionVisualizationData, Mesh mesh)
        {
            if (functionVisualizationData == null)
            {
                Debug.LogError($"Can't visualize the function since the {nameof(FunctionVisualizationData)} is null.", gameObject);
                return;
            }

            _numberOfInstancesToDraw = Mathf.RoundToInt(functionVisualizationData.Instances.Value);
            if (_numberOfInstancesToDraw == 0)
            {
                return;
            }

            VisualizeFunction(functionVisualizationData, mesh);
            _startOffsetAngle += functionVisualizationData.Speed.Value * Time.deltaTime;
        }
    }
}

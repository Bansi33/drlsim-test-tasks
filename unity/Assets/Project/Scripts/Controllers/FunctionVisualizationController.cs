using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Class that controls the visualization of the polar coordinates function defined by: 
    /// r = Sin(angle * A) + Cos(angle * B). The visualization is requested every frame.
    /// </summary>
    public class FunctionVisualizationController : MonoBehaviour
    {
        [Header("References: ")]
        [Tooltip("Reference to the class that actually implements the visualization method.")]
        [SerializeField] private FunctionVisualization _functionVisualizer = null;
        [Tooltip("Reference to the view that will display the controls that affect function visualization.")]
        [SerializeField] private ControlsView _controlsView = null;

        [Header("Options: ")]
        [Tooltip("Class containing current properties for the function visualization.")]
        [SerializeField] private FunctionVisualizationData _functionVisualizationData = null;

        private void Awake()
        {
            // Request displaying function visualization properties on the view and subscribe to value changes.
            DisplayConstrainedValue(_functionVisualizationData.Instances, nameof(_functionVisualizationData.Instances));
            DisplayConstrainedValue(_functionVisualizationData.Speed, nameof(_functionVisualizationData.Speed));
            DisplayConstrainedValue(_functionVisualizationData.Size, nameof(_functionVisualizationData.Size));
            DisplayConstrainedValue(_functionVisualizationData.Radius, nameof(_functionVisualizationData.Radius));
            DisplayConstrainedValue(_functionVisualizationData.ConstA, nameof(_functionVisualizationData.ConstA));
            DisplayConstrainedValue(_functionVisualizationData.ConstB, nameof(_functionVisualizationData.ConstB));

            // Display input field for setting the color pattern.
            _controlsView.DisplayLabeledInputField(nameof(_functionVisualizationData.Pattern), _functionVisualizationData.Pattern,
                (value) => { _functionVisualizationData.Pattern = value; });
        }

        private void Update()
        {
            _functionVisualizer.VisualizeData(_functionVisualizationData);
        }

        /// <summary>
        /// Function requests the display of the provided <paramref name="constrainedValue"/>
        /// on the controls view.
        /// </summary>
        /// <param name="constrainedValue">Constrained value whose contents will be displayed.</param>
        /// <param name="label">Label that will be displayed alongside the constrained value on the view.</param>
        private void DisplayConstrainedValue(ConstrainedValue constrainedValue, string label)
        {
            _controlsView.DisplayLabeledConstrainedValue(
                label: label,
                minValue: constrainedValue.MinValue,
                maxValue: constrainedValue.MaxValue,
                initialValue: constrainedValue.Value,
                wholeNumbersOnly: constrainedValue.WholeNumbersOnly,
                onValueUpdated: (value) => { constrainedValue.Value = value; });
        }
    }
}
using UnityEngine;

namespace DRL
{
    public class FunctionVisualizationController : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private FunctionVisualization _functionVisualizer = null;
        [SerializeField] private ControlsView _controlsView = null;

        [Header("Options: ")]
        [SerializeField] private FunctionVisualizationData _functionVisualizationData = null;

        private void Start()
        {
            DisplayConstrainedValue(_functionVisualizationData.Instances, nameof(_functionVisualizationData.Instances));
            DisplayConstrainedValue(_functionVisualizationData.Speed, nameof(_functionVisualizationData.Speed));
            DisplayConstrainedValue(_functionVisualizationData.Size, nameof(_functionVisualizationData.Size));
            DisplayConstrainedValue(_functionVisualizationData.Radius, nameof(_functionVisualizationData.Radius));
            DisplayConstrainedValue(_functionVisualizationData.ConstA, nameof(_functionVisualizationData.ConstA));
            DisplayConstrainedValue(_functionVisualizationData.ConstB, nameof(_functionVisualizationData.ConstB));

            _controlsView.DisplayLabeledInputField(nameof(_functionVisualizationData.Pattern), _functionVisualizationData.Pattern,
                (value) => { _functionVisualizationData.Pattern = value; });
        }

        private void Update()
        {
            _functionVisualizer.VisualizeData(_functionVisualizationData);
        }

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
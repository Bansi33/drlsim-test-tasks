using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Controller for setting the application frame rate and V-sync.
    /// </summary>
    public class ApplicationController : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private ControlsView _controlsView = null;

        [Header("Options: ")]
        [Tooltip("Constrained value specifying the desired Application frame rate.")]
        [SerializeField] private ConstrainedValue _targetFrameRate = default;

        private void Start()
        {
            // Set the application frame rate to the initial conditions.
            UpdateTargetFPS(_targetFrameRate.Value);

            // Ask the controls view to display the slider for changing the desired
            // application frame rate and subscribe to changes in value.
            _controlsView.DisplayLabeledConstrainedValue(
                label: "Max FPS",
                minValue: _targetFrameRate.MinValue,
                maxValue: _targetFrameRate.MaxValue,
                initialValue: _targetFrameRate.Value,
                wholeNumbersOnly: _targetFrameRate.WholeNumbersOnly,
                onValueUpdated: UpdateTargetFPS);
        }

        /// <summary>
        /// Function updates the application target frame rate to the provided 
        /// <paramref name="targetFPS"/> value. Additionally, if the value exceeds the maximum
        /// refresh rate of the display device, the V-sync is disabled.
        /// </summary>
        /// <param name="targetFPS">Desired frame rate of the application.</param>
        private void UpdateTargetFPS(float targetFPS)
        {
            _targetFrameRate.Value = targetFPS;
            Application.targetFrameRate = Mathf.RoundToInt(_targetFrameRate.Value);

            // Disable V-sync if the desired FPS is larger than the screen refresh rate.
            QualitySettings.vSyncCount = _targetFrameRate.Value > Screen.currentResolution.refreshRate ? 0 : 1;
        }
    }
}
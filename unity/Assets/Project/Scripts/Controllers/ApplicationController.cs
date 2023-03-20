using UnityEngine;

namespace DRL
{
    public class ApplicationController : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private ControlsView _controlsView = null;

        [Header("Options: ")]
        [SerializeField] private ConstrainedValue _targetFrameRate = default;

        private void Start()
        {
            UpdateTargetFPS(_targetFrameRate.Value);

            _controlsView.DisplayLabeledConstrainedValue(
                label: "Max FPS",
                minValue: _targetFrameRate.MinValue,
                maxValue: _targetFrameRate.MaxValue,
                initialValue: _targetFrameRate.Value,
                wholeNumbersOnly: _targetFrameRate.WholeNumbersOnly,
                onValueUpdated: UpdateTargetFPS);
        }

        private void UpdateTargetFPS(float targetFPS)
        {
            _targetFrameRate.Value = targetFPS;
            Application.targetFrameRate = Mathf.RoundToInt(_targetFrameRate.Value);

            // Disable V-sync if the desired FPS is larger than the screen refresh rate.
            QualitySettings.vSyncCount = _targetFrameRate.Value > Screen.currentResolution.refreshRate ? 0 : 1;
        }
    }
}
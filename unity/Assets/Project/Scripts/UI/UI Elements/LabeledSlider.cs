using System;
using UnityEngine;
using UnityEngine.UI;

namespace DRL
{
    /// <summary>
    /// Class representing an UI slider with a label. It provides a callback function
    /// that gets invoked whenever the value of the slider changes.
    /// </summary>
    public class LabeledSlider : LabeledUIElement
    {
        [Header("References: ")]
        [SerializeField] private Slider _slider = null;

        /// <summary>
        /// Callback function invoked whenever the value changes on the slider,
        /// providing the value currently assigned to the slider.
        /// </summary>
        public Action<float> OnSliderValueChanged = null;

        private void OnEnable()
        {
            if (_slider != null)
            {
                _slider.onValueChanged.AddListener((value) => OnSliderValueChanged?.Invoke(value));
            }
        }

        private void OnDisable()
        {
            if (_slider != null)
            {
                _slider.onValueChanged.RemoveAllListeners();
            }
        }

        /// <summary>
        /// Function initializes the slider based on the provided arguments.
        /// </summary>
        /// <param name="minValue">Minimal value that the slider can have.</param>
        /// <param name="maxValue">Maximum value that the slider can have.</param>
        /// <param name="initialValue">The initial value that the slider will have.</param>
        /// <param name="wholeValuesOnly">Should the slider only display whole values or not.</param>
        public void InitializeSlider(float minValue, float maxValue, float initialValue, bool wholeValuesOnly)
        {
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            _slider.wholeNumbers = wholeValuesOnly;

            _slider.SetValueWithoutNotify(initialValue);            
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;

namespace DRL
{
    public class LabeledSlider : LabeledUIElement
    {
        [Header("References: ")]
        [SerializeField] private Slider _slider = null;

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

        public void InitializeSlider(float minValue, float maxValue, float initialValue, bool wholeValuesOnly)
        {
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            _slider.wholeNumbers = wholeValuesOnly;

            _slider.SetValueWithoutNotify(initialValue);            
        }
    }
}

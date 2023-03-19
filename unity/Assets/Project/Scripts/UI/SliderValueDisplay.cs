using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DRL
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SliderValueDisplay : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private Slider _slider = null;

        private TextMeshProUGUI _valueText = null;

        private TextMeshProUGUI ValueText
        {
            get
            {
                if (_valueText == null)
                {
                    _valueText = GetComponent<TextMeshProUGUI>();
                }
                return _valueText;
            }
        }

        private void OnEnable()
        {
            if(_slider != null)
            {
                _slider.onValueChanged.AddListener(UpdateText);
            }
        }

        private void Start()
        {
            UpdateText(_slider.value);
        }

        private void OnDisable()
        {
            if (_slider != null)
            {
                _slider.onValueChanged.RemoveListener(UpdateText);
            }
        }

        private void UpdateText(float sliderValue)
        {
            ValueText.text = _slider.wholeNumbers ? sliderValue.ToString("0.") : sliderValue.ToString("0.0");
        }
    }
}

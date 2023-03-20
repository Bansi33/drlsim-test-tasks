using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DRL
{
    /// <summary>
    /// Class that displays the value of the slider as a text. Must be assigned to the 
    /// object containing the <see cref="TextMeshProUGUI"/> component.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SliderValueDisplay : MonoBehaviour
    {
        private readonly string WHOLE_NUMBERS_TEXT_FORMAT = "0.";
        private readonly string DECIMAL_NUMBERS_TEXT_FORMAT = "0.0";

        [Header("References: ")]
        [SerializeField] private Slider _slider = null;

        
        private TextMeshProUGUI _valueText = null;
        /// <summary>
        /// Reference to the <see cref="TextMeshProUGUI"/> component that is displaying
        /// the value of the <see cref="_slider"/> as a text.
        /// </summary>
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

        /// <summary>
        /// Function updates the textual value based on the provided <paramref name="sliderValue"/>.
        /// </summary>
        /// <param name="sliderValue">Current value of the slider that needs to be displayed as a text.</param>
        private void UpdateText(float sliderValue)
        {
            ValueText.text = _slider.wholeNumbers ? 
                sliderValue.ToString(WHOLE_NUMBERS_TEXT_FORMAT) : 
                sliderValue.ToString(DECIMAL_NUMBERS_TEXT_FORMAT);
        }
    }
}

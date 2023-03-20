using System;
using UnityEngine;

namespace DRL
{
    /// <summary>
    /// View class representing controls that affect the visualization of the function.
    /// </summary>
    public class ControlsView : MonoBehaviour
    {
        [Header("References:")]
        [Tooltip("Transform under which the UI elements will be instantiated.")]
        [SerializeField] private Transform _controlsHolderTransform = null;
        [Tooltip("Prefab of the labeled UI slider.")]
        [SerializeField] private GameObject _labeledSliderPrefab = null;
        [Tooltip("Prefab of the labeled UI input field.")]
        [SerializeField] private GameObject _labeledInputFieldPrefab = null;

        private void Awake()
        {
            // Check if all prefabs have been correctly assigned.
            if (_controlsHolderTransform == null)
            {
                Debug.LogError($"Controls holder transform reference hasn't been assigned!", gameObject);
            }

            if (_labeledSliderPrefab == null)
            {
                Debug.LogError($"Labeled slider prefab reference hasn't been assigned!", gameObject);
            }

            if (_labeledInputFieldPrefab == null)
            {
                Debug.LogError($"Labeled input field prefab reference hasn't been assigned!", gameObject);
            }
        }

        /// <summary>
        /// Function creates a slider that represent the value ranged provided via the <paramref name="minValue"/> and
        /// <paramref name="maxValue"/> parameters and initializes it to the <paramref name="initialValue"/>.
        /// Whenever the slider's value changes, the <paramref name="onValueUpdated"/> callback function will be invoked.
        /// </summary>
        /// <param name="label">Label that will be displayed next to the slider.</param>
        /// <param name="minValue">Minimum value that the slider can obtain.</param>
        /// <param name="maxValue">Maximum value that the slider can obtain.</param>
        /// <param name="initialValue">Initial value that will be assigned to the slider.</param>
        /// <param name="wholeNumbersOnly">Should the slider display only whole numbers or decimal numbers.</param>
        /// <param name="onValueUpdated">Callback function that will be invoked whenever slider value changes.</param>
        public void DisplayLabeledConstrainedValue(string label, float minValue, float maxValue, float initialValue, bool wholeNumbersOnly, Action<float> onValueUpdated)
        {
            if(_controlsHolderTransform == null || _labeledSliderPrefab == null)
            {
                Debug.LogError($"Can't display labeled slider since not all components have been assigned!", gameObject);
                return;
            }

            // Instantiate labeled slider prefab.
            GameObject labeledSliderHolder = Instantiate(_labeledSliderPrefab, _controlsHolderTransform);
            LabeledSlider labeledSlider = labeledSliderHolder.GetComponent<LabeledSlider>();

            if(labeledSlider == null)
            {
                Debug.LogError($"Labeled slider prefab needs to have a {nameof(LabeledSlider)} component assigned!", gameObject);
                Destroy(labeledSliderHolder);
                return;
            }

            // Set the label and slider properties.
            labeledSlider.SetLabel(label);
            labeledSlider.InitializeSlider(minValue, maxValue, initialValue, wholeNumbersOnly);

            // Whenever slider's value changes, make sure to invoke the callback function.
            labeledSlider.OnSliderValueChanged += onValueUpdated;
        }

        /// <summary>
        /// Function display an input field with a label and ensures that the <paramref name="onValueUpdated"/> function
        /// gets invoked whenever value changes on the input field. Additionally, it sets the input field to the 
        /// <paramref name="initialValue"/> after instantiation.
        /// </summary>
        /// <param name="label">The label of the input field.</param>
        /// <param name="initialValue">Initial value that will be assigned to the input field.</param>
        /// <param name="onValueUpdated">Callback function that will be invoked whenever input field value changes.</param>
        public void DisplayLabeledInputField(string label, string initialValue, Action<string> onValueUpdated)
        {
            if (_controlsHolderTransform == null || _labeledInputFieldPrefab == null)
            {
                Debug.LogError($"Can't display labeled input field since not all components have been assigned!", gameObject);
                return;
            }

            // Instantiate labeled input slider prefab.
            GameObject labeledInputFieldHolder = Instantiate(_labeledInputFieldPrefab, _controlsHolderTransform);
            LabeledInputField labeledInputField = labeledInputFieldHolder.GetComponent<LabeledInputField>();

            if (labeledInputField == null)
            {
                Debug.LogError($"Labeled input field prefab needs to have a {nameof(LabeledInputField)} component assigned!", gameObject);
                Destroy(labeledInputFieldHolder);
                return;
            }

            // Set the label and input field initial value.
            labeledInputField.SetLabel(label);
            labeledInputField.InitializeInputField(initialValue);

            // Whenever input field's value changes, make sure to invoke the callback function.
            labeledInputField.OnInputValueChanged += onValueUpdated;
        }
    }
}
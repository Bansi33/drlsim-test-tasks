using System;
using UnityEngine;

namespace DRL
{
    public class ControlsView : MonoBehaviour
    {
        [Header("References:")]
        [SerializeField] private Transform _controlsHolderTransform = null;
        [SerializeField] private GameObject _labeledSliderPrefab = null;
        [SerializeField] private GameObject _labeledInputFieldPrefab = null;

        private void Awake()
        {
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

        public void DisplayLabeledConstrainedValue(string label, float minValue, float maxValue, float initialValue, bool wholeNumbersOnly, Action<float> onValueUpdated)
        {
            if(_controlsHolderTransform == null || _labeledSliderPrefab == null)
            {
                return;
            }

            GameObject labeledSliderHolder = Instantiate(_labeledSliderPrefab, _controlsHolderTransform);
            LabeledSlider labeledSlider = labeledSliderHolder.GetComponent<LabeledSlider>();

            if(labeledSlider == null)
            {
                Debug.LogError($"Labeled slider prefab needs to have a {nameof(LabeledSlider)} component assigned!", gameObject);
                Destroy(labeledSliderHolder);
                return;
            }

            labeledSlider.SetLabel(label);
            labeledSlider.InitializeSlider(minValue, maxValue, initialValue, wholeNumbersOnly);
            labeledSlider.OnSliderValueChanged += onValueUpdated;
        }

        public void DisplayLabeledInputField(string label, string initialValue, Action<string> onValueUpdated)
        {
            if (_controlsHolderTransform == null || _labeledInputFieldPrefab == null)
            {
                return;
            }

            GameObject labeledInputFieldHolder = Instantiate(_labeledInputFieldPrefab, _controlsHolderTransform);
            LabeledInputField labeledInputField = labeledInputFieldHolder.GetComponent<LabeledInputField>();

            if (labeledInputField == null)
            {
                Debug.LogError($"Labeled input field prefab needs to have a {nameof(LabeledInputField)} component assigned!", gameObject);
                Destroy(labeledInputFieldHolder);
                return;
            }

            labeledInputField.SetLabel(label);
            labeledInputField.InitializeInputField(initialValue);
            labeledInputField.OnInputValueChanged += onValueUpdated;
        }
    }
}
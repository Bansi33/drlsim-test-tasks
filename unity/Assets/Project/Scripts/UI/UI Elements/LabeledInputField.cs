using System;
using TMPro;
using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Class representing a labeled input field that provides the callback whenever the 
    /// input value changes.
    /// </summary>
    public class LabeledInputField : LabeledUIElement
    {
        [Header("References: ")]
        [SerializeField] private TMP_InputField _inputField = null;

        /// <summary>
        /// Callback function invoked whenever the value changes on the input field,
        /// providing the string currently assigned to the input field.
        /// </summary>
        public Action<string> OnInputValueChanged = null;

        private void OnEnable()
        {
            if (_inputField != null)
            {
                _inputField.onValueChanged.AddListener((value) => OnInputValueChanged?.Invoke(value));
            }
        }

        private void OnDisable()
        {
            if (_inputField != null)
            {
                _inputField.onValueChanged.RemoveAllListeners();
            }
        }

        /// <summary>
        /// Function initializes the <see cref="_inputField"/> to the provided
        /// <paramref name="initialValue"/>.
        /// </summary>
        /// <param name="initialValue">The initial string that will be applied to the input field.</param>
        public void InitializeInputField(string initialValue)
        {
            _inputField.text = initialValue;
        }
    }
}

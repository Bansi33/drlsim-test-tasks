using System;
using TMPro;
using UnityEngine;

namespace DRL
{
    public class LabeledInputField : LabeledUIElement
    {
        [Header("References: ")]
        [SerializeField] private TMP_InputField _inputField = null;

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

        public void InitializeInputField(string initialValue)
        {
            _inputField.text = initialValue;
        }
    }
}

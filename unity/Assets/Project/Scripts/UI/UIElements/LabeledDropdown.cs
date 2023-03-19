using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DRL
{
    public class LabeledDropdown : LabeledUIElement
    {
        [Header("References: ")]
        [SerializeField] private TMP_Dropdown _dropdown = null;

        public Action<int> OnDropdownValueChanged = null;

        private void OnEnable()
        {
            if (_dropdown != null)
            {
                _dropdown.onValueChanged.AddListener((value) => OnDropdownValueChanged?.Invoke(value));
            }
        }

        private void OnDisable()
        {
            if (_dropdown != null)
            {
                _dropdown.onValueChanged.RemoveAllListeners();
            }
        }

        public void InitializeDropdown(int initialValue, Type enumeratorType)
        {
            _dropdown.options.Clear();

            List<string> enumeratorValues = new List<string>();
            foreach (object enumValue in Enum.GetValues(enumeratorType))
            {
                enumeratorValues.Add(enumValue.ToString());
            }

            _dropdown.AddOptions(enumeratorValues);
            _dropdown.value = initialValue;
        }
    }
}

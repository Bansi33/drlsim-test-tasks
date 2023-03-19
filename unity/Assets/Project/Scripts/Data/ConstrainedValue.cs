using System;
using UnityEngine;

namespace DRL
{
    [Serializable]
    public class ConstrainedValue
    {
        [SerializeField] private float _currentValue = default;
        [SerializeField] private float _minValue = default;
        [SerializeField] private float _maxValue = default;
        [SerializeField] private bool _wholeNumbersOnly = default;

        public float MinValue
        {
            get { return _minValue; }
        }

        public float MaxValue
        {
            get { return _maxValue; }
        }

        public float Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = Mathf.Clamp(value, _minValue, _maxValue);

                if (_wholeNumbersOnly)
                {
                    _currentValue = Mathf.RoundToInt(_currentValue);
                }
            }
        }

        public bool WholeNumbersOnly { get { return _wholeNumbersOnly; } }

#if UNITY_EDITOR
        public void ValidateData()
        {
            if(_minValue > _maxValue)
            {
                _minValue = _maxValue;
            }

            if(_maxValue < _minValue)
            {
                _maxValue = _minValue;
            }

            _currentValue = Mathf.Clamp(_currentValue, _minValue, _maxValue);

            if (_wholeNumbersOnly)
            {
                _minValue = Mathf.RoundToInt(_minValue);
                _maxValue = Mathf.RoundToInt(_maxValue);
                _currentValue = Mathf.RoundToInt(Mathf.Clamp(_currentValue, _minValue, _maxValue));
            }
        }
#endif
    }
}
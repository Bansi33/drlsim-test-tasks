using System;
using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Data class representing a value that needs to be constrained between a minimum
    /// and maximum values.
    /// </summary>
    [Serializable]
    public class ConstrainedValue
    {
        [SerializeField] private float _currentValue = default;
        [SerializeField] private float _minValue = default;
        [SerializeField] private float _maxValue = default;
        [SerializeField] private bool _wholeNumbersOnly = default;

        /// <summary>
        /// Minimum value of the constrained data.
        /// </summary>
        public float MinValue
        {
            get 
            {
                if (_minValue > _maxValue)
                {
                    _minValue = _maxValue;
                }
                return ApplyCorrectConstraintFormat(_minValue); 
            }
        }

        /// <summary>
        /// Maximum value of the constrained data.
        /// </summary>
        public float MaxValue
        {
            get 
            {
                if (_maxValue < _minValue)
                {
                    _maxValue = _minValue;
                }
                return ApplyCorrectConstraintFormat(_maxValue); 
            }
        }

        /// <summary>
        /// Current value, constrained between <see cref="MinValue"> and <see cref="MaxValue"/>.
        /// </summary>
        public float Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = ApplyCorrectConstraintFormat(Mathf.Clamp(value, MinValue, MaxValue));
            }
        }

        /// <summary>
        /// Property indicating if the value supports only whole numbers or not. Used to simplify
        /// the implementation and remove the need for generics or mutliple classes.
        /// </summary>
        public bool WholeNumbersOnly { get { return _wholeNumbersOnly; } }

        /// <summary>
        /// Function applies the correct constrained to the <paramref name="value"/>
        /// based on the <see cref="WholeNumbersOnly"/> property.
        /// </summary>
        /// <param name="value">Value that will be returned in the correct constraint.</param>
        /// <returns>Correctly constrained value based on the <see cref="WholeNumbersOnly"/> property.</returns>
        private float ApplyCorrectConstraintFormat(float value)
        {
            return _wholeNumbersOnly ? Mathf.RoundToInt(value) : value;
        }
    }
}
using TMPro;
using UnityEngine;

namespace DRL
{
    /// <summary>
    /// Base class for all labeled UI elements. 
    /// Provides functionality for assigning the label of the element.
    /// </summary>
    public abstract class LabeledUIElement : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] protected TextMeshProUGUI _label = null;

        /// <summary>
        /// Function initializes the label of the UI element to the
        /// provided <paramref name="label"/>.
        /// </summary>
        /// <param name="label">The label of the UI element.</param>
        public virtual void SetLabel(string label)
        {
            _label.text = label;
        }
    }
}

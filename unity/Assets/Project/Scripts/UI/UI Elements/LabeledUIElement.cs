using TMPro;
using UnityEngine;

namespace DRL
{
    public abstract class LabeledUIElement : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] protected TextMeshProUGUI _label = null;

        public virtual void SetLabel(string label)
        {
            _label.text = label;
        }
    }
}

using TMPro;
using UnityEngine;

namespace Anvil
{
    public class LabledFaceButton : FaceButton, ILabeledUIButton
    {
        [SerializeField] protected TMP_Text _label;
        public void SetLabel(string label)
        {
            if (_label != null)
            {
                _label.text = label;
            }
        }
    }
}
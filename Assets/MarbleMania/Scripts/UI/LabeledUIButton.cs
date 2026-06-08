using UnityEngine;

namespace Anvil
{
    public class LabeledUIButton : AnimatedUIButton, ILabeledUIButton
    {
        [SerializeField] protected TMPro.TMP_Text _labelText;

        protected override bool OnClick()
        {
            return base.OnClick();
        }

        public void SetLabel(string label)
        {
            if (_labelText != null)
            {
                _labelText.text = label;
            }
        }
    }
}
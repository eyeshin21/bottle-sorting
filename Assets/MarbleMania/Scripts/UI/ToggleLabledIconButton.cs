using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class ToggleLabledIconButton : ToggleButton
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;

        public void SetIcon(Sprite icon)
        {
            if (_icon == null)
            {
                return;
            }
            _icon.sprite = icon;
        }

        public void SetLabel(string label)
        {
            if (_label == null)
            {
                return;
            }
            _label.text = label;
        }
    }
}
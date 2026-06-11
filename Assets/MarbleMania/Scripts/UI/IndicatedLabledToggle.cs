using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Anvil
{
    public class IndicatedLabledToggle : ToggleButton
    {
        [FormerlySerializedAs("_icon")] [SerializeField] private Image _indicator;
        [SerializeField] private TMP_Text _label;

        public void SetIcon(Sprite icon)
        {
            if (_indicator == null)
            {
                return;
            }
            _indicator.sprite = icon;
        }

        public override void UpdateStateRender()
        {
            base.UpdateStateRender();
            if (_indicator != null)
            {
                _indicator.gameObject.SetActive(state);
            }
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
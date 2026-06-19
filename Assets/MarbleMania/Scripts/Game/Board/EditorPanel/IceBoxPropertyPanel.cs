using Anvil;
using TMPro;
using UnityEngine;

namespace MarbleMania.EditorPanel
{
    public class IceBoxPropertyPanel : NormalBoxPropertyPanel
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private UIButton _setButton;

        public override void Load(Box box)
        {
            base.Load(box);
            IceBox iBox = box as IceBox;
            _inputField.text = iBox.Hp.ToString();
            _setButton.ClearListeners();
            _setButton.AddListener(() =>
            {
                if (int.TryParse(_inputField.text, out var hp))
                {
                    iBox.Hp = hp;
                }
            });
        }
    }
}
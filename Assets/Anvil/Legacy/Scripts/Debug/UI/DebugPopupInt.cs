#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class DebugPopupInt : DebugPopupBehaviour
    {
        [SerializeField] GameObject _titleText;
        [SerializeField] GameObject _labelText;
        [SerializeField] InputField _inputField;
        [SerializeField] Button _continueButton;
        [SerializeField] Button _cancelButton;
        [SerializeField] Button _closeButton;

        int InputValue => _inputField.text.ToInt();

        /// <summary>
        /// continueCallback(input)
        /// </summary>
        public void Show(string title, string label, int value, string continueButton, Callback<int> continueCallback)
        {
            _titleText.SetText(title);
            _labelText.SetText(label);
            _inputField.text = value.ToString();
            _continueButton.SetText(continueButton);
            _continueButton.AddListener(() =>
            {
                int inputValue = InputValue;
                Hide(() => continueCallback?.Invoke(inputValue));
            });
            _cancelButton.AddListener(Hide);
            _closeButton.AddListener(Hide);

            var buttons = _continueButton.transform.parent;
            buttons.ForceLayout();

            Show();
        }

        /// <summary>
        /// callback(button, input) (button is 1 or 2)
        /// </summary>
        public void Show(string title, string label, int value, string button1, string button2, Callback<int, int> callback)
        {
            var _button1 = _cancelButton;
            var _button2 = _continueButton;
            _titleText.SetText(title);
            _labelText.SetText(label);
            _inputField.text = value.ToString();
            _button1.SetText(button1);
            _button1.AddListener(() =>
            {
                int inputValue = InputValue;
                Hide(() => callback?.Invoke(1, inputValue));
            });
            _button2.SetText(button2);
            _button2.AddListener(() =>
            {
                int inputValue = InputValue;
                Hide(() => callback?.Invoke(2, inputValue));
            });
            _closeButton.AddListener(Hide);

            var buttons = _continueButton.transform.parent;
            buttons.ForceLayout();

            Show();
        }
    }
}
#endif
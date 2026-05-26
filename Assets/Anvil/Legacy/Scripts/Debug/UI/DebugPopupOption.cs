#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Anvil.Legacy
{
    public class DebugPopupOption : DebugPopupBehaviour
    {
        [SerializeField] GameObject _titleText;
        [SerializeField] GameObject _messageText;
        [SerializeField] GameObject _button;
        [SerializeField] Button _closeButton;

        public void Show(string title, string message, string button, Callback<int> buttonCallback)
        {
            Show(title, message, button, buttonCallback, null);
        }

        public void Show(string title, string message, string button1, string button2, Callback<int> buttonCallback)
        {
            Show(title, message, button1, buttonCallback, () =>
            {
                AddButton(button2, buttonCallback, 2);
            });
        }

        public void Show(string title, string message, string button1, string button2, string button3, Callback<int> buttonCallback)
        {
            Show(title, message, button1, buttonCallback, () =>
            {
                AddButton(button2, buttonCallback, 2);
                AddButton(button3, buttonCallback, 3);
            });
        }

        void Show(string title, string message, string button, Callback<int> buttonCallback, Callback addButtonCallback)
        {
            _titleText.SetText(title);
            _messageText.SetText(message);
            _button.SetText(button);
            _button.GetComponentInChildren<Button>().AddListener(() => Hide(buttonCallback, 1));
            addButtonCallback?.Invoke();
            _closeButton.AddListener(() => Hide(buttonCallback, 0));

            Show();
        }

        void AddButton(string text, Callback<int> buttonCallback, int buttonId)
        {
            AddButton(text, () => Hide(buttonCallback, buttonId));
        }

        public GameObject AddContent(GameObject prefab)
        {
            if (prefab == null) return null;

            float messageHeight = 150;
            float spacing = 10;
            float bottomPadding = 50;

            var parent = _messageText.transform.parent;
            var content = prefab.Create(parent);
            var contentRectTransform = content.GetComponent<RectTransform>();
            float contentHeight = contentRectTransform.sizeDelta.y;
            float totalHeight = messageHeight + spacing + contentHeight + bottomPadding;
            parent.GetComponent<RectTransform>().SetHeight(totalHeight);

            var anchoredPos = new Vector3(0, -totalHeight * 0.5f + bottomPadding + contentHeight * 0.5f);
            contentRectTransform.anchoredPosition3D = anchoredPos;

            var messageRectTransform = _messageText.GetComponent<RectTransform>();
            messageRectTransform.SetHeight(messageHeight);
            messageRectTransform.SetTop();

            return content;
        }

        public T AddContent<T>(GameObject prefab) where T : Component
        {
            var content = AddContent(prefab);
            return content != null ? content.GetComponent<T>() : default;
        }
    }
}
#endif
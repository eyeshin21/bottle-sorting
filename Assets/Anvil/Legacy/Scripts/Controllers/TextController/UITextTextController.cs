using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class UITextTextController : ITextController, IColorController
    {
        Text _uiText;

        void Construct(Text uiText)
        {
            _uiText = uiText;
        }

        public GameObject GameObject => _uiText?.gameObject;

        public string Text
        {
            get => _uiText.text;
            set => _uiText.text = value;
        }

        public float FontSize
        {
            get => _uiText.fontSize;
            set => _uiText.fontSize = Helper.RoundToInt(value);
        }

        public TextAnchor Alignment
        {
            get => _uiText.alignment;
            set => _uiText.alignment = value;
        }

        public Color Color
        {
            get => _uiText.color;
            set => _uiText.color = value;
        }

        public bool ResizeEnabled
        {
            get => _uiText.resizeTextForBestFit;
            set => _uiText.resizeTextForBestFit = value;
        }

        public bool WrappingEnabled
        {
            get => _uiText.horizontalOverflow == HorizontalWrapMode.Wrap;
            set => _uiText.horizontalOverflow = value ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
        }

        public Vector2 TextSize
        {
            get
            {
                var size = _uiText.GetSize();
#if UNITY_EDITOR
                size.x -= 20; //TODO
#endif
                return size;
            }
        }

        public void SetText(object text)
        {
            Text = text.GetString();
        }

        public void ForceUpdateText()
        {
            LegacyLog.NotSupported(this, "ForceUpdateText");
        }

        public void ReturnToPool()
        {
            _uiText = null;
            Pool.Return(this);
        }

        static Pool<UITextTextController> _pool;
        static Pool<UITextTextController> Pool => _pool ??= new();

        public static UITextTextController Create(Text uiText)
        {
            var controller = Pool.Get();
            controller.Construct(uiText);
            return controller;
        }
    }
}
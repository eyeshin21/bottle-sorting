using UnityEngine;

namespace Anvil.Legacy
{
    public class DefaultTextController : ITextController
    {
        string _text;
        Vector2 _textSize;
        float _fontSize;
        TextAnchor _alignment;
        Color _color = Color.white;
        bool _resizeEnabled;
        bool _wrappingEnabled;

        public GameObject GameObject => default;

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public Vector2 TextSize => _textSize;

        public float FontSize
        {
            get => _fontSize;
            set => _fontSize = value;
        }

        public TextAnchor Alignment
        {
            get => _alignment;
            set => _alignment = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public bool ResizeEnabled
        {
            get => _resizeEnabled;
            set => _resizeEnabled = value;
        }

        public bool WrappingEnabled
        {
            get => _wrappingEnabled;
            set => _wrappingEnabled = value;
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
            _text = "";
            Pool.Return(this);
        }

        static Pool<DefaultTextController> _pool;
        static Pool<DefaultTextController> Pool => _pool ??= new();

        public static DefaultTextController Create()
        {
            return Pool.Get();
        }
    }
}
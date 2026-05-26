using UnityEngine;
using TMPro;

namespace Anvil.Legacy
{
    public class TextMeshProTextController : ITextController, IColorController
    {
        TMP_Text _tmpText;

        void Construct(TMP_Text tmpText)
        {
            _tmpText = tmpText;
        }

        public GameObject GameObject => _tmpText?.gameObject;

        public string Text
        {
            get => _tmpText.text;
            set => _tmpText.text = value;
        }

        public float FontSize
        {
            get => _tmpText.fontSize;
            set => _tmpText.fontSize = value;
        }

        public TextAnchor Alignment
        {
            get => _tmpText.alignment.ToTextAnchor();
            set => _tmpText.alignment = value.ToTextAlignmentOptions();
        }

        public Color Color
        {
            get => _tmpText.color;
            set => _tmpText.color = value;
        }

        public bool ResizeEnabled
        {
            get => _tmpText.enableAutoSizing;
            set => _tmpText.enableAutoSizing = value;
        }

        public bool WrappingEnabled
        {
            get => _tmpText.enableWordWrapping;
            set => _tmpText.enableWordWrapping = value;
        }

        public Vector2 TextSize => _tmpText.GetSize();

        public void SetText(object text)
        {
            Text = text.GetString();
        }

        public void ForceUpdateText()
        {
            _tmpText.ForceMeshUpdate();
        }

        public void ReturnToPool()
        {
            _tmpText = null;
            Pool.Return(this);
        }

        static Pool<TextMeshProTextController> _pool;
        static Pool<TextMeshProTextController> Pool => _pool ??= new();

        public static TextMeshProTextController Create(TMP_Text tmpText)
        {
            var controller = Pool.Get();
            controller.Construct(tmpText);
            return controller;
        }
    }
}
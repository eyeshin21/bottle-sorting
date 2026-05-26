using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public abstract class TextAdapter
    {
        public abstract GameObject GameObject { get; }
        public abstract string Text { get; set; }
        public abstract Vector3 TextSize { get; }
        public abstract TextAnchor Anchor { get; set; }
        public abstract float FontSize { get; set; }
        public abstract bool ResizeEnabled { get; set; }
        public abstract bool WrappingEnabled { get; set; }
        public abstract Color Color { get; set; }

        public void SetText(int value)
        {
            Text = value.ToString();
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public virtual void ForceUpdate()
        {

        }

        #region Static
        public static void SetText(GameObject go, object text)
        {
            SetText(go, text.ToString());
        }

        public static void SetText(GameObject go, string text)
        {
            if (go == null)
            {
                LegacyLog.Warning($"SetText({text}): GameObject is null!");
                return;
            }

            var textMeshPro = go.GetComponentInChildren<TMP_Text>();
            if (textMeshPro != null)
            {
                textMeshPro.text = text;
                return;
            }

            var uiText = go.GetComponentInChildren<Text>();
            if (uiText != null)
            {
                uiText.text = text;
                return;
            }

            var textMesh = go.GetComponentInChildren<TextMesh>();
            if (textMesh != null)
            {
                textMesh.text = text;
                return;
            }

            LegacyLog.Warning($"Can't set text \"{text}\"!");
        }
        #endregion

        #region Create
        public static TextAdapter Create(TMP_Text textMeshPro)
        {
            return new TextMeshProAdapter(textMeshPro);
        }

        public static TextAdapter Create(Text text)
        {
            return new UITextAdapter(text);
        }

        public static TextAdapter Create(TextMesh textMesh)
        {
            return new TextMeshAdapter(textMesh);
        }

        public static TextAdapter Create(GameObject gameObject)
        {
            TextAdapter adapter = null;
            gameObject.BrowseChildren(go =>
            {
                var textMeshPro = go.GetComponent<TMP_Text>();
                if (textMeshPro != null)
                {
                    adapter = new TextMeshProAdapter(textMeshPro);
                    return false;
                }

                var text = go.GetComponent<Text>();
                if (text != null)
                {
                    adapter = new UITextAdapter(text);
                    return false;
                }

                var textMesh = go.GetComponent<TextMesh>();
                if (textMesh != null)
                {
                    adapter = new TextMeshAdapter(textMesh);
                }

                return true;
            });

            if (adapter == null)
            {
                LegacyLog.Warning($"Can't create text adapter for {gameObject.name}");
                adapter = new DefaultTextAdapter();
            }

            return adapter;
        }
        #endregion

        #region Adapters
        class UITextAdapter : TextAdapter
        {
            private Text _text;

            public UITextAdapter(Text text)
            {
                _text = text;
            }

            public override GameObject GameObject => _text.gameObject;

            public override string Text
            {
                get => _text.text;
                set => _text.text = value;
            }

            public override Vector3 TextSize
            {
                get
                {
                    var text = _text.text;
                    var generator = new TextGenerator();
                    var settings = _text.GetGenerationSettings(_text.rectTransform.rect.size);
                    float width = generator.GetPreferredWidth(text, settings);
                    float height = generator.GetPreferredHeight(text, settings);

                    return new Vector3(width, height);
                }
            }

            public override TextAnchor Anchor
            {
                get => _text.alignment;
                set => _text.alignment = value;
            }

            public override float FontSize
            {
                get => _text.fontSize;
                set => _text.fontSize = Mathf.RoundToInt(value);
            }

            public override bool ResizeEnabled
            {
                get => _text.resizeTextForBestFit;
                set => _text.resizeTextForBestFit = value;
            }

            public override bool WrappingEnabled
            {
                get => _text.horizontalOverflow == HorizontalWrapMode.Wrap;
                set => _text.horizontalOverflow = value ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            }

            public override Color Color
            {
                get => _text.color;
                set => _text.color = value;
            }
        }

        class TextMeshProAdapter : TextAdapter
        {
            private TMP_Text _textMeshPro;

            public TextMeshProAdapter(TMP_Text textMeshPro)
            {
                _textMeshPro = textMeshPro;
            }

            public override GameObject GameObject => _textMeshPro.gameObject;

            public override string Text
            {
                get => _textMeshPro.text;
                set => _textMeshPro.text = value;
            }

            public override Vector3 TextSize => _textMeshPro.textBounds.size;

            public override TextAnchor Anchor
            {
                get
                {
                    var alignment = _textMeshPro.alignment;

                    if (alignment == TextAlignmentOptions.TopLeft) return TextAnchor.UpperLeft;
                    if (alignment == TextAlignmentOptions.Top) return TextAnchor.UpperCenter;
                    if (alignment == TextAlignmentOptions.TopRight) return TextAnchor.UpperRight;

                    if (alignment == TextAlignmentOptions.Left) return TextAnchor.MiddleLeft;
                    if (alignment == TextAlignmentOptions.Center) return TextAnchor.MiddleCenter;
                    if (alignment == TextAlignmentOptions.Right) return TextAnchor.MiddleRight;

                    if (alignment == TextAlignmentOptions.BottomLeft) return TextAnchor.LowerLeft;
                    if (alignment == TextAlignmentOptions.Bottom) return TextAnchor.LowerCenter;
                    if (alignment == TextAlignmentOptions.BottomRight) return TextAnchor.LowerRight;

                    Assert.Todo(alignment);
                    return TextAnchor.MiddleCenter;
                }
                set
                {
                    _textMeshPro.alignment = GetAlignment(value);
                }
            }

            TextAlignmentOptions GetAlignment(TextAnchor anchor)
            {
                if (anchor == TextAnchor.UpperLeft) return TextAlignmentOptions.TopLeft;
                if (anchor == TextAnchor.UpperCenter) return TextAlignmentOptions.Top;
                if (anchor == TextAnchor.UpperRight) return TextAlignmentOptions.TopRight;

                if (anchor == TextAnchor.MiddleLeft) return TextAlignmentOptions.Left;
                if (anchor == TextAnchor.MiddleCenter) return TextAlignmentOptions.Center;
                if (anchor == TextAnchor.MiddleRight) return TextAlignmentOptions.Right;

                if (anchor == TextAnchor.LowerLeft) return TextAlignmentOptions.BottomLeft;
                if (anchor == TextAnchor.LowerCenter) return TextAlignmentOptions.Bottom;
                if (anchor == TextAnchor.LowerRight) return TextAlignmentOptions.BottomRight;

                Assert.Todo(anchor);
                return TextAlignmentOptions.Center;
            }

            public override float FontSize
            {
                get => _textMeshPro.fontSize;
                set => _textMeshPro.fontSize = value;
            }

            public override bool ResizeEnabled
            {
                get => _textMeshPro.enableAutoSizing;
                set => _textMeshPro.enableAutoSizing = value;
            }

            public override bool WrappingEnabled
            {
                get => _textMeshPro.textWrappingMode != TextWrappingModes.NoWrap;
                set => _textMeshPro.textWrappingMode = value ? TextWrappingModes.Normal : TextWrappingModes.NoWrap;
            }

            public override Color Color
            {
                get => _textMeshPro.color;
                set => _textMeshPro.color = value;
            }

            public override void ForceUpdate()
            {
                _textMeshPro.ForceMeshUpdate();
            }
        }

        class TextMeshAdapter : TextAdapter
        {
            private TextMesh _textMesh;

            public TextMeshAdapter(TextMesh textMesh)
            {
                _textMesh = textMesh;
            }

            public override GameObject GameObject => _textMesh.gameObject;

            public override string Text
            {
                get => _textMesh.text;
                set => _textMesh.text = value;
            }

            public override Vector3 TextSize => Vector3.zero; //TODO:

            public override TextAnchor Anchor
            {
                get => _textMesh.anchor;
                set => _textMesh.anchor = value;
            }

            public override float FontSize
            {
                get => _textMesh.fontSize;
                set => _textMesh.fontSize = Mathf.RoundToInt(value);
            }

            public override bool ResizeEnabled
            {
                get => false;
                set
                {
                    if (value)
                    {
                        LegacyLog.Warning("TextMesh not suppoted resize!");
                    }
                }
            }

            public override bool WrappingEnabled
            {
                get => false;
                set
                {
                    if (value)
                    {
                        LegacyLog.Warning("TextMesh not suppoted wrap!");
                    }
                }
            }

            public override Color Color
            {
                get => _textMesh.color;
                set => _textMesh.color = value;
            }
        }

        class DefaultTextAdapter : TextAdapter
        {
            private GameObject _gameObject;
            private string _text;
            private Vector3 _textSize;
            private TextAnchor _anchor;
            private float _fontSize;
            private bool _resizeEnabled;
            private bool _wrappingEnabled;
            private Color _color;

            public override GameObject GameObject => _gameObject;

            public override string Text
            {
                get => _text;
                set => _text = value;
            }

            public override Vector3 TextSize => _textSize;

            public override TextAnchor Anchor
            {
                get => _anchor;
                set => _anchor = value;
            }

            public override float FontSize
            {
                get => _fontSize;
                set => _fontSize = value;
            }

            public override bool ResizeEnabled
            {
                get => _resizeEnabled;
                set => _resizeEnabled = value;
            }

            public override bool WrappingEnabled
            {
                get => _wrappingEnabled;
                set => _wrappingEnabled = value;
            }

            public override Color Color
            {
                get => _color;
                set => _color = value;
            }
        }
        #endregion
    }
}
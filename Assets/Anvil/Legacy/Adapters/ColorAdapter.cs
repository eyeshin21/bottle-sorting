using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// #define SPINE_ENABLED
#if SPINE_ENABLED
using Spine.Unity;
#endif

namespace Anvil.Legacy
{
    public interface IColorAdapter
    {
        GameObject GameObject { get; }
        Color Color { get; set; }
        float Alpha { get; set; }
    }

    public abstract class ColorAdapter : IColorAdapter
    {
        public abstract GameObject GameObject { get; }
        public abstract Color Color { get; set; }

        public virtual float Alpha
        {
            get => Color.a;
            set
            {
                var color = Color;
                color.a = value;
                Color = color;
            }
        }

        public static ColorAdapter Create(SpriteRenderer spriteRenderer)
        {
            return new SpriteRendererColorAdapter(spriteRenderer);
        }

        public static ColorAdapter Create(Image image)
        {
            return new ImageColorAdapter(image);
        }
        public static ColorAdapter Create(Graphic graphic)
        {
            return new UIGraphicColorAdapter(graphic);
        }

        public static ColorAdapter Create(Text text)
        {
            return new TextColorAdapter(text);
        }

        public static ColorAdapter Create(TMP_Text tmpText)
        {
            return new TextMeshProColorAdapter(tmpText);
        }

        public static ColorAdapter Create(Renderer renderer)
        {
            return new RendererColorAdapter(renderer);
        }

        public static IColorAdapter Create(Component component)
        {
            return Create(component.gameObject);
        }

        static List<IColorAdapter> _colorAdapters = new List<IColorAdapter>();
        public static IColorAdapter Create(GameObject gameObject, bool withChildren = true)
        {
            _colorAdapters.Clear();
            gameObject.BrowseChildrenBFS(go =>
            {
                var colorAdapter = CreateColorAdapter(go, out bool continueChildren);
                if (colorAdapter != null)
                {
                    _colorAdapters.Add(colorAdapter);
                }
                return withChildren && continueChildren;
            });

            int count = _colorAdapters.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create color adapter for {gameObject}!");
                return new DefaultColorAdapter(gameObject);
            }

            if (count == 1)
            {
                var colorAdapter = _colorAdapters[0];
                _colorAdapters.Clear();
                return colorAdapter;
            }

            var colorAdapter2 = new CompositeColorAdapter(_colorAdapters);
            _colorAdapters.Clear();
            return colorAdapter2;
        }

        static IColorAdapter CreateColorAdapter(GameObject gameObject, out bool continueChildren)
        {
            continueChildren = true;

            var colorAdapter = gameObject.GetComponent<IColorAdapter>();
            if (colorAdapter != null)
            {
                continueChildren = false;
                return colorAdapter;
            }

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                return Create(spriteRenderer);
            }

            var image = gameObject.GetComponent<Image>();
            if (image != null)
            {
                return Create(image);
            }
            var graphic = gameObject.GetComponent<Graphic>();
            if (graphic != null)
            {
                // Debug.Log("grafic found");
                return Create(graphic);
            }

            var text = gameObject.GetComponent<Text>();
            if (text != null)
            {
                return Create(text);
            }

            var tmpText = gameObject.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                return Create(tmpText);
            }

            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                return Create(renderer);
            }

            return null;
        }

        class UIGraphicColorAdapter : ColorAdapter
        {
            private Graphic _graphic;

            public UIGraphicColorAdapter(Graphic graphic)
            {
                _graphic = graphic;
            }
            public override GameObject GameObject => _graphic.gameObject;

            public override Color Color
            {
                get => _graphic.color;
                set => _graphic.color = value;
            }
        }
        class SpriteRendererColorAdapter : ColorAdapter
        {
            private SpriteRenderer _spriteRenderer;

            public SpriteRendererColorAdapter(SpriteRenderer spriteRenderer)
            {
                _spriteRenderer = spriteRenderer;
            }

            public override GameObject GameObject => _spriteRenderer.gameObject;

            public override Color Color
            {
                get => _spriteRenderer.color;
                set => _spriteRenderer.color = value;
            }
        }

        class ImageColorAdapter : ColorAdapter
        {
            private Image _image;

            public ImageColorAdapter(Image image)
            {
                _image = image;
            }

            public override GameObject GameObject => _image.gameObject;

            public override Color Color
            {
                get => _image.color;
                set => _image.color = value;
            }
        }

        class TextColorAdapter : ColorAdapter
        {
            private Text _text;

            public TextColorAdapter(Text text)
            {
                _text = text;
            }

            public override GameObject GameObject => _text.gameObject;

            public override Color Color
            {
                get => _text.color;
                set => _text.color = value;
            }
        }

        class TextMeshProColorAdapter : ColorAdapter
        {
            private TMP_Text _tmpText;

            public TextMeshProColorAdapter(TMP_Text tmpText)
            {
                _tmpText = tmpText;
            }

            public override GameObject GameObject => _tmpText.gameObject;

            public override Color Color
            {
                get => _tmpText.color;
                set => _tmpText.color = value;
            }
        }

        class RendererColorAdapter : ColorAdapter
        {
            private static readonly string TintColorProperty = "_TintColor";
            private static readonly string ColorProperty = "_Color";

            private Renderer _renderer;
            private bool _isTint;
            private bool _hasColor;
            private Color _getSetColor;

            public RendererColorAdapter(Renderer renderer)
            {
                _renderer = renderer;

                if (_renderer.material.HasProperty(TintColorProperty))
                {
                    _isTint = true;
                }
                else
                {
                    _isTint = false;
                    if (renderer.GetComponent<LineRenderer>() == null && _renderer.material.HasProperty(ColorProperty))
                    {
                        _hasColor = true;
                    }
                    else
                    {
                        //Log.Warning($"Not has color: {renderer.name}");
                        _hasColor = false;
                    }
                }
            }

            public override GameObject GameObject => _renderer.gameObject;

            public override Color Color
            {
                get => _isTint ? _renderer.material.GetColor(TintColorProperty) : (_hasColor ? _renderer.material.color : _getSetColor);
                set
                {
                    if (_isTint)
                    {
                        _renderer.material.SetColor(TintColorProperty, value);
                    }
                    else if (_hasColor)
                    {
                        _renderer.material.color = value;
                    }
                    else
                    {
                        _getSetColor = value;
                    }
                }
            }
        }

        class DefaultColorAdapter : ColorAdapter
        {
            private GameObject _gameObject;
            private Color _color = Color.white;

            public DefaultColorAdapter(GameObject gameObject)
            {
                _gameObject = gameObject;
            }

            public override GameObject GameObject => _gameObject;

            public override Color Color
            {
                get => _color;
                set => _color = value;
            }
        }

        class CompositeColorAdapter : ColorAdapter
        {
            private List<IColorAdapter> _adapters;
            private int _count;
            private Color _color = Color.white;

            public CompositeColorAdapter(List<IColorAdapter> adapters)
            {
                _count = adapters.Count;
                _adapters = new List<IColorAdapter>(_count);
                _adapters.AddRange(adapters);
            }

            public override GameObject GameObject => _count > 0 ? _adapters[0].GameObject : null;

            public override Color Color
            {
                get => _count > 0 ? _adapters[0].Color : _color;
                set
                {
                    for (int i = 0; i < _count; i++)
                    {
                        _adapters[i].Color = value;
                    }
                    _color = value;
                }
            }

            public override float Alpha
            {
                get => _count > 0 ? _adapters[0].Alpha : _color.a;
                set
                {
                    for (int i = 0; i < _count; i++)
                    {
                        _adapters[i].Alpha = value;
                    }
                    _color.a = value;
                }
            }
        }
    }
}

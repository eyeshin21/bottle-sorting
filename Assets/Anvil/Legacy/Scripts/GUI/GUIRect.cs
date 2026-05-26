using UnityEngine;

namespace Anvil.Legacy
{
    public class GUIRect
    {
        Rect _beginRect;
        Rect _rect;
        Color _color;
        Texture _texture;
        bool _isSet;
        //Callback _onSet;

        public Rect Rect
        {
            get => _rect;
            set => _rect = value;
        }

        public float PaddingLeft { get; set; }
        public float PaddingRight { get; set; }
        public float MaxWidth { get; set; }

        public GUIRect()
        {
            _color = Color.white;
        }

        public GUIRect(Color color)
        {
            _color = color;
        }

        public void OnGUI()
        {
            if (_isSet)
            {
                if (_texture == null)
                {
                    _texture = GUIHelper.GetBackground(_color);
                }
                GUI.DrawTexture(_rect, _texture);
            }
        }

        public void BeginCheckRect()
        {
            //if (_isSet) return;

            if (Event.current.type == EventType.Repaint)
            {
                _beginRect = GUILayoutUtility.GetLastRect();
            }
        }

        public void EndCheckRect()
        {
            //if (_isSet) return;
            if (Event.current.type != EventType.Repaint) return;

            _isSet = true;
            var endRect = GUILayoutUtility.GetLastRect();
            float left = Mathf.Min(_beginRect.xMin, endRect.xMin);
            float right = Mathf.Max(_beginRect.xMax, endRect.xMax);
            float top = Mathf.Min(_beginRect.yMin, endRect.yMin);
            float bottom = Mathf.Max(_beginRect.yMax, endRect.yMax);
            //Log.Debug($"beginRect={_beginRect}, endRect={endRect}");
            //Log.Debug($"begin: min=({_beginRect.xMin},{_beginRect.yMin}), max=({_beginRect.xMax},{_beginRect.yMax})");
            //Log.Debug($"end: min=({endRect.xMin},{endRect.yMin}), max=({endRect.xMax},{endRect.yMax})");

            _rect.x = left;
            _rect.y = top;
            _rect.width = right - left;
            _rect.height = bottom - top;

            float maxWidth = MaxWidth;
            if (maxWidth > 0)
            {
                _rect.width = Mathf.Min(_rect.width, maxWidth);
            }

            _rect.width += PaddingLeft + PaddingRight;
            _rect.x += PaddingRight * 0.5f - PaddingLeft * 0.5f;

            //_onSet?.Invoke();
        }

        public void Reset()
        {
            _isSet = false;
        }
    }
}
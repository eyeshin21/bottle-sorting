using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class GUIDebugger
    {
        List<Rect> _rects = new List<Rect>();
        Color _color;
        Texture _texture;
        bool _isRepaint;

        public GUIDebugger()
        {
            _color = new Color(0, 1, 0, 0.5f);
        }

        public GUIDebugger(Color color)
        {
            _color = color;
        }

        public void SetColor(Color color)
        {
            _color = color;
            _texture = null;
        }

        public void AddRect(Rect rect)
        {
            _rects.Add(rect);
        }

        public void AddLastRect()
        {
            //if (Event.current.type == EventType.Repaint)
            if (_isRepaint)
            {
                _rects.Add(GUILayoutUtility.GetLastRect());
            }
        }

        public void Clear()
        {
            _rects.Clear();
        }

        public void OnGUI()
        {
            if (_texture == null)
            {
                _texture = GUIHelper.GetBackground(_color);
            }

            int count = _rects.Count;
            for (int i = 0; i < count; i++)
            {
                GUI.DrawTexture(_rects[i], _texture);
            }

            _isRepaint = Event.current.type == EventType.Repaint;
            if (_isRepaint)
            {
                _rects.Clear();
            }
        }
    }
}
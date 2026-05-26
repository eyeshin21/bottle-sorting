#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class WindowWidthTracker
    {
#if UNITY_EDITOR
        EditorWindow _window;
#endif
        float _windowWidth;
        float _lastTime;

        public float WindowWidth => _windowWidth;

#if UNITY_EDITOR
        public WindowWidthTracker(EditorWindow window)
        {
            _window = window;
            _windowWidth = window.GetWidth();
            _lastTime = TimeHelper.EditorTimeSinceStartup;
        }
#endif

        /// <summary>
        /// Returns true if window's width changed.
        /// </summary>
        public bool Update()
        {
#if UNITY_EDITOR
            float time = TimeHelper.EditorTimeSinceStartup;
            if (time - _lastTime >= 1f)
            {
                _lastTime = time;
                float width = _window.GetWidth();
                if (!width.IsEquals(_windowWidth))
                {
                    _windowWidth = width;
                    return true;
                }
            }
#endif
            return false;
        }
    }
}
#endif
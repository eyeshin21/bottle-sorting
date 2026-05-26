using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class EditorHelper
    {
#if UNITY_EDITOR
        static string GetWindowTitle<T>(string title) where T : EditorWindow
        {
            if (string.IsNullOrEmpty(title))
            {
                var name = Helper.GetClassName<T>();
                if (name.StartsWith("Window"))
                {
                    name = name.Substring(6);
                }
                else if (name.EndsWith("Window"))
                {
                    name = name.Substring(0, name.Length - 6);
                }
                title = Helper.GetNicifyName(name);
            }
            return title;
        }

        public static T ShowEditorWindow<T>(string title = "") where T : EditorWindow
        {
            var window = GetWindow<T>();
            window.titleContent.text = GetWindowTitle<T>(title);
            window.Show();
            return window;
        }

        public static void ShowEditorWindow<T>(string title, Rect rect) where T : EditorWindow
        {
            var window = GetWindowWithRect<T>(rect);
            window.titleContent.text = GetWindowTitle<T>(title);
            window.Show();
        }

        public static void RepaintCurrentLevelEditorWindow()
        {
            var window = EditorWindow.focusedWindow;
            if (window != null)
            {
                var title = window.titleContent.text;
                if (title.Contains("Level Editor"))
                {
                    window.Repaint();
                }
                else
                {
                    LegacyLog.Warning($"Can't find Level Editor Window! ({title})");
                }
            }
            else
            {
                LegacyLog.Warning("Focused window is null!");
            }
        }
#endif
    }
}
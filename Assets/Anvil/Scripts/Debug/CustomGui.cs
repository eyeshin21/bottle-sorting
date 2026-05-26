using UnityEditor;
using UnityEngine;

namespace Anvil.Legacy
{
    public static class CustomGUI
    {
        public static readonly GUIStyle splitLine;

#region Line

        static CustomGUI()
        {
            GUISkin skin = GUI.skin;

            splitLine = new GUIStyle();
            splitLine.normal.background = Texture2D.whiteTexture;
            splitLine.stretchWidth = true;
            splitLine.margin = new RectOffset(0, 0, 7, 7);
        }

        private static readonly Color splitterColor = new Color(0.157f, 0.157f, 0.157f);

        // GUILayout Style
        public static void Line(Color rgb, float thickness = 1)
        {
            Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitLine, GUILayout.Height(thickness));

            if (Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = rgb;
                splitLine.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        public static void Line(float thickness, GUIStyle splitterStyle)
        {
            Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitterStyle, GUILayout.Height(thickness));

            if (Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = splitterColor;
                splitterStyle.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        public static void Line(float thickness = 1)
        {
            Line(thickness, splitLine);
        }

        // GUI Style
        public static void Line(Rect position)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = splitterColor;
                splitLine.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

#endregion

        private static GUIStyle _toggleStyle = null;
        private static GUIStyle _buttonStyle = null;

        public static GUIStyle ToggleStyle
        {
            get
            {
                if (_toggleStyle != null)
                {
                    return _toggleStyle;
                }

                Texture2D blackTxt = new Texture2D(1, 1);
                blackTxt.SetPixel(0, 0, new Color(0, 0f, 0f, 1f));
                blackTxt.Apply(true);
                _toggleStyle = new GUIStyle(GUI.skin.button)
                {
                    fixedHeight = 70,
                    normal = { background = blackTxt, textColor = Color.white },
                    onNormal = { background = Texture2D.whiteTexture, textColor = Color.black },
                    stretchWidth = true,
                    padding = new RectOffset(10,10,10,10),
                    alignment = TextAnchor.MiddleLeft,

                };
                return _toggleStyle;
            }
        }
        public static GUIStyle CommonButtonStyle
        {
            get
            {
                if (_buttonStyle != null)
                {
                    return _buttonStyle;
                }

                Texture2D blackTxt = new Texture2D(1, 1);
                blackTxt.SetPixel(0, 0, new Color(0, 0f, 0f, 1f));
                blackTxt.Apply(true);
                _buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fixedHeight = 45,
                    normal = { background = Texture2D.whiteTexture, textColor = Color.black },
                    onNormal = { background = blackTxt, textColor = Color.white},
                    stretchWidth = true,
                    padding = new RectOffset(10,10,10,10),
                    alignment = TextAnchor.MiddleLeft,

                };
                return _buttonStyle;
            }
        }

        public static GUIStyle CreateToggleGUIStyle(bool state)
        {
            GUIStyle ret = new GUIStyle(ToggleStyle);
            ret.fontStyle = state ? FontStyle.Bold : FontStyle.Normal;
            return ret;
        }

    }
}

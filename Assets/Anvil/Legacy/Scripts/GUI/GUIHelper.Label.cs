using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        #region Text
        public static void Label(string text, params GUILayoutOption[] options)
        {
            GUILayout.Label(text, options);
        }

        public static void Label(GUIContent content, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, options);
        }

        public static void Label(string text, float labelWidth)
        {
            if (labelWidth > 0)
            {
                GUILayout.Label(text, GUILayout.Width(labelWidth));
            }
            else if (labelWidth < 0)
            {
                GUILayout.Label(text, GUILayout.Width(labelWidth), GUILayout.ExpandWidth(true));
            }
            else
            {
                GUILayout.Label(text);
            }
        }

        public static void Label(string text, GUIStyle style, float labelWidth)
        {
            if (style == null)
            {
                Label(text, labelWidth);
            }
            else
            {
                if (labelWidth > 0)
                {
                    GUILayout.Label(text, style, GUILayout.Width(labelWidth));
                }
                else if (labelWidth < 0)
                {
                    GUILayout.Label(text, style, GUILayout.Width(labelWidth), GUILayout.ExpandWidth(true));
                }
                else
                {
                    GUILayout.Label(text, style);
                }
            }
        }

        public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.Label(text, style, options);
        }
        #endregion

        #region Content
        public static void Label(GUIContent content)
        {
            GUILayout.Label(content);
        }

        public static void Label(GUIContent content, float labelWidth)
        {
            if (labelWidth > 0)
            {
                GUILayout.Label(content, GUILayout.Width(labelWidth));
            }
            else if (labelWidth < 0)
            {
                GUILayout.Label(content, GUILayout.Width(labelWidth), GUILayout.ExpandWidth(true));
            }
            else
            {
                GUILayout.Label(content);
            }
        }

        public static void Label(GUIContent content, GUIStyle style, float labelWidth)
        {
            if (content == null) return;

            if (style == null)
            {
                Label(content, labelWidth);
            }
            else
            {
                if (labelWidth > 0)
                {
                    GUILayout.Label(content, style, GUILayout.Width(labelWidth));
                }
                else if (labelWidth < 0)
                {
                    GUILayout.Label(content, style, GUILayout.ExpandWidth(true));
                }
                else
                {
                    GUILayout.Label(content, style);
                }
            }
        }

        public static void Label(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, style, options);
        }
        #endregion

        #region Layout
        public static void LabelLeft(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(text);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void LabelLeft(float spacing, string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spacing);
            GUILayout.Label(text);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void LabelLeft(GUIContent content)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(content);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void LabelLeft(float spacing, GUIContent content)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spacing);
            GUILayout.Label(content);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        #endregion
    }
}
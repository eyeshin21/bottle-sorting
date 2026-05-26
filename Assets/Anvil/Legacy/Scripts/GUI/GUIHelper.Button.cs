using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        public static readonly GUIContent AddContent = new("+");
        public static readonly GUIContent RemoveContent = new("-");
        public static readonly GUIContent CopyContent = new("Copy");
        public static readonly GUIContent PasteContent = new("Paste");
        public static readonly GUIContent ClearContent = new("Clear");
        public static readonly GUIContent SwapContent = new("Swap");

        public static GUILayoutOption[] AddRemoveButtonOptions = new GUILayoutOption[] { GUILayout.Width(20) };

        #region Text
        public static bool Button(string text, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, options);
        }

        public static bool Button(string text, string tooltip, params GUILayoutOption[] options)
        {
            _content.text = text;
            _content.tooltip = tooltip;
            return GUILayout.Button(_content, options);
        }

        public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, style, options);
        }

        public static bool Button(string text, ref float width)
        {
            if (width <= 0)
            {
                width = GetButtonWidth(text);
            }
            return GUILayout.Button(text, GUILayout.Width(width));
        }

        public static bool Button(string text, ref GUILayoutOption width)
        {
            if (width == null)
            {
                width = GUILayout.Width(GetButtonWidth(text));
            }
            return GUILayout.Button(text, width);
        }

        public static bool Button(string text, Func<bool> enabledFunc)
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = guiEnabled && enabledFunc();
            bool result = GUILayout.Button(text);
            GUI.enabled = guiEnabled;
            return result;
        }
        #endregion

        #region Content
        public static bool Button(GUIContent content, params GUILayoutOption[] options)
        {
            return GUILayout.Button(content, options);
        }

        public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUILayout.Button(content, style, options);
        }

        public static bool Button(GUIContent content, ref float width)
        {
            if (width <= 0)
            {
                width = GetButtonWidth(content);
            }
            return GUILayout.Button(content, GUILayout.Width(width));
        }

        public static bool Button(GUIContent content, ref GUILayoutOption width)
        {
            if (width == null)
            {
                width = GUILayout.Width(GetButtonWidth(content));
            }
            return GUILayout.Button(content, width);
        }

        public static bool ButtonAdd()
        {
            return GUILayout.Button(AddContent, AddRemoveButtonOptions);
        }

        public static bool ButtonRemove()
        {
            return GUILayout.Button(RemoveContent, AddRemoveButtonOptions);
        }

        public static bool ButtonCopy()
        {
            return GUILayout.Button(CopyContent);
        }

        public static bool ButtonPaste()
        {
            return GUILayout.Button(PasteContent);
        }

        public static bool ButtonClear()
        {
            return GUILayout.Button(ClearContent);
        }

        public static bool ButtonSwap()
        {
            return GUILayout.Button(SwapContent);
        }
        #endregion

        #region Layout
        public static bool ButtonLeft(string text)
        {
            return LayoutLeft(() => GUILayout.Button(text));
        }

        public static bool ButtonLeft(float spacing, string text)
        {
            return LayoutLeft(spacing, () => GUILayout.Button(text));
        }

        public static bool ButtonLeft(GUIContent content)
        {
            return LayoutLeft(() => GUILayout.Button(content));
        }

        public static bool ButtonLeft(float spacing, GUIContent content)
        {
            return LayoutLeft(spacing, () => GUILayout.Button(content));
        }

        public static bool ButtonCenter(string text)
        {
            return LayoutCenter(() => GUILayout.Button(text));
        }

        public static bool ButtonCenter(GUIContent content)
        {
            return LayoutCenter(() => GUILayout.Button(content));
        }
        #endregion
    }
}
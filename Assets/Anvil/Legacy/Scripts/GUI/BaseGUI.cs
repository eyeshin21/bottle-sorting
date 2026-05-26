using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public abstract class BaseGUI : IGUI
    {
        protected static readonly float GUISpace = 2;

        public abstract void OnGUI();

        #region Create
        protected static GUIContent CreateContent(Sprite sprite) => new GUIContent(sprite?.texture);

        protected static GUILayoutOption[] CreateLayoutOptions(float width, float height) => new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) };
        #endregion

        #region Set/Restore
        protected static void SetGUIEnabled(bool enabled, Callback callback)
        {
            if (enabled == GUI.enabled)
            {
                callback();
            }
            else
            {
                GUI.enabled = enabled;
                callback();
                GUI.enabled = !enabled;
            }
        }

        protected Stack<bool> _guiEnableds;
        protected void SetGUIEnabledAnd(bool enabled)
        {
            if (_guiEnableds == null)
            {
                _guiEnableds = new Stack<bool>();
            }
            bool guiEnabled = GUI.enabled;
            _guiEnableds.Push(guiEnabled);
            GUI.enabled = guiEnabled && enabled;
        }
        protected void RestoreGUIEnabled()
        {
            if (_guiEnableds == null)
            {
                LegacyLog.Warning("Enableds is null!");
            }
            else if (_guiEnableds.Count > 0)
            {
                GUI.enabled = _guiEnableds.Pop();
            }
            else
            {
                LegacyLog.Warning("Enableds is empty!");
            }
        }

        protected Stack<Color> _guiColors;
        protected void SetGUIColor(Color color)
        {
            if (_guiColors == null)
            {
                _guiColors = new Stack<Color>();
            }
            _guiColors.Push(GUI.color);
            GUI.color = color;
        }
        protected void RestoreGUIColor()
        {
            if (_guiColors == null)
            {
                LegacyLog.Warning("Colors is null!");
            }
            else if (_guiColors.Count > 0)
            {
                GUI.color = _guiColors.Pop();
            }
            else
            {
                LegacyLog.Warning("Colors is empty!");
            }
        }
        #endregion

        #region Layout
        protected static void BeginHorizontal()
        {
            GUILayout.BeginHorizontal();
        }

        protected static void EndHorizontal()
        {
            GUILayout.EndHorizontal();
        }

        protected static void BeginVertical()
        {
            GUILayout.BeginVertical();
        }

        protected static void EndVertical()
        {
            GUILayout.EndVertical();
        }

        protected static void LayoutHorizontal(Callback callback)
        {
            GUIHelper.LayoutHorizontal(callback);
        }

        protected static void LayoutLeft(Callback callback)
        {
            GUIHelper.LayoutLeft(callback);
        }

        protected static void LayoutVertical(Callback callback)
        {
            GUIHelper.LayoutVertical(callback);
        }

        protected static void Space(float pixels)
        {
            GUILayout.Space(pixels);
        }

        protected static void FlexibleSpace()
        {
            GUILayout.FlexibleSpace();
        }

        protected static void Line(float width = -1, float height = 1)
        {
            GUIHelper.Line(width, height);
        }

        protected static void LayoutList<T>(string label, List<T> list, float itemWidth = 0) where T : IGUIController
        {
            GUIHelper.LayoutList(label, list, itemWidth);
        }
        #endregion

        #region Controls
        protected static void Label(string text, params GUILayoutOption[] options)
        {
            GUILayout.Label(text, options);
        }

        protected static void Label(GUIContent content, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, options);
        }

        protected static void Label(Rect rect, GUIContent content)
        {
            GUI.Label(rect, content);
        }

        protected static void Label(Rect rect, float margins, GUIContent content)
        {
            GUI.Label(rect.AddMargins(margins), content);
        }

        protected static bool Button(string text)
        {
            return GUIHelper.Button(text);
        }

        protected static bool Button(string text, ref float buttonWidth)
        {
            return GUIHelper.Button(text, ref buttonWidth);
        }

        protected static bool Toggle(bool value, string text, params GUILayoutOption[] options)
        {
            return GUILayout.Toggle(value, text, options);
        }

        protected static bool Toggle(bool value, GUIContent content, params GUILayoutOption[] options)
        {
            return GUILayout.Toggle(value, content, options);
        }

        protected static void Toggle(ref bool value, string text, params GUILayoutOption[] options)
        {
            value = GUILayout.Toggle(value, text, options);
        }

        protected static void Toggle(ref bool value, GUIContent content, params GUILayoutOption[] options)
        {
            value = GUILayout.Toggle(value, content, options);
        }

        /// <summary>
        /// Returns true if value changed.
        /// </summary>
        protected static bool CheckToggle(ref bool value, GUIContent content, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, content, options);
            if (newValue != value)
            {
                value = newValue;
                return true;
            }
            return false;
        }

        protected static bool Toggle(Rect rect, bool value, string text)
        {
            return GUI.Toggle(rect, value, text);
        }

        protected static bool Toggle(Rect rect, bool value, GUIContent content)
        {
            return GUI.Toggle(rect, value, content);
        }

        protected static void Toggle(ref bool value, Rect rect, GUIContent content)
        {
            value = GUI.Toggle(rect, value, content);
        }

        /// <summary>
        /// Returns true if value changed.
        /// </summary>
        protected static bool CheckToggle(ref bool value, Rect rect, GUIContent content)
        {
            bool newValue = GUI.Toggle(rect, value, content);
            if (newValue != value)
            {
                value = newValue;
                return true;
            }
            return false;
        }

        protected static void Box(string text, params GUILayoutOption[] options)
        {
            GUILayout.Box(text, options);
        }

        protected static void Box(GUIContent content, params GUILayoutOption[] options)
        {
            GUILayout.Box(content, options);
        }

        protected static void Box(Rect rect, GUIContent content)
        {
            GUI.Box(rect, content);
        }
        #endregion

        #region Buttons
        protected static bool ButtonAdd()
        {
            return GUIHelper.ButtonAdd();
        }

        protected static bool ButtonRemove()
        {
            return GUIHelper.ButtonRemove();
        }

        protected static bool ButtonCopy()
        {
            return GUIHelper.ButtonCopy();
        }

        protected static bool ButtonPaste()
        {
            return GUIHelper.ButtonPaste();
        }

        protected static bool ButtonClear()
        {
            return GUIHelper.ButtonClear();
        }

        protected static bool ButtonSwap()
        {
            return GUIHelper.ButtonSwap();
        }
        #endregion
    }
}
using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        static readonly float ToggleWidth = 15;
        static readonly float EnumDropdownWidth = 20;

        public static float GetLabelWidth(string text)
        {
            _content.text = text;
            return LabelStyle.CalcSize(_content).x;
        }

        public static float GetLabelWidth(GUIContent content)
        {
            return content != null ? LabelStyle.CalcSize(content).x : 0;
        }

        /// <summary>
        /// Returns max width.
        /// </summary>
        public static float GetLabelWidth(string text1, string text2)
        {
            return Mathf.Max(GetLabelWidth(text1), GetLabelWidth(text2));
        }

        public static float GetButtonWidth(string text)
        {
            _content.text = text;
            return ButtonStyle.CalcSize(_content).x;
        }

        public static float GetButtonWidth(GUIContent content)
        {
            return ButtonStyle.CalcSize(content).x;
        }

        public static float GetEnumWidth<T>() where T : Enum
        {
            var names = Enum.GetNames(typeof(T));
            return GetEnumWidth(names);
        }

        public static float GetEnumWidth(string[] options)
        {
            int length = options.GetLength();
            if (length == 0) return 0;

            var style = LabelStyle;
            _content.text = options[0];
            float maxWidth = style.CalcSize(_content).x;
            for (int i = 1; i < length; i++)
            {
                _content.text = options[i];
                maxWidth = Mathf.Max(maxWidth, style.CalcSize(_content).x);
            }
            return maxWidth + EnumDropdownWidth;
        }
    }
}
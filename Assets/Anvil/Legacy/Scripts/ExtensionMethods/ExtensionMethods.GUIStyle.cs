using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static GUIStyle SetBold(this GUIStyle style)
        {
            style.fontStyle = FontStyle.Bold;
            return style;
        }

        public static GUIStyle SetItalic(this GUIStyle style)
        {
            style.fontStyle = FontStyle.Italic;
            return style;
        }

        public static GUIStyle SetFontSize(this GUIStyle style, int fontSize)
        {
            style.fontSize = fontSize;
            return style;
        }

        public static GUIStyle SetAlignment(this GUIStyle style, TextAnchor alignment)
        {
            style.alignment = alignment;
            return style;
        }

        public static GUIStyle SetTextColor(this GUIStyle style, int r, int g, int b)
        {
            return SetTextColor(style, Helper.CreateColor(r, g, b));
        }

        public static GUIStyle SetTextColor(this GUIStyle style, Color color)
        {
            style.normal.textColor = color;
            style.active.textColor = color;
            style.focused.textColor = color;
            style.hover.textColor = color;
            style.onNormal.textColor = color;
            style.onActive.textColor = color;
            style.onFocused.textColor = color;
            style.onHover.textColor = color;
            return style;
        }

        public static GUIStyle SetBackgroundColor(this GUIStyle style, int r, int g, int b)
        {
            return SetBackgroundColor(style, Helper.CreateColor(r, g, b));
        }

        public static GUIStyle SetBackgroundColor(this GUIStyle style, Color color)
        {
            style.normal.background = GUIHelper.GetBackground(color);
            return style;
        }

        public static GUIStyle SetPaddingRight(this GUIStyle style, int padding)
        {
            style.padding.right = padding;
            return style;
        }

        public static GUIStyle SetPaddingHorizontal(this GUIStyle style, int padding)
        {
            var paddings = style.padding;
            paddings.left = paddings.right = padding;
            return style;
        }

        public static GUIStyle SetPaddings(this GUIStyle style, int padding)
        {
            var paddings = style.padding;
            paddings.left = paddings.right = padding;
            paddings.top = paddings.bottom = padding;
            return style;
        }

        public static GUIStyle SetMarginRight(this GUIStyle style, int padding)
        {
            style.margin.right = padding;
            return style;
        }

        public static GUIStyle SetMarginHorizontal(this GUIStyle style, int margin)
        {
            var margins = style.margin;
            margins.left = margins.right = margin;
            return style;
        }

        public static GUIStyle SetMarginVertical(this GUIStyle style, int margin)
        {
            var margins = style.margin;
            margins.top = margins.bottom = margin;
            return style;
        }

        public static GUIStyle SetMargins(this GUIStyle style, int margin)
        {
            var margins = style.margin;
            margins.left = margins.right = margin;
            margins.top = margins.bottom = margin;
            return style;
        }

        public static GUIStyle SetMargins(this GUIStyle style, int horizontal, int vertical)
        {
            var margins = style.margin;
            margins.left = margins.right = horizontal;
            margins.top = margins.bottom = vertical;
            return style;
        }

        public static GUIStyle RemoveMarginsTopBottom(this GUIStyle style)
        {
            var margin = style.margin;
            margin.top = margin.bottom = 0;
            return style;
        }

        public static GUIStyle RemoveMargins(this GUIStyle style)
        {
            var margin = style.margin;
            margin.left = margin.right = margin.top = margin.bottom = 0;
            return style;
        }

        public static float GetWidth(this GUIStyle style, string text)
        {
            return style.CalcSize(new GUIContent(text)).x;
        }

        public static float GetEnumMaxWidth<T>(this GUIStyle enumStyle) where T : Enum
        {
            var names = Enum.GetNames(typeof(T));
            float maxWidth = -1;
            int count = names.Length;
            for (int i = 0; i < count; i++)
            {
                float width = GetWidth(enumStyle, names[i]);
                if (width > maxWidth)
                {
                    maxWidth = width;
                }
            }

            return maxWidth + 8; // Right padding
        }
    }
}
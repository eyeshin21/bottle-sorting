using UnityEngine;
using TMPro;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static Vector2 GetSize(this TMP_Text tmpText)
        {
            //return tmpText.GetRenderedValues(true);
            //return tmpText.textBounds.size;
            return tmpText.bounds.size;
        }

        public static Vector2 GetSize(this TMP_Text tmpText, string text)
        {
            return tmpText.GetPreferredValues(text);
        }

        public static void SetAutoSize(this TMP_Text tmpText, bool autoSize)
        {
            tmpText.enableAutoSizing = autoSize;
        }

        public static TextAlignmentOptions ToTextAlignmentOptions(this TextAnchor textAnchor)
        {
            if (textAnchor == TextAnchor.UpperLeft) return TextAlignmentOptions.TopLeft;
            if (textAnchor == TextAnchor.UpperCenter) return TextAlignmentOptions.Top;
            if (textAnchor == TextAnchor.UpperRight) return TextAlignmentOptions.TopRight;

            if (textAnchor == TextAnchor.MiddleLeft) return TextAlignmentOptions.Left;
            if (textAnchor == TextAnchor.MiddleCenter) return TextAlignmentOptions.Center;
            if (textAnchor == TextAnchor.MiddleRight) return TextAlignmentOptions.Right;

            if (textAnchor == TextAnchor.LowerLeft) return TextAlignmentOptions.BottomLeft;
            if (textAnchor == TextAnchor.LowerCenter) return TextAlignmentOptions.Bottom;
            if (textAnchor == TextAnchor.LowerRight) return TextAlignmentOptions.BottomRight;

            Assert.Todo(textAnchor);
            return TextAlignmentOptions.Center;
        }

        public static TextAnchor ToTextAnchor(this TextAlignmentOptions alignmentOptions)
        {
            if (alignmentOptions == TextAlignmentOptions.TopLeft) return TextAnchor.UpperLeft;
            if (alignmentOptions == TextAlignmentOptions.Top) return TextAnchor.UpperCenter;
            if (alignmentOptions == TextAlignmentOptions.TopRight) return TextAnchor.UpperRight;

            if (alignmentOptions == TextAlignmentOptions.Left) return TextAnchor.MiddleLeft;
            if (alignmentOptions == TextAlignmentOptions.Center) return TextAnchor.MiddleCenter;
            if (alignmentOptions == TextAlignmentOptions.Right) return TextAnchor.MiddleRight;

            if (alignmentOptions == TextAlignmentOptions.BottomLeft) return TextAnchor.LowerLeft;
            if (alignmentOptions == TextAlignmentOptions.Bottom) return TextAnchor.LowerCenter;
            if (alignmentOptions == TextAlignmentOptions.BottomRight) return TextAnchor.LowerRight;

            Assert.Todo(alignmentOptions);
            return TextAnchor.MiddleCenter;
        }
    }
}
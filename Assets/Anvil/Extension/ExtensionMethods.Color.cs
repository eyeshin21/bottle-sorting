using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static bool IsBlack(this Color color)
        {
            return color.r == 0 && color.g == 0 && color.b == 0;
        }

        public static Vector3 GetRGB(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static Color SetRGB(this Color color, Vector3 rgb)
        {
            color.r = rgb.x;
            color.g = rgb.y;
            color.b = rgb.z;
            return color;
        }

        public static float GetAlpha(this Color color)
        {
            return color.a;
        }

        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Color GetColor(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        public static Color GetReverseRGB(this Color color)
        {
            color.r = 1 - color.r;
            color.g = 1 - color.g;
            color.b = 1 - color.b;
            return color;
        }

        /// <summary>
        /// Returns RRGGBB or RRGGBBAA
        /// </summary>
        public static string ToHexadecimalString(this Color color)
        {
            return color.a != 1 ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color);
        }

        /// <summary>
        /// Returns RRGGBB or RRGGBBAA
        /// </summary>
        public static string ToHexadecimalString(this Color? color)
        {
            return color.HasValue ? ToHexadecimalString(color.Value) : string.Empty;
        }
    }
}
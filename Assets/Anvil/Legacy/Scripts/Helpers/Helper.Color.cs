using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static readonly float ByteInverse = 1f / 255;

        static float ConvertToFloat(int rgba)
        {
            return rgba <= 0 ? 0f : (rgba >= 1 ? 1f : rgba * ByteInverse);
        }

        public static Color CreateColor(int rgb, int a = 255)
        {
            float value = ConvertToFloat(rgb);
            return new Color(value, value, value, ConvertToFloat(a));
        }

        public static Color CreateColor(int r, int g, int b, int a = 255)
        {
            return new Color(ConvertToFloat(r), ConvertToFloat(g), ConvertToFloat(b), ConvertToFloat(a));
        }

        public static Color CreateColor(float rgb, float a = 1f)
        {
            return new Color(rgb, rgb, rgb, a);
        }

        public static void CopyRGB(Color color)
        {
            int r = RoundToInt(color.r * 255);
            int g = RoundToInt(color.g * 255);
            int b = RoundToInt(color.b * 255);
            var s = $"{r}, {g}, {b}";
            s.CopyToClipboard();
        }

        public static Color[] CreateDefaultColors<T>() where T : struct
        {
            return CreateDefaultColors(GetEnumCount<T>());
        }

        public static Color[] CreateDefaultColors(int count)
        {
            var colors = new Color[count];
            for (int i = 0; i < count; i++)
            {
                colors[i] = Color.white;
            }
            return colors;
        }

        public static DoubleColor[] CreateDoubleColors(int count)
        {
            var colors = new DoubleColor[count];
            for (int i = 0; i < count; i++)
            {
                var color = new DoubleColor();
                color.Reset();
                colors[i] = color;
            }
            return colors;
        }

        public static TripleColor[] CreateTripleColors(int count)
        {
            var colors = new TripleColor[count];
            for (int i = 0; i < count; i++)
            {
                var color = new TripleColor();
                color.Reset();
                colors[i] = color;
            }
            return colors;
        }
    }
}
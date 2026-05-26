using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        static readonly int FloatToInt = 1000000;
        static readonly int FloatLength = 6;

        public static bool IsEven(this int n)
        {
            return (n & 1) == 0;
        }

        public static bool IsEquals(this float a, float b)
        {
            return Mathf.Approximately(a, b);
        }

        public static float GetSquare(this float value)
        {
            return value * value;
        }

        /// <summary>
        /// Angle is in degrees.
        /// </summary>
        public static Quaternion ToRotation(this float angle)
        {
            return Quaternion.Euler(0, 0, angle);
        }

        /// <summary>
        /// Invariant culture.
        /// </summary>
        public static string ToString2(this float value)
        {
            int intValue = Helper.RoundToInt(value * FloatToInt);
            if (intValue > 0)
            {
                return _ToString2(intValue);
            }

            if (intValue < 0)
            {
                return "-" + _ToString2(-intValue);
            }

            return "0";
        }

        static string _ToString2(int value)
        {
            var s = value.ToString();
            int length = s.Length;
            if (length > FloatLength)
            {
                int intLength = length - FloatLength;
                bool hasFloat = false;
                for (int i = intLength; i < length; i++)
                {
                    if (s[i] != '0')
                    {
                        hasFloat = true;
                        break;
                    }
                }
                return hasFloat ? $"{s.Substring(0, intLength)}.{s.Substring(intLength)}" : s.Substring(0, intLength);
            }
            return "0." + s;
        }

        //public static string ToString3(this float value)
        //{
        //    var s = value.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture);
        //    int length = s.Length;
        //    if (s[length - 1] == '0')
        //    {
        //        int endIndex = length - 2;
        //        while (endIndex > 0 && s[endIndex] == '0')
        //        {
        //            endIndex--;
        //        }
        //        if (endIndex > 0 && s[endIndex] == '.')
        //        {
        //            endIndex--;
        //        }
        //        return s.Substring(0, endIndex + 1);
        //    }
        //    return s;
        //}

        public static string ToString2(this double value)
        {
            return ToString2((float)value);
        }

        //#if UNITY_EDITOR
        //        [UnityEditor.MenuItem("Test/Float")]
        //        static void TestFloat()
        //        {
        //            bool isOk = true;
        //            for (int i = 0; i < 1000; i++)
        //            {
        //                float value = Random.Range(-100, 100);
        //                var s2 = value.ToString2();
        //                var s3 = value.ToString3();
        //                if (s2 != s3)
        //                {
        //                   LegacyLog.Warning($"{s2} vs {s3}");
        //                    isOk = false;
        //                }
        //            }

        //            if (isOk)
        //            {
        //               LegacyLog.Debug("Test Float: OK!");
        //            }
        //        }
        //#endif
    }
}
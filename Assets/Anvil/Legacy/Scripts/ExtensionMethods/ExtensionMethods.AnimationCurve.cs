using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static float GetEndTime(this AnimationCurve curve)
        {
            var keys = curve.keys;
            int count = keys.Length;
            return keys[count - 1].time;
        }

        public static float GetEndValue(this AnimationCurve curve)
        {
            var keys = curve.keys;
            int count = keys.Length;
            return keys[count - 1].value;
        }

        public static bool IsTime01(this AnimationCurve curve)
        {
            var keys = curve.keys;
            int count = keys.Length;
            for (int i = 0; i < count; i++)
            {
                float time = keys[i].time;
                if (time < 0 || time > 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using Anvil;

namespace Anvil
{
    public enum Random3
    {
        Random1,
        Random2,
        Random3,
    }
    public static partial class Helper
    {
        static List<int> _indices = new();
        static List<int> _indices2 = new();
        static List<int> _indices3 = new();

        public static bool GetRandomBool()
        {
            return Random.value > 0.5f;
        }

        public static bool GetRandomBool(int ratio)
        {
            if (ratio <= 0) return false;
            if (ratio >= 100) return true;
            return Random.value * 100 <= ratio;
        }

        /// <summary>
        /// Returns random angle between [0, 360]
        /// </summary>
        public static float GetRandomAngle()
        {
            return Random.Range(0f, 360f);
        }

        public static Quaternion GetRandomRotation()
        {
            return Quaternion.Euler(0, 0, GetRandomAngle());
        }

        /// <summary>
        /// Returns random value in [0,count-1]
        /// </summary>
        public static int GetRandomIndex(int count)
        {
            return count > 1 ? Random.Range(0, count) : 0;
        }

        public static T GetRandomRange<T>(T minInclusive, T maxInclusive) where T : System.Enum
        {
            int value = GetRandomRange((int)(object)minInclusive, (int)(object)maxInclusive);
            return (T)(object)value;
        }

        /// <summary>
        /// Returns a random int within [minInclusive..maxInclusive].
        /// Checks to swap min-max.
        /// </summary>
        public static int GetRandomRange(int minInclusive, int maxInclusive)
        {
            return minInclusive == maxInclusive ? minInclusive : (minInclusive < maxInclusive ? Random.Range(minInclusive, maxInclusive + 1) : Random.Range(maxInclusive, minInclusive + 1));
        }

        /// <summary>
        /// Returns a random float within [minInclusive..maxInclusive].
        /// Checks to swap min-max.
        /// </summary>
        public static float GetRandomRange(float minInclusive, float maxInclusive)
        {
            return minInclusive < maxInclusive ? Random.Range(minInclusive, maxInclusive) : Random.Range(maxInclusive, minInclusive);
        }

        public static float GetRandom(float value1, float value2, float value3)
        {
            int rnd = GetRandomRange(1, 3);
            return rnd == 1 ? value1 : (rnd == 2 ? value2 : value3);
        }
    }
}
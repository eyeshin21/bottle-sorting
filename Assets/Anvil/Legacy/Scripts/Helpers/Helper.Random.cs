using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
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

        public static List<int> GetRandomIndices(int count)
        {
            _indices.SetRandomIndices(count);
            return _indices;
        }

        public static List<int> GetRandomIndices2(int count)
        {
            _indices2.SetRandomIndices(count);
            return _indices2;
        }

        public static List<int> GetRandomIndices3(int count)
        {
            _indices3.SetRandomIndices(count);
            return _indices3;
        }

        public static void GetRandomIndices(ref List<int> indices, int count)
        {
            if (count > 0)
            {
                int missing;
                if (indices == null)
                {
                    indices = new List<int>();
                    missing = count;
                }
                else
                {
                    missing = count - indices.Count;
                }

                if (missing > 0)
                {
                    for (int i = 0; i < missing; i++)
                    {
                        indices.Add(0);
                    }
                }

                for (int i = 0; i < count; i++)
                {
                    indices[i] = i;
                }

                indices.Swap(count);
            }
            else
            {
                LegacyLog.Warning($"Invalid count: {count}");
            }
        }

        public static void GetRandomIndices(int count, Random3 random, out List<int> indices1, out List<int> indices2, out List<int> indices3)
        {
            _indices.SetIndices(count);
            _indices2.SetIndices(count);
            _indices3.SetMinCount(count);
            if (random == Random3.Random1)
            {
                _indices.SwapDiff();
            }
            else if (random == Random3.Random2)
            {
                _indices2.SwapDiff();
            }
            else
            {
                Assert.IsEquals(random, Random3.Random3);
                _indices.SwapDiff();
                _indices2.SwapDiff();
            }

            // Set target indices
            for (int i = 0; i < count; i++)
            {
                int index = _indices[i];
#if DEBUG_MODE
                bool found = false;
#endif
                for (int j = 0; j < count; j++)
                {
                    if (_indices2[j] == index)
                    {
                        _indices3[i] = j;
#if DEBUG_MODE
                        found = true;
#endif
                        break;
                    }
                }
#if DEBUG_MODE
                if (!found)
                {
                   LegacyLog.Error($"Not found target for index {index}!");
                }
#endif
            }

            indices1 = _indices;
            indices2 = _indices2;
            indices3 = _indices3;
        }
    }
}
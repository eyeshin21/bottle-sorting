using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static bool RandomBool()
        {
            return Random.value > 0.5f;
        }

        public static bool RandomBool(int ratio)
        {
            if (ratio <= 0) return false;
            if (ratio >= 100) return true;
            return Random.value * 100 <= ratio;
        }

        /// <summary>
        /// Returns random value in [0,count-1]
        /// </summary>
        public static int RandomIndex(int count)
        {
            return count > 1 ? Random.Range(0, count) : 0;
        }

        /// <summary>
        /// Returns random value in [min,max]
        /// </summary>
        public static int RandomRange(int min, int max)
        {
            return min == max ? min : Random.Range(min, max + 1);
        }

        public static Vector2 GetRandom(Vector2 v, Vector2 v2)
        {
            return new Vector2(Random.Range(v.x, v2.x), Random.Range(v.y, v2.y));
        }

        public static void GetRandomDirections(ref Direction[] directions)
        {
            if (directions == null)
            {
                directions = new Direction[] { Direction.Left, Direction.Up, Direction.Right, Direction.Down };
            }
            directions.Swap();
        }

        public static bool GetRandom<T>(List<T> list, Func<T, int> weightFunc, out T item)
        {
            int count = list.GetCount();
            if (count > 0)
            {
                int totalWeight = 0;
                for (int i = 0; i < count; i++)
                {
                    int weight = weightFunc(list[i]);
                    if (weight > 0)
                    {
                        totalWeight += weight;
                    }
                }

                if (totalWeight > 0)
                {
                    int rand = Random.Range(0, totalWeight + 1);
                    for (int i = 0; i < count; i++)
                    {
                        var item2 = list[i];
                        int weight = weightFunc(item2);
                        if (weight > 0)
                        {
                            if (rand <= weight)
                            {
                                item = item2;
                                return true;
                            }

                            rand -= weight;
                        }
                    }

                    // Last
                    item = list[count - 1];
                    return true;
                }
            }

            item = default;
            return false;
        }

        public static bool GetRandom<T>(List<T> list, ref int totalWeight, Func<T, int> weightFunc, out T item)
        {
            int count = list.GetCount();
            if (count > 0)
            {
                if (totalWeight < 1)
                {
                    totalWeight = 0;
                    for (int i = 0; i < count; i++)
                    {
                        int weight = weightFunc(list[i]);
                        if (weight > 0)
                        {
                            totalWeight += weight;
                        }
                    }
                }

                if (totalWeight > 0)
                {
                    int rand = Random.Range(0, totalWeight + 1);
                    for (int i = 0; i < count; i++)
                    {
                        var item2 = list[i];
                        int weight = weightFunc(item2);
                        if (weight > 0)
                        {
                            if (rand <= weight)
                            {
                                item = item2;
                                return true;
                            }

                            rand -= weight;
                        }
                    }

                    // Last
                    item = list[count - 1];
                    return true;
                }
            }

            item = default;
            return false;
        }

        public static int[] CreateRandomValues(int start, int end)
        {
            int count = end - start + 1;
            var values = new int[count];
            int index = 0;
            for (int i = start; i <= end; i++)
            {
                values[index++] = i;
            }
            values.Swap();
            return values;
        }
    }
}
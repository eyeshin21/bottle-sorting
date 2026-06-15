using UnityEngine;
using System.Collections.Generic;

namespace Anvil
{
    public static partial class Helper
    {
        public static bool RandomBool()
        {
            return Random.value > 0.5f;
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
    }
}
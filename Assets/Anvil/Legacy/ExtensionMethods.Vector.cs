using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static Vector3 AddDeltaX(this Vector3 v, float deltaX)
        {
            return new Vector3(v.x + deltaX, v.y, v.z);
        }

        public static Vector3 AddDeltaY(this Vector3 v, float deltaY)
        {
            return new Vector3(v.x, v.y + deltaY, v.z);
        }

        public static Vector3 Add(this Vector3 v, Vector2 v2)
        {
            return new Vector3(v.x + v2.x, v.y + v2.y, v.z);
        }

        public static bool IsLessThan(this Vector3 v, Vector3 v2, float maxDistance)
        {
            float deltaX = v2.x - v.x;
            float deltaY = v2.y - v.y;
            return deltaX * deltaX + deltaY * deltaY < maxDistance * maxDistance;
        }

        public static bool IsLessThanOrEqual(this Vector3 v, Vector3 v2, float maxDistance)
        {
            float deltaX = v2.x - v.x;
            float deltaY = v2.y - v.y;
            return deltaX * deltaX + deltaY * deltaY <= maxDistance * maxDistance;
        }

        public static bool IsGreaterThan(this Vector3 v, Vector3 v2, float minDistance)
        {
            float deltaX = v2.x - v.x;
            float deltaY = v2.y - v.y;
            return deltaX * deltaX + deltaY * deltaY > minDistance * minDistance;
        }

        public static bool IsGreaterThanOrEqual(this Vector3 v, Vector3 v2, float minDistance)
        {
            float deltaX = v2.x - v.x;
            float deltaY = v2.y - v.y;
            return deltaX * deltaX + deltaY * deltaY >= minDistance * minDistance;
        }

        public static void Reverse(this Vector3 pos, ref float left, ref float top, ref float right, ref float bottom)
        {
            left -= pos.x;
            right -= pos.x;
            top -= pos.y;
            bottom -= pos.y;
        }
    }
}
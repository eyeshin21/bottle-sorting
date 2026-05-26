using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static Vector2 GetValue(this Vector2? v)
        {
            return v.HasValue ? v.Value : Vector2.zero;
        }

        public static Vector3 GetValue(this Vector3? v)
        {
            return v.HasValue ? v.Value : Vector3.zero;
        }

        public static bool IsEquals(this Vector3 v, Vector3 v2)
        {
            return Mathf.Approximately(v.x, v2.x) && Mathf.Approximately(v.y, v2.y) && Mathf.Approximately(v.z, v2.z);
        }

        public static float GetDistance(this Vector3 v, Vector3 v2)
        {
            float deltaX = v2.x - v.x;
            float deltaY = v2.y - v.y;
            return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static float GetDistanceSquare(this Vector3 v, Vector3 v2)
        {
            float deltaX = v2.x - v.x;
            float deltaY = v2.y - v.y;
            return deltaX * deltaX + deltaY * deltaY;
        }

        public static Vector2 AddDeltaX(this Vector2 v, float deltaX)
        {
            v.x += deltaX;
            return v;
        }

        public static Vector2 AddDeltaY(this Vector2 v, float deltaY)
        {
            v.y += deltaY;
            return v;
        }

        public static Vector2 AddDelta(this Vector2 v, float deltaX, float deltaY)
        {
            v.x += deltaX;
            v.y += deltaY;
            return v;
        }

        public static Vector3 AddDeltaX(this Vector3 v, float deltaX)
        {
            v.x += deltaX;
            return v;
        }

        public static Vector3 AddDeltaY(this Vector3 v, float deltaY)
        {
            v.y += deltaY;
            return v;
        }

        public static Vector3 AddDelta(this Vector3 v, float deltaX, float deltaY)
        {
            v.x += deltaX;
            v.y += deltaY;
            return v;
        }

        public static Vector3 Add(this Vector3 v, Vector2 v2)
        {
            v.x += v2.x;
            v.y += v2.y;
            return v;
        }

        public static Vector3 Add(this Vector3 v, Vector3 v2)
        {
            v.x += v2.x;
            v.y += v2.y;
            v.z += v2.z;
            return v;
        }

        public static Vector3 Add(this Vector3 v, Vector2 v2, Vector3 scale)
        {
            v.x += v2.x * scale.x;
            v.y += v2.y * scale.y;
            return v;
        }

        public static Vector3 Add(this Vector3 v, Vector3 v2, Vector3 scale)
        {
            v.x += v2.x * scale.x;
            v.y += v2.y * scale.y;
            v.z += v2.z * scale.z;
            return v;
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
    }
}
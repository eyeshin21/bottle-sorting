using System.Collections.Generic;
using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        public static bool IsPointInsideRect(this Vector3 point, Vector3 rectCenter, float rectWidth, float rectHeight)
        {
            float left = rectCenter.x - rectWidth / 2;
            float right = rectCenter.x + rectWidth / 2;
            float top = rectCenter.y + rectHeight / 2;
            float bottom = rectCenter.y - rectHeight / 2;
            return point.x >= left && point.x <= right && point.y >= bottom && point.y <= top;
        }
        public static Vector2 xy(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
        public static Vector2 xz(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
        public static Vector3 xz3(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }
        public static Vector3 To3DVector3(this Vector2 v2)
        {
            return new Vector3(v2.x, 0, v2.y);
        }

    }
}

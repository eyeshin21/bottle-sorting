using UnityEngine;

namespace Anvil.Legacy
{
    public enum CornerType
    {
        None,

        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,

        // Light/Dark
        Bottom
    }

    public static partial class ExtensionMethods
    {
        public static Vector3 GetScaleFromTopLeft(this CornerType cornerType)
        {
            if (cornerType == CornerType.TopRight) return new Vector3(-1, 1, 1);
            if (cornerType == CornerType.BottomLeft) return new Vector3(1, -1, 1);
            if (cornerType == CornerType.BottomRight) return new Vector3(-1, -1, 1);
            return Vector3.one;
        }

        public static Quaternion GetRotationFromTopLeft(this CornerType cornerType)
        {
            if (cornerType == CornerType.TopRight) return Quaternion.Euler(0, 0, -90);
            if (cornerType == CornerType.BottomLeft) return Quaternion.Euler(0, 0, 90);
            if (cornerType == CornerType.BottomRight) return Quaternion.Euler(0, 0, 180);
            return Quaternion.identity;
        }
    }
}
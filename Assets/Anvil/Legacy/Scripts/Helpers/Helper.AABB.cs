using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static void GetAABB(float width, float height, out float left, out float top, out float right, out float bottom)
        {
            left = -width * 0.5f;
            right = left + width;
            bottom = -height * 0.5f;
            top = bottom + height;
        }
    }
}
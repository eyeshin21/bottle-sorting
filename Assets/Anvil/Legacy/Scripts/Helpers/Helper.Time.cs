using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static float GetDuration(Vector3 startPos, Vector3 endPos, float speed)
        {
            return GetDistance(startPos, endPos) / speed;
        }
    }
}
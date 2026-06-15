using Anvil;
using UnityEngine;

namespace Anvil
{
    public static partial class GizmosHelper
    {
        static void _DrawAABB(float left, float top, float right, float bottom)
        {
            var pos1 = new Vector3(left, top);
            var pos2 = new Vector3(right, top);
            var pos3 = new Vector3(right, bottom);
            var pos4 = new Vector3(left, bottom);
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawLine(pos2, pos3);
            Gizmos.DrawLine(pos3, pos4);
            Gizmos.DrawLine(pos4, pos1);
        }

        static void _DrawCrossAABB(float left, float top, float right, float bottom)
        {
            var pos1 = new Vector3(left, top);
            var pos2 = new Vector3(right, top);
            var pos3 = new Vector3(right, bottom);
            var pos4 = new Vector3(left, bottom);
            // Rect
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawLine(pos2, pos3);
            Gizmos.DrawLine(pos3, pos4);
            Gizmos.DrawLine(pos4, pos1);
            // Cross
            Gizmos.DrawLine(pos1, pos3);
            Gizmos.DrawLine(pos2, pos4);
        }

    }
}
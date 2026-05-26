#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class DebugHelper
    {
        const float DoublePI = Mathf.PI * 2.0f;
        const float AngleStep = 3f; // Degrees
        const float DefaultDuration = 1f;

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = DefaultDuration)
        {
            Debug.DrawLine(start, end, color, duration);
        }

        public static void DrawAABB(float left, float top, float right, float bottom, Color color, float duration = DefaultDuration)
        {
            var pos1 = new Vector3(left, top);
            var pos2 = new Vector3(right, top);
            var pos3 = new Vector3(right, bottom);
            var pos4 = new Vector3(left, bottom);
            DrawLine(pos1, pos2, color, duration);
            DrawLine(pos2, pos3, color, duration);
            DrawLine(pos3, pos4, color, duration);
            DrawLine(pos4, pos1, color, duration);
        }

        public static void DrawCircle(Vector3 centerPos, float radius, Color color, float duration = DefaultDuration)
        {
            DrawOval(centerPos, radius, radius, AngleStep, color, duration);
        }

        public static void DrawCircle(Vector3 centerPos, float radius, float angleStep, Color color, float duration = DefaultDuration)
        {
            DrawOval(centerPos, radius, radius, angleStep, color, duration);
        }

        public static void DrawOval(Vector3 centerPos, float xRadius, float yRadius, Color color, float duration = DefaultDuration)
        {
            DrawOval(centerPos, xRadius, yRadius, AngleStep, color, duration);
        }

        public static void DrawOval(Vector3 centerPos, float xRadius, float yRadius, float angleStep, Color color, float duration = DefaultDuration)
        {
            if (angleStep <= 0)
            {
                angleStep = AngleStep;
            }
            angleStep *= Mathf.Deg2Rad;

            var from = new Vector3(centerPos.x + xRadius, centerPos.y, centerPos.z);
            var to = Vector3.zero;
            float angle = 0;
            do
            {
                angle += angleStep;

                if (angle < DoublePI)
                {
                    to.x = centerPos.x + Mathf.Cos(angle) * xRadius;
                    to.y = centerPos.y + Mathf.Sin(angle) * yRadius;
                    DrawLine(from, to, color, duration);
                    from = to;
                }
                else
                {
                    to.x = centerPos.x + xRadius;
                    to.y = centerPos.y;
                    DrawLine(from, to, color, duration);
                    break;
                }
            }
            while (true);
        }

        public static void DrawPath(List<Vector3> points, Color color, float duration = DefaultDuration)
        {
            int count = points.GetCount();
            if (count > 1)
            {
                for (int i = 0; i < count - 1; i++)
                {
                    Debug.DrawLine(points[i], points[i + 1], color, duration);
                }
            }
        }
    }
}
#endif
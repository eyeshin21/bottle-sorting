#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class EditorContext
    {
        public static List<Vector3> Points { get; set; }
        public static bool ClosedPoints { get; set; }

        public static void SetPoints(Vector2[] points)
        {
            int count = points.GetLength();
            var newPoints = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                newPoints.Add(points[i]);
            }
            Points = newPoints;
        }

        public static void SetPoints(Vector3[] points)
        {
            Points = new List<Vector3>(points);
        }

        public static void SetPoints(Vector3[] points, int count)
        {
            count = Mathf.Min(count, points.GetLength());
            var list = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(points[i]);
            }
            Points = list;
        }
    }
}
#endif
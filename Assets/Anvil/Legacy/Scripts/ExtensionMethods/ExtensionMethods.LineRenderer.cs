using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static List<Vector3> GetPositions2(this LineRenderer lineRenderer)
        {
            if (lineRenderer != null)
            {
                int count = lineRenderer.positionCount;
                var positions = new Vector3[count];
                int count2 = lineRenderer.GetPositions(positions);
                if (count2 == count)
                {
                    return new List<Vector3>(positions);
                }
                LegacyLog.Warning($"count={count} vs count2={count2}");
                var list = new List<Vector3>(count2);
                for (int i = 0; i < count2; i++)
                {
                    list.Add(positions[i]);
                }
                return list;
            }
            return null;
        }

        public static void SetPositions(this LineRenderer lineRenderer, List<Vector3> points, int count = -1)
        {
            if (lineRenderer != null)
            {
                int pointCount = points.GetCount();
                if (count < 0)
                {
                    count = pointCount;
                }
                else
                {
                    count = Mathf.Min(count, pointCount);
                }
                lineRenderer.positionCount = count;
                for (int i = 0; i < count; i++)
                {
                    lineRenderer.SetPosition(i, points[i]);
                }
            }
        }
    }
}
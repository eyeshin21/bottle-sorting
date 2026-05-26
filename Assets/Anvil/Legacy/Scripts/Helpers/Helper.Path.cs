using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static List<Vector3> GetPoints(List<Vector2> points, float maxDistance, bool closed = false)
        {
            if (points == null) return null;

            int count = points.Count;
            var list = new List<Vector3>(count);
            if (count == 0) return list;

            float maxDistanceSquare = maxDistance * maxDistance;
            var point = points[0];
            list.Add(point);
            float prevX = point.x;
            float prevY = point.y;
            int max = closed ? count : count - 1;
            for (int i = 1; i <= max; i++)
            {
                if (i < count)
                {
                    point = points[i];
                }
                else
                {
                    point = points[0];
                }
                float deltaX = point.x - prevX;
                float deltaY = point.y - prevY;
                float distanceSquare = deltaX * deltaX + deltaY * deltaY;
                if (distanceSquare > maxDistanceSquare)
                {
                    float distance = Mathf.Sqrt(distanceSquare);
                    float angle = Mathf.Atan2(deltaY, deltaX);
                    int segmentCount = Mathf.CeilToInt(distance / maxDistance);
                    float step = distance / segmentCount;
                    float stepX = step * Mathf.Cos(angle);
                    float stepY = step * Mathf.Sin(angle);
                    var midPoint = new Vector3(prevX, prevY);
                    for (int j = 1; j < segmentCount; j++)
                    {
                        midPoint.x += stepX;
                        midPoint.y += stepY;
                        list.Add(midPoint);
                    }
                }
                if (i < count)
                {
                    list.Add(point);
                    prevX = point.x;
                    prevY = point.y;
                }
            }

            return list;
        }

        public static List<Vector3> GetPoints(List<Vector3> points, float maxDistance, bool closed = false)
        {
            if (points == null) return null;

            int count = points.Count;
            var list = new List<Vector3>(count);
            if (count == 0) return list;

            float maxDistanceSquare = maxDistance * maxDistance;
            var point = points[0];
            list.Add(point);
            float prevX = point.x;
            float prevY = point.y;
            int max = closed ? count : count - 1;
            for (int i = 1; i <= max; i++)
            {
                if (i < count)
                {
                    point = points[i];
                }
                else
                {
                    point = points[0];
                }
                float deltaX = point.x - prevX;
                float deltaY = point.y - prevY;
                float distanceSquare = deltaX * deltaX + deltaY * deltaY;
                if (distanceSquare > maxDistanceSquare)
                {
                    float distance = Mathf.Sqrt(distanceSquare);
                    float angle = Mathf.Atan2(deltaY, deltaX);
                    int segmentCount = Mathf.CeilToInt(distance / maxDistance);
                    float step = distance / segmentCount;
                    float stepX = step * Mathf.Cos(angle);
                    float stepY = step * Mathf.Sin(angle);
                    var midPoint = new Vector3(prevX, prevY);
                    for (int j = 1; j < segmentCount; j++)
                    {
                        midPoint.x += stepX;
                        midPoint.y += stepY;
                        list.Add(midPoint);
                    }
                }
                if (i < count)
                {
                    list.Add(point);
                    prevX = point.x;
                    prevY = point.y;
                }
            }

            return list;
        }
    }
}
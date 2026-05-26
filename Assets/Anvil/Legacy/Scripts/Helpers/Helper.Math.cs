using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        /// <summary>
        /// Returns angle in degrees.
        /// </summary>
        public static float GetAngle(float deltaX, float deltaY)
        {
            float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }
            Assert.IsInRange(angle, 0, 360);
            return angle;
        }

        /// <summary>
        /// Returns angle in degrees.
        /// </summary>
        public static float GetAngle(Vector3 from, Vector3 to)
        {
            float deltaX = to.x - from.x;
            float deltaY = to.y - from.y;
            float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }
            Assert.IsInRange(angle, 0, 360);
            return angle;
        }

        /// <summary>
        /// Returns angle in degrees.
        /// </summary>
        public static void GetAngleAndLength(Vector3 from, Vector3 to, out float angle, out float length)
        {
            float deltaX = to.x - from.x;
            float deltaY = to.y - from.y;
            angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }
            Assert.IsInRange(angle, 0, 360);
            length = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static float GetQuadBezierLength(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float timeStep = 0.01f)
        {
            float length = 0;
            float prevX = startPos.x;
            float prevY = startPos.y;
            float t = timeStep;
            float deltaX, deltaY;

            while (t < 1)
            {
                float u = 1 - t;
                float a = u * u;
                float b = 2 * u * t;
                float c = t * t;
                float x = a * startPos.x + b * controlPos.x + c * endPos.x;
                float y = a * startPos.y + b * controlPos.y + c * endPos.y;
                deltaX = x - prevX;
                deltaY = y - prevY;
                length += Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
                prevX = x;
                prevY = y;
                t += timeStep;
            }

            deltaX = endPos.x - prevX;
            deltaY = endPos.y - prevY;
            length += Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

            return length;
        }

        public static float GetCubicBezierLength(Vector3 startPos, Vector3 control1Pos, Vector3 control2Pos, Vector3 endPos, float timeStep = 0.01f)
        {
            float length = 0;
            float prevX = startPos.x;
            float prevY = startPos.y;
            float t = timeStep;
            float deltaX, deltaY;

            while (t < 1)
            {
                float u = 1 - t;
                float a = u * u * u;
                float b = 3 * u * u * t;
                float c = 3 * u * t * t;
                float d = t * t * t;
                float x = a * startPos.x + b * control1Pos.x + c * control2Pos.x + d * endPos.x;
                float y = a * startPos.y + b * control1Pos.y + c * control2Pos.y + d * endPos.y;
                deltaX = x - prevX;
                deltaY = y - prevY;
                length += Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
                prevX = x;
                prevY = y;
                t += timeStep;
            }

            deltaX = endPos.x - prevX;
            deltaY = endPos.y - prevY;
            length += Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);

            return length;
        }

        /// <summary>
        /// > 0: point is on the counter-clockwise side of line (above)
        /// < 0: point is on the clockwise side of line (below)
        /// = 0: point is exactly on the line
        /// </summary>
        public static float GetCrossProduct(Vector3 v1, Vector3 v2, Vector3 point)
        {
            float deltaX = v2.x - v1.x;
            float deltaY = v2.y - v1.y;
            return deltaX * (point.y - v1.y) - deltaY * (point.x - v1.x);
        }

        public static bool IsCross(Vector3 v1, Vector3 v2, Vector3 point1, Vector3 point2)
        {
            float deltaX = v2.x - v1.x;
            float deltaY = v2.y - v1.y;
            float crossProduct1 = deltaX * (point1.y - v1.y) - deltaY * (point1.x - v1.x);
            float crossProduct2 = deltaX * (point2.y - v1.y) - deltaY * (point2.x - v1.x);
            return crossProduct1 < 0 ? crossProduct2 >= 0 : crossProduct2 < 0;
        }

        public static bool GetIntersect(Vector3 v11, Vector3 v12, Vector3 v21, Vector3 v22, out Vector3 point)
        {
            float a1 = v12.y - v11.y;
            float b1 = v11.x - v12.x;
            float a2 = v22.y - v21.y;
            float b2 = v21.x - v22.x;

            float det = a1 * b2 - a2 * b1;
            //if (det != 0)
            if (Mathf.Abs(det) >= 0.01f)
            {
                float c1 = a1 * v11.x + b1 * v11.y;
                float c2 = a2 * v21.x + b2 * v21.y;
                float x = (b2 * c1 - b1 * c2) / det;
                float y = (a1 * c2 - a2 * c1) / det;
                point = new Vector3(x, y);
                return true;
            }

            point = Vector3.zero;
            return false;
        }

        public static bool IsInsideCircle(Vector3 centerPos, float radius, Vector3 pos)
        {
            float deltaX = pos.x - centerPos.x;
            float deltaY = pos.y - centerPos.y;
            return deltaX * deltaX + deltaY * deltaY <= radius * radius;
        }

        public static bool IsInsideCircle2(Vector3 centerPos, float radiusSquare, Vector3 pos)
        {
            float deltaX = pos.x - centerPos.x;
            float deltaY = pos.y - centerPos.y;
            return deltaX * deltaX + deltaY * deltaY <= radiusSquare;
        }

        public static float GetTriangleArea(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return Mathf.Abs((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) * 0.5f);
        }

        public static bool IsInsideTriangle(Vector3 point, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float area = GetTriangleArea(p1, p2, p3) + 0.001f;
            float area2 = GetTriangleArea(point, p1, p2);
            if (area2 > area) return false;
            area2 += GetTriangleArea(point, p1, p3);
            if (area2 > area) return false;
            area2 += GetTriangleArea(point, p2, p3);
            if (area2 > area) return false;
            return true;
        }

        public static bool IsInsideRect(Vector3 point, Vector3 centerPos, float size)
        {
            return IsInsideRect(point, centerPos, size, size);
        }

        public static bool IsInsideRect(Vector3 point, Vector3 centerPos, float width, float height)
        {
            if (point.x < centerPos.x - width * 0.5f) return false;
            if (point.x > centerPos.x + width * 0.5f) return false;
            if (point.y < centerPos.y - height * 0.5f) return false;
            if (point.y > centerPos.y + height * 0.5f) return false;
            return true;
        }
    }
}
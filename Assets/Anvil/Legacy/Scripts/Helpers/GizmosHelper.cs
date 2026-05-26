using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class GizmosHelper
    {
        const float DoublePI = Mathf.PI * 2.0f;
        const float LineStep = 0.01f;
        const float AngleStep = 3f; // Degrees
        const float ArrowAngle = 30f; // Degrees
        const float ArrowLength = 0.1f;
        const float PointRadius = 0.025f;
        static readonly Color PointColor = Color.white;

        static void Draw(Color? color, Action callback)
        {
            if (color.HasValue)
            {
                var gizmosColor = Gizmos.color;
                Gizmos.color = color.Value;
                {
                    callback();
                }
                Gizmos.color = gizmosColor;
            }
            else
            {
                callback();
            }
        }

        public static void DrawHLine(float y, float x1, float x2, Color? color = null)
        {
            DrawLine(new Vector3(x1, y), new Vector3(x2, y), color);
        }

        public static void DrawVLine(float x, float y1, float y2, Color? color = null)
        {
            DrawLine(new Vector3(x, y1), new Vector3(x, y2), color);
        }

        //public static void DrawHorizontalLine(Transform transform, Color? color = null)
        //{
        //    if (transform != null)
        //    {
        //        DrawHorizontalLine(transform.position.y, color);
        //    }
        //}

        //public static void DrawHorizontalLine(float y, Color? color = null)
        //{
        //    float left = Context.MainCamera.GetLeft();
        //    float right = -left;
        //    DrawLine(new Vector3(left, y), new Vector3(right, y), color);
        //}

        public static void DrawLine(Transform startTransform, Transform endTransform, Color? color = null)
        {
            if (startTransform == null || endTransform == null) return;
            Draw(color, () =>
            {
                Gizmos.DrawLine(startTransform.position, endTransform.position);
            });
        }

        public static void DrawLine(Vector3 startPos, Vector3 endPos, Color? color = null)
        {
            Draw(color, () =>
            {
                Gizmos.DrawLine(startPos, endPos);
            });
        }

        public static void DrawLine(Vector3 startPos, Vector3 endPos, float thickness, Color? color = null)
        {
            Draw(color, () =>
            {
                int lineCount = Mathf.RoundToInt(thickness / LineStep);
                if (lineCount == 0)
                {
                    Gizmos.DrawLine(startPos, endPos);
                    return;
                }

                float lineStep = thickness / lineCount;
                float deltaX = endPos.x - startPos.x;
                float deltaY = endPos.y - startPos.y;
                float angle = Mathf.Atan2(deltaY, deltaX) + 90 * Mathf.Deg2Rad;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);
                float halfThickness = thickness * 0.5f;
                float x1 = startPos.x - cos * halfThickness;
                float y1 = startPos.y - sin * halfThickness;
                float x2 = endPos.x - cos * halfThickness;
                float y2 = endPos.y - sin * halfThickness;
                var pos1 = new Vector3(x1, y1);
                var pos2 = new Vector3(x2, y2);
                Gizmos.DrawLine(pos1, pos2);
                deltaX = cos * lineStep;
                deltaY = sin * lineStep;
                for (int i = 0; i < lineCount; i++)
                {
                    pos1.x += deltaX;
                    pos1.y += deltaY;
                    pos2.x += deltaX;
                    pos2.y += deltaY;
                    Gizmos.DrawLine(pos1, pos2);
                }
            });
        }

        public static void DrawLines(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color? color = null)
        {
            Draw(color, () =>
            {
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p3, p4);
                Gizmos.DrawLine(p4, p1);
            });
        }

        public static void DrawLines(List<Vector3> points, bool isLoop = false, Color? color = null)
        {
            int count = points != null ? points.Count : 0;
            if (count < 2) return;

            Draw(color, () =>
            {
                for (int i = 0; i < count - 1; i++)
                {
                    Gizmos.DrawLine(points[i], points[i + 1]);
                }

                if (isLoop && count > 2)
                {
                    Gizmos.DrawLine(points[count - 1], points[0]);
                }
            });
        }

        public static void DrawTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Color? color = null)
        {
            Draw(color, () =>
            {
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p3, p1);
            });
        }

        public static void DrawQuadrangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color? color = null)
        {
            Draw(color, () =>
            {
                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p3, p4);
                Gizmos.DrawLine(p4, p1);
            });
        }

        //public static void DrawOBB(BoxCollider2D collider, Color? color = null)
        //{
        //    if (collider != null)
        //    {
        //        Draw(color, () =>
        //        {
        //            collider.GetOBB(out Vector3 p1, out Vector3 p2, out Vector3 p3, out Vector3 p4);
        //            Gizmos.DrawLine(p1, p2);
        //            Gizmos.DrawLine(p2, p3);
        //            Gizmos.DrawLine(p3, p4);
        //            Gizmos.DrawLine(p4, p1);
        //        });
        //    }
        //}

        //public static void DrawOBB(Transform transform, float width, float height, Color? color = null, bool drawX = false)
        //{
        //    Draw(color, () =>
        //    {
        //        float halfWidth = width * 0.5f;
        //        float halfHeight = height * 0.5f;
        //        var pos1 = transform.TransformPoint(-halfWidth, halfHeight, 0);
        //        var pos2 = transform.TransformPoint(halfWidth, halfHeight, 0);
        //        var pos3 = transform.TransformPoint(halfWidth, -halfHeight, 0);
        //        var pos4 = transform.TransformPoint(-halfWidth, -halfHeight, 0);
        //        Gizmos.DrawLine(pos1, pos2);
        //        Gizmos.DrawLine(pos2, pos3);
        //        Gizmos.DrawLine(pos3, pos4);
        //        Gizmos.DrawLine(pos4, pos1);

        //        if (drawX)
        //        {
        //            Gizmos.DrawLine(pos1, pos3);
        //            Gizmos.DrawLine(pos2, pos4);
        //        }
        //    });
        //}

        public static void DrawCross(Vector3 centerPos, Color? color = null)
        {
            DrawCross(centerPos, 0.2f, 0.02f, color);
        }

        public static void DrawCross(Vector3 centerPos, float length, Color? color = null)
        {
            DrawCross(centerPos, length, 0.02f, color);
        }

        public static void DrawCross(Vector3 centerPos, float length, float thickness, Color? color = null)
        {
            Draw(color, () =>
            {
                float lineStep = 0.01f;
                int halfRepeatCount = Mathf.CeilToInt(thickness / lineStep);
                lineStep = thickness / halfRepeatCount;
                int repeatCount = halfRepeatCount * 2 + 1;

                var pos1 = centerPos;
                pos1.x -= length * 0.5f;
                var pos2 = pos1;
                pos2.x += length;
                pos1.y = pos2.y = pos1.y + lineStep * halfRepeatCount;
                for (int i = 0; i < repeatCount; i++)
                {
                    Gizmos.DrawLine(pos1, pos2);
                    pos1.y = pos2.y = pos1.y - lineStep;
                }

                pos1 = centerPos;
                pos1.y += length * 0.5f;
                pos2 = pos1;
                pos2.y = pos1.y - length;
                pos1.x = pos2.x = pos1.x - lineStep * halfRepeatCount;
                for (int i = 0; i < repeatCount; i++)
                {
                    Gizmos.DrawLine(pos1, pos2);
                    pos1.x = pos2.x = pos1.x + lineStep;
                }
            });
        }

        public static void DrawRect(float left, float top, float right, float bottom, Color? color = null)
        {
            DrawAABB(left, top, right, bottom, color);
        }

        public static void DrawRect(Vector3 centerPos, float width, float height, Color? color = null)
        {
            float left = centerPos.x - width * 0.5f;
            float right = left + width;
            float top = centerPos.y + height * 0.5f;
            float bottom = top - height;
            DrawAABB(left, top, right, bottom, color);
        }

        public static void DrawRect(Vector3 topLeft, Vector3 bottomRight, Color? color = null)
        {
            DrawAABB(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y, color);
        }

        public static void DrawRect(Rect rect, Color? color = null)
        {
            DrawAABB(rect.x, rect.y, rect.x + rect.width, rect.y - rect.height, color);
        }

        public static void DrawCrossRect(float left, float top, float right, float bottom, Color? color = null)
        {
            DrawCrossAABB(left, top, right, bottom, color);
        }

        public static void DrawCrossRect(Vector3 centerPos, float size, Color? color = null)
        {
            DrawCrossRect(centerPos, size, size, color);
        }

        public static void DrawCrossRect(Vector3 centerPos, float width, float height, Color? color = null)
        {
            float left = centerPos.x - width * 0.5f;
            float right = left + width;
            float top = centerPos.y + height * 0.5f;
            float bottom = top - height;
            DrawCrossAABB(left, top, right, bottom, color);
        }

        public static void DrawCrossRect(Vector3 topLeft, Vector3 bottomRight, Color? color = null)
        {
            DrawCrossAABB(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y, color);
        }

        public static void DrawCrossRect(Rect rect, Color? color = null)
        {
            DrawCrossAABB(rect.x, rect.y, rect.x + rect.width, rect.y - rect.height, color);
        }

        public static void DrawSquare(Vector3 centerPos, float size, Color? color = null)
        {
            DrawRect(centerPos, size, size, color);
        }

        public static void DrawQuadrilateral(Vector3 centerPos, Vector3 pos1, Vector3 pos2, Vector3 pos3, Vector3 pos4, Color? color = null)
        {
            Draw(color, () =>
            {
                pos1 += centerPos;
                pos2 += centerPos;
                pos3 += centerPos;
                pos4 += centerPos;
                Gizmos.DrawLine(pos1, pos2);
                Gizmos.DrawLine(pos2, pos3);
                Gizmos.DrawLine(pos3, pos4);
                Gizmos.DrawLine(pos4, pos1);
            });
        }

        static Color GetColliderColor(Color? color)
        {
            return color.HasValue ? color.Value : Color.green;
        }

        public static void DrawCollider(Collider2D collider, Color? color = null, bool drawPoints = false)
        {
            if (collider == null) return;

            var box = collider as BoxCollider2D;
            if (box != null)
            {
                DrawCollider(box, color);
                return;
            }

            var circle = collider as CircleCollider2D;
            if (circle != null)
            {
                DrawCollider(circle, color);
                return;
            }

            var edge = collider as EdgeCollider2D;
            if (edge != null)
            {
                DrawCollider(edge, color, drawPoints);
                return;
            }

            var polygon = collider as PolygonCollider2D;
            if (polygon != null)
            {
                DrawCollider(polygon, color, drawPoints);
                return;
            }

            var bounds = collider.bounds;
            float left = bounds.min.x;
            float right = bounds.max.x;
            float bottom = bounds.min.y;
            float top = bounds.max.y;
            DrawAABB(left, top, right, bottom, GetColliderColor(color));
        }

        public static void DrawCollider(BoxCollider2D box, Color? color = null)
        {
            if (box == null) return;
            var bounds = box.bounds;
            var center = bounds.center;
            var size = bounds.size;
            DrawRect(center, size.x, size.y, GetColliderColor(color));
        }

        public static void DrawCollider(CircleCollider2D circle, Color? color = null)
        {
            if (circle == null) return;
            var center = circle.bounds.center;
            var radius = circle.radius;
            var scale = circle.transform.lossyScale;
            //DrawOval(center, radius * scale.x, radius * scale.y, 3, GetColliderColor(color));
            DrawCircle(center, radius * Mathf.Max(scale.x, scale.y), 3, GetColliderColor(color));
        }

        public static void DrawCollider(EdgeCollider2D edge, Color? color = null, bool drawPoints = false)
        {
            if (edge == null) return;

            var pos = edge.transform.position;
            var scale = edge.transform.lossyScale;
            var points = edge.points;
            int pointCount = points.Length;
            if (pointCount < 2) return;

            Draw(GetColliderColor(color), () =>
            {
                if (drawPoints)
                {
                    var pos1 = pos.Add(points[0], scale);
                    DrawPoint(pos1, PointColor);
                    for (int i = 1; i < pointCount; i++)
                    {
                        var pos2 = pos.Add(points[i], scale);
                        DrawPoint(pos2, PointColor);
                        Gizmos.DrawLine(pos1, pos2);
                        pos1 = pos2;
                    }
                }
                else
                {
                    var pos1 = pos.Add(points[0], scale);
                    for (int i = 1; i < pointCount; i++)
                    {
                        var pos2 = pos.Add(points[i], scale);
                        Gizmos.DrawLine(pos1, pos2);
                        pos1 = pos2;
                    }
                }
            });
        }

        public static void DrawCollider(PolygonCollider2D polygon, Color? color = null, bool drawPoints = false)
        {
            if (polygon == null) return;
            int pathCount = polygon.pathCount;
            if (pathCount == 0) return;

            var pos = polygon.transform.position;
            var scale = polygon.transform.lossyScale;
            for (int i = 0; i < pathCount; i++)
            {
                var points = polygon.GetPath(i);
                int pointCount = points.Length;
                if (pointCount < 2) continue;

                Draw(GetColliderColor(color), () =>
                {
                    if (drawPoints)
                    {
                        var pos1 = pos.Add(points[0], scale);
                        DrawPoint(pos1, PointColor);
                        var firstPos = pos1;
                        for (int i = 1; i < pointCount; i++)
                        {
                            var pos2 = pos.Add(points[i], scale);
                            DrawPoint(pos2, PointColor);
                            Gizmos.DrawLine(pos1, pos2);
                            pos1 = pos2;
                        }
                        Gizmos.DrawLine(pos1, firstPos);
                    }
                    else
                    {
                        var pos1 = pos.Add(points[0], scale);
                        var firstPos = pos1;
                        for (int i = 1; i < pointCount; i++)
                        {
                            var pos2 = pos.Add(points[i], scale);
                            Gizmos.DrawLine(pos1, pos2);
                            pos1 = pos2;
                        }
                        Gizmos.DrawLine(pos1, firstPos);
                    }
                });
            }
        }

        public static void FillRect(float left, float top, float width, float height, Color? color = null)
        {
            FillAABB(left, top, left + width, top - height, color);
        }

        public static void FillSquareFromCenter(Vector3 centerPos, float size, Color? color = null)
        {
            FillRectFromCenter(centerPos, size, size, color);
        }

        public static void FillRectFromCenter(Vector3 centerPos, float width, float height, Color? color = null)
        {
            float left = centerPos.x - width * 0.5f;
            float right = left + width;
            float top = centerPos.y + height * 0.5f;
            float bottom = top - height;
            FillAABB(left, top, right, bottom, color);
        }

        public static void FillBorder(float left, float top, float right, float bottom,
            float innerLeft, float innerTop, float innerRight, float innerBottom, Color? color = null)
        {
            // Top
            FillAABB(left, top, right, innerTop, color);
            // Left
            FillAABB(left, innerTop, innerLeft, innerBottom, color);
            // Right
            FillAABB(innerRight, innerTop, right, innerBottom, color);
            // Bottom
            FillAABB(left, innerBottom, right, bottom, color);
        }

        public static void DrawGrid(Transform transform, int rowCount, int columnCount, float cellSize, Color? color = null)
        {
            var centerPos = transform.GetPosition();
            float left = centerPos.x - columnCount * cellSize * 0.5f;
            float top = centerPos.y + rowCount * cellSize * 0.5f;
            DrawGrid(left, top, rowCount, columnCount, cellSize, cellSize, color);
        }

        public static void DrawGrid(float left, float top, int rowCount, int columnCount, float cellSize, Color? color = null)
        {
            DrawGrid(left, top, rowCount, columnCount, cellSize, cellSize, color);
        }

        public static void DrawGrid(float left, float top, int rowCount, int columnCount, float cellWidth, float cellHeight, Color? color = null)
        {
            Draw(color, () =>
            {
                var pos1 = new Vector3(left, top);
                var pos2 = new Vector3(left + columnCount * cellWidth, top);

                for (int i = 0; i <= rowCount; i++)
                {
                    Gizmos.DrawLine(pos1, pos2);
                    pos1.y -= cellHeight;
                    pos2.y = pos1.y;
                }

                pos1.y = top;
                pos2.x = pos1.x;
                pos2.y = pos1.y - rowCount * cellHeight;

                for (int i = 0; i <= columnCount; i++)
                {
                    Gizmos.DrawLine(pos1, pos2);
                    pos1.x += cellWidth;
                    pos2.x = pos1.x;
                }
            });
        }

        public static void DrawGrid(Vector3 centerPos, int rowCount, int columnCount, float cellWidth, float cellHeight, float spacingX, float spacingY, Color? color = null)
        {
            float width = (cellWidth + spacingX) * columnCount - spacingX;
            float height = (cellHeight + spacingY) * rowCount - spacingY;
            float left = centerPos.x - width * 0.5f;
            float top = centerPos.y + height * 0.5f;
            DrawGrid(left, top, rowCount, columnCount, cellWidth, cellHeight, spacingX, spacingY, color);
        }

        public static void DrawGrid(float left, float top, int rowCount, int columnCount, float cellWidth, float cellHeight, float spacingX, float spacingY, Color? color = null)
        {
            Draw(color, () =>
            {
                float stepX = cellWidth + spacingX;
                float stepY = cellHeight + spacingY;
                float l = left;
                float t = top;
                float b = t - cellHeight;
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        _DrawAABB(l, t, l + cellWidth, b);
                        l += stepX;
                    }

                    l = left;
                    t -= stepY;
                    b = t - cellHeight;
                }
            });
        }

        public static void DrawCoords(float left, float top, int rowCount, int columnCount, float cellSize, Color? color = null)
        {
            Draw(color, () =>
            {
                int fontSize = 120;

                // Horizontal
                {
                    float l = left + 0.5f * cellSize;
                    float t = top + 0.2f;
                    var pos = new Vector3(l, t);
                    for (int i = 0; i < columnCount; i++)
                    {
                        DrawText(i.ToString(), pos, color, fontSize);
                        pos.x += cellSize;
                    }
                }

                // Vertical
                {
                    float l = left - 0.5f;
                    float t = top - 0.5f * cellSize - 0.2f;
                    var pos = new Vector3(l, t);
                    for (int i = 0; i < rowCount; i++)
                    {
                        DrawText(i.ToString(), pos, color, fontSize);
                        pos.y -= cellSize;
                    }
                }
            });
        }

        /// <summary>
        /// Angle in degrees, counter-clockwise.
        public static void DrawArc(Vector3 centerPos, float radius, float startAngle, float endAngle, float angleStep = AngleStep, Color? color = null)
        {
            Draw(color, () =>
            {
                if (angleStep <= 0)
                {
                    angleStep = AngleStep;
                }

                _DrawArc(centerPos, radius, startAngle, endAngle, angleStep);
            });
        }

        /// <summary>
        /// Angle in degrees, counter-clockwise.
        public static void FillArc(Vector3 centerPos, float radius, float startAngle, float endAngle, float thickness, float angleStep = AngleStep, Color? color = null)
        {
            Draw(color, () =>
            {
                if (angleStep <= 0)
                {
                    angleStep = AngleStep;
                }

                float radiusStep = 0.01f;
                int count = Mathf.CeilToInt(thickness / radiusStep);
                radiusStep = thickness / count;

                float startRadius = radius - thickness * 0.5f;
                float r = startRadius;
                for (int i = 0; i < count; i++)
                {
                    _DrawArc(centerPos, r, startAngle, endAngle, angleStep);
                    r += radiusStep;
                }

                _DrawArc(centerPos, startRadius + thickness, startAngle, endAngle, angleStep);
            });
        }

        public static void _DrawArc(Vector3 centerPos, float radius, float startAngle, float endAngle, float angleStep)
        {
            float deltaAngle = endAngle - startAngle;
            int count = Mathf.CeilToInt(Mathf.Abs(deltaAngle) / angleStep);
            angleStep = deltaAngle / count;

            startAngle *= Mathf.Deg2Rad;
            endAngle *= Mathf.Deg2Rad;
            angleStep *= Mathf.Deg2Rad;

            float angle = startAngle;
            var pos1 = centerPos;
            pos1.x += radius * Mathf.Cos(angle);
            pos1.y += radius * Mathf.Sin(angle);

            for (int i = 0; i < count; i++)
            {
                angle += angleStep;
                var pos2 = centerPos;
                pos2.x += radius * Mathf.Cos(angle);
                pos2.y += radius * Mathf.Sin(angle);
                Gizmos.DrawLine(pos1, pos2);

                pos1 = pos2;
            }

            var endPos = centerPos;
            endPos.x += radius * Mathf.Cos(endAngle);
            endPos.y += radius * Mathf.Sin(endAngle);
            Gizmos.DrawLine(pos1, endPos);
        }

        public static void DrawCircle(Vector3 centerPos, float radius, Color? color = null)
        {
            DrawCircle(centerPos, radius, AngleStep, color);
        }

        public static void DrawCircle(Vector3 centerPos, float radius, float angleStep, Color? color = null)
        {
            Draw(color, () =>
            {
                _DrawOval(centerPos, radius, radius, angleStep);
            });
        }

        static void _DrawCircle(Vector3 centerPos, float radius, float angleStep)
        {
            _DrawOval(centerPos, radius, radius, angleStep);
        }

        public static void DrawOval(Vector3 centerPos, float xRadius, float yRadius, float angleStep = AngleStep, Color? color = null)
        {
            Draw(color, () =>
            {
                _DrawOval(centerPos, xRadius, yRadius, angleStep);
            });
        }

        static void _DrawOval(Vector3 centerPos, float xRadius, float yRadius, float angleStep)
        {
            if (angleStep <= 0)
            {
                angleStep = AngleStep;
            }
            angleStep *= Mathf.Deg2Rad;

            Vector3 from = new Vector3(centerPos.x + xRadius, centerPos.y, centerPos.z);
            Vector3 to = Vector3.zero;
            float angle = 0;
            do
            {
                angle += angleStep;

                if (angle < DoublePI)
                {
                    to.x = centerPos.x + Mathf.Cos(angle) * xRadius;
                    to.y = centerPos.y + Mathf.Sin(angle) * yRadius;
                    Gizmos.DrawLine(from, to);
                    from = to;
                }
                else
                {
                    to.x = centerPos.x + xRadius;
                    to.y = centerPos.y;
                    Gizmos.DrawLine(from, to);
                    break;
                }
            }
            while (true);
        }

        public static void FillPoint(Vector3 pos, Color? color = null)
        {
            FillPoint(pos, PointRadius - 0.01f * 0.5f, color);
        }

        public static void FillPoint(Vector3 pos, float radius, Color? color = null)
        {
            FillCircle(pos, radius, AngleStep, color);
        }

        public static void FillCircle(Vector3 centerPos, float radius, Color? color = null)
        {
            FillCircle(centerPos, radius, AngleStep, color);
        }

        public static void FillCircle(Vector3 centerPos, float radius, float angleStep, Color? color = null)
        {
            FillArc(centerPos, radius, 0, 360, 0.01f, angleStep, color);
        }

        public static void DrawQuadBezier(Vector3 startPos, Vector3 controlPos, Vector3 endPos, Color? color = null, Color? lineColor = null)
        {
            DrawQuadBezier(startPos, controlPos, endPos, 0.01f, color, lineColor);
        }

        public static void DrawQuadBezier(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float timeStep = 0.01f, Color? color = null, Color? lineColor = null)
        {
            Draw(color, () =>
            {
                timeStep = Mathf.Max(timeStep, 0.01f);

                var prevPos = startPos;
                float t = 0;
                while (t < 1)
                {
                    float u = 1 - t;
                    float a = u * u;
                    float b = 2 * u * t;
                    float c = t * t;

                    float x = a * startPos.x + b * controlPos.x + c * endPos.x;
                    float y = a * startPos.y + b * controlPos.y + c * endPos.y;
                    var pos = new Vector3(x, y, 0);
                    Gizmos.DrawLine(prevPos, pos);
                    prevPos = pos;

                    t += timeStep;
                }

                Gizmos.DrawLine(prevPos, endPos);
            });

            if (lineColor.HasValue)
            {
                DrawLine(startPos, controlPos, lineColor);
                DrawLine(controlPos, endPos, lineColor);
            }
        }

        public static void DrawCubicBezier(Vector3 startPos, Vector3 controlPos, Vector3 control2Pos, Vector3 endPos, Color? color = null, Color? lineColor = null)
        {
            DrawCubicBezier(startPos, controlPos, control2Pos, endPos, 0.01f, color, lineColor);
        }

        public static void DrawCubicBezier(Vector3 startPos, Vector3 controlPos, Vector3 control2Pos, Vector3 endPos, float timeStep = 0.01f, Color? color = null, Color? lineColor = null)
        {
            Draw(color, () =>
            {
                timeStep = Mathf.Max(timeStep, 0.01f);

                var prevPos = startPos;
                float t = 0;
                while (t < 1)
                {
                    float t2 = 1 - t;

                    float a = t2 * t2 * t2;
                    float b = 3 * t2 * t2 * t;
                    float c = 3 * t2 * t * t;
                    float d = t * t * t;

                    float x = a * startPos.x + b * controlPos.x + c * control2Pos.x + d * endPos.x;
                    float y = a * startPos.y + b * controlPos.y + c * control2Pos.y + d * endPos.y;
                    var pos = new Vector3(x, y, 0);
                    Gizmos.DrawLine(prevPos, pos);
                    prevPos = pos;

                    t += timeStep;
                }

                Gizmos.DrawLine(prevPos, endPos);
            });

            if (lineColor.HasValue)
            {
                DrawLine(startPos, controlPos, lineColor);
                DrawLine(controlPos, control2Pos, lineColor);
                DrawLine(control2Pos, endPos, lineColor);
            }
        }

        public static void DrawCurve(Vector3 startPos, Vector3 endPos, AnimationCurve curve, Color? color = null)
        {
            Draw(color, () =>
            {
                float deltaX = endPos.x - startPos.x;
                float deltaY = endPos.y - startPos.y;
                var prevPos = startPos;
                var pos = startPos;
                float t = 0;
                do
                {
                    t += 0.01f;
                    pos.x = startPos.x + t * deltaX;
                    pos.y = startPos.y + curve.Evaluate(t) * deltaY;
                    Gizmos.DrawLine(prevPos, pos);
                    prevPos = pos;
                }
                while (t < 1);
            });
        }

        public static void DrawCurve(Vector3 startPos, Vector3 endPos, AnimationCurve xCurve, AnimationCurve yCurve, Color? color = null)
        {
            Draw(color, (Action)(() =>
            {
                float deltaX = endPos.x - startPos.x;
                float deltaY = endPos.y - startPos.y;
                var prevPos = startPos;
                var pos = startPos;
                float t = 0;
                do
                {
                    t += 0.01f;
                    pos.x = startPos.x + xCurve.Evaluate(t) * deltaX;
                    pos.y = startPos.y + yCurve.Evaluate(t) * deltaY;
                    Gizmos.DrawLine(prevPos, pos);
                    prevPos = pos;
                }
                while (t < 1);
            }));
        }

        public static void DrawPolyline(Vector3 centerPos, Vector2[] points, bool closed = false, Color? color = null)
        {
            int pointCount = points.GetLength();
            if (pointCount < 2) return;

            Draw(color, () =>
            {
                var pos1 = centerPos + (Vector3)points[0];
                for (int i = 1; i < pointCount; i++)
                {
                    var pos2 = centerPos + (Vector3)points[i];
                    Gizmos.DrawLine(pos1, pos2);
                    pos1 = pos2;
                }
                if (closed)
                {
                    Gizmos.DrawLine(pos1, centerPos + (Vector3)points[0]);
                }
            });
        }

        public static void DrawPolyline(Vector3 centerPos, List<Vector2> points, bool closed = false, Color? color = null)
        {
            int pointCount = points.GetCount();
            if (pointCount < 2) return;

            Draw(color, () =>
            {
                var pos1 = centerPos + (Vector3)points[0];
                for (int i = 1; i < pointCount; i++)
                {
                    var pos2 = centerPos + (Vector3)points[i];
                    Gizmos.DrawLine(pos1, pos2);
                    pos1 = pos2;
                }
                if (closed)
                {
                    Gizmos.DrawLine(pos1, centerPos + (Vector3)points[0]);
                }
            });
        }

        public static void DrawPolyline(Vector3 centerPos, List<Vector3> points, bool closed = false, Color? color = null)
        {
            int pointCount = points.GetCount();
            if (pointCount < 2) return;

            Draw(color, () =>
            {
                var pos1 = centerPos + points[0];
                for (int i = 1; i < pointCount; i++)
                {
                    var pos2 = centerPos + points[i];
                    Gizmos.DrawLine(pos1, pos2);
                    pos1 = pos2;
                }
                if (closed)
                {
                    Gizmos.DrawLine(pos1, centerPos + points[0]);
                }
            });
        }

        public static void DrawPolyline(List<Vector3> points, bool closed = false, Color? color = null)
        {
            int pointCount = points.GetCount();
            if (pointCount < 2) return;

            Draw(color, () =>
            {
                var pos1 = points[0];
                for (int i = 1; i < pointCount; i++)
                {
                    var pos2 = points[i];
                    Gizmos.DrawLine(pos1, pos2);
                    pos1 = pos2;
                }
                if (closed)
                {
                    Gizmos.DrawLine(pos1, points[0]);
                }
            });
        }

        public static void DrawPoint(Vector3 pos, Color? color = null)
        {
            Draw(color, () =>
            {
                _DrawCircle(pos, PointRadius, AngleStep);
            });
        }

        public static void DrawPoint(Vector3 pos, float radius, Color? color = null)
        {
            Draw(color, () =>
            {
                _DrawCircle(pos, radius, AngleStep);
            });
        }

        public static void DrawPoints(List<Vector3> points, Color? color = null)
        {
            Draw(color, () =>
            {
                int pointCount = points.GetCount();
                for (int i = 0; i < pointCount; i++)
                {
                    _DrawCircle(points[i], PointRadius, AngleStep);
                }
            });
        }

        public static void DrawPoints(Vector3 centerPos, List<Vector2> points, Color? color = null)
        {
            DrawPoints(centerPos, points, PointRadius, color);
        }

        public static void DrawPoints(Vector3 centerPos, List<Vector2> points, float dotRadius, Color? color = null)
        {
            Draw(color, () =>
            {
                int pointCount = points.GetCount();
                for (int i = 0; i < pointCount; i++)
                {
                    _DrawCircle(centerPos + (Vector3)points[i], dotRadius, AngleStep);
                }
            });
        }

        public static void DrawPoints(Vector3 centerPos, List<Vector3> points, Color? color = null)
        {
            DrawPoints(centerPos, points, points.GetCount(), PointRadius, color);
        }

        public static void DrawPoints(Vector3 centerPos, List<Vector3> points, int count, Color? color = null)
        {
            DrawPoints(centerPos, points, count, PointRadius, color);
        }

        public static void DrawPoints(Vector3 centerPos, List<Vector3> points, float radius, Color? color = null)
        {
            DrawPoints(centerPos, points, points.GetCount(), radius, color);
        }

        public static void DrawPoints(Vector3 centerPos, List<Vector3> points, int count, float radius, Color? color = null)
        {
            Draw(color, () =>
            {
                if (count < 0)
                {
                    count = points.GetCount();
                }
                for (int i = 0; i < count; i++)
                {
                    _DrawCircle(centerPos + points[i], radius, AngleStep);
                }
            });
        }

        static GUIStyle _textStyle;
        static GUIStyle TextStyle
        {
            get
            {
                if (_textStyle == null)
                {
                    _textStyle = new GUIStyle(GUI.skin.label);
                    _textStyle.fontStyle = FontStyle.Bold;
                }

                return _textStyle;
            }
        }

        public static void DrawText(string text, Vector3 position, Color? color = null, int fontSize = 200, bool alignLeft = false)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(text)) return;

            var content = new GUIContent(text);
            var style = TextStyle;

            // Center alignment
            style.fontSize = fontSize;
            var size = style.CalcSize(content);
            float width = size.x * 0.01f / 3.1f;
            if (!alignLeft)
            {
                position.x -= width * 0.5f;
            }
            position.y += 0.5f;

            //float zoom = UnityEditor.SceneView.currentDrawingSceneView.camera.orthographicSize;
            float zoom = Camera.current.orthographicSize;
            style.fontSize = Mathf.FloorToInt(fontSize / zoom);
            style.normal.textColor = color.HasValue ? color.Value : Color.black;

            UnityEditor.Handles.Label(position, content, style);
#endif
        }

        /// <summary>
        /// arrowAngle in degrees
        /// </summary>
        public static void DrawArrowLeft(Vector3 position, float length, Color? color = null, float arrowAngle = ArrowAngle, float arrowLength = ArrowLength)
        {
            Draw(color, () =>
            {
                if (arrowAngle <= 0) arrowAngle = ArrowAngle;
                if (arrowLength <= 0) arrowLength = ArrowLength;

                var arrowPos = position;
                arrowPos.x -= length;
                Gizmos.DrawLine(position, arrowPos);

                GetXY(arrowAngle, arrowLength, out float x, out float y);

                var pos1 = arrowPos;
                pos1.x += x;
                pos1.y += y;
                Gizmos.DrawLine(arrowPos, pos1);

                var pos2 = arrowPos;
                pos2.x += x;
                pos2.y -= y;
                Gizmos.DrawLine(arrowPos, pos2);
            });
        }

        /// <summary>
        /// arrowAngle in degrees
        /// </summary>
        public static void DrawArrowRight(Vector3 position, float length, Color? color = null, float arrowAngle = ArrowAngle, float arrowLength = ArrowLength)
        {
            Draw(color, () =>
            {
                if (arrowAngle <= 0) arrowAngle = ArrowAngle;
                if (arrowLength <= 0) arrowLength = ArrowLength;

                var arrowPos = position;
                arrowPos.x += length;
                Gizmos.DrawLine(position, arrowPos);

                GetXY(arrowAngle, arrowLength, out float x, out float y);

                var pos1 = arrowPos;
                pos1.x -= x;
                pos1.y += y;
                Gizmos.DrawLine(arrowPos, pos1);

                var pos2 = arrowPos;
                pos2.x -= x;
                pos2.y -= y;
                Gizmos.DrawLine(arrowPos, pos2);
            });
        }

        /// <summary>
        /// arrowAngle in degrees
        /// </summary>
        public static void DrawArrowUp(Vector3 position, float length, Color? color = null, float arrowAngle = ArrowAngle, float arrowLength = ArrowLength)
        {
            Draw(color, () =>
            {
                if (arrowAngle <= 0) arrowAngle = ArrowAngle;
                if (arrowLength <= 0) arrowLength = ArrowLength;

                var arrowPos = position;
                arrowPos.y += length;
                Gizmos.DrawLine(position, arrowPos);

                GetXY(arrowAngle, arrowLength, out float x, out float y);

                var pos1 = arrowPos;
                pos1.x -= y;
                pos1.y -= x;
                Gizmos.DrawLine(arrowPos, pos1);

                var pos2 = arrowPos;
                pos2.x += y;
                pos2.y -= x;
                Gizmos.DrawLine(arrowPos, pos2);
            });
        }

        /// <summary>
        /// arrowAngle in degrees
        /// </summary>
        public static void DrawArrowDown(Vector3 position, float length, Color? color = null, float arrowAngle = ArrowAngle, float arrowLength = ArrowLength)
        {
            Draw(color, () =>
            {
                if (arrowAngle <= 0) arrowAngle = ArrowAngle;
                if (arrowLength <= 0) arrowLength = ArrowLength;

                var arrowPos = position;
                arrowPos.y -= length;
                Gizmos.DrawLine(position, arrowPos);

                GetXY(arrowAngle, arrowLength, out float x, out float y);

                var pos1 = arrowPos;
                pos1.x -= y;
                pos1.y += x;
                Gizmos.DrawLine(arrowPos, pos1);

                var pos2 = arrowPos;
                pos2.x += y;
                pos2.y += x;
                Gizmos.DrawLine(arrowPos, pos2);
            });
        }

        public static void DrawArrow(Transform startTransform, Transform endTransform, Color? color = null, float arrowAngle = ArrowAngle, float arrowLength = ArrowLength)
        {
            if (startTransform != null && endTransform != null)
            {
                DrawArrow(startTransform.position, endTransform.position, color, arrowAngle, arrowLength);
            }
        }

        /// <summary>
        /// arrowAngle in degrees
        /// </summary>
        public static void DrawArrow(Vector3 startPos, Vector3 endPos, Color? color = null, float arrowAngle = ArrowAngle, float arrowLength = ArrowLength)
        {
            Draw(color, () =>
            {
                if (arrowAngle <= 0) arrowAngle = ArrowAngle;
                if (arrowLength <= 0) arrowLength = ArrowLength;

                Gizmos.DrawLine(startPos, endPos);

                float angle = 180 + Helper.GetAngle(startPos, endPos);
                GetXY(angle - arrowAngle, arrowLength, out float x, out float y);
                Gizmos.DrawLine(endPos, endPos.AddDelta(x, y));

                GetXY(angle + arrowAngle, arrowLength, out x, out y);
                Gizmos.DrawLine(endPos, endPos.AddDelta(x, y));
            });
        }

        //public static void DrawUninteractable()
        //{
        //    Draw(Color.gray, () =>
        //    {
        //        Context.MainCamera.GetAABB(out float left, out float top, out float right, out float bottom);
        //        var pos1 = new Vector3(left, top);
        //        var pos2 = new Vector3(right, top);
        //        var pos3 = new Vector3(right, bottom);
        //        var pos4 = new Vector3(left, bottom);
        //        Gizmos.DrawLine(pos1, pos3);
        //        Gizmos.DrawLine(pos2, pos4);
        //    });
        //}

        /// <summary>
        /// angle in degrees
        /// </summary>
        static void GetXY(float angle, float radius, out float x, out float y)
        {
            angle *= Mathf.Deg2Rad;

            x = radius * Mathf.Cos(angle);
            y = radius * Mathf.Sin(angle);
        }

        public static void DrawCells(bool[,] cells, Vector3 centerPos, float cellSize, Color cellColor, Color gridColor)
        {
            if (cells == null) return;

            float cellPadding = 0.1f;
            int rowCount = cells.GetLength(0);
            int columnCount = cells.GetLength(1);
            float width = columnCount * cellSize;
            float height = rowCount * cellSize;
            float left = centerPos.x - width * 0.5f;
            float top = centerPos.y + height * 0.5f;

            float t = top;
            for (int row = 0; row < rowCount; row++)
            {
                float l = left;
                float r = l + cellSize;
                float b = t - cellSize;
                for (int column = 0; column < columnCount; column++)
                {
                    if (cells[row, column])
                    {
                        FillAABB(l + cellPadding, t - cellPadding, r - cellPadding, b + cellPadding, cellColor);
                    }

                    l = r;
                    r += cellSize;
                }
                t = b;
            }

            DrawGrid(left, top, rowCount, columnCount, cellSize, cellSize, gridColor);
        }

        static Color[] _rectTransformColors;
        static Color[] RectTransformColors
        {
            get
            {
                if (_rectTransformColors == null)
                {
                    _rectTransformColors = new Color[]
                    {
                        Color.red,
                        Color.green,
                        Color.blue,
                        Color.yellow,
                        Color.cyan,
                        Color.magenta,
                        Color.gray,
                    };
                }
                return _rectTransformColors;
            }
        }

        public static void DrawRectTransform(RectTransform rectTransform, Color? color = null)
        {
            if (rectTransform != null)
            {
                if (!color.HasValue) color = Color.blue;

                rectTransform.GetAABB(out float left, out float top, out float right, out float bottom);
                DrawAABB(left, top, right, bottom, color);
                // Inner
                DrawAABB(left + LineStep, top - LineStep, right - LineStep, bottom + LineStep, color);
                // Outer
                DrawAABB(left - LineStep, top + LineStep, right + LineStep, bottom - LineStep, color);
            }
        }

        public static void DrawRectTransforms(Transform transform, bool includeInactive = false)
        {
            DrawRectTransforms(transform, 0, includeInactive);
        }

        static void DrawRectTransforms(Transform transform, int colorIndex, bool includeInactive)
        {
            if (!includeInactive && !transform.gameObject.activeSelf) return;

            var rectTransform = transform.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                var colors = RectTransformColors;
                int colorCount = colors.Length;
                int index = colorIndex < colorCount - 1 ? colorIndex + 1 : colorCount - 1;
                DrawRectTransform(rectTransform, colors[index]);
                colorIndex++;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                DrawRectTransforms(transform.GetChild(i), colorIndex, includeInactive);
            }
        }

        public static void DrawPlayRect(Transform playTopLeft, Transform playBottomRight, Color? color = null)
        {
            if (playTopLeft != null && playBottomRight != null)
            {
                var topLeft = playTopLeft.position;
                var bottomRight = playBottomRight.position;
                DrawAABB(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y, color ?? Color.blue);
            }
        }

        public static void DrawPath(Vector3[] points, Color? color = null)
        {
            int count = points.GetCount();
            if (count > 0)
            {
                if (count == 1)
                {
                    DrawCross(points[0], 0.02f, color);
                }
                else
                {
                    Draw(color, () =>
                    {
                        for (int i = 0; i < count - 1; i++)
                        {
                            Gizmos.DrawLine(points[i], points[i + 1]);
                        }
                    });
                }
            }
        }

        public static void DrawPath(List<Vector3> points, Color? color = null)
        {
            int count = points.GetCount();
            if (count > 0)
            {
                if (count == 1)
                {
                    DrawCross(points[0], 0.02f, color);
                }
                else
                {
                    Draw(color, () =>
                    {
                        for (int i = 0; i < count - 1; i++)
                        {
                            Gizmos.DrawLine(points[i], points[i + 1]);
                        }
                    });
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class SpritePathRenderer : MeshBehaviour
    {
        [SerializeField, OnSpriteChanged("OnChanged")] Sprite _sprite;
        [SerializeField, OnValueChanged("OnChanged")] float _thickness = 0.35f;
        [SerializeField, OnValueChanged("OnChanged")] int _pointCount;
        //[SerializeField, CheckValueChanged("OnChanged")] bool _reverse;

        public void Construct2(List<Vector3> points)
        {
#if UNITY_EDITOR
            _points = points.GetCopy();
#endif
            int pointCount = points.GetCount();
            if (_pointCount > 0)
            {
                pointCount = Mathf.Min(pointCount, _pointCount);
            }
            if (pointCount < 2)
            {
                ClearMesh();
                return;
            }

            //if (_reverse)
            //{
            //    points.Reverse();
            //}

            if (_sprite == null)
            {
                ClearMesh();
                return;
            }
            SetTexture(_sprite);

            _sprite.GetSize(out float width, out float height);
            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
            float left = -width * 0.5f;
            float bottom = -height * 0.5f;
            var texture = _sprite.texture;
            float uvWidth = 100f / texture.width;
            float uvHeight = 100f / texture.height;
            float halfPI = Mathf.PI * 0.5f;
            float halfThickness = Mathf.Max(_thickness, 0.01f) * 0.5f;

            int quadCount = pointCount - 1;
            var vertices = CreateVertices(quadCount);
            var uvs = CreateUVs(quadCount);
            int vertexIndex = 0;
            int uvIndex = 0;
            Vector3 p1 = Vector3.zero, p2 = Vector3.zero, p3 = Vector3.zero, p4 = Vector3.zero;

            for (int i = 0; i < quadCount; i++)
            {
                var point1 = points[i];
                var point2 = points[i + 1];
                float deltaX = point2.x - point1.x;
                float deltaY = point2.y - point1.y;
                float angle = Mathf.Atan2(deltaY, deltaX) - halfPI;
                float dX = halfThickness * Mathf.Cos(angle);
                float dY = halfThickness * Mathf.Sin(angle);
                p1.x = point1.x + dX;
                p1.y = point1.y + dY;
                p2.x = point1.x - dX;
                p2.y = point1.y - dY;
                p3.x = point2.x + dX;
                p3.y = point2.y + dY;
                p4.x = point2.x - dX;
                p4.y = point2.y - dY;
                vertices[vertexIndex++] = p1;
                vertices[vertexIndex++] = p2;
                vertices[vertexIndex++] = p3;
                vertices[vertexIndex++] = p4;
                uvs[uvIndex++] = new Vector2(uvLeft + (p1.x - left) * uvWidth, uvBottom + (p1.y - bottom) * uvHeight);
                uvs[uvIndex++] = new Vector2(uvLeft + (p2.x - left) * uvWidth, uvBottom + (p2.y - bottom) * uvHeight);
                uvs[uvIndex++] = new Vector2(uvLeft + (p3.x - left) * uvWidth, uvBottom + (p3.y - bottom) * uvHeight);
                uvs[uvIndex++] = new Vector2(uvLeft + (p4.x - left) * uvWidth, uvBottom + (p4.y - bottom) * uvHeight);
            }

            var triangles = CreateTriangles(quadCount);
            UpdateMesh(vertices, triangles, uvs);
        }

        public void Construct(List<Vector3> points)
        {
#if UNITY_EDITOR
            _points = points.GetCopy();
#endif
            int pointCount = points.GetCount();
            if (_pointCount > 0)
            {
                pointCount = Mathf.Min(pointCount, _pointCount);
            }
            if (pointCount < 2)
            {
                ClearMesh();
                return;
            }

            //if (_reverse)
            //{
            //    points.Reverse();
            //}

            if (_sprite == null)
            {
                ClearMesh();
                return;
            }
            SetTexture(_sprite);

            _sprite.GetSize(out float width, out float height);
            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
            float left = -width * 0.5f;
            float bottom = -height * 0.5f;
            var texture = _sprite.texture;
            //Log.Debug($"size={width}x{height}, uv=({uvLeft}, {uvTop}, {uvRight}, {uvBottom}), textureSize={texture.width}x{texture.height}");
            float uvWidth = 100f / texture.width;
            float uvHeight = 100f / texture.height;
            float halfPI = Mathf.PI * 0.5f;
            float halfThickness = Mathf.Max(_thickness, 0.01f) * 0.5f;

            int quadCount = pointCount - 1;
            var vertices = CreateVertices(quadCount);
            var uvs = CreateUVs(quadCount);
            int vertexIndex = 0;
            int uvIndex = 0;

            Vector3 p1 = Vector3.zero, p2 = Vector3.zero, p3 = Vector3.zero, p4 = Vector3.zero;
            Vector3 p5 = Vector3.zero, p6 = Vector3.zero, p7, p8;
            Vector3 mid1, mid2, mid3, mid4;
            var point1 = points[0];
            var point2 = points[1];
            float deltaX = point2.x - point1.x;
            float deltaY = point2.y - point1.y;
            float angle = Mathf.Atan2(deltaY, deltaX) - halfPI;
            float dX = halfThickness * Mathf.Cos(angle);
            float dY = halfThickness * Mathf.Sin(angle);
            p1.x = point1.x + dX;
            p1.y = point1.y + dY;
            p2.x = point1.x - dX;
            p2.y = point1.y - dY;
            p3.x = point2.x + dX;
            p3.y = point2.y + dY;
            p4.x = point2.x - dX;
            p4.y = point2.y - dY;
            mid1 = p1;
            mid2 = p2;
            p7 = p3;
            p8 = p4;

            for (int i = 2; i <= pointCount; i++)
            {
                if (i < pointCount)
                {
                    var point3 = points[i];
                    deltaX = point3.x - point2.x;
                    deltaY = point3.y - point2.y;
                    angle = Mathf.Atan2(deltaY, deltaX) - halfPI;
                    dX = halfThickness * Mathf.Cos(angle);
                    dY = halfThickness * Mathf.Sin(angle);
                    p5.x = point2.x + dX;
                    p5.y = point2.y + dY;
                    p6.x = point2.x - dX;
                    p6.y = point2.y - dY;
                    p7.x = point3.x + dX;
                    p7.y = point3.y + dY;
                    p8.x = point3.x - dX;
                    p8.y = point3.y - dY;

                    if (!Helper.GetIntersect(p1, p3, p5, p7, out mid3))
                    {
                        mid3 = (p3 + p5) * 0.5f;
                    }

                    if (!Helper.GetIntersect(p2, p4, p6, p8, out mid4))
                    {
                        mid4 = (p4 + p6) * 0.5f;
                    }

                    point2 = point3;
                    p1 = p5;
                    p2 = p6;
                    p3 = p7;
                    p4 = p8;
                }
                else
                {
                    mid3 = p7;
                    mid4 = p8;
                }

                vertices[vertexIndex++] = mid1;
                vertices[vertexIndex++] = mid2;
                vertices[vertexIndex++] = mid3;
                vertices[vertexIndex++] = mid4;
                uvs[uvIndex++] = new Vector2(uvLeft + (mid1.x - left) * uvWidth, uvBottom + (mid1.y - bottom) * uvHeight);
                uvs[uvIndex++] = new Vector2(uvLeft + (mid2.x - left) * uvWidth, uvBottom + (mid2.y - bottom) * uvHeight);
                uvs[uvIndex++] = new Vector2(uvLeft + (mid3.x - left) * uvWidth, uvBottom + (mid3.y - bottom) * uvHeight);
                uvs[uvIndex++] = new Vector2(uvLeft + (mid4.x - left) * uvWidth, uvBottom + (mid4.y - bottom) * uvHeight);

                mid1 = mid3;
                mid2 = mid4;
            }

            var triangles = CreateTriangles(quadCount);
            UpdateMesh(vertices, triangles, uvs);
        }

#if UNITY_EDITOR
        List<Vector3> _points;

        void OnChanged()
        {
            Construct(_points);
        }

        [MenuItem("CONTEXT/SpritePathRenderer/Paste Points")]
        static void PastePoints(MenuCommand menuCommand)
        {
            var points = EditorContext.Points;
            if (points == null)
            {
                LegacyLog.Warning("Points required!");
                return;
            }

            var pathRenderer = menuCommand.To<SpritePathRenderer>();
            pathRenderer._pointCount = 0;
            pathRenderer.Construct(points);
        }
#endif
    }
}
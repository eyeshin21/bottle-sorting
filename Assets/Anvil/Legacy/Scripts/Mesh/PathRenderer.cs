using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class PathRenderer : MeshBehaviour
    {
        [SerializeField, OnSpriteChanged("OnChanged")] Sprite _sprite;
        [SerializeField, OnValueChanged("OnChanged")] float _thickness = 0.25f;
        [SerializeField, OnValueChanged("OnChanged")] int _pointCount;
        //[SerializeField, CheckValueChanged("OnChanged")] bool _reverse;

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

            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
            var texture = _sprite.texture;
            //Log.Debug($"size={width}x{height}, uv=({uvLeft}, {uvTop}, {uvRight}, {uvBottom}), textureSize={texture.width}x{texture.height}");
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
                SetUVs(uvs, ref uvIndex, uvLeft, uvTop, uvRight, uvBottom);

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

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            int pointCount = _points.GetCount();
            if (pointCount > 0)
            {
                if (_pointCount > 0)
                {
                    pointCount = Mathf.Min(pointCount, _pointCount);
                }
                GizmosHelper.DrawPoints(transform.position, _points, pointCount, Color.red);
            }
        }

        [MenuItem("CONTEXT/PathRenderer/Paste Points")]
        static void PastePoints(MenuCommand menuCommand)
        {
            var points = EditorContext.Points;
            if (points == null)
            {
                LegacyLog.Warning("Points required!");
                return;
            }

            var pathRenderer = menuCommand.To<PathRenderer>();
            pathRenderer._pointCount = 0;
            pathRenderer.Construct(points);
        }
#endif
    }
}
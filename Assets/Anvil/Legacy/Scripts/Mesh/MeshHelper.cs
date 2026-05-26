using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class MeshHelper
    {
        static readonly float DoublePI = Mathf.PI * 2;

        public static Mesh GenerateCircle(float radius, float segmentLength)
        {
            segmentLength = Mathf.Max(segmentLength, 0.01f);
            int sideCount = Helper.RoundToInt(DoublePI * radius / segmentLength);
            return GeneratePolygon(radius, sideCount);
        }

        public static Mesh GeneratePolygon(float radius, int sideCount)
        {
            int vertexCount = sideCount + 2;
            int indexCount = sideCount * 3;

            var mesh = new Mesh();
            var vertices = new List<Vector3>(vertexCount);
            var indices = new int[indexCount];
            float angleStep = DoublePI / sideCount;
            vertices.Add(Vector3.zero);
            vertices.Add(new Vector3(radius, 0, 0));
            float angle = angleStep;
            int index = 0;
            for (int i = 2; i < vertexCount; i++)
            {
                vertices.Add(new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0));
                angle += angleStep;

                indices[index++] = 0;
                indices[index++] = i - 1;
                indices[index++] = i;
            }
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}
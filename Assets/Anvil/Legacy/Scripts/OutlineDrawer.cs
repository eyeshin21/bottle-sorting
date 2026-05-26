using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class OutlineDrawer : MonoBehaviour
    {
        [SerializeField] Color _drawColor = Color.white;

        List<Vector3> _points;

        List<Vector3> GetOutlinePoints(Sprite sprite)
        {
            if (sprite == null) return null;

            // Get triangles and vertices from sprite
            var triangles = sprite.triangles;
            var vertices = sprite.vertices;

            // Get just the outer edges from the sprite's triangles (ignore or remove any shared edges)
            var edges = new Dictionary<string, KeyValuePair<int, int>>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                for (int e = 0; e < 3; e++)
                {
                    int vert1 = triangles[i + e];
                    int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                    string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                    if (edges.ContainsKey(edge))
                    {
                        edges.Remove(edge);
                    }
                    else
                    {
                        edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                    }
                }
            }

            // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
            var lookup = new Dictionary<int, int>();
            foreach (var edge in edges.Values)
            {
                if (!lookup.ContainsKey(edge.Key))
                {
                    lookup.Add(edge.Key, edge.Value);
                }
            }

            // Loop through edge vertices in order
            int startVert = 0;
            int nextVert = startVert;
            int highestVert = startVert;
            var points = new List<Vector3>();
            do
            {
                // Add vertex
                points.Add(vertices[nextVert]);

                // Get next vertex
                nextVert = lookup[nextVert];

                // Store highest vertex (to know what shape to move to next)
                if (nextVert > highestVert)
                {
                    highestVert = nextVert;
                }
            }
            while (nextVert != startVert);

            return points;
        }

        List<Vector3> GetOutlinePoints(Mesh mesh)
        {
            if (mesh == null) return null;

            // Get triangles and vertices from mesh
            var triangles = mesh.triangles;
            var vertices = mesh.vertices;

            // Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
            var edges = new Dictionary<string, KeyValuePair<int, int>>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                for (int e = 0; e < 3; e++)
                {
                    int vert1 = triangles[i + e];
                    int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                    string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                    if (edges.ContainsKey(edge))
                    {
                        edges.Remove(edge);
                    }
                    else
                    {
                        edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                    }
                }
            }

            // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
            var lookup = new Dictionary<int, int>();
            foreach (var edge in edges.Values)
            {
                if (!lookup.ContainsKey(edge.Key))
                {
                    lookup.Add(edge.Key, edge.Value);
                }
            }

            // Loop through edge vertices in order
            int startVert = 0;
            int nextVert = startVert;
            int highestVert = startVert;
            var points = new List<Vector3>();
            do
            {
                // Add vertex
                points.Add(vertices[nextVert]);

                // Get next vertex
                nextVert = lookup[nextVert];

                // Store highest vertex (to know what shape to move to next)
                if (nextVert > highestVert)
                {
                    highestVert = nextVert;
                }
            }
            while (nextVert != startVert);

            return points;
        }

        void OnDrawGizmos()
        {
            if (_points == null)
            {
                var spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    _points = GetOutlinePoints(spriteRenderer.sprite);
                }
                else
                {
                    var meshFilter = GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        _points = GetOutlinePoints(meshFilter.mesh);
                    }
                }

                if (_points == null)
                {
                    return;
                }
            }

            var gizmosColor = Gizmos.color;
            Gizmos.color = _drawColor;
            var pos = transform.position;
            int pointCount = _points.Count;
            var pos1 = pos + _points[0];
            for (int i = 1; i < pointCount; i++)
            {
                var pos2 = pos + _points[i];
                Gizmos.DrawLine(pos1, pos2);
                pos1 = pos2;
            }
            Gizmos.DrawLine(pos1, pos + _points[0]);
            Gizmos.color = gizmosColor;
        }
    }
}
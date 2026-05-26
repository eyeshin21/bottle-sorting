using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    /*
     *       X     Z
     *       ^     ^
     *       |     |
     * +-----+-----+-----+
     * |     |     |     |
     * +-----+-----+-----+---> W
     * |     |     |     |
     * +-----+-----+-----+---> Y
     * |     |     |     |
     * +-----+-----+-----+
     */
    public enum UVs
    {
        Left,
        Top,
        Right,
        Bottom,
        X,
        Y,
        Z,
        W
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshBehaviour : MonoBehaviour
    {
        protected static readonly Color32 White = new Color32(255, 255, 255, 255);

        protected Renderer _renderer;
        public Renderer Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = GetComponent<Renderer>();
                }
                return _renderer;
            }
        }

        public Color Color
        {
            get => Renderer.GetColor();
            set => Renderer.SetColor(value);
        }

        public float Alpha
        {
            get => Renderer.GetAlpha();
            set => Renderer.SetAlpha(value);
        }

        public int SortingLayerIndex
        {
            get => Renderer.GetSortingLayerIndex();
            set => Renderer.SetSortingLayerIndex(value);
        }

        public int SortingLayerID
        {
            get => Renderer.GetSortingLayerID();
            set => Renderer.SetSortingLayerID(value);
        }

        public int SortingOrder
        {
            get => Renderer.GetSortingOrder();
            set => Renderer.SetSortingOrder(value);
        }

        protected Mesh _mesh;
        protected Mesh Mesh
        {
            get
            {
                if (_mesh == null)
                {
                    var meshFilter = GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        var mesh = new Mesh();
                        mesh.name = GetType().Name;
                        meshFilter.mesh = mesh;
                        _mesh = meshFilter.sharedMesh;
                    }
                }

                return _mesh;
            }
        }

        protected static Vector3[] CreateVertices(int quadCount)
        {
            return new Vector3[quadCount * 4];
        }

        protected static Vector3[] CreateVertices(float left, float top, float right, float bottom)
        {
            return new Vector3[4]
            {
                new Vector3(left, bottom),
                new Vector3(left, top),
                new Vector3(right, bottom),
                new Vector3(right, top)
            };
        }

        protected static int[] CreateTriangles(int quadCount)
        {
            int[] triangles = new int[quadCount * 6];
            int index = 0;
            int index4 = 0;
            for (int quad = 0; quad < quadCount; quad++)
            {
                triangles[index++] = index4;
                triangles[index++] = index4 + 1;
                triangles[index++] = index4 + 2;
                triangles[index++] = index4 + 2;
                triangles[index++] = index4 + 1;
                triangles[index++] = index4 + 3;
                index4 += 4;
            }
            return triangles;
        }

        protected static Vector2[] CreateUVs(int quadCount)
        {
            return new Vector2[quadCount * 4];
        }

        protected static Vector2[] CreateUVs(float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            return new Vector2[4]
            {
                new Vector2(uvLeft, uvBottom),
                new Vector2(uvLeft, uvTop),
                new Vector2(uvRight, uvBottom),
                new Vector2(uvRight, uvTop)
            };
        }

        protected static Vector2[] CreateUVs(int quadCount, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            var uvs = new Vector2[quadCount * 4];
            int index = 0;
            for (int i = 0; i < quadCount; i++)
            {
                //uvs[index++].Set(uvLeft, uvBottom);
                //uvs[index++].Set(uvLeft, uvTop);
                //uvs[index++].Set(uvRight, uvBottom);
                //uvs[index++].Set(uvRight, uvTop);

                uvs[index].x = uvLeft;
                uvs[index].y = uvBottom;
                index++;
                uvs[index].x = uvLeft;
                uvs[index].y = uvTop;
                index++;
                uvs[index].x = uvRight;
                uvs[index].y = uvBottom;
                index++;
                uvs[index].x = uvRight;
                uvs[index].y = uvTop;
                index++;
            }
            return uvs;
        }

        protected static Color[] CreateColors(int quadCount)
        {
            return new Color[quadCount * 4];
        }

        protected static Color[] CreateColors(int quadCount, Color color)
        {
            int count = quadCount * 4;
            var colors = new Color[count];
            for (int i = 0; i < count; i++)
            {
                colors[i] = color;
            }
            return colors;
        }

        protected static Color[] CreateColors(Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
        {
            return new Color[4] { bottomLeft, topLeft, bottomRight, topRight };
        }

        protected static void SetPositions(float width, float height, ref Vector3 pos0, ref Vector3 pos1, ref Vector3 pos2, ref Vector3 pos3)
        {
            float left = -width * 0.5f;
            float right = left + width;
            float top = height * 0.5f;
            float bottom = top - height;
            pos0.x = left;
            pos0.y = bottom;
            pos1.x = left;
            pos1.y = top;
            pos2.x = right;
            pos2.y = bottom;
            pos3.x = right;
            pos3.y = top;
        }

        /*
         *    p1
         *     +-----------+
         *    /           /
         *   /           /
         *  +-----------+
         *  p           p2
         * 
         */
        protected static void GetParallelogramPositions(Vector3 pos, Vector3 pos1, Vector3 pos2, float length,
                                                        out Vector3 outPos1, out Vector3 outPos2, out Vector3 outPos3)
        {
            outPos1 = pos;
            outPos2 = pos;
            outPos3 = pos;

            float deltaX1 = pos1.x - pos.x;
            float deltaY1 = pos1.y - pos.y;
            float angle1 = Mathf.Atan2(deltaY1, deltaX1);
            outPos1.x += Mathf.Cos(angle1) * length;
            outPos1.y += Mathf.Sin(angle1) * length;

            float deltaX2 = pos2.x - pos.x;
            float deltaY2 = pos2.y - pos.y;
            float angle2 = Mathf.Atan2(deltaY2, deltaX2);
            outPos2.x += Mathf.Cos(angle2) * length;
            outPos2.y += Mathf.Sin(angle2) * length;

            outPos3.x = outPos1.x + Mathf.Cos(angle2) * length;
            outPos3.y = outPos1.y + Mathf.Sin(angle2) * length;
        }

        protected static void SetVertices(Vector3[] vertices, float left, float top, float right, float bottom)
        {
            vertices[0].x = vertices[1].x = left;
            vertices[2].x = vertices[3].x = right;
            vertices[0].y = vertices[2].y = bottom;
            vertices[1].y = vertices[3].y = top;
        }

        protected static void SetVertices(Vector3[] vertices, int index, float left, float top, float right, float bottom)
        {
            vertices[index++].Set(left, bottom, 0);
            vertices[index++].Set(left, top, 0);
            vertices[index++].Set(right, bottom, 0);
            vertices[index++].Set(right, top, 0);
        }

        protected static void SetVertices(Vector3[] vertices, ref int index, float left, float top, float right, float bottom)
        {
            vertices[index++].Set(left, bottom, 0);
            vertices[index++].Set(left, top, 0);
            vertices[index++].Set(right, bottom, 0);
            vertices[index++].Set(right, top, 0);
        }

        protected static void SetVertices(Vector3[] vertices, ref int index, Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
        {
            vertices[index++] = bottomLeft;
            vertices[index++] = topLeft;
            vertices[index++] = bottomRight;
            vertices[index++] = topRight;
        }

        protected static void SetTriangles(int[] triangles, int index, int vertexIndex)
        {
            triangles[index++] = vertexIndex;
            triangles[index++] = vertexIndex + 1;
            triangles[index++] = vertexIndex + 2;
            triangles[index++] = vertexIndex + 2;
            triangles[index++] = vertexIndex + 1;
            triangles[index++] = vertexIndex + 3;
        }

        protected static void SetUVs(Vector2[] uvs, int index)
        {
            uvs[index++].Set(0, 0);
            uvs[index++].Set(0, 1);
            uvs[index++].Set(1, 0);
            uvs[index++].Set(1, 1);
        }

        protected static void SetUVs(Vector2[] uvs, int index, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            uvs[index].x = uvLeft;
            uvs[index].y = uvBottom;
            index++;
            uvs[index].x = uvLeft;
            uvs[index].y = uvTop;
            index++;
            uvs[index].x = uvRight;
            uvs[index].y = uvBottom;
            index++;
            uvs[index].x = uvRight;
            uvs[index].y = uvTop;
            index++;
        }

        protected static void SetUVs(Vector2[] uvs, ref int index, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            uvs[index].x = uvLeft;
            uvs[index].y = uvBottom;
            index++;
            uvs[index].x = uvLeft;
            uvs[index].y = uvTop;
            index++;
            uvs[index].x = uvRight;
            uvs[index].y = uvBottom;
            index++;
            uvs[index].x = uvRight;
            uvs[index].y = uvTop;
            index++;
        }

        protected static void SetColors(Color[] colors, ref int index, Color color)
        {
            colors[index++] = color;
            colors[index++] = color;
            colors[index++] = color;
            colors[index++] = color;
        }

        protected static void SetUVs9Slices(Sprite sprite, Vector2[] uvs)
        {
            if (sprite == null) return;
            Assert.IsTrue(uvs.Length >= 36);

            sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom,
                          out float uvX, out float uvY, out float uvZ, out float uvW);

            int index = 0;

            // Top-Left
            SetUVs(uvs, ref index, uvLeft, uvTop, uvX, uvW);
            // Top
            SetUVs(uvs, ref index, uvX, uvTop, uvZ, uvW);
            // Top-Right
            SetUVs(uvs, ref index, uvZ, uvTop, uvRight, uvW);

            // Left
            SetUVs(uvs, ref index, uvLeft, uvW, uvX, uvY);
            // Center
            SetUVs(uvs, ref index, uvX, uvW, uvZ, uvY);
            // Right
            SetUVs(uvs, ref index, uvZ, uvW, uvRight, uvY);

            // Bottom-Left
            SetUVs(uvs, ref index, uvLeft, uvY, uvX, uvBottom);
            // Bottom
            SetUVs(uvs, ref index, uvX, uvY, uvZ, uvBottom);
            // Bottom-Right
            SetUVs(uvs, ref index, uvZ, uvY, uvRight, uvBottom);
        }

        protected void SetTexture(Sprite sprite)
        {
            Renderer.SetTexture(sprite);
        }

        protected void SetTexture(Sprite sprite1, Sprite sprite2)
        {
            if (sprite1 != null)
            {
                Renderer.SetTexture(sprite1);
            }
            else if (sprite2 != null)
            {
                Renderer.SetTexture(sprite2);
            }
        }

        protected void SetUVs(Vector2[] uvs, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            uvs[0].x = uvLeft;
            uvs[0].y = uvBottom;
            uvs[1].x = uvLeft;
            uvs[1].y = uvTop;
            uvs[2].x = uvRight;
            uvs[2].y = uvBottom;
            uvs[3].x = uvRight;
            uvs[3].y = uvTop;
        }

        protected void UpdateMesh(float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            var vertices = CreateVertices(left, top, right, bottom);
            var triangles = CreateTriangles(1);
            var uvs = CreateUVs(uvLeft, uvTop, uvRight, uvBottom);
            UpdateMesh(vertices, triangles, uvs);
        }

        protected void UpdateMesh(Vector3[] vertices, int[] triangles, Vector2[] uvs)
        {
            var mesh = Mesh;
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
        }

        protected void UpdateMesh(Vector3[] vertices, int[] triangles, Color[] colors)
        {
            var mesh = Mesh;
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.colors = colors;
            mesh.RecalculateNormals();
        }

        protected void UpdateMesh(Vector3[] vertices, int[] triangles, Vector2[] uvs, Color[] colors)
        {
            var mesh = Mesh;
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.colors = colors;
            mesh.RecalculateNormals();
        }

        protected void AddQuad(List<MeshQuad> quads, float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            var quad = MeshQuad.GetQuad(left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
            quads.Add(quad);
        }

        protected void AddHQuad(List<MeshQuad> quads, float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom, float minAnchor, float maxAnchor)
        {
            left += (right - left) * minAnchor;
            right -= (right - left) * (1f - maxAnchor);
            uvLeft += (uvRight - uvLeft) * minAnchor;
            uvRight -= (uvRight - uvLeft) * (1f - maxAnchor);
            var quad = MeshQuad.GetQuad(left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
            quads.Add(quad);
        }

        protected void AddQuads(List<MeshQuad> quads, float left, float top, int rowCount, int columnCount, float cellSize, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            float l = left;
            float t = top;
            float r, b;
            for (int row = 0; row < rowCount; row++)
            {
                r = l + cellSize;
                b = t - cellSize;
                for (int column = 0; column < columnCount; column++)
                {
                    quads.Add(MeshQuad.GetQuad(l, t, r, b, uvLeft, uvTop, uvRight, uvBottom));
                    l = r;
                    r += cellSize;
                }
                l = left;
                t -= cellSize;
            }
        }

        protected void AddQuadsWithBottomPadding(List<MeshQuad> quads, float left, float top, int rowCount, int columnCount, float cellSize, float uvLeft, float uvTop, float uvRight, float uvBottom, float padding)
        {
            float l = left;
            float t = top;
            float r, b;
            for (int row = 0; row < rowCount; row++)
            {
                r = l + cellSize;
                b = t - cellSize;
                if (row == rowCount - 1)
                {
                    b += padding;
                    uvBottom += (uvTop - uvBottom) * padding / cellSize;
                }
                for (int column = 0; column < columnCount; column++)
                {
                    quads.Add(MeshQuad.GetQuad(l, t, r, b, uvLeft, uvTop, uvRight, uvBottom));
                    l = r;
                    r += cellSize;
                }
                l = left;
                t -= cellSize;
            }
        }

        protected void UpdateMesh(List<MeshQuad> quads, bool returnToPool = true)
        {
            //TODO: Optimize
            var meshController = MeshController.Get(Mesh, quads.Count);
            meshController.AddQuads(quads);
            meshController.UpdateMesh();

            if (returnToPool)
            {
                MeshQuad.Return(quads);
            }
        }

        protected void ClearMesh()
        {
            if (_mesh != null)
            {
                _mesh.Clear();
                _mesh.RecalculateBounds();
            }
        }

        public virtual void Init()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            meshRenderer.lightProbeUsage = LightProbeUsage.Off;
            meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            meshRenderer.sharedMaterial = new Material(Shaders.SpriteDefault);
        }

#if UNITY_EDITOR
        static string[] _sortingLayerNames;
        static string[] SortingLayerNames
        {
            get
            {
                var layers = SortingLayer.layers;
                int layerCount = layers.Length;
                if (_sortingLayerNames == null || _sortingLayerNames.Length != layerCount)
                {
                    _sortingLayerNames = new string[layerCount];

                    for (int i = 0; i < layerCount; i++)
                    {
                        _sortingLayerNames[i] = layers[i].name;
                    }
                }

                return _sortingLayerNames;
            }
        }

        protected MeshRenderer _meshRenderer;
        protected bool _drawMesh;

        protected bool PositionHandle(string label, ref Vector3 localPos, bool usePositionHandle = false)
        {
            return EditorHelper.PositionHandle(transform, label, ref localPos, usePositionHandle);
        }

        protected void Toggle(string label, ref bool value)
        {
            value = EditorGUILayout.Toggle(label, value);
        }

        /// <summary>
        /// Returns true if value changed.
        /// </summary>
        protected bool CheckToggle(string label, ref bool value)
        {
            bool newValue = EditorGUILayout.Toggle(label, value);
            if (newValue != value)
            {
                value = newValue;
                return true;
            }
            return false;
        }

        public virtual void OnInspectorGUI()
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
            if (_meshRenderer != null)
            {
                int layerIndex = SortingLayer.GetLayerValueFromID(_meshRenderer.sortingLayerID);
                int newLayerIndex = EditorGUILayout.Popup("Sorting Layer", layerIndex, SortingLayerNames);
                if (newLayerIndex != layerIndex)
                {
                    Undo.RecordObject(_meshRenderer, "Sorting Layer changed");
                    _meshRenderer.sortingLayerID = SortingLayer.layers[newLayerIndex].id;
                    EditorUtility.SetDirty(_meshRenderer);
                }

                int sortingOrder = _meshRenderer.sortingOrder;
                int newSortingOrder = EditorGUILayout.IntField("Sorting Order", sortingOrder);
                if (newSortingOrder != sortingOrder)
                {
                    Undo.RecordObject(_meshRenderer, "Sorting Order changed");
                    _meshRenderer.sortingOrder = newSortingOrder;
                    EditorUtility.SetDirty(_meshRenderer);
                }
            }

            Toggle("Draw Mesh", ref _drawMesh);
        }

        protected virtual void OnDrawGizmos()
        {
            if (_drawMesh && _mesh != null)
            {
                if (_mesh.vertexCount > 0)
                {
                    var color = Gizmos.color;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireMesh(_mesh, transform.position, transform.rotation, transform.lossyScale);
                    Gizmos.color = color;
                }
            }

            EditorHelper.RepaintGizmos();
        }

        protected virtual void Reset()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            //if (meshRenderer.sharedMaterial == null)
            {
                // Disable shadow and light
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                meshRenderer.receiveShadows = false;
                meshRenderer.lightProbeUsage = LightProbeUsage.Off;
                meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

                // Set material
                meshRenderer.sharedMaterial = new Material(Shaders.SpriteDefault);
            }
        }

        protected static bool Button(string label)
        {
            return GUILayout.Button(label, GUILayout.MinWidth(80));
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MeshBehaviour), true), CanEditMultipleObjects, DisallowMultipleComponent]
    public class MeshBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            (target as MeshBehaviour).OnInspectorGUI();
        }
    }
#endif
}
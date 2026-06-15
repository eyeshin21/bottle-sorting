using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
{
    /*
     *      1		3
     * 		+-------+
     * 		| \	    |
     * 		|   \	|
     * 		|	  \	|
     * 		+-------+
     *      0       2
     */
    public class UV4
    {
        private Vector2 _uv0;
        private Vector2 _uv1;
        private Vector2 _uv2;
        private Vector2 _uv3;

        public Vector2 UV0 => _uv0;
        public Vector2 UV1 => _uv1;
        public Vector2 UV2 => _uv2;
        public Vector2 UV3 => _uv3;

        public UV4(float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            Construct(uvLeft, uvTop, uvRight, uvBottom);
        }

        public UV4(Sprite sprite)
        {
            sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
            Construct(uvLeft, uvTop, uvRight, uvBottom);
        }

        void Construct(float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            _uv0.x = uvLeft;
            _uv0.y = uvBottom;
            _uv1.x = uvLeft;
            _uv1.y = uvTop;
            _uv2.x = uvRight;
            _uv2.y = uvBottom;
            _uv3.x = uvRight;
            _uv3.y = uvTop;
        }
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshBehaviour : MonoBehaviour
    {
        protected static readonly Color32 White = new Color32(255, 255, 255, 255);

        // Temporary
        protected Vector3[] _vertices;
        protected int[] _triangles;
        protected Vector2[] _uvs;
        private int _index4, _index6;

        private Renderer _renderer;
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

        private Mesh _mesh;
        public Mesh Mesh
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

        /// <summary>
        /// Free quads when update mesh.
        /// </summary>
        protected virtual bool FreeQuads => true;

        protected void SetTexture(Sprite sprite)
        {
            Renderer.SetTexture(sprite);
        }

        protected void Construct(int quadCount)
        {
            if (quadCount > 0)
            {
                _vertices = new Vector3[quadCount * 4];
                _triangles = new int[quadCount * 6];
                _uvs = new Vector2[quadCount * 4];

                _index4 = 0;
                _index6 = 0;
            }
            else
            {
                ClearMesh();
            }
        }

        protected void ResetIndices()
        {
            _index4 = 0;
            _index6 = 0;
        }

        protected virtual void UpdateMesh()
        {
            UpdateMesh(_vertices, _triangles, _uvs);

            if (FreeQuads)
            {
                _vertices = null;
                _triangles = null;
                _uvs = null;
            }
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

        protected void ClearMesh()
        {
            if (_mesh != null)
            {
                _mesh.Clear();
                _mesh.RecalculateBounds();
            }
        }

        public void Init()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            meshRenderer.lightProbeUsage = LightProbeUsage.Off;
            meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;

            meshRenderer.sharedMaterial = new Material(Shaders.SpriteDefault);
        }

        #region Helper
        void AddVertices(float left, float top, float right, float bottom)
        {
            _vertices[_index4].Set(left, bottom, 0);
            _vertices[_index4 + 1].Set(left, top, 0);
            _vertices[_index4 + 2].Set(right, bottom, 0);
            _vertices[_index4 + 3].Set(right, top, 0);
        }

        void AddTriangles()
        {
            _triangles[_index6] = _index4;
            _triangles[_index6 + 1] = _index4 + 1;
            _triangles[_index6 + 2] = _index4 + 2;
            _triangles[_index6 + 3] = _index4 + 2;
            _triangles[_index6 + 4] = _index4 + 1;
            _triangles[_index6 + 5] = _index4 + 3;

            _index6 += 6;
        }

        void AddUVs(float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            _uvs[_index4].x = uvLeft;
            _uvs[_index4].y = uvBottom;
            _uvs[_index4 + 1].x = uvLeft;
            _uvs[_index4 + 1].y = uvTop;
            _uvs[_index4 + 2].x = uvRight;
            _uvs[_index4 + 2].y = uvBottom;
            _uvs[_index4 + 3].x = uvRight;
            _uvs[_index4 + 3].y = uvTop;

            _index4 += 4;
        }

        void AddUVs(UV4 uv4)
        {
            _uvs[_index4] = uv4.UV0;
            _uvs[_index4 + 1] = uv4.UV1;
            _uvs[_index4 + 2] = uv4.UV2;
            _uvs[_index4 + 3] = uv4.UV3;

            _index4 += 4;
        }

        protected void AddQuad(float left, float top, float right, float bottom, float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            AddVertices(left, top, right, bottom);
            AddTriangles();
            AddUVs(uvLeft, uvTop, uvRight, uvBottom);
        }

        protected void AddQuad(float left, float top, float right, float bottom, UV4 uv4)
        {
            AddVertices(left, top, right, bottom);
            AddTriangles();
            AddUVs(uv4);
        }
        #endregion

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

        private MeshRenderer _meshRenderer;
        private bool _showMesh;

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

            _showMesh = EditorGUILayout.Toggle("Show Mesh", _showMesh);
        }

        void OnDrawGizmos()
        {
            if (!_showMesh || _mesh == null) return;

            if (_mesh.vertexCount > 0)
            {
                var color = Gizmos.color;
                Gizmos.color = Color.blue;
                Gizmos.DrawWireMesh(_mesh, transform.position, transform.localRotation, transform.localScale);
                Gizmos.color = color;
            }
        }

        void Reset()
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
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MeshBehaviour), true), CanEditMultipleObjects, DisallowMultipleComponent]
    public class MeshBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var meshBehaviour = target as MeshBehaviour;
            meshBehaviour.OnInspectorGUI();
        }
    }
#endif
}
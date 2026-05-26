using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    public partial class UIMeshBehaviour : MaskableGraphic, ILayoutElement
    {
        protected static readonly float PixelsPerUnit = 100;

        protected enum TriangleType
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
        }

        [SerializeField, OnSpriteChanged]
        protected Sprite _sprite;

        public virtual Sprite sprite
        {
            get => _sprite;
            set
            {
                if (Set(ref _sprite, value))
                {
                    //GeneratedUVs();
                    //SetAllDirty();
                    OnDirty();
                }
            }
        }

        [NonSerialized]
        protected Sprite _overrideSprite;
        public virtual Sprite overrideSprite
        {
            get => _overrideSprite;
            set
            {
                if (Set(ref _overrideSprite, value))
                {
                    //GeneratedUVs();
                    //SetAllDirty();
                    OnDirty();
                }
            }
        }

        public float Alpha
        {
            get => color.a;
            set
            {
                var color = this.color;
                color.a = value;
                this.color = color;
            }
        }

        //[SerializeField]
        //protected bool _useNativeSize;
        //public bool UseNativeSize
        //{
        //    get => _useNativeSize;
        //    set
        //    {
        //        if (_useNativeSize != value)
        //        {
        //            _useNativeSize = value;
        //            SetAllDirty();
        //        }
        //    }
        //}

        protected virtual Sprite activeSprite => _overrideSprite != null ? _overrideSprite : _sprite;

        static Material _defaultETC1Material = null;
        /// <summary>
        /// Default material used to draw everything if no explicit material was specified.
        /// </summary>
        static Material defaultETC1Material => _defaultETC1Material ??= Canvas.GetETC1SupportedCanvasMaterial();

        public override Texture mainTexture
        {
            get
            {
                if (activeSprite == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }
                    return s_WhiteTexture;
                }
                return activeSprite.texture;
            }
        }

        public override Material material
        {
            get
            {
                if (m_Material != null) return m_Material;
                if (activeSprite != null && activeSprite.associatedAlphaSplitTexture != null) return defaultETC1Material;
                return defaultMaterial;
            }
        }

        public float pixelsPerUnit
        {
            get
            {
                float spritePixelsPerUnit = 100;
                if (activeSprite)
                    spritePixelsPerUnit = activeSprite.pixelsPerUnit;

                float referencePixelsPerUnit = 100;
                if (canvas)
                    referencePixelsPerUnit = canvas.referencePixelsPerUnit;

                return spritePixelsPerUnit / referencePixelsPerUnit;
            }
        }

        protected static float CanvasScale => 0.01f / Context.MainCanvas.transform.localScale.x;

        protected UIMeshBehaviour()
        {
            useLegacyMeshGeneration = false;
        }

        /// <summary>
        /// Sets size delta.
        /// </summary>
        public virtual void SetSize(float width, float height)
        {
            var rectTransform = transform as RectTransform;
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        public virtual void SetAnchoredPosition(float x, float y)
        {
            var rectTransform = transform as RectTransform;
            rectTransform.anchoredPosition = new Vector2(x, y);
        }

        protected virtual void GeneratedUVs() { }

        protected virtual void ResolutionToNativeSize(float distance) { }

        protected virtual void OnDirty()
        {
            SetVerticesDirty();
        }

        protected void OnPopulateMesh(VertexHelper vh, Callback<Sprite> callback)
        {
            vh.Clear();

            var sprite = activeSprite;
            if (sprite != null)
            {
                callback(sprite);
            }

#if UNITY_EDITOR
            FillMesh(vh);
#endif
        }

#if UNITY_EDITOR
        protected virtual void OnSpriteChanged()
        {

        }

        public virtual void OnInspectorGUI()
        {
            GUIHelper.Toggle(ref _showMesh, "Show Mesh");
        }
#endif

        #region ILayoutElement
        public virtual float minWidth => 0;
        public virtual float preferredWidth => activeSprite ? activeSprite.rect.size.x / pixelsPerUnit : 0;
        public virtual float flexibleWidth => -1;
        public virtual float minHeight => 0;
        public virtual float preferredHeight => activeSprite ? activeSprite.rect.size.y / pixelsPerUnit : 0;
        public virtual float flexibleHeight => -1;
        public virtual int layoutPriority => 0;

        public virtual void CalculateLayoutInputHorizontal() { }

        public virtual void CalculateLayoutInputVertical() { }
        #endregion

        #region Helper
        protected static UIVertex[] _quadVertices = new UIVertex[4];

        protected virtual void SetNativeSize(Sprite sprite)
        {
            if (sprite != null)
            {
                rectTransform.sizeDelta = sprite.rect.size;
            }
        }

        protected void GetAABB(out float left, out float top, out float right, out float bottom)
        {
            float sizeX = rectTransform.rect.width;
            float sizeY = rectTransform.rect.height;
            float offsetX = -rectTransform.pivot.x * sizeX;
            float offsetY = -rectTransform.pivot.y * sizeY;
            left = offsetX;
            right = left + sizeX;
            bottom = offsetY;
            top = bottom + sizeY;
        }

        protected void GetAABB(Sprite sprite, out float left, out float top, out float right, out float bottom)
        {
            float sizeX = rectTransform.rect.width;
            float sizeY = rectTransform.rect.height;
            float offsetX = -rectTransform.pivot.x * sizeX;
            float offsetY = -rectTransform.pivot.y * sizeY;
            left = offsetX;
            right = left + sizeX;
            bottom = offsetY;
            top = bottom + sizeY;

            if (sprite.packingMode == SpritePackingMode.Tight)
            {
                var rect = sprite.rect;
                var textureRect = sprite.textureRect;
                float paddingLeft = textureRect.x - rect.x;
                float paddingBottom = textureRect.y - rect.y;
                float paddingRight = rect.width - (paddingLeft + textureRect.width);
                float paddingTop = rect.height - (paddingBottom + textureRect.height);
                float width = right - left;
                float height = top - bottom;
                float scaleX = width / rect.width;
                float scaleY = height / rect.height;
                //Log.Debug($"padding=({paddingLeft}, {paddingTop}, {paddingRight}, {paddingBottom}), scale=({scaleX}, {scaleY})");
                left += paddingLeft * scaleX;
                top -= paddingTop * scaleY;
                right -= paddingRight * scaleX;
                bottom += paddingBottom * scaleY;
            }
        }

        protected void AddQuad(VertexHelper vh, float left, float top, float right, float bottom,
                               float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, color);
        }

        protected void AddQuad(VertexHelper vh, float left, float top, float right, float bottom,
                               float uvLeft, float uvTop, float uvRight, float uvBottom, Color color)
        {
            var vertex = UIVertex.simpleVert;
            vertex.color = color;

            /*
             *      3		2
             * 		+-------+
             * 		| 	    |
             * 		|   	|
             * 		|	  	|
             * 		+-------+
             *      0       1
             */
            //vertex.position = new Vector3(left, bottom);
            //vertex.uv0 = new Vector4(uvLeft, uvBottom);
            //_quadVertices[0] = vertex;

            //vertex.position = new Vector3(right, bottom);
            //vertex.uv0 = new Vector4(uvRight, uvBottom);
            //_quadVertices[1] = vertex;

            //vertex.position = new Vector3(right, top);
            //vertex.uv0 = new Vector4(uvRight, uvTop);
            //_quadVertices[2] = vertex;

            //vertex.position = new Vector3(left, top);
            //vertex.uv0 = new Vector4(uvLeft, uvTop);
            //_quadVertices[3] = vertex;

            //vh.AddUIVertexQuad(_quadVertices);

            /*
             *      1		3
             * 		+-------+
             * 		| 	  / |
             * 		|   /  	|
             * 		| /	  	|
             * 		+-------+
             *      0       2
             */
            int index = vh.currentVertCount;
            vertex.position = new Vector3(left, bottom);
            vertex.uv0 = new Vector4(uvLeft, uvBottom);
            vh.AddVert(vertex);

            vertex.position = new Vector3(left, top);
            vertex.uv0 = new Vector4(uvLeft, uvTop);
            vh.AddVert(vertex);

            vertex.position = new Vector3(right, bottom);
            vertex.uv0 = new Vector4(uvRight, uvBottom);
            vh.AddVert(vertex);

            vertex.position = new Vector3(right, top);
            vertex.uv0 = new Vector4(uvRight, uvTop);
            vh.AddVert(vertex);

            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index, index + 3, index + 2);
        }

        protected void AddQuad(VertexHelper vh, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4,
                               float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            var vertex = UIVertex.simpleVert;
            vertex.color = color;

            /*
             *      1		3
             * 		+-------+
             * 		| 	  / |
             * 		|   /  	|
             * 		| /	  	|
             * 		+-------+
             *      0       2
             */
            int index = vh.currentVertCount;
            vertex.position = new Vector3(x1, y1);
            vertex.uv0 = new Vector4(uvLeft, uvBottom);
            vh.AddVert(vertex);

            vertex.position = new Vector3(x2, y2);
            vertex.uv0 = new Vector4(uvLeft, uvTop);
            vh.AddVert(vertex);

            vertex.position = new Vector3(x4, y4);
            vertex.uv0 = new Vector4(uvRight, uvBottom);
            vh.AddVert(vertex);

            vertex.position = new Vector3(x3, y3);
            vertex.uv0 = new Vector4(uvRight, uvTop);
            vh.AddVert(vertex);

            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index, index + 3, index + 2);
        }

        protected void AddQuad(VertexHelper vh, float left, float top, float right, float bottom,
                               float uvLeft, float uvTop, float uvRight, float uvBottom, UVTransformType transformType)
        {
            var vertex = UIVertex.simpleVert;
            vertex.color = color;
            int index = vh.currentVertCount;

            // (1, 3, 0, 2)
            /*
             *      1		3
             * 		+-------o
             * 		|` 	    |
             * 		|  `   	|
             * 		| 	 ` 	|
             * 		o------`o
             *      0       2
             */
            if (transformType == UVTransformType.RotateLeft)
            {
                vertex.position = new Vector3(left, bottom);
                vertex.uv0 = new Vector4(uvLeft, uvTop);
                vh.AddVert(vertex);

                vertex.position = new Vector3(left, top);
                vertex.uv0 = new Vector4(uvRight, uvTop);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, bottom);
                vertex.uv0 = new Vector4(uvLeft, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, top);
                vertex.uv0 = new Vector4(uvRight, uvBottom);
                vh.AddVert(vertex);

                vh.AddTriangle(index + 1, index + 3, index + 2);
                vh.AddTriangle(index + 1, index + 2, index);

                return;
            }

            // (2, 0, 3, 1)
            /*
             *      1		3
             * 		o-------o
             * 		|` 	    |
             * 		|  `   	|
             * 		| 	 ` 	|
             * 		o------`+
             *      0       2
             */
            if (transformType == UVTransformType.RotateRight)
            {
                vertex.position = new Vector3(left, bottom);
                vertex.uv0 = new Vector4(uvRight, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(left, top);
                vertex.uv0 = new Vector4(uvLeft, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, bottom);
                vertex.uv0 = new Vector4(uvRight, uvTop);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, top);
                vertex.uv0 = new Vector4(uvLeft, uvTop);
                vh.AddVert(vertex);

                vh.AddTriangle(index + 2, index, index + 1);
                vh.AddTriangle(index + 2, index + 1, index + 3);

                return;
            }

            if (transformType == UVTransformType.FlipHorizontal)
            {
                var tmp = uvLeft;
                uvLeft = uvRight;
                uvRight = tmp;
            }
            else if (transformType == UVTransformType.FlipVertical)
            {
                var tmp = uvTop;
                uvTop = uvBottom;
                uvBottom = tmp;
            }
            else if (transformType == UVTransformType.FlipAll)
            {
                var tmp = uvLeft;
                uvLeft = uvRight;
                uvRight = tmp;

                tmp = uvTop;
                uvTop = uvBottom;
                uvBottom = tmp;
            }

            /*
             *      1		3
             * 		+-------+
             * 		| 	  / |
             * 		|   /  	|
             * 		| /	  	|
             * 		+-------+
             *      0       2
             */
            vertex.position = new Vector3(left, bottom);
            vertex.uv0 = new Vector4(uvLeft, uvBottom);
            vh.AddVert(vertex);

            vertex.position = new Vector3(left, top);
            vertex.uv0 = new Vector4(uvLeft, uvTop);
            vh.AddVert(vertex);

            vertex.position = new Vector3(right, bottom);
            vertex.uv0 = new Vector4(uvRight, uvBottom);
            vh.AddVert(vertex);

            vertex.position = new Vector3(right, top);
            vertex.uv0 = new Vector4(uvRight, uvTop);
            vh.AddVert(vertex);

            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index, index + 3, index + 2);
        }

        protected void AddQuadFromCenter(VertexHelper vh, float x, float y, float width, float height,
                                            float uvLeft, float uvTop, float uvRight, float uvBottom)
        {
            float left = x - width * 0.5f;
            float right = left + width;
            float bottom = y - height * 0.5f;
            float top = bottom + height;
            AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
        }

        protected void AddQuadFromCenter(VertexHelper vh, float x, float y, float width, float height,
                                            float uvLeft, float uvTop, float uvRight, float uvBottom, UVTransformType transformType)
        {
            if (transformType == UVTransformType.RotateLeft || transformType == UVTransformType.RotateRight)
            {
                float tmp = width;
                width = height;
                height = tmp;
            }
            float left = x - width * 0.5f;
            float right = left + width;
            float bottom = y - height * 0.5f;
            float top = bottom + height;
            AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, transformType);
        }

        protected void AddTriangle(VertexHelper vh, float left, float top, float right, float bottom,
                               float uvLeft, float uvTop, float uvRight, float uvBottom, TriangleType triangleType)
        {
            var vertex = UIVertex.simpleVert;
            vertex.color = color;

            /*
             *      1		3
             * 		+-------+
             * 		| 	  / |
             * 		|   /  	|
             * 		| /	  	|
             * 		+-------+
             *      0       2
             */
            int index = vh.currentVertCount;
            if (triangleType == TriangleType.TopLeft)
            {
                vertex.position = new Vector3(left, bottom);
                vertex.uv0 = new Vector4(uvLeft, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(left, top);
                vertex.uv0 = new Vector4(uvLeft, uvTop);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, top);
                vertex.uv0 = new Vector4(uvRight, uvTop);
                vh.AddVert(vertex);
            }
            else if (triangleType == TriangleType.TopRight)
            {
                vertex.position = new Vector3(right, bottom);
                vertex.uv0 = new Vector4(uvRight, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(left, top);
                vertex.uv0 = new Vector4(uvLeft, uvTop);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, top);
                vertex.uv0 = new Vector4(uvRight, uvTop);
                vh.AddVert(vertex);
            }
            else if (triangleType == TriangleType.BottomLeft)
            {
                vertex.position = new Vector3(right, bottom);
                vertex.uv0 = new Vector4(uvRight, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(left, bottom);
                vertex.uv0 = new Vector4(uvLeft, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(left, top);
                vertex.uv0 = new Vector4(uvLeft, uvTop);
                vh.AddVert(vertex);
            }
            else if (triangleType == TriangleType.BottomRight)
            {
                vertex.position = new Vector3(left, bottom);
                vertex.uv0 = new Vector4(uvLeft, uvBottom);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, top);
                vertex.uv0 = new Vector4(uvRight, uvTop);
                vh.AddVert(vertex);

                vertex.position = new Vector3(right, bottom);
                vertex.uv0 = new Vector4(uvRight, uvBottom);
                vh.AddVert(vertex);
            }

            vh.AddTriangle(index, index + 1, index + 2);
        }

        protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
        {
            UIVertex[] vbo = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }
            return vbo;
        }
        #endregion

#if UNITY_EDITOR
        Mesh _mesh;
        bool _showMesh;

        protected void FillMesh(VertexHelper vh)
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
            }
            vh.FillMesh(_mesh);
        }

        protected virtual void OnDrawGizmos()
        {
            if (_showMesh)
            {
                var mesh = _mesh;
                if (mesh != null && mesh.vertexCount > 0)
                {
                    var color = Gizmos.color;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireMesh(mesh, transform.position, transform.localRotation, transform.localScale * 0.01f);
                    Gizmos.color = color;

                }
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UIMeshBehaviour), true), CanEditMultipleObjects, DisallowMultipleComponent]
    public class UIMeshBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            (target as UIMeshBehaviour).OnInspectorGUI();
        }
    }
#endif
}
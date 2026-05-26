using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class ListSpriteRenderer : MeshBehaviour
    {
        [SerializeField] Sprite _sprite;
        [SerializeField] Vector2 _size;
        [SerializeField] Color _color = Color.white;

        void Start()
        {
            Color = _color;
        }

        public void Construct(List<Vector3> points)
        {
            if (_sprite == null) return;
            SetTexture(_sprite);

            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

            float width = _size.x;
            float height = _size.y;
            if (width <= 0 || height <= 0)
            {
                _sprite.GetSize(out width, out height);
            }
            float left = -width * 0.5f;
            float right = left + width;
            float bottom = -height * 0.5f;
            float top = bottom + height;

            int quadCount = points.Count;
            var vertices = CreateVertices(quadCount);
            var uvs = CreateUVs(quadCount);
            int vertexIndex = 0;
            int uvIndex = 0;
            for (int i = 0; i < quadCount; i++)
            {
                var point = points[i];
                SetVertices(vertices, ref vertexIndex, point.x + left, point.y + top, point.x + right, point.y + bottom);
                SetUVs(uvs, ref uvIndex, uvLeft, uvTop, uvRight, uvBottom);
            }

            var triangles = CreateTriangles(quadCount);
            UpdateMesh(vertices, triangles, uvs);
        }
    }
}
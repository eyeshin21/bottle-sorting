using UnityEngine;

namespace Anvil.Legacy
{
    /*
     * 1   3  5   7
     * +---+--+---+
     * |\  |\ |\  |
     * |  \| \|  \|
     * +---+--+---+
     * 0   2  4   6
     */
    public class RotatedLineRenderer : MeshBehaviour
    {
        [SerializeField, OnSpriteChanged] Sprite _sprite;
        [SerializeField, OnValueChanged("OnLengthChanged")] float _length = 1;
        [SerializeField, OnValueChanged("OnColorChanged")] Color _color = Color.white;

        Vector3[] _vertices;
        static readonly int[] _triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3,
            2, 3, 4,
            4, 3, 5,
            4, 5, 6,
            6, 5, 7
        };
        Vector2[] _uvs = new Vector2[8];

        float _halfWidth;
        bool _isDirtyVertices = true;
        bool _isDirtyUVs = true;

        public float Length
        {
            get => _length;
            set
            {
                _length = value;
                _isDirtyVertices = true;
                UpdateMesh();
            }
        }

        void Start()
        {
            Color = _color;
            UpdateMesh();
        }

        void UpdateMesh()
        {
            if (_sprite == null)
            {
                ClearMesh();
            }
            else
            {
                if (_isDirtyVertices)
                {
                    UpdateVertices();
                }

                if (_isDirtyUVs)
                {
                    UpdateUVs();
                }

                UpdateMesh(_vertices, _triangles, _uvs);
            }

            _isDirtyVertices = _isDirtyUVs = false;
        }

        void UpdateVertices()
        {
            if (_vertices == null)
            {
                _vertices = new Vector3[8];

                SetTexture(_sprite);
                _sprite.GetSize(out float width, out float height);

                _halfWidth = width * 0.5f;
                float halfHeight = height * 0.5f;
                _vertices[0].x = _vertices[1].x = -_halfWidth;
                _vertices[2].x = _vertices[3].x = 0;
                _vertices[0].y = _vertices[2].y = _vertices[4].y = _vertices[6].y = -halfHeight;
                _vertices[1].y = _vertices[3].y = _vertices[5].y = _vertices[7].y = halfHeight;
            }

            float length = Mathf.Max(_length, 0);
            _vertices[4].x = _vertices[5].x = length;
            _vertices[6].x = _vertices[7].x = length + _halfWidth;
        }

        void UpdateUVs()
        {
            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
            _uvs[0].x = _uvs[1].x = uvLeft;
            _uvs[2].x = _uvs[3].x = _uvs[4].x = _uvs[5].x = (uvLeft + uvRight) * 0.5f;
            _uvs[6].x = _uvs[7].x = uvRight;
            _uvs[0].y = _uvs[2].y = _uvs[4].y = _uvs[6].y = uvBottom;
            _uvs[1].y = _uvs[3].y = _uvs[5].y = _uvs[7].y = uvTop;
        }

#if UNITY_EDITOR
        void OnSpriteChanged()
        {
            _vertices = null;
            _isDirtyVertices = true;
            _isDirtyUVs = true;
            UpdateMesh();
        }

        void OnLengthChanged()
        {
            _isDirtyVertices = true;
            UpdateMesh();
        }

        void OnColorChanged()
        {
            Color = _color;
        }
#endif
    }
}
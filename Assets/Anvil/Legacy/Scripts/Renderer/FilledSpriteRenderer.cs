using UnityEngine;

namespace Anvil.Legacy
{
    [ExecuteInEditMode]
    public class FilledSpriteRenderer : MeshBehaviour
    {
        [SerializeField] Sprite _sprite;
        [SerializeField] FilledSpriteMode _fillMode = FilledSpriteMode.Left;
        [SerializeField, Range(0, 1)] float _fillAmount;

        Vector3[] _vertices = CreateVertices(1);
        int[] _triangles = CreateTriangles(1);
        Vector2[] _uvs = CreateUVs(1);
        float _left, _top, _right, _bottom;
        float _uvLeft, _uvTop, _uvRight, _uvBottom;
        bool _isDirtySprite = true;
        bool _isDirty;

        public void SetSprite(Sprite sprite)
        {
            _sprite = sprite;
            _isDirtySprite = true;
            _isDirty = true;
        }

        public void SetFillMode(FilledSpriteMode fillMode)
        {
            _fillMode = fillMode;
            _isDirty = true;
        }

        public void SetFillAmount(float fillAmount)
        {
            _fillAmount = fillAmount;
            _isDirty = true;
        }

        void UpdateSprite()
        {
            if (_isDirtySprite)
            {
                _sprite.GetSize(out float width, out float height);
                _left = -width * 0.5f;
                _right = _left + width;
                _bottom = -height * 0.5f;
                _top = _bottom + height;
                _sprite.GetUVs(out _uvLeft, out _uvTop, out _uvRight, out _uvBottom);
                SetTexture(_sprite);
                _isDirtySprite = false;
            }

            float fillAmount = Mathf.Clamp01(_fillAmount);
            if (_fillMode == FilledSpriteMode.Left)
            {
                float right = _left + (_right - _left) * fillAmount;
                float uvRight = _uvLeft + (_uvRight - _uvLeft) * fillAmount;
                SetVertices(_vertices, _left, _top, right, _bottom);
                SetUVs(_uvs, _uvLeft, _uvTop, uvRight, _uvBottom);
            }
            else if (_fillMode == FilledSpriteMode.Right)
            {
                float left = _right - (_right - _left) * fillAmount;
                float uvLeft = _uvRight - (_uvRight - _uvLeft) * fillAmount;
                SetVertices(_vertices, left, _top, _right, _bottom);
                SetUVs(_uvs, uvLeft, _uvTop, _uvRight, _uvBottom);
            }
            else if (_fillMode == FilledSpriteMode.Bottom)
            {
                float top = _bottom + (_top - _bottom) * fillAmount;
                float uvTop = _uvBottom + (_uvTop - _uvBottom) * fillAmount;
                SetVertices(_vertices, _left, top, _right, _bottom);
                SetUVs(_uvs, _uvLeft, uvTop, _uvRight, _uvBottom);
            }
            else if (_fillMode == FilledSpriteMode.Top)
            {
                float bottom = _top - (_top - _bottom) * fillAmount;
                float uvBottom = _uvTop - (_uvTop - _uvBottom) * fillAmount;
                SetVertices(_vertices, _left, _top, _right, bottom);
                SetUVs(_uvs, _uvLeft, _uvTop, _uvRight, uvBottom);
            }
            else if (_fillMode == FilledSpriteMode.RightFlip)
            {
                float left = _right - (_right - _left) * fillAmount;
                float uvLeft = _uvLeft + (_uvRight - _uvLeft) * fillAmount;
                SetVertices(_vertices, left, _top, _right, _bottom);
                SetUVs(_uvs, uvLeft, _uvTop, _uvLeft, _uvBottom);
            }
            else
            {
                LegacyLog.Todo(_fillMode);
            }

            UpdateMesh(_vertices, _triangles, _uvs);
        }

        void LateUpdate()
        {
            if (_isDirty)
            {
                _isDirty = false;
                UpdateSprite();
            }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            _isDirtySprite = true;
            _isDirty = true;
        }
#endif
    }
}
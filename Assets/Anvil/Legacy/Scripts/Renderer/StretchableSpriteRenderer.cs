using UnityEngine;

namespace Anvil.Legacy
{
    public class StretchableSpriteRenderer : MeshBehaviour, ISizeSetter
    {
        [SerializeField] Sprite _sprite;
#if UNITY_EDITOR
        [SerializeField, OnValueChanged("OnSizeChanged")] Vector2 _size = Vector2.one;
#endif
        [SerializeField] Pivot _pivot;
        //[SerializeField] Color _color = Color.white;

        //void Start()
        //{
        //    Color = _color;
        //}

        public void SetSize(float width, float height)
        {
#if UNITY_EDITOR
            _size.Set(width, height);
#endif
            SetTexture(_sprite);
            if (_sprite == null) return;

            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
            _pivot.GetTopLeft(width, height, out float top, out float left);
            float right = left + width;
            float bottom = top - height;
            UpdateMesh(left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
        }

#if UNITY_EDITOR
        void OnSizeChanged()
        {
            SetSize(_size.x, _size.y);
        }
#endif
    }
}
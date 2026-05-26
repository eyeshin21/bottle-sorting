#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public class DebugSprite : MeshBehaviour
    {
        [SerializeField, OnSpriteChanged("OnSpriteChanged")] Sprite _sprite;
        [SerializeField] Pivot _pivot;
        [SerializeField] Vector2 _size = Vector2.one;

        void UpdateSprite()
        {
            SetTexture(_sprite);
            if (_sprite == null) return;

            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
            _pivot.GetVertices(_size, out float left, out float top, out float right, out float bottom);
            var texture = _sprite.texture;
            float pixelsPerUnit = _sprite.pixelsPerUnit;
            float uvWidth = _size.x * pixelsPerUnit / texture.width;
            float uvHeight = _size.y * pixelsPerUnit / texture.height;
            float uvLeft2 = uvLeft;
            float uvTop2 = uvTop;
            if (_pivot == Pivot.TopLeft)
            {

            }
            else if (_pivot == Pivot.Top)
            {
                uvLeft2 = (uvLeft + uvRight - uvWidth) * 0.5f;
            }
            else if (_pivot == Pivot.TopRight)
            {
                uvLeft2 = uvRight - uvWidth;
            }
            else if (_pivot == Pivot.Left)
            {
                uvTop2 = (uvTop + uvBottom + uvHeight) * 0.5f;
            }
            else if (_pivot == Pivot.Center)
            {
                uvLeft2 = (uvLeft + uvRight - uvWidth) * 0.5f;
                uvTop2 = (uvTop + uvBottom + uvHeight) * 0.5f;
            }
            else if (_pivot == Pivot.Right)
            {
                uvLeft2 = uvRight - uvWidth;
                uvTop2 = (uvTop + uvBottom + uvHeight) * 0.5f;
            }
            else if (_pivot == Pivot.BottomLeft)
            {
                uvTop2 = uvBottom + uvHeight;
            }
            else if (_pivot == Pivot.Bottom)
            {
                uvLeft2 = (uvLeft + uvRight - uvWidth) * 0.5f;
                uvTop2 = uvBottom + uvHeight;
            }
            else if (_pivot == Pivot.BottomRight)
            {
                uvLeft2 = uvRight - uvWidth;
                uvTop2 = uvBottom + uvHeight;
            }
            float uvRight2 = uvLeft2 + uvWidth;
            float uvBottom2 = uvTop2 - uvHeight;
            UpdateMesh(left, top, right, bottom, uvLeft2, uvTop2, uvRight2, uvBottom2);
        }

        void OnSpriteChanged()
        {
            if (_sprite != null)
            {
                _sprite.GetSize(out float width, out float height);
                _size.Set(width, height);
                UpdateSprite();
            }
        }

        void OnValidate()
        {
            UpdateSprite();
        }
    }
}
#endif
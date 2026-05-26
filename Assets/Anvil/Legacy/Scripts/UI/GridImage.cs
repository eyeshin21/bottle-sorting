using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class GridImage : UIMeshBehaviour
    {
        [SerializeField] int _rowCount;
        [SerializeField] int _columnCount;
        [SerializeField] Vector2 _itemSize;

        /// <summary>
        /// Returns size in pixels.
        /// </summary>
        public Vector2 ItemSize => activeSprite.GetSize(_itemSize);

        public void Construct(int rowCount, int columnCount, Vector2? itemSize = null)
        {
            _rowCount = rowCount;
            _columnCount = columnCount;
            _itemSize = itemSize.GetValue();
            OnDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            OnPopulateMesh(vh, sprite =>
            {
                if (_rowCount < 1 || _columnCount < 1) return;

                GetAABB(out float left, out float top, out float right, out float bottom);
                sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

                var itemSize = sprite.GetSize(_itemSize);
                float stepX = (right - left) / _columnCount;
                float stepY = (top - bottom) / _rowCount;

                left -= itemSize.x * 0.5f;
                top += itemSize.y * 0.5f;
                for (int row = 0; row <= _rowCount; row++)
                {
                    float l = left;
                    float b = top - itemSize.y;
                    for (int column = 0; column <= _columnCount; column++)
                    {
                        AddQuad(vh, l, top, l + itemSize.x, b, uvLeft, uvTop, uvRight, uvBottom);
                        l += stepX;
                    }
                    top -= stepY;
                }
            });
        }
    }
}
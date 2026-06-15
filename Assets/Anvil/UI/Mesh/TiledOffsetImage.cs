using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class TiledOffsetImage : UIMeshBehaviour
    {
        [SerializeField] Vector2 _spacing;
        [SerializeField] Vector2 _oddPadding;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            var sprite = activeSprite;
            if (sprite == null) return;

            GetAABB(out float left, out float top, out float right, out float bottom);
            sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

            float width = right - left;
            float height = top - bottom;
            sprite.GetSizeInPixels(out float spriteWidth, out float spriteHeight);
            float stepX = spriteWidth + _spacing.x;
            float stepY = spriteHeight + _spacing.y;
            int columnCount = Mathf.CeilToInt((width - Mathf.Epsilon + _spacing.x) / stepX);
            int rowCount = Mathf.CeilToInt((height - Mathf.Epsilon + _spacing.y) / stepY);
            float t = top;
            bool oddRow = false;
            for (int i = 0; i < rowCount; i++)
            {
                float l = left;
                if (oddRow)
                {
                    l += _oddPadding.x;
                }

                bool oddColumn = false;
                float b = t - spriteHeight;
                for (int j = 0; j < columnCount; j++)
                {
                    if (l < right)
                    {
                        float t2 = t;
                        float b2 = b;
                        if (oddColumn)
                        {
                            t2 -= _oddPadding.y;
                            b2 -= _oddPadding.y;
                        }

                        if (t2 > bottom)
                        {
                            float uvBottom2 = uvBottom;
                            if (b2 < bottom)
                            {
                                b2 = bottom;
                                uvBottom2 = uvTop - (uvTop - uvBottom) * (t2 - bottom) / spriteHeight;
                            }

                            float r = l + spriteWidth;
                            float uvRight2 = uvRight;
                            if (r > right)
                            {
                                r = right;
                                uvRight2 = uvLeft + (uvRight - uvLeft) * (right - l) / spriteWidth;
                            }

                            AddQuad(vh, l, t2, r, b2, uvLeft, uvTop, uvRight2, uvBottom2);
                        }
                    }

                    l += stepX;
                    oddColumn = !oddColumn;
                }

                t -= stepY;
                if (t < bottom) break;

                oddRow = !oddRow;
            }
        }
    }
}
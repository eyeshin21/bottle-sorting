using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class TopStretchImage : UIMeshBehaviour
    {
        [SerializeField] float _stretchHeight = 150;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            var sprite = activeSprite;
            if (sprite == null) return;

            GetAABB(out float left, out float top, out float right, out float bottom);
            sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

            AddSliced(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
            if (_stretchHeight > 0)
            {
                sprite.GetHorizontalBorderAndUVs(out float borderLeft, out float borderRight, out float uvX, out float uvZ);
                float top2 = top + _stretchHeight;
                if (borderLeft > 0 || borderRight > 0)
                {
                    // Left
                    if (borderLeft > 0)
                    {
                        AddQuad(vh, left, top2, left + borderLeft, top, uvLeft, uvTop, uvX, uvTop);
                    }
                    // Middle
                    AddQuad(vh, left + borderLeft, top2, right - borderRight, top, uvX, uvTop, uvZ, uvTop);
                    // Right
                    if (borderRight > 0)
                    {
                        AddQuad(vh, right - borderRight, top2, right, top, uvZ, uvTop, uvRight, uvTop);
                    }
                }
                else
                {
                    AddQuad(vh, left, top2, right, top, uvLeft, uvTop, uvRight, uvTop);
                }
            }
        }
    }
}
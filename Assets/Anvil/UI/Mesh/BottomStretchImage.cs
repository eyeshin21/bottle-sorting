using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public class BottomStretchImage : UIMeshBehaviour
    {
        [SerializeField] float _stretchHeight = 200;

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
                float bottom2 = bottom - _stretchHeight;
                if (borderLeft > 0 || borderRight > 0)
                {
                    // Left
                    if (borderLeft > 0)
                    {
                        AddQuad(vh, left, bottom, left + borderLeft, bottom2, uvLeft, uvBottom, uvX, uvBottom);
                    }
                    // Middle
                    AddQuad(vh, left + borderLeft, bottom, right - borderRight, bottom2, uvX, uvBottom, uvZ, uvBottom);
                    // Right
                    if (borderRight > 0)
                    {
                        AddQuad(vh, right - borderRight, bottom, right, bottom2, uvZ, uvBottom, uvRight, uvBottom);
                    }
                }
                else
                {
                    AddQuad(vh, left, bottom, right, bottom2, uvLeft, uvBottom, uvRight, uvBottom);
                }
            }
        }
    }
}
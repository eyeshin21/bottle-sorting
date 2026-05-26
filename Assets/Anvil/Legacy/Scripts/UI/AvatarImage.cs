using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class AvatarImage : UIMeshBehaviour
    {
        [SerializeField] float _cornerSize = 0.2f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            OnPopulateMesh(vh, sprite =>
            {
                GetAABB(out float left, out float top, out float right, out float bottom);
                sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

                if (_cornerSize <= 0)
                {
                    AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                    return;
                }

                sprite.GetSize(out float spriteWidth, out float spriteHeight);
                float cornerWidth = _cornerSize;
                float cornerHeight = cornerWidth;
                float uvOffsetX = (uvRight - uvLeft) * cornerWidth / spriteWidth;
                float uvOffsetY = (uvTop - uvBottom) * cornerHeight / spriteHeight;
                float uvX = uvLeft + uvOffsetX;
                float uvZ = uvRight - uvOffsetX;
                float uvY = uvBottom + uvOffsetY;
                float uvW = uvTop - uvOffsetY;

                float width = right - left;
                float height = top - bottom;
                float scaleX = width / spriteWidth;
                float scaleY = height / spriteHeight;
                cornerWidth *= scaleX;
                cornerHeight *= scaleY;

                float x = left + cornerWidth;
                float y = bottom + cornerHeight;
                float z = right - cornerWidth;
                float w = top - cornerHeight;

                /* Top */
                // Left
                AddTriangle(vh, left, top, x, w, uvLeft, uvTop, uvX, uvW, TriangleType.BottomRight);
                // Center
                AddQuad(vh, x, top, z, w, uvX, uvTop, uvZ, uvW);
                // Right
                AddTriangle(vh, z, top, right, w, uvZ, uvTop, uvRight, uvW, TriangleType.BottomLeft);

                /* Center */
                AddQuad(vh, left, w, right, y, uvLeft, uvW, uvRight, uvY);

                /* Bottom */
                // Left
                AddTriangle(vh, left, y, x, bottom, uvLeft, uvY, uvX, uvBottom, TriangleType.TopRight);
                // Center
                AddQuad(vh, x, y, z, bottom, uvX, uvY, uvZ, uvBottom);
                // Right
                AddTriangle(vh, z, y, right, bottom, uvZ, uvY, uvRight, uvBottom, TriangleType.TopLeft);
            });
        }

#if UNITY_EDITOR
        protected override void OnSpriteChanged()
        {
            SetNativeSize(activeSprite);
        }
#endif
    }
}
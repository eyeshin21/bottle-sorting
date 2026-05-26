using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class UICapsule : UIMeshBehaviour
    {
        [SerializeField] float _length;

        public float Length
        {
            get => _length;
            set
            {
                if (Set(ref _length, Mathf.Max(value, 0)))
                {
                    OnDirty();
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            OnPopulateMesh(vh, sprite =>
            {
                sprite.GetRectSize(out float spriteWidth, out float spriteHeight);
                Helper.GetAABB(spriteWidth, spriteHeight, out float left, out float top, out float right, out float bottom);
                sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

                if (_length <= 0)
                {
                    AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                }
                else
                {
                    float middle1 = 0;
                    float middle2 = middle1 + _length;
                    float uvMiddle1 = (uvLeft + uvRight) * 0.5f;
                    float uvMiddle2 = uvMiddle1;
                    AddQuad(vh, left, top, middle1, bottom, uvLeft, uvTop, uvMiddle1, uvBottom);
                    AddQuad(vh, middle1, top, middle2, bottom, uvMiddle1, uvTop, uvMiddle2, uvBottom);
                    AddQuad(vh, middle2, top, middle2 + spriteWidth * 0.5f, bottom, uvMiddle2, uvTop, uvRight, uvBottom);
                }
            });
        }
    }
}
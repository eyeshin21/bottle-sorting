using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class UIImage : UIMeshBehaviour
    {
        [SerializeField] UVTransformType _transformType;

#if UNITY_EDITOR
        protected override void OnSpriteChanged()
        {
            SetNativeSize(_sprite);
        }
#endif

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            OnPopulateMesh(vh, sprite =>
            {
                GetAABB(out float left, out float top, out float right, out float bottom);
                sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
                AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, _transformType);
            });
        }
    }
}
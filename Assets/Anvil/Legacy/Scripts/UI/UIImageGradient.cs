using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class UIImageGradient : UIMeshBehaviour
    {
        [SerializeField] Color _topLeft = Color.white;
        [SerializeField] Color _topRight = Color.white;
        [SerializeField] Color _bottomLeft = Color.white;
        [SerializeField] Color _bottomRight = Color.white;

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

                var vertex = UIVertex.simpleVert;
                vertex.color = _bottomLeft;
                vertex.position = new Vector3(left, bottom);
                vertex.uv0 = new Vector4(uvLeft, uvBottom);
                _quadVertices[0] = vertex;

                vertex.color = _bottomRight;
                vertex.position = new Vector3(right, bottom);
                vertex.uv0 = new Vector4(uvRight, uvBottom);
                _quadVertices[1] = vertex;

                vertex.color = _topRight;
                vertex.position = new Vector3(right, top);
                vertex.uv0 = new Vector4(uvRight, uvTop);
                _quadVertices[2] = vertex;

                vertex.color = _topLeft;
                vertex.position = new Vector3(left, top);
                vertex.uv0 = new Vector4(uvLeft, uvTop);
                _quadVertices[3] = vertex;

                vh.AddUIVertexQuad(_quadVertices);
            });
        }
    }
}
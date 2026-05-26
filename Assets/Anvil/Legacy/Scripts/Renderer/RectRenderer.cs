using UnityEngine;

namespace Anvil.Legacy
{
    public class RectRenderer : MeshBehaviour, ISizeSetter
    {
        [SerializeField] Color _color = Color.white;

        static Sprite _pixel;
        static Sprite Pixel
        {
            get
            {
                if (_pixel == null)
                {
                    var texture = new Texture2D(1, 1);
                    texture.SetPixel(0, 0, Color.white);
                    texture.Apply();
                    _pixel = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100);
                }
                return _pixel;
            }
        }

        void Start()
        {
            Color = _color;
        }

        public void SetSize(float width, float height)
        {
            SetTexture(Pixel);

            float left = -width * 0.5f;
            float right = left + width;
            float bottom = -height * 0.5f;
            float top = bottom + height;

            var vertices = CreateVertices(1);
            var uvs = CreateUVs(1);
            SetVertices(vertices, 0, left, top, right, bottom);
            SetUVs(uvs, 0, 0f, 1f, 1f, 0f);
            var triangles = CreateTriangles(1);
            UpdateMesh(vertices, triangles, uvs);
        }
    }
}
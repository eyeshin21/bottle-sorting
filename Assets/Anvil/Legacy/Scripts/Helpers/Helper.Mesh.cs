using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static Mesh CreateQuadMesh(Material material, out float textureWidth, out float textureHeight)
        {
            textureWidth = textureHeight = 1f;

            if (material != null)
            {
                var texture = material.mainTexture;
                if (texture != null)
                {
                    textureWidth = texture.width * 0.01f;
                    textureHeight = texture.height * 0.01f;
                    //Log.Debug($"textureWidth={textureWidth}, textureHeight={textureHeight}");
                }
                else
                {
                    LegacyLog.Warning("Texture is null!");
                }
            }
            else
            {
                LegacyLog.Warning("Material is null!");
            }

            return CreateQuadMesh(textureWidth, textureHeight);
        }

        public static Mesh CreateQuadMesh(float width, float height)
        {
            float left = -width * 0.5f;
            float right = left + width;
            float bottom = -height * 0.5f;
            float top = bottom + height;

            var mesh = new Mesh();
            mesh.vertices = new Vector3[4]
            {
                new Vector3(left, bottom, 0),
                new Vector3(right, bottom, 0),
                new Vector3(left, top, 0),
                new Vector3(right, top, 0)
            };
            mesh.triangles = new int[6]
            {
                0, 2, 1,
                2, 3, 1
            };
            mesh.uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
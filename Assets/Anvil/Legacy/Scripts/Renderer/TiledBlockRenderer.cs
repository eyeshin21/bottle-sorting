using UnityEngine;

namespace Anvil.Legacy
{
    public class TiledBlockRenderer : MeshBehaviour, IBlockRenderer
    {
        [SerializeField] Sprite _sprite;

        public void SetBlocks(bool[,] blocks, float cellSize)
        {
            blocks.GetSize(out int rowCount, out int columnCount);
            SetBlocks(blocks, rowCount, columnCount, cellSize);
        }

        public void SetBlocks(bool[,] blocks, int rowCount, int columnCount, float cellSize)
        {
            if (_sprite == null) return;
            SetTexture(_sprite);

            int quadCount = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    if (blocks[row, column])
                    {
                        quadCount++;
                    }
                }
            }

            if (quadCount == 0)
            {
                ClearMesh();
                return;
            }

            _sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);

            float width = columnCount * cellSize;
            float height = rowCount * cellSize;
            float left = -width * 0.5f;
            float top = height * 0.5f;

            var vertices = CreateVertices(quadCount);
            var uvs = CreateUVs(quadCount);
            int vertexIndex = 0;
            int uvIndex = 0;
            float l = left, t = top;
            float r, b;

            for (int row = 0; row < rowCount; row++)
            {
                r = l + cellSize;
                b = t - cellSize;
                for (int column = 0; column < columnCount; column++)
                {
                    if (blocks[row, column])
                    {
                        SetVertices(vertices, ref vertexIndex, l, t, r, b);
                        SetUVs(uvs, ref uvIndex, uvLeft, uvTop, uvRight, uvBottom);
                    }
                    l = r;
                    r += cellSize;
                }
                l = left;
                t -= cellSize;
            }

            var triangles = CreateTriangles(quadCount);
            UpdateMesh(vertices, triangles, uvs);
        }
    }
}
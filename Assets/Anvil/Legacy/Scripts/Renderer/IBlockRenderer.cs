using UnityEngine;

namespace Anvil.Legacy
{
    public interface IBlockRenderer
    {
        void SetBlocks(bool[,] blocks, float cellSize);
        void SetBlocks(bool[,] blocks, int rowCount, int columnCount, float cellSize);
    }
}
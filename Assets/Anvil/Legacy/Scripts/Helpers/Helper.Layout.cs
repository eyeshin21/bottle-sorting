using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static void GetTopLeft(int rowCount, int columnCount, float cellSize, out float top, out float left)
        {
            float width = columnCount * cellSize;
            float height = rowCount * cellSize;
            left = -width * 0.5f;
            top = height * 0.5f;
        }
    }
}
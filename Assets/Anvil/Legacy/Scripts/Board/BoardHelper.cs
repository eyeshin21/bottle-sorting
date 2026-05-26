using UnityEngine;

namespace Anvil.Legacy
{
    public static class BoardHelper
    {
        public static CellType[,] GetCellTypes(bool[,] cells)
        {
            cells.GetSize(out int rowCount, out int columnCount);
            return GetCellTypes(cells, rowCount, columnCount);
        }

        public static CellType[,] GetCellTypes(bool[,] cells, int rowCount, int columnCount)
        {
            var cellTypes = new CellType[rowCount, columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    if (cells[row, column])
                    {
                        var cellType = CellType.None;
                        // Left
                        if (column == 0 || !cells[row, column - 1])
                        {
                            cellType |= CellType.Left;
                        }
                        // Top
                        if (row == 0 || !cells[row - 1, column])
                        {
                            cellType |= CellType.Top;
                        }
                        // Right
                        if (column == columnCount - 1 || !cells[row, column + 1])
                        {
                            cellType |= CellType.Right;
                        }
                        // Bottom
                        if (row == rowCount - 1 || !cells[row + 1, column])
                        {
                            cellType |= CellType.Bottom;
                        }
                        cellTypes[row, column] = cellType;
                    }
                }
            }

            return cellTypes;
        }
    }
}
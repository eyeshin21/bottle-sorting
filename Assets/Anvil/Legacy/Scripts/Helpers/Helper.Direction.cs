using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static Direction GetDirection(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            if (fromRow == toRow)
            {
                if (fromColumn < toColumn) return Direction.Right;
                if (fromColumn > toColumn) return Direction.Left;
                return Direction.None;
            }

            if (fromColumn == toColumn)
            {
                if (fromRow < toRow) return Direction.Down;
                return Direction.Up;
            }

            LegacyLog.Error($"({fromRow},{fromColumn}) to ({toRow},{toColumn})");
            return Direction.None;
        }
    }
}
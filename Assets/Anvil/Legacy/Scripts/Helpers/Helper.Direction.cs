using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static Direction4 GetDirection(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            if (fromRow == toRow)
            {
                if (fromColumn < toColumn) return Direction4.Right;
                if (fromColumn > toColumn) return Direction4.Left;
                return Direction4.None;
            }

            if (fromColumn == toColumn)
            {
                if (fromRow < toRow) return Direction4.Down;
                return Direction4.Up;
            }

            LegacyLog.Error($"({fromRow},{fromColumn}) to ({toRow},{toColumn})");
            return Direction4.None;
        }
    }
}
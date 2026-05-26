using UnityEngine;

namespace Anvil.Legacy
{
    public class CoreCell : ICoreCell
    {
        int _row, _column;

        public CoreCell(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public void Get(out int row, out int column)
        {
            row = _row;
            column = _column;
        }

        public override string ToString()
        {
            return $"({_row},{_column})";
        }
    }
}
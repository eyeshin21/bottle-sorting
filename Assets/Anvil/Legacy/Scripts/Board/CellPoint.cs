using UnityEngine;

namespace Anvil.Legacy
{
    public class CellPoint
    {
        int _row, _column;

        public int Row => _row;
        public int Column => _column;

        public CellPoint(int value)
        {
            _row = _column = value;
        }

        public CellPoint(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public void Get(out int row, out int column)
        {
            row = _row;
            column = _column;
        }

        public void Set(int row, int column)
        {
            _row = row;
            _column = column;
        }

        public bool IsNegative()
        {
            return _row < 0 || _column < 0;
        }

        public void SetNegative()
        {
            _row = _column = -1;
        }

        public bool IsAt(int row, int column)
        {
            return row == _row && column == _column;
        }

        public bool Equals(CellPoint other)
        {
            return other._row == _row && other._column == _column;
        }

        public Direction4 GetDirectionTo(CellPoint other)
        {
            if (other._row == _row)
            {
                if (other._column < _column) return Direction4.Left;
                if (other._column > _column) return Direction4.Right;
            }
            else if (other._column == _column)
            {
                if (other._row < _row) return Direction4.Up;
                if (other._row > _row) return Direction4.Down;
            }

            return Direction4.None;
        }

        public int GetCellCount(CellPoint other)
        {
            if (other._row == _row)
            {
                return Mathf.Abs(other._column - _column) + 1;
            }
            else if (other._column == _column)
            {
                return Mathf.Abs(other._row - _row) + 1;
            }

            LegacyLog.Warning($"GetCellCount: {this} + {other}");
            return Mathf.Abs(other._column - _column) + Mathf.Abs(other._row - _row) + 1;
        }

        public override string ToString()
        {
            return $"({_row},{_column})";
        }
    }
}
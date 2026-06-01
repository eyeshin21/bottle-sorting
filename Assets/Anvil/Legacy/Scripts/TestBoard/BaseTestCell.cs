using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class BaseTestCell<TBoard, TCell> : ITestCell<TBoard, TCell>, IData
        where TBoard : ITestBoard<TBoard, TCell>
        where TCell : ITestCell<TBoard, TCell>
    {
        protected TBoard _board;
        protected int _row, _column;

        public TBoard Board => _board;
        public int Row => _row;
        public int Column => _column;

        public virtual void Construct(TBoard board, int row, int column)
        {
            _board = board;
            _row = row;
            _column = column;
        }

        public void Get(out int row, out int column)
        {
            row = _row;
            column = _column;
        }

        public TCell GetAdjacentCell(Direction4 direction)
        {
            if (_board != null)
            {
                if (direction == Direction4.Left) return _board.GetCell(_row, _column - 1);
                if (direction == Direction4.Right) return _board.GetCell(_row, _column + 1);
                if (direction == Direction4.Up) return _board.GetCell(_row - 1, _column);
                if (direction == Direction4.Down) return _board.GetCell(_row + 1, _column);
            }
            return default;
        }

        public void GetNeighbourCells(out TCell left, out TCell up, out TCell right, out TCell down)
        {
            left = _board.GetCell(_row, _column - 1);
            right = _board.GetCell(_row, _column + 1);
            up = _board.GetCell(_row - 1, _column);
            down = _board.GetCell(_row + 1, _column);
        }

        //public abstract bool Empty { get; }
        //public abstract void Clear();

        public abstract string Serialize();
        public abstract void Deserialize(string json);

        public override string ToString()
        {
            return $"({_row},{_column})";
        }
    }
}
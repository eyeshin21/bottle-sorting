using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class BaseCell<TBoard, TCell> : ICell<TBoard, TCell>, IData
        where TBoard : IBoard<TBoard, TCell>
        where TCell : ICell<TBoard, TCell>
    {
        protected TBoard _board;
        protected int _row, _column;
        protected Vector3 _localPos;

        public TBoard Board => _board;
        public int Row => _row;
        public int Column => _column;
        public Vector3 LocalPosition => _localPos;
        public Vector3 Position => _board.GetCellPosition(_row, _column);

        public abstract bool Empty { get; }

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

        public TCell GetAdjacentCell(Direction direction)
        {
            if (_board != null)
            {
                if (direction == Direction.Left) return _board.GetCell(_row, _column - 1);
                if (direction == Direction.Right) return _board.GetCell(_row, _column + 1);
                if (direction == Direction.Up) return _board.GetCell(_row - 1, _column);
                if (direction == Direction.Down) return _board.GetCell(_row + 1, _column);
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

        public virtual void SetLocalPosition(Vector3 localPos)
        {
            _localPos = localPos;
        }

        public abstract string Serialize();
        public abstract void Deserialize(string json);
        public abstract void Clear();

        public override string ToString()
        {
            return $"({_row},{_column})";
        }
    }
}
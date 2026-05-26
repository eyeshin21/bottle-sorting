using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public abstract class BaseTestBoard<TBoard, TCell> : ITestBoard<TBoard, TCell>
        where TBoard : ITestBoard<TBoard, TCell>
        where TCell : ITestCell<TBoard, TCell>
    {
        protected TCell[,] _cells;
        protected int _rowCount, _columnCount;

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;

        protected abstract TBoard _board { get; }
        protected abstract TCell NewCell();

        public virtual void Construct(int rowCount, int columnCount)
        {
            _cells = new TCell[rowCount, columnCount];
            _rowCount = rowCount;
            _columnCount = columnCount;

            var board = _board;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    var cell = NewCell();
                    cell.Construct(board, row, column);
                    _cells[row, column] = cell;
                }
            }
        }

        #region Cell
        public TCell GetCell(int row, int column)
        {
            if (row >= 0 && row < _rowCount && column >= 0 && column < _columnCount)
            {
                return _cells[row, column];
            }
            return default;
        }

        public bool HasCell(AcceptFunc<TCell> acceptFunc)
        {
            for (int row = 0; row < _rowCount; row++)
            {
                for (int column = 0; column < _columnCount; column++)
                {
                    if (acceptFunc(_cells[row, column]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void ForEachCell(Callback<TCell> callback)
        {
            for (int row = 0; row < _rowCount; row++)
            {
                for (int column = 0; column < _columnCount; column++)
                {
                    callback(_cells[row, column]);
                }
            }
        }

        public void ForEachCell(ContinueFunc<TCell> continueFunc)
        {
            for (int row = 0; row < _rowCount; row++)
            {
                for (int column = 0; column < _columnCount; column++)
                {
                    if (!continueFunc(_cells[row, column]))
                    {
                        return;
                    }
                }
            }
        }

        public void ForEachCellRandom(Callback<TCell> callback)
        {
            var indices = RandomIndices;
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                int index = indices[i];
                int row = index / _columnCount;
                int column = index % _columnCount;
                callback(_cells[row, column]);
            }
        }

        public void ForEachCellRandom(ContinueFunc<TCell> continueFunc)
        {
            var indices = RandomIndices;
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                int index = indices[i];
                int row = index / _columnCount;
                int column = index % _columnCount;
                if (!continueFunc(_cells[row, column]))
                {
                    return;
                }
            }
        }
        #endregion

        #region Others
        protected List<int> _randomIndices;
        protected List<int> RandomIndices
        {
            get
            {
                Helper.GetRandomIndices(ref _randomIndices, _rowCount * _columnCount);
                return _randomIndices;
            }
        }
        #endregion
    }
}
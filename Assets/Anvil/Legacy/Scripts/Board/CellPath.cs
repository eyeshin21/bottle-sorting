using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class CellPath : IPoolItem
    {
        int _row, _column; // Start
        List<CellPathSegment> _segments = new();

        public int Length
        {
            get
            {
                int length = 0;
                int segmentCount = _segments.Count;
                for (int i = 0; i < segmentCount; i++)
                {
                    int segmentLength = _segments[i].Length;
                    Assert.IsPositive(segmentLength);
                    length += segmentLength;
                    if (i > 0)
                    {
                        length--;
                    }
                }
                return length;
            }
        }

        public int SegmentCount => _segments.Count;

        void Construct(int row, int column)
        {
            Assert.IsEmpty(_segments);
            _row = row;
            _column = column;
        }

        void Construct(int row, int column, int toRow, int toColumn)
        {
            Assert.IsEmpty(_segments);
            _row = row;
            _column = column;
            var direction = Helper.GetDirection(row, column, toRow, toColumn);
            int length = direction.IsHorizontal() ? Mathf.Abs(toColumn - column) + 1 : Mathf.Abs(toRow - row) + 1;
            var segment = CellPathSegment.Create(direction, length);
            _segments.Add(segment);
        }

        public void AddCell(int row, int column, ref int lastRow, ref int lastColumn, ref CellPathSegment lastSegment)
        {
            var direction = Helper.GetDirection(lastRow, lastColumn, row, column);
            if (lastSegment == null)
            {
                Assert.IsEmpty(_segments);
                lastSegment = CellPathSegment.Create(direction, 2);
                _segments.Add(lastSegment);
            }
            else
            {
                if (lastSegment.Direction == direction)
                {
                    lastSegment.Length++;
                }
                else
                {
                    lastSegment = CellPathSegment.Create(direction, 2);
                    _segments.Add(lastSegment);
                }
            }
            lastRow = row;
            lastColumn = column;
        }

        /// <summary>
        /// callback(row, column)
        /// </summary>
        public void ForEachCell(Callback<int, int> callback, bool skipStart = false)
        {
            if (!skipStart)
            {
                callback(_row, _column);
            }

            int row = _row;
            int column = _column;
            int segmentCount = _segments.Count;
            for (int i = 0; i < segmentCount; i++)
            {
                _segments[i].Get(out Direction4 direction, out int length);

                if (direction == Direction4.Left)
                {
                    for (int j = 1; j < length; j++)
                    {
                        column--;
                        callback(row, column);
                    }
                }
                else if (direction == Direction4.Right)
                {
                    for (int j = 1; j < length; j++)
                    {
                        column++;
                        callback(row, column);
                    }
                }
                else if (direction == Direction4.Up)
                {
                    for (int j = 1; j < length; j++)
                    {
                        row--;
                        callback(row, column);
                    }
                }
                else if (direction == Direction4.Down)
                {
                    for (int j = 1; j < length; j++)
                    {
                        row++;
                        callback(row, column);
                    }
                }
            }
        }

        /// <summary>
        /// Start + Crosses + End.
        /// callback(row, column)
        /// </summary>
        public void ForEachAnchorCell(Callback<int, int> callback)
        {
            callback(_row, _column);

            int row = _row;
            int column = _column;
            int segmentCount = _segments.Count;
            for (int i = 0; i < segmentCount; i++)
            {
                _segments[i].Get(out Direction4 direction, out int length);

                if (direction == Direction4.Left)
                {
                    column -= length - 1;
                    callback(row, column);
                }
                else if (direction == Direction4.Right)
                {
                    column += length - 1;
                    callback(row, column);
                }
                else if (direction == Direction4.Up)
                {
                    row -= length - 1;
                    callback(row, column);
                }
                else if (direction == Direction4.Down)
                {
                    row += length - 1;
                    callback(row, column);
                }
            }
        }

        public override string ToString()
        {
            return $"({_row},{_column}): segments={_segments.ToString(" -> ")}";
        }

        #region Pool
        static PoolItem<CellPath> _pool;
        static PoolItem<CellPath> Pool => _pool ??= new();

        public static CellPath Create(int row, int column)
        {
            var path = Pool.Get();
            path.Construct(row, column);
            return path;
        }

        public static CellPath Create(int row, int column, int toRow, int toColumn)
        {
            var path = Pool.Get();
            path.Construct(row, column, toRow, toColumn);
            return path;
        }

        void IPoolItem.OnReturnPool()
        {
            CellPathSegment.Return(_segments);
        }

        public static void Return(CellPath path)
        {
            Assert.IsNotNull(path);
            Pool.Return(path);
        }

        public static void Return(List<CellPath> paths)
        {
            Pool.Return(paths);
        }
        #endregion
    }
}
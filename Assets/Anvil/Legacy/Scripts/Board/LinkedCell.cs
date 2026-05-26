using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class LinkedCell : ICoreCell
    {
        int _row, _column;
        LinkedCell _left, _top, _right, _bottom;

        public LinkedCell Left => _left;
        public LinkedCell Top => _top;
        public LinkedCell Right => _right;
        public LinkedCell Bottom => _bottom;

        public CellBorderType BorderType
        {
            get
            {
                if (_top != null)
                {
                    if (_left != null)
                    {
                        if (_right != null)
                        {
                            return _bottom != null ? CellBorderType.None : CellBorderType.Bottom;
                        }
                        // _right = null
                        return _bottom != null ? CellBorderType.Right : CellBorderType.BottomRight;
                    }
                    // _left = null
                    if (_right != null)
                    {
                        return _bottom != null ? CellBorderType.Left : CellBorderType.BottomLeft;
                    }
                    // _right = null
                    return _bottom != null ? CellBorderType.LeftRight : CellBorderType.BottomLeftRight;
                }

                // _top = null
                if (_left != null)
                {
                    if (_right != null)
                    {
                        return _bottom != null ? CellBorderType.Top : CellBorderType.TopBottom;
                    }
                    // _right = null
                    return _bottom != null ? CellBorderType.TopRight : CellBorderType.RightTopBottom;
                }

                // _left = null
                if (_right != null)
                {
                    return _bottom != null ? CellBorderType.TopLeft : CellBorderType.LeftTopBottom;
                }

                // _right = null
                return _bottom != null ? CellBorderType.TopLeftRight : CellBorderType.All;
            }
        }

        public bool IsAt(int row, int column)
        {
            return row == _row && column == _column;
        }

        public void Get(out int row, out int column)
        {
            row = _row;
            column = _column;
        }

        /// <summary>
        /// cell + this
        /// </summary>
        public void LinkLeft(LinkedCell cell)
        {
            Assert.IsNull(_left, $"[{this}] LinkLeft: left={_left}, cell={cell}");
            Assert.IsNull(cell._right);
            _left = cell;
            cell._right = this;
        }

        /// <summary>
        /// cell
        ///  +
        /// this
        /// </summary>
        public void LinkTop(LinkedCell cell)
        {
            Assert.IsNull(_top, $"[{this}] LinkTop: top={_top}, cell={cell}");
            Assert.IsNull(cell._bottom);
            _top = cell;
            cell._bottom = this;
        }

        /// <summary>
        /// this + cell
        /// </summary>
        public void LinkRight(LinkedCell cell)
        {
            Assert.IsNull(_right, $"[{this}] LinkRight: right={_right}, cell={cell}");
            Assert.IsNull(cell._left);
            _right = cell;
            cell._left = this;
        }

        /// <summary>
        /// this
        ///  +
        /// cell
        /// </summary>
        public void LinkBottom(LinkedCell cell)
        {
            Assert.IsNull(_bottom, $"[{this}] LinkBottom: bottom={_bottom}, cell={cell}");
            Assert.IsNull(cell._top);
            _bottom = cell;
            cell._top = this;
        }

        /// <summary>
        /// cell + this
        /// </summary>
        //public void CheckLinkLeft(LinkedCell cell)
        //{
        //    if (_left == null)
        //    {
        //        Assert.IsNull(cell._right);
        //        _left = cell;
        //        cell._right = this;
        //    }
        //    else
        //    {
        //        Assert.IsEquals(_left, cell, $"[{this}] CheckLinkLeft: left={_left}, cell={cell}");
        //    }
        //}

        /// <summary>
        /// cell
        ///  +
        /// this
        /// </summary>
        //public void CheckLinkTop(LinkedCell cell)
        //{
        //    if (_top == null)
        //    {
        //        Assert.IsNull(cell._bottom);
        //        _top = cell;
        //        cell._bottom = this;
        //    }
        //    else
        //    {
        //        Assert.IsEquals(_top, cell, $"[{this}] CheckLinkTop: top={_top}, cell={cell}");
        //    }
        //}

        /// <summary>
        /// this + cell
        /// </summary>
        //public void CheckLinkRight(LinkedCell cell)
        //{
        //    if (_right == null)
        //    {
        //        Assert.IsNull(cell._left);
        //        _right = cell;
        //        cell._left = this;
        //    }
        //    else
        //    {
        //        Assert.IsEquals(_right, cell, $"[{this}] CheckLinkRight: right={_right}, cell={cell}");
        //    }
        //}

        /// <summary>
        /// this
        ///  +
        /// cell
        /// </summary>
        //public void CheckLinkBottom(LinkedCell cell)
        //{
        //    if (_bottom == null)
        //    {
        //        Assert.IsNull(cell._top);
        //        _bottom = cell;
        //        cell._top = this;
        //    }
        //    else
        //    {
        //        Assert.IsEquals(_bottom, cell, $"[{this}] CheckLinkBottom: bottom={_bottom}, cell={cell}");
        //    }
        //}

        #region Get
        static bool[,] _visiteds;

        static void ResetVisiteds(int rowCount, int columnCount)
        {
            if (_visiteds.IsSize(rowCount, columnCount))
            {
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        _visiteds[row, column] = false;
                    }
                }
            }
            else
            {
                _visiteds = new bool[rowCount, columnCount];
            }
        }

        public static List<List<LinkedCell>> GetAll(int rowCount, int columnCount, AcceptFunc<int, int> acceptFunc)
        {
            ResetVisiteds(rowCount, columnCount);

            var list = new List<List<LinkedCell>>();
            List<LinkedCell> cells;

            void FloodFill(LinkedCell cell)
            {
                cell.Get(out int row, out int column);

                // Up
                if (row > 0)
                {
                    int row2 = row - 1;
                    if (_visiteds[row2, column])
                    {
                        if (cell.Top == null)
                        {
                            var cell2 = cells.Get(item => item.IsAt(row2, column));
                            if (cell2 != null)
                            {
                                cell.LinkTop(cell2);
                            }
                        }
                    }
                    else
                    {
                        _visiteds[row2, column] = true;
                        if (acceptFunc(row2, column))
                        {
                            var cell2 = GetCell(row2, column);
                            cell.LinkTop(cell2);
                            cells.Add(cell2);
                            FloodFill(cell2);
                        }
                    }
                }

                // Left
                if (column > 0)
                {
                    int column2 = column - 1;
                    if (_visiteds[row, column2])
                    {
                        if (cell.Left == null)
                        {
                            var cell2 = cells.Get(item => item.IsAt(row, column2));
                            if (cell2 != null)
                            {
                                cell.LinkLeft(cell2);
                            }
                        }
                    }
                    else
                    {
                        _visiteds[row, column2] = true;
                        if (acceptFunc(row, column2))
                        {
                            var cell2 = GetCell(row, column2);
                            cell.LinkLeft(cell2);
                            cells.Add(cell2);
                            FloodFill(cell2);
                        }
                    }
                }

                // Right
                if (column < columnCount - 1)
                {
                    int column2 = column + 1;
                    if (_visiteds[row, column2])
                    {
                        if (cell.Right == null)
                        {
                            var cell2 = cells.Get(item => item.IsAt(row, column2));
                            if (cell2 != null)
                            {
                                cell.LinkRight(cell2);
                            }
                        }
                    }
                    else
                    {
                        _visiteds[row, column2] = true;
                        if (acceptFunc(row, column2))
                        {
                            var cell2 = GetCell(row, column2);
                            cell.LinkRight(cell2);
                            cells.Add(cell2);
                            FloodFill(cell2);
                        }
                    }
                }

                // Down
                if (row < rowCount - 1)
                {
                    int row2 = row + 1;
                    if (_visiteds[row2, column])
                    {
                        if (cell.Bottom == null)
                        {
                            var cell2 = cells.Get(item => item.IsAt(row2, column));
                            if (cell2 != null)
                            {
                                cell.LinkBottom(cell2);
                            }
                        }
                    }
                    else
                    {
                        _visiteds[row2, column] = true;
                        if (acceptFunc(row2, column))
                        {
                            var cell2 = GetCell(row2, column);
                            cell.LinkBottom(cell2);
                            cells.Add(cell2);
                            FloodFill(cell2);
                        }
                    }
                }
            }

            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    if (!_visiteds[row, column])
                    {
                        _visiteds[row, column] = true;
                        if (acceptFunc(row, column))
                        {
                            cells = new List<LinkedCell>();
                            var cell = GetCell(row, column);
                            cells.Add(cell);
                            FloodFill(cell);
                            list.Add(cells);
                        }
                    }
                }
            }

            return list;
        }
        #endregion

        #region Pool
        public void ReturnToPool()
        {
            _left = _top = _right = _bottom = null;
            Pool.Return(this);
        }

        static Pool<LinkedCell> _pool;
        static Pool<LinkedCell> Pool => _pool ??= new();

        public static LinkedCell GetCell(int row, int column)
        {
            var cell = Pool.Get();
            cell._row = row;
            cell._column = column;
            return cell;
        }

        public static void Return(List<LinkedCell> cells)
        {
            int count = cells.GetCount();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    cells[i].ReturnToPool();
                }
                cells.Clear();
            }
        }
        #endregion

#if UNITY_EDITOR
        public override string ToString()
        {
            return $"({_row},{_column})";
        }

        public static void DrawGizmos(Transform transform, int rowCount, int columnCount, float cellSize, List<List<LinkedCell>> listCells)
        {
            if (listCells == null) return;

            var pos = transform.position;
            Helper.GetTopLeft(rowCount, columnCount, cellSize, out float top, out float left);
            left += pos.x;
            top += pos.y;

            foreach (var cells in listCells)
            {
                foreach (var cell in cells)
                {
                    cell.Get(out int row, out int column);
                    pos.x = left + column * cellSize + cellSize * 0.5f;
                    pos.y = top - row * cellSize - cellSize * 0.5f;
                    GizmosHelper.DrawText($"{(int)cell.BorderType}", pos, Color.blue);
                }
            }
        }
#endif
    }
}
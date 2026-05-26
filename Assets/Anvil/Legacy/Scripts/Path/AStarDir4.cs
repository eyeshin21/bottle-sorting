using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    /// <summary>
    /// https://www.geeksforgeeks.org/a-search-algorithm/
    /// </summary>
    public class AStarDir4
    {
        class Pair
        {
            public int row, column;

            public Pair(int row, int column)
            {
                this.row = row;
                this.column = column;
            }

            public void Get(out int row, out int column)
            {
                row = this.row;
                column = this.column;
            }

            public bool IsAt(int row, int column)
            {
                return row == this.row && column == this.column;
            }

            public bool IsValid(int rowCount, int columnCount)
            {
                return row >= 0 && row < rowCount && column >= 0 && column < columnCount;
            }

            public bool IsEquals(Pair pair)
            {
                return row == pair.row && column == pair.column;
            }

            public override string ToString()
            {
                return $"({row},{column})";
            }
        }

        class Node
        {
            public int row, column;
            public int f;

            public Node(int row, int column, int f)
            {
                this.row = row;
                this.column = column;
                this.f = f;
            }

            public void Get(out int row, out int column)
            {
                row = this.row;
                column = this.column;
            }

            public bool IsAt(int row, int column)
            {
                return row == this.row && column == this.column;
            }

            public bool IsValid(int rowCount, int columnCount)
            {
                return row >= 0 && row < rowCount && column >= 0 && column < columnCount;
            }

            public bool IsEquals(Pair pair)
            {
                return row == pair.row && column == pair.column;
            }

            public override string ToString()
            {
                return $"({row},{column}): f={f}";
            }
        }

        class Cell
        {
            public int parentRow, parentColumn;
            // f = g + h
            public int f, g, h;

            public Cell()
            {
                parentRow = parentColumn = -1;
                f = g = h = -1;
            }

            public void Start(int parentRow, int parentColumn)
            {
                this.parentRow = parentRow;
                this.parentColumn = parentColumn;
                f = g = h = 0;
            }

            public void Set(int parentRow, int parentColumn)
            {
                this.parentRow = parentRow;
                this.parentColumn = parentColumn;
            }

            public void Set(int parentRow, int parentColumn, int f, int g, int h)
            {
                this.parentRow = parentRow;
                this.parentColumn = parentColumn;
                this.f = f;
                this.g = g;
                this.h = h;
            }

            public bool IsParent(int parentRow, int parentColumn)
            {
                return parentRow == this.parentRow && parentColumn == this.parentColumn;
            }

            public void GetParent(out int parentRow, out int parentColumn)
            {
                parentRow = this.parentRow;
                parentColumn = this.parentColumn;
            }

            public void Reset()
            {
                parentRow = parentColumn = -1;
                f = g = h = -1;
            }
        }

        Cell[,] _cells;
        List<Node> _openList = new();
        bool[,] _closedList;
        int _rowCount, _columnCount;
        Stack<Pair> _pairs = new();

        // Down, Left, Right, Up
        static int[] _dirRows = new int[] { 1, 0, 0, -1 };
        static int[] _dirColumns = new int[] { 0, -1, 1, 0 };

        public CellPath FindPath(bool[,] blocks, int srcRow, int srcColumn, int destRow, int destColumn)
        {
            //Log.Debug($"Find path: ({srcRow},{srcColumn}) => ({destRow},{destColumn})");
            blocks.GetSize(out int rowCount, out int columnCount);

            Assert.IsTrue(srcRow >= 0 && srcRow < rowCount && srcColumn >= 0 && srcColumn < columnCount);
            Assert.IsTrue(destRow >= 0 && destRow < rowCount && destColumn >= 0 && destColumn < columnCount);
            Assert.IsFalse(blocks[srcRow, srcColumn]);
            Assert.IsFalse(blocks[destRow, destColumn]);
            Assert.IsFalse(srcRow == destRow && srcColumn == destColumn);

            int row, column;
            if (_rowCount != rowCount || _columnCount != columnCount)
            {
                _rowCount = rowCount;
                _columnCount = columnCount;
                _cells = new Cell[rowCount, columnCount];
                _closedList = new bool[rowCount, columnCount];

                for (row = 0; row < rowCount; row++)
                {
                    for (column = 0; column < columnCount; column++)
                    {
                        _cells[row, column] = new Cell();
                    }
                }
            }
            else
            {
                for (row = 0; row < rowCount; row++)
                {
                    for (column = 0; column < columnCount; column++)
                    {
                        _cells[row, column].Reset();
                        _closedList[row, column] = false;
                    }
                }
            }
            _openList.Clear();

            // Initialising the parameters of the starting node
            row = srcRow;
            column = srcColumn;
            _cells[row, column].Start(row, column);

            // Put the starting cell on the open list and set its
            // 'f' as 0
            _openList.Add(new Node(row, column, 0));

            while (_openList.Count > 0)
            {
                var p = _openList[0];
                _openList.RemoveAt(0);

                // Add this vertex to the closed list
                p.Get(out row, out column);
                _closedList[row, column] = true;
                //Log.Warning($"Remove {p} (count={_openList.Count})");

                // Generating all the 4 successors of this cell
                for (int i = 0; i < 4; i++)
                {
                    int newRow = row + _dirRows[i];
                    int newColumn = column + _dirColumns[i];
                    //Log.Debug($"({row},{column}) => ({newRow},{newColumn})");

                    // If this successor is a valid cell
                    if (newRow >= 0 && newRow < _rowCount && newColumn >= 0 && newColumn < _columnCount)
                    {
                        // If the destination cell is the same as the current successor
                        if (newRow == destRow && newColumn == destColumn)
                        {
                            _cells[newRow, newColumn].Set(row, column);
                            return GetPath(destRow, destColumn);
                        }

                        // If the successor is already on the closed list or if it is blocked, then ignore it.
                        if (!_closedList[newRow, newColumn] && !blocks[newRow, newColumn])
                        {
                            int gNew = _cells[row, column].g + 1;
                            int hNew = Mathf.Abs(destRow - newRow) + Mathf.Abs(destColumn - newColumn);
                            int fNew = gNew + hNew;

                            // If it isn’t on the open list, add it to the open list. Make the current square
                            // the parent of this square. Record the f, g, and h costs of the square cell
                            var cellNew = _cells[newRow, newColumn];
                            if (cellNew.f < 0 || cellNew.f > fNew)
                            {
                                _openList.InsertAsc(new Node(newRow, newColumn, fNew), item => item.f);
                                cellNew.Set(row, column, fNew, gNew, hNew);
                                //Log.Debug($"Add ({row},{column}) => ({newRow},{newColumn}): fNew={fNew} (count={_openList.Count})");
                            }
                            //else
                            //{
                            //   LegacyLog.Warning($"Skip add ({row},{column}) => ({newRow},{newColumn}): f={cellNew.f} vs fNew={fNew}");
                            //}
                        }
                    }
                }
            }

            //Log.Warning($"Can't find path ({srcRow},{srcColumn}) => ({destRow},{destColumn})");
            return null;
        }

        CellPath GetPath(int row, int column)
        {
            //int destRow = row, destColumn = column;
            _pairs.Clear();

            while (!_cells[row, column].IsParent(row, column))
            {
                _pairs.Push(new Pair(row, column));
                _cells[row, column].GetParent(out row, out column);
            }

            _pairs.Push(new Pair(row, column));
            if (_pairs.Count > 0)
            {
                Pair p = _pairs.Pop();
                p.Get(out int lastRow, out int lastColumn);
                CellPathSegment lastSegment = null;
                var path = CellPath.Create(lastRow, lastColumn);
                while (_pairs.Count > 0)
                {
                    p = _pairs.Pop();
                    path.AddCell(p.row, p.column, ref lastRow, ref lastColumn, ref lastSegment);
                }
                //Log.Debug($"Get path ({destRow},{destColumn}): {path}");
                return path;
            }

            //Log.Warning($"Can't get path ({destRow},{destColumn})!");
            return null;
        }
    }
}
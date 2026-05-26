using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class BorderHelper
    {
        static readonly float Epsilon = 0.01f;

        static bool[,] _boolCells;
        static bool[,] _visiteds;

        static void SetBoolCells(int rowCount, int columnCount, bool value)
        {
            if (_boolCells.IsSize(rowCount, columnCount))
            {
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        _boolCells[row, column] = value;
                    }
                }
            }
            else
            {
                _boolCells = new bool[rowCount, columnCount];
                if (value)
                {
                    for (int row = 0; row < rowCount; row++)
                    {
                        for (int column = 0; column < columnCount; column++)
                        {
                            _boolCells[row, column] = value;
                        }
                    }
                }
            }
        }

        static void SetBoolCells<T>(List<T> cells, bool value) where T : ICoreCell
        {
            foreach (var cell in cells)
            {
                cell.Get(out int row, out int column);
                _boolCells[row, column] = value;
            }
        }

        public static List<BorderNode> GetBorder(int rowCount, int columnCount, float cellSize)
        {
            float width = columnCount * cellSize;
            float height = rowCount * cellSize;
            float left = -width * 0.5f;
            float right = left + width;
            float bottom = -height * 0.5f;
            float top = bottom + height;

            var topLeft = new BorderNode(left, top, CornerType.TopLeft);
            var topRight = new BorderNode(right, top, CornerType.TopRight);
            var bottomRight = new BorderNode(right, bottom, CornerType.BottomRight);
            var bottomLeft = new BorderNode(left, bottom, CornerType.BottomLeft);

            topLeft.LinkHorizontal(topRight);
            topRight.LinkVertical(bottomRight);
            bottomRight.LinkHorizontal(bottomLeft);
            bottomLeft.LinkVertical(topLeft);

            return new List<BorderNode>(4) { topLeft, topRight, bottomRight, bottomLeft };
        }

        public static List<BorderNode> GetBorder<T>(int rowCount, int columnCount, float cellSize, List<T> cells) where T : ICoreCell
        {
            List<BorderNode> nodes = new();
            BorderNode lastNode;
            Direction nextDirection;
            float startX, startY;
            bool isClosed;

            SetBoolCells(rowCount, columnCount, false);
            SetBoolCells(cells, true);

            bool HasCell(int row, int column)
            {
                if (row >= 0 && row < rowCount && column >= 0 && column < columnCount)
                {
                    return _boolCells[row, column];
                }
                return false;
            }

            void AddNode(float x, float y, CornerType cornerType, bool outer = false)
            {
                Assert.IsFalse(isClosed);
                BorderNode node;
                if (Mathf.Abs(x - startX) < Epsilon && Mathf.Abs(y - startY) < Epsilon)
                {
                    isClosed = true;
                    node = nodes[0];
                }
                else
                {
                    node = new BorderNode(x, y, cornerType, outer);
                    nodes.Add(node);
                }

                if (nextDirection.IsHorizontal())
                {
                    lastNode.LinkHorizontal(node);
                }
                else
                {
                    lastNode.LinkVertical(node);
                }

                lastNode = node;
            }

            Helper.GetTopLeft(rowCount, columnCount, cellSize, out float top, out float left);
            var cell = cells[0];
            cell.Get(out int row, out int column);
            float x = left + column * cellSize;
            float y = top - row * cellSize;
            lastNode = new BorderNode(x, y, CornerType.TopLeft);
            nodes.Add(lastNode);

            nextDirection = Direction.Right;
            startX = x;
            startY = y;
            isClosed = false;

            var cornerType = lastNode.CornerType;
            do
            {
                if (cornerType == CornerType.TopLeft)
                {
                    if (nextDirection.IsRight())
                    {
                        x += cellSize;

                        /* Right-Up
                         *      +----+
                         *      ^    |
                         *      |    |
                         * 1--->2----+
                         * |    |    |
                         * |    |    |
                         * +----+----+
                         */
                        if (HasCell(row - 1, column + 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomRight, true);

                            row--;
                            column++;
                            nextDirection = Direction.Up;
                        }
                        /* Right-Right
                         * 1--->2--->+
                         * |    |    |
                         * |    |    |
                         * +----+----+
                         */
                        else if (HasCell(row, column + 1))
                        {
                            column++;
                        }
                        /* Right-Down
                         * 1--->2
                         * |    |
                         * |    v
                         * +----+
                         */
                        else
                        {
                            // 2
                            AddNode(x, y, CornerType.TopRight);
                            nextDirection = Direction.Down;
                        }
                    }
                    else if (nextDirection.IsDown())
                    {
                        y -= cellSize;

                        /* Down-Left
                         *      1----+
                         *      |    |
                         *      v    |
                         * +<---2----+
                         * |    |    |
                         * |    |    |
                         * +----+----+
                         */
                        if (HasCell(row + 1, column - 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomRight, true);

                            row++;
                            column--;
                            nextDirection = Direction.Left;
                        }
                        /* Down-Down
                         * 1----+
                         * |    |
                         * v    |
                         * 2----+
                         * |    |
                         * v    |
                         * +----+
                         */
                        else if (HasCell(row + 1, column))
                        {
                            row++;
                        }
                        /* Down-Right
                         * 1----+
                         * |    |
                         * v    |
                         * 2--->+
                         */
                        else
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomLeft);
                            nextDirection = Direction.Right;
                        }
                    }
                    else
                    {
                        Assert.Bug($"{cornerType}: nextDirection={nextDirection}");
                    }
                }
                else if (cornerType == CornerType.TopRight)
                {
                    if (nextDirection.IsLeft())
                    {
                        x -= cellSize;

                        /* Left-Up
                         * +----+
                         * |    ^
                         * |    |
                         * +----2<---1
                         * |    |    |
                         * |    |    |
                         * +----+----+
                         */
                        if (HasCell(row - 1, column - 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomLeft, true);

                            row--;
                            column--;
                            nextDirection = Direction.Up;
                        }
                        /* Left-Left
                         * +<---2<---1
                         * |    |    |
                         * |    |    |
                         * +----+----+
                         */
                        else if (HasCell(row, column - 1))
                        {
                            column--;
                        }
                        /* Left-Down
                         * 2<---1
                         * |    |
                         * v    |
                         * +----+
                         */
                        else
                        {
                            LegacyLog.Warning($"{cornerType}: Left-Down");

                            // 2
                            AddNode(x, y, CornerType.TopLeft);
                            nextDirection = Direction.Down;
                        }
                    }
                    else if (nextDirection.IsDown())
                    {
                        y -= cellSize;

                        /* Down-Right
                         * +----1
                         * |    |
                         * |    v
                         * +----2--->+
                         * |    |    |
                         * |    |    |
                         * +----+----+
                         */
                        if (HasCell(row + 1, column + 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomLeft, true);

                            row++;
                            column++;
                            nextDirection = Direction.Right;
                        }
                        /* Down-Down
                         * +----1
                         * |    |
                         * |    v
                         * +----2
                         * |    |
                         * |    v
                         * +----+
                         */
                        else if (HasCell(row + 1, column))
                        {
                            row++;
                        }
                        /* Down-Left
                         * +----1
                         * |    |
                         * |    v
                         * +<---2
                         */
                        else
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomRight);
                            nextDirection = Direction.Left;
                        }
                    }
                    else
                    {
                        Assert.Bug($"{cornerType}: nextDirection={nextDirection}");
                    }
                }
                else if (cornerType == CornerType.BottomRight)
                {
                    if (nextDirection.IsLeft())
                    {
                        x -= cellSize;

                        /* Left-Down
                         * +----+----+
                         * |    |    |
                         * |    |    |
                         * +----2<---1
                         * |    |
                         * |    v
                         * +----+
                         */
                        if (HasCell(row + 1, column - 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.TopLeft, true);

                            row++;
                            column--;
                            nextDirection = Direction.Down;
                        }
                        /* Left-Left
                         * +----+----+
                         * |    |    |
                         * |    |    |
                         * +<---2<---1
                         */
                        else if (HasCell(row, column - 1))
                        {
                            column--;
                        }
                        /* Left-Up
                         * +----+
                         * ^    |
                         * |    |
                         * 2<---1
                         */
                        else
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomLeft);
                            nextDirection = Direction.Up;
                        }
                    }
                    else if (nextDirection.IsUp())
                    {
                        y += cellSize;

                        /* Up-Right
                         * +----+----+
                         * |    |    |
                         * |    |    |
                         * +----2--->+
                         * |    ^
                         * |    |
                         * +----1
                         */
                        if (HasCell(row - 1, column + 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.TopLeft, true);

                            row--;
                            column++;
                            nextDirection = Direction.Right;
                        }
                        /* Up-Up
                         * +----+
                         * |    ^
                         * |    |
                         * +----2
                         * |    ^
                         * |    |
                         * +----1
                         */
                        else if (HasCell(row - 1, column))
                        {
                            row--;
                        }
                        /* Up-Left
                         * +<---2
                         * |    ^
                         * |    |
                         * +----1
                         */
                        else
                        {
                            // 2
                            AddNode(x, y, CornerType.TopRight);
                            nextDirection = Direction.Left;
                        }
                    }
                    else
                    {
                        Assert.Bug($"{cornerType}: nextDirection={nextDirection}");
                    }
                }
                else if (cornerType == CornerType.BottomLeft)
                {
                    if (nextDirection.IsUp())
                    {
                        y += cellSize;

                        /* Up-Left
                         * +----+----+
                         * |    |    |
                         * |    |    |
                         * +<---2----+
                         *      ^    |
                         *      |    |
                         *      1----+
                         */
                        if (HasCell(row - 1, column - 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.TopRight, true);

                            row--;
                            column--;
                            nextDirection = Direction.Left;
                        }
                        /* Up-Up
                         * +----+
                         * ^    |
                         * |    |
                         * 2----+
                         * ^    |
                         * |    |
                         * 1----+
                         */
                        else if (HasCell(row - 1, column))
                        {
                            row--;
                        }
                        /* Up-Right
                         * 2--->+
                         * ^    |
                         * |    |
                         * 1----+
                         */
                        else
                        {
                            // 2
                            AddNode(x, y, CornerType.TopLeft);
                            nextDirection = Direction.Right;
                        }
                    }
                    else if (nextDirection.IsRight())
                    {
                        x += cellSize;

                        /* Right-Down
                         * +----+----+
                         * |    |    |
                         * |    |    |
                         * 1--->2----+
                         *      |    |
                         *      v    |
                         *      +----+
                         */
                        if (HasCell(row + 1, column + 1))
                        {
                            // 2
                            AddNode(x, y, CornerType.TopRight, true);

                            row++;
                            column++;
                            nextDirection = Direction.Down;
                        }
                        /* Right-Right
                         * +----+----+
                         * |    |    |
                         * |    |    |
                         * 1--->2--->+
                         */
                        else if (HasCell(row, column + 1))
                        {
                            column++;
                        }
                        /* Right-Up
                         * +----+
                         * |    ^
                         * |    |
                         * 1--->2
                         */
                        else
                        {
                            // 2
                            AddNode(x, y, CornerType.BottomRight);
                            nextDirection = Direction.Up;
                        }
                    }
                    else
                    {
                        Assert.Bug($"{cornerType}: nextDirection={nextDirection}");
                    }
                }

                cornerType = lastNode.CornerTypeWithoutOuter;
            }
            while (!isClosed);

            return nodes;
        }

        public static List<List<BorderNode>> GetBorders<T>(int rowCount, int columnCount, float cellSize, List<List<T>> listcells) where T : ICoreCell
        {
            var borders = new List<List<BorderNode>>();
            listcells.ForEach2(cells =>
            {
                borders.Add(GetBorder(rowCount, columnCount, cellSize, cells));
            });
            return borders;
        }

        public static List<BorderNode> GetBorder(int[,] cells, float cellSize)
        {
            cells.GetSize(out int rowCount, out int columnCount);
            var list = new List<ICoreCell>();
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    if (cells[row, column] > 0)
                    {
                        list.Add(new CoreCell(row, column));
                    }
                }
            }
            return GetBorder(rowCount, columnCount, cellSize, list);
        }

        public static List<List<BorderNode>> GetBorders(int[,] cells, float cellSize)
        {
            cells.GetSize(out int rowCount, out int columnCount);
            return GetBorders(rowCount, columnCount, cellSize, (row, col) => cells[row, col] > 0);
        }

        public static List<List<BorderNode>> GetBorders(int rowCount, int columnCount, float cellSize, AcceptFunc<int, int> acceptFunc)
        {
            void FloodFill(List<ICoreCell> cells, int row, int column)
            {
                if (_visiteds[row, column]) return;
                _visiteds[row, column] = true;

                if (!acceptFunc(row, column)) return;

                cells.Add(new CoreCell(row, column));

                // Left
                if (column > 0) FloodFill(cells, row, column - 1);
                // Up
                if (row > 0) FloodFill(cells, row - 1, column);
                // Right
                if (column < columnCount - 1) FloodFill(cells, row, column + 1);
                // Down
                if (row < rowCount - 1) FloodFill(cells, row + 1, column);
            }

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

            var borders = new List<List<BorderNode>>();
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    if (!_visiteds[row, column])
                    {
                        _visiteds[row, column] = true;
                        if (acceptFunc(row, column))
                        {
                            var cells = new List<ICoreCell>();
                            cells.Add(new CoreCell(row, column));
                            // Left
                            if (column > 0) FloodFill(cells, row, column - 1);
                            // Up
                            if (row > 0) FloodFill(cells, row - 1, column);
                            // Right
                            if (column < columnCount - 1) FloodFill(cells, row, column + 1);
                            // Down
                            if (row < rowCount - 1) FloodFill(cells, row + 1, column);
                            var border = GetBorder(rowCount, columnCount, cellSize, cells);
                            borders.Add(border);
                        }
                    }
                }
            }
            return borders;
        }

#if UNITY_EDITOR
        public static void Draw(Transform transform, List<List<BorderNode>> borders)
        {
            if (borders != null)
            {
                var pos = transform.GetPosition();
                foreach (var border in borders)
                {
                    Draw(border, pos.x, pos.y);
                }
            }
        }

        public static void Draw(List<BorderNode> nodes, float offsetX = 0, float offsetY = 0)
        {
            if (nodes == null) return;

            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            int count = nodes.Count;

            var color = Gizmos.color;
            var color1 = Color.blue;
            var color2 = Color.red;
            bool isColor1 = true;
            for (int i = 0; i < count; i++)
            {
                var node1 = nodes[i];
                var node2 = nodes[i < count - 1 ? i + 1 : 0];
                pos1.x = node1.X + offsetX;
                pos1.y = node1.Y + offsetY;
                pos2.x = node2.X + offsetX;
                pos2.y = node2.Y + offsetY;

                Gizmos.color = isColor1 ? color1 : color2;
                Gizmos.DrawLine(pos1, pos2);
                isColor1 = !isColor1;
            }
            Gizmos.color = color;
        }
#endif
    }
}
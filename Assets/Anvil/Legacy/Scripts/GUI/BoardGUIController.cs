using UnityEngine;

namespace Anvil.Legacy
{
    public interface IBoardGUIHandler
    {
        void ShiftLeft();
        void ShiftUp();
        void ShiftRight();
        void ShiftDown();

        void MoveLeft(int column);
        void MoveUp(int row);
        void MoveRight(int column);
        void MoveDown(int row);

        void ClearRow(int row);
        void ClearColumn(int column);
    }

    public class BoardGUIController : IGUI
    {
        static GUIContent ClearContent = new("C");

        static GUIContent MoveLeftContent = new("\u2190");
        static GUIContent MoveUpContent = new("\u2191");
        static GUIContent MoveRightContent = new("\u2192");
        static GUIContent MoveDownContent = new("\u2193");

        static GUIContent ShiftLeftContent = new("\u219E");
        static GUIContent ShiftUpContent = new("\u219F");
        static GUIContent ShiftRightContent = new("\u21A0");
        static GUIContent ShiftDownContent = new("\u21A1");

        class RowGUIController
        {
            IBoardGUIHandler _handler;
            int _row;
            Rect _rect;
            float _halfWidth, _halfHeight;

            public bool UpEnabled { get; set; } = true;
            public bool DownEnabled { get; set; } = true;

            public RowGUIController(IBoardGUIHandler handler, int row, Rect rect)
            {
                _handler = handler;
                _row = row;
                _rect = rect;
                _halfWidth = rect.width * 0.5f;
                _halfHeight = rect.height * 0.5f;
            }

            public void OnGUI()
            {
                var rect = _rect;
                rect.width = _halfWidth;
                // Clear
                if (GUI.Button(rect, ClearContent))
                {
                    _handler.ClearRow(_row);
                }
                rect.x += _halfWidth;
                rect.height = _halfHeight;
                // Up
                if (UpEnabled)
                {
                    if (GUI.Button(rect, MoveUpContent))
                    {
                        _handler.MoveUp(_row);
                    }
                }
                else
                {
                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = false;
                    if (GUI.Button(rect, MoveUpContent))
                    {
                        _handler.MoveUp(_row);
                    }
                    GUI.enabled = guiEnabled;
                }
                rect.y += _halfHeight;
                // Down
                if (DownEnabled)
                {
                    if (GUI.Button(rect, MoveDownContent))
                    {
                        _handler.MoveDown(_row);
                    }
                }
                else
                {
                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = false;
                    if (GUI.Button(rect, MoveDownContent))
                    {
                        _handler.MoveDown(_row);
                    }
                    GUI.enabled = guiEnabled;
                }
            }
        }

        class ColumnGUIController
        {
            IBoardGUIHandler _handler;
            int _column;
            Rect _rect;
            float _halfWidth, _halfHeight;

            public bool LeftEnabled { get; set; } = true;
            public bool RightEnabled { get; set; } = true;

            public ColumnGUIController(IBoardGUIHandler handler, int column, Rect rect)
            {
                _handler = handler;
                _column = column;
                _rect = rect;
                _halfWidth = rect.width * 0.5f;
                _halfHeight = rect.height * 0.5f;
            }

            public void OnGUI()
            {
                var rect = _rect;
                rect.height = _halfHeight;
                // Clear
                if (GUI.Button(rect, ClearContent))
                {
                    _handler.ClearColumn(_column);
                }
                rect.y += _halfHeight;
                rect.width = _halfWidth;
                // Left
                if (LeftEnabled)
                {
                    if (GUI.Button(rect, MoveLeftContent))
                    {
                        _handler.MoveLeft(_column);
                    }
                }
                else
                {
                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = false;
                    if (GUI.Button(rect, MoveLeftContent))
                    {
                        _handler.MoveLeft(_column);
                    }
                    GUI.enabled = guiEnabled;
                }
                rect.x += _halfWidth;
                // Right
                if (RightEnabled)
                {
                    if (GUI.Button(rect, MoveRightContent))
                    {
                        _handler.MoveRight(_column);
                    }
                }
                else
                {
                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = false;
                    if (GUI.Button(rect, MoveRightContent))
                    {
                        _handler.MoveRight(_column);
                    }
                    GUI.enabled = guiEnabled;
                }
            }
        }

        IBoardGUIHandler _handler;
        Rect _rectShiftLeft, _rectShiftUp, _rectShiftRight, _rectShiftDown;
        RowGUIController[] _rowControllers;
        ColumnGUIController[] _columnControllers;
        int _rowCount, _columnCount;

        public BoardGUIController(IBoardGUIHandler handler, Rect boardRect, int rowCount, int columnCount, float cellSize, float cellSpace, float space, float buttonWidth)
        {
            _handler = handler;
            _rowCount = rowCount;
            _columnCount = columnCount;

            float shiftSpace = space;
            float shiftButtonWidth = buttonWidth;
            float rowControllerSpace = space;
            float rowControllerButtonWidth = buttonWidth;
            float columnControllerSpace = space;
            float columnControllerButtonHeight = buttonWidth;

            float stepX = cellSize + cellSpace;
            float stepY = stepX;
            float boardWidth = columnCount * stepX - cellSpace;
            float boardHeight = rowCount * stepY - cellSpace;

            _rectShiftLeft = new Rect(boardRect.x - shiftSpace - shiftButtonWidth, boardRect.y, shiftButtonWidth, boardHeight);
            _rectShiftUp = new Rect(boardRect.x, boardRect.y - shiftSpace - shiftButtonWidth, boardWidth, shiftButtonWidth);
            _rectShiftRight = new Rect(boardRect.x + boardWidth + shiftSpace, boardRect.y, shiftButtonWidth, boardHeight);
            _rectShiftDown = new Rect(boardRect.x, boardRect.y + boardHeight + shiftSpace, boardWidth, shiftButtonWidth);

            _rowControllers = new RowGUIController[rowCount];
            var rowRect = new Rect(boardRect.x + boardWidth + shiftSpace + shiftButtonWidth + rowControllerSpace, boardRect.y, rowControllerButtonWidth * 2, cellSize);
            for (int row = 0; row < rowCount; row++)
            {
                _rowControllers[row] = new RowGUIController(handler, row, rowRect);
                rowRect.y += stepY;
            }
            _rowControllers[0].UpEnabled = false;
            _rowControllers[rowCount - 1].DownEnabled = false;

            _columnControllers = new ColumnGUIController[columnCount];
            var columnRect = new Rect(boardRect.x, boardRect.y + boardHeight + shiftSpace + shiftButtonWidth + columnControllerSpace, cellSize, columnControllerButtonHeight * 2);
            for (int column = 0; column < columnCount; column++)
            {
                _columnControllers[column] = new ColumnGUIController(handler, column, columnRect);
                columnRect.x += stepX;
            }
            _columnControllers[0].LeftEnabled = false;
            _columnControllers[columnCount - 1].RightEnabled = false;
        }

        public void OnGUI()
        {
            if (GUI.Button(_rectShiftLeft, ShiftLeftContent))
            {
                _handler.ShiftLeft();
            }

            if (GUI.Button(_rectShiftUp, ShiftUpContent))
            {
                _handler.ShiftUp();
            }

            if (GUI.Button(_rectShiftRight, ShiftRightContent))
            {
                _handler.ShiftRight();
            }

            if (GUI.Button(_rectShiftDown, ShiftDownContent))
            {
                _handler.ShiftDown();
            }

            for (int row = 0; row < _rowCount; row++)
            {
                _rowControllers[row].OnGUI();
            }

            for (int column = 0; column < _columnCount; column++)
            {
                _columnControllers[column].OnGUI();
            }
        }
    }
}
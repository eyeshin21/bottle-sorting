using UnityEngine;
using System.Collections.Generic;
using Anvil.Legacy.Actions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public abstract class BaseBoard<TBoard, TCell> : MonoBehaviour, IBoard<TBoard, TCell>
#if UNITY_EDITOR
        , IInspector
#endif
        where TBoard : IBoard<TBoard, TCell>
        where TCell : ICell<TBoard, TCell>
    {
        protected static readonly float CellEpsilon = 0.001f;

        protected TCell[,] _cells;
        protected int _rowCount, _columnCount;
        protected float _unscaledCellSize = 1;
        protected float _cellSize = 1;
        protected float _scale = 1;

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;
        public float CellSize => _cellSize;
        public float UnscaledCellSize => _unscaledCellSize;
        public float Width => _columnCount * _cellSize;
        public float Height => _rowCount * _cellSize;
        public float Scale => _scale;

        protected abstract TBoard _board { get; }
        protected abstract TCell NewCell();

        public virtual void Construct(int rowCount, int columnCount, float unscaledCellSize)
        {
            _cells = new TCell[rowCount, columnCount];
            _rowCount = rowCount;
            _columnCount = columnCount;
            _unscaledCellSize = unscaledCellSize;
            _cellSize = unscaledCellSize;

            float left = (1 - columnCount) * unscaledCellSize * 0.5f;
            float top = (rowCount - 1) * unscaledCellSize * 0.5f;
            var localPos = new Vector3(left, top);
            var board = _board;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    var cell = NewCell();
                    cell.Construct(board, row, column);
                    _cells[row, column] = cell;
                    localPos.x += unscaledCellSize;
                }
                localPos.x = left;
                localPos.y -= unscaledCellSize;
            }
        }

        #region Position
        public float Left => transform.position.x - _columnCount * _cellSize * 0.5f;
        public float Right => transform.position.x + _columnCount * _cellSize * 0.5f;
        public float Top => transform.position.y + _rowCount * _cellSize * 0.5f;
        public float Bottom => transform.position.y - _rowCount * _cellSize * 0.5f;

        public float GetLeft(int column)
        {
            return Left + column * _cellSize;
        }

        public float GetRight(int column)
        {
            return Left + (column + 1) * _cellSize;
        }

        public float GetTop(int row)
        {
            return Top - row * _cellSize;
        }

        public float GetBottom(int row)
        {
            return Top - (row + 1) * _cellSize;
        }

        public void GetTopLeft(out float top, out float left)
        {
            var pos = transform.position;
            top = pos.y + _rowCount * _cellSize * 0.5f;
            left = pos.x - _columnCount * _cellSize * 0.5f;
        }

        public void GetBottomRight(out float bottom, out float right)
        {
            var pos = transform.position;
            bottom = pos.y - _rowCount * _cellSize * 0.5f;
            right = pos.x + _columnCount * _cellSize * 0.5f;
        }

        public void GetLeftRight(out float left, out float right)
        {
            float halfWidth = _columnCount * _cellSize * 0.5f;
            var pos = transform.position;
            left = pos.x - halfWidth;
            right = pos.x + halfWidth;
        }

        public void GetAABB(out float left, out float top, out float right, out float bottom)
        {
            float halfWidth = _columnCount * _cellSize * 0.5f;
            float halfHeight = _rowCount * _cellSize * 0.5f;
            var pos = transform.position;
            left = pos.x - halfWidth;
            top = pos.y + halfHeight;
            right = pos.x + halfWidth;
            bottom = pos.y - halfHeight;
        }

        public bool GetAABB(AcceptFunc<TCell> acceptFunc, out float left, out float top, out float right, out float bottom)
        {
            left = top = right = bottom = 0;

            int minRow = -1;
            for (int row = 0; row < _rowCount; row++)
            {
                for (int column = 0; column < _columnCount; column++)
                {
                    if (acceptFunc(_cells[row, column]))
                    {
                        minRow = row;
                        row = _rowCount;
                        break;
                    }
                }
            }
            if (minRow < 0) return false;

            int maxRow = minRow;
            for (int row = _rowCount - 1; row > minRow; row--)
            {
                for (int column = 0; column < _columnCount; column++)
                {
                    if (acceptFunc(_cells[row, column]))
                    {
                        maxRow = row;
                        row = 0;
                        break;
                    }
                }
            }

            int minColumn = 0;
            for (int column = 0; column < _columnCount; column++)
            {
                for (int row = minRow; row <= maxRow; row++)
                {
                    if (acceptFunc(_cells[row, column]))
                    {
                        minColumn = column;
                        column = _columnCount;
                        break;
                    }
                }
            }

            int maxColumn = _columnCount - 1;
            for (int column = _columnCount - 1; column > minColumn; column--)
            {
                for (int row = minRow; row <= maxRow; row++)
                {
                    if (acceptFunc(_cells[row, column]))
                    {
                        maxColumn = column;
                        column = 0;
                        break;
                    }
                }
            }

            left = GetCellLeft(minColumn);
            right = GetCellRight(maxColumn);
            top = GetCellTop(minRow);
            bottom = GetCellBottom(maxRow);
            //Log.Debug($"minRow={minRow}, minColumn={minColumn}, maxRow={maxRow}, maxColumn={maxColumn}");
            //Log.Debug($"({left}, {top}, {right}, {bottom}");

            return true;
        }

        public Vector3 GetLocalPosition(int row, int column)
        {
            float x = ((1 - _columnCount) * 0.5f + column) * _unscaledCellSize;
            float y = ((_rowCount - 1) * 0.5f - row) * _unscaledCellSize;
            return new Vector3(x, y);
        }
        #endregion

        #region Cell
        public TCell GetCell(int row, int column)
        {
            if (row >= 0 && row < _rowCount && column >= 0 && column < _columnCount)
            {
                return _cells[row, column];
            }
            return default;
        }

        public TCell GetCell(float x, float y)
        {
            if (GetCell(x, y, out int row, out int column))
            {
                return _cells[row, column];
            }
            return default;
        }

        public TCell GetCell(Vector3 pos)
        {
            if (GetCell(pos.x, pos.y, out int row, out int column))
            {
                return _cells[row, column];
            }
            return default;
        }

        public int GetRow(float y)
        {
            return Mathf.FloorToInt((Top - y - CellEpsilon) / _cellSize);
        }

        public int GetColumn(float x)
        {
            return Mathf.FloorToInt((x - Left - CellEpsilon) / _cellSize);
        }

        public bool GetCell(Vector3 pos, out int row, out int column)
        {
            return GetCell(pos.x, pos.y, out row, out column);
        }

        public bool GetCell(float x, float y, out int row, out int column)
        {
            var pos = transform.position;
            float halfWidth = _columnCount * _cellSize * 0.5f;
            float halfHeight = _rowCount * _cellSize * 0.5f;
            float left = pos.x - halfWidth;
            float top = pos.y + halfHeight;

            if (x >= left && x <= pos.x + halfWidth)
            {
                if (y <= top && y >= pos.y - halfHeight)
                {
                    row = Mathf.Min((int)((top - y + CellEpsilon) / _cellSize), _rowCount - 1);
                    column = Mathf.Min((int)((x - left + CellEpsilon) / _cellSize), _columnCount - 1);
                    return true;
                }
            }

            row = column = -1;
            return false;
        }

        public float GetCellLeft(int column)
        {
            return transform.position.x + ((1 - _columnCount) * 0.5f + column - 0.5f) * _cellSize;
        }

        public float GetCellRight(int column)
        {
            return transform.position.x + ((1 - _columnCount) * 0.5f + column + 0.5f) * _cellSize;
        }

        public float GetCellTop(int row)
        {
            return transform.position.y + ((_rowCount - 1) * 0.5f - row + 0.5f) * _cellSize;
        }

        public float GetCellBottom(int row)
        {
            return transform.position.y + ((_rowCount - 1) * 0.5f - row - 0.5f) * _cellSize;
        }

        public Vector3 GetCellPosition(int row, int column)
        {
            var pos = transform.position;
            pos.x += ((1 - _columnCount) * 0.5f + column) * _cellSize;
            pos.y += ((_rowCount - 1) * 0.5f - row) * _cellSize;
            return pos;
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
        #endregion

        #region Action
        protected virtual void DelayCall(float delay, Callback callback)
        {
            gameObject.DelayCall(delay, callback);
        }

        protected virtual ActionBehaviour PlayAction(ActionX action, UpdateType updateType = UpdateType.Update)
        {
            return gameObject.PlayAction(action, updateType);
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

        #region Debug
#if UNITY_EDITOR
        protected bool _drawGrid;
        protected bool _drawCoords;
        protected bool _drawDebug;

        public virtual void OnInspectorGUI()
        {
            if (!Application.isPlaying) return;

            Toggle("Draw Grid", ref _drawGrid);
            Toggle("Draw Coords", ref _drawCoords);
            Toggle("Draw Debug", ref _drawDebug);
        }

        protected void Toggle(string text, ref bool value)
        {
            GUIHelper.Toggle(text, ref value);
        }

        protected virtual void OnDrawGizmos()
        {
            if (_drawGrid)
            {
                GetTopLeft(out float top, out float left);
                GizmosHelper.DrawGrid(left, top, _rowCount, _columnCount, _cellSize, Color.white);
            }

            if (_drawCoords)
            {
                GetTopLeft(out float top, out float left);
                GizmosHelper.DrawCoords(left, top, _rowCount, _columnCount, _cellSize, Color.yellow);
            }

            if (_drawDebug)
            {
                OnDrawDebug();
            }
        }

        protected virtual void OnDrawDebug()
        {

        }
#endif
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BaseBoard<,>), true), CanEditMultipleObjects, DisallowMultipleComponent]
    public class BaseBoardEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            (target as IInspector).OnInspectorGUI();
        }
    }
#endif
}
using System;
using System.Collections.Generic;
using Anvil;
using Anvil.Legacy;
using Drawing;
using NaughtyAttributes;
using UnityEngine;

public interface IGridCell
{
    public int Row { get; }
    public int Column { get; }
}

public class GridCell : IGridCell
{
    private int _row;
    private int _column;
    private Tray _tray;
    public Vector3 localPosition;
    public int Row => _row;
    public int Column => _column;
    public Tray Tray => _tray;

    public GridCell(int row, int column)
    {
        _row = row;
        _column = column;
    }
    public bool IsEmpty => _tray == null;
    public void AddTray(Tray tray)
    {
        _tray = tray;
    }

    public void RemoveTray()
    {
        _tray = null;
    }
}

public class TrayGrid : MonoBehaviourGizmos
{
    [Serializable]
    public class TrayPositionData
    {
        public Tray prefabTray;
        public int row;
        public int column;
    }
    [SerializeField]private List<TrayPositionData> _testData;
    
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private GameObject _trayPrefab;
    [SerializeField] private Tray[] _trays;
    [SerializeField] private Tray[,] _trayMap;
    [SerializeField] private int _row;

    private float _cellSize;
    private float _unscaledWidth;
    private float  _unscaledHeight;
    private float _height;
    private float _width;
    private GridCell[,] _gridCells;

    public Tray[] Trays => _trays;

    private void Awake()
    {
        Construct(GameConfig.SlotWidth);
        _trays = new Tray[transform.childCount];
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out Tray tray))
            {
                _trays[child.GetSiblingIndex()] = tray;
            }
        }

        Generate(_testData);
    }

    private void Construct(float cellSize)
    {
        _cellSize = cellSize;
        _trayMap = new Tray[_rowCount, _columnCount];
        _gridCells = new GridCell[_rowCount, _columnCount];
        _unscaledHeight = _rowCount * _cellSize;
        _unscaledWidth = _columnCount * _cellSize;
        _height = _unscaledHeight * transform.localScale.z;
        _width = _unscaledWidth * transform.localScale.x;
        for (int row = 0; row < _rowCount; row++)
        {
            for (int column = 0; column < _columnCount; column++)
            {
                var cell = new GridCell(row, column);
                _gridCells[row, column]  = cell;
                cell.localPosition = CalculateLocalPosition(cell);
            }
        }
    }
    private void Generate(List<TrayPositionData> testData)
    {
        GameObjectPool.ClearManagedChild(gameObject);
        foreach (var positionData in testData)
        {
            GridCell cell = GetCell(positionData.row, positionData.column);
            Debug.Log($"cell {cell.Row}-{cell.Column}");
            if (cell == null || !cell.IsEmpty) continue;
            if (!CheckAvailableSpace(positionData.prefabTray, cell, out List<GridCell> cells)) continue;
            cells.Add(cell);
            Tray tray = GameObjectPool.CreateObject<Tray>(transform, positionData.prefabTray.gameObject);
            RegisterTray(positionData.prefabTray, cells);
            Vector3 centerPosition = Vector3.zero;
            foreach (GridCell gridCell in cells)
            {
                centerPosition += gridCell.localPosition;
            }
            centerPosition /= cells.Count;
            tray.SetPositionToCenter(TransformPoint(centerPosition));
        }
    }
    private Vector3 TransformPoint(Vector3 localPosition)
    {
        return transform.TransformPoint(localPosition);
    }
    private Vector3 CalculateLocalPosition(GridCell cell)
    {        
        float x = cell.Column * _cellSize - _unscaledWidth / 2 + _cellSize / 2;
        float z = _unscaledHeight / 2 - cell.Row * _cellSize - _cellSize / 2;
        return new Vector3(x, 0, z);
    }
    private bool IsInBound(int row, int column)
    {
        return  row >= 0 && row < _rowCount && column >= 0 && column < _columnCount;
    }

    private GridCell GetCell(int row, int column)
    {
        if (!IsInBound(row, column)) return null;
        return _gridCells[row, column];
    }
    private GridCell OffSetCellInverseCoord(GridCell cell, Vector2Int offset)
    {
        return GetCell(cell.Row - offset.y, cell.Column + offset.x);
    }
    private bool RegisterTray(Tray tray, int row, int column)
    {
        var cell = GetCell(row, column);
        if (cell == null) return false;
        if (!CheckAvailableSpace(tray, cell, out var cells)) return false;
        foreach (var cell2 in cells)
        {
            cell2.AddTray(tray);
        }
        return true;
    }

    private bool RegisterTray(Tray tray, List<GridCell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell.IsEmpty) return false;
            cell.AddTray(tray);
        }

        return true;
    }

    // private List<GridCell> _tempCells = new  List<GridCell>();
    private bool CheckAvailableSpace(Tray tray, GridCell cell, out List<GridCell> cells)
    {
        cells = new List<GridCell>();
        foreach (Vector2Int offset in tray.ShapeProfile)
        {
            var targetCell =OffSetCellInverseCoord(cell, offset);
            if (targetCell == null || !targetCell.IsEmpty)
            {
                Debug.LogError($"cannot register tray at {cell.Row}-{cell.Column}");
                return false;
            }
            cells.Add(targetCell);
        }

        return true;
    }

    public GridCell GetCellNear(Vector3 worldPos, out Directions directionsFromPoint)
    {
        directionsFromPoint = default;
        Vector3 localPos = TransformPoint(worldPos);
        int rowIndex = Mathf.FloorToInt(localPos.x / _cellSize);
        int colIndex =  Mathf.FloorToInt(localPos.z / _cellSize);
        var cell = GetCell(Mathf.Clamp(rowIndex, 0, _rowCount - 1), 
                        Mathf.Clamp(colIndex, 0, _columnCount - 1));
        if (cell == null) return null;
        if (!Mathf.Approximately(localPos.z, cell.localPosition.z) &&
            localPos.x.IsInRange(cell.localPosition.x - _cellSize / 2, cell.localPosition.x + _cellSize / 2))
        {
            if (localPos.z > cell.localPosition.z)
            {
                directionsFromPoint |= Directions.Up;
            }
            else
            {
                directionsFromPoint |= Directions.Down;
            }
        }
        else if (!Mathf.Approximately(localPos.x, cell.localPosition.x) &&
            localPos.z.IsInRange(cell.localPosition.z - _cellSize / 2, cell.localPosition.z + _cellSize / 2))
        {
            if (localPos.x > cell.localPosition.x)
            {
                directionsFromPoint |= Directions.Right;
            }
            else
            {
                directionsFromPoint |= Directions.Left;
            }
        }

        return cell;
    }
    public Tray GetTrayAlong(GridCell cell, Directions direction)
    {
        if (cell == null) return null;
        int maxIndex = (direction == Directions.Up || direction == Directions.Down) ? _rowCount - 1 : _columnCount - 1;
        for (int i = 0; i <= maxIndex; i++)
        {
            var targetCell = OffsetCell(cell, direction, i);
            if (targetCell == null) break;
            if (!targetCell.IsEmpty) return targetCell.Tray;
        }
        return null;
    }
    public GridCell OffsetCell(GridCell cell, Directions direction, int offset = 1)
    {
        if (cell == null) return null;
        switch (direction)
        {
            case Directions.Up:
                return GetCell(cell.Row - offset, cell.Column);
            case Directions.Down:
                return GetCell(cell.Row + offset, cell.Column);
            case Directions.Left:
                return GetCell(cell.Row, cell.Column - offset);
            case Directions.Right:
                return GetCell(cell.Row, cell.Column + offset);
        }

        return null;
    }
        
    [Button]
    private void TestGenerate()
    {
        Construct(GameConfig.SlotWidth);
        Generate(_testData);
    }

    public override void DrawGizmos()
    {
        float top = transform.position.z + _height / 2;
        float left = transform.position.x - _width / 2;
        float bottom = transform.position.z - _height / 2;
        float right = transform.position.x + _width / 2;
        for (int i = 0; i <= _columnCount; i++)
        {
            float x = left + i * _cellSize;
            Draw.Line(new Vector3(x, transform.position.y, top), new Vector3 (x, transform.position.y, bottom));
        }

        for (int i = 0; i <= _rowCount; i++)
        {
            float z = bottom + i * _cellSize;
            Draw.Line(new Vector3(left, transform.position.y, z), new Vector3 (right, transform.position.y, z));
        }

        if (_gridCells == null)
        {
            return;
        }
        foreach (GridCell cell in _gridCells)
        {
            // Draw.SphereOutline(TransformPoint(cell.localPosition), 0.015f);
            Draw.Label2D(TransformPoint(cell.localPosition), $"{cell.Row}-{cell.Column}");
        }
    }
    // public void 
}
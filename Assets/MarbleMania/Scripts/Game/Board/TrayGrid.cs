using System;
using System.Collections.Generic;
using Anvil.Legacy;
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

public class TrayGrid : MonoBehaviour
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
        foreach (var positionData in testData)
        {
            GridCell cell = GetCell(positionData.row, positionData.column);
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
        float x = cell.Column * _cellSize - _unscaledWidth / 2;
        float z = cell.Row * _cellSize + _unscaledHeight / 2;
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
    private GridCell OffSetCell(GridCell cell, Vector2Int offset)
    {
        return GetCell(cell.Row + offset.x, cell.Column + offset.y);
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
            var targetCell =OffSetCell(cell, offset);
            if (targetCell == null || !targetCell.IsEmpty)
            {
                Debug.LogError($"cannot register tray at {cell.Row}-{cell.Column}");
                return false;
            }
            cells.Add(targetCell);
        }

        return true;
    }

    // public void 
}
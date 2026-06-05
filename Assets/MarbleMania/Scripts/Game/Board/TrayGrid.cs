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
    public Vector3 localPosition;
    protected int _row;
    protected int _column;
    public int Row => _row;
    public int Column => _column;

    public GridCell()
    {
    }
    public GridCell(int row, int column)
    {
        _row = row;
        _column = column;
    }
    
}

public class TrayGridCell : GridCell
{
    public TrayGridCell(int row , int column) : base(row, column)
    {
    }
    private Tray _tray;
    
    public Tray Tray => _tray;
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

[Serializable]
public class TrayGridData
{
    public int row;
    public int col;
    public List<TrayPositionData> _gridData;
}
[Serializable]
public class TrayPositionData
{
    public TrayType type;
    public ColorType trayColor;
    public int row;
    public int column;
}
public class TrayGrid : MonoBehaviourGizmos
{

    [SerializeField]private List<TrayPositionData> _testData;
    
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private GameObject _trayPrefab;
    [SerializeField] private List<Tray> _trays;
    [SerializeField] private Tray[,] _trayMap;
    [SerializeField] private int _row;

    private float _cellSize;
    private float _unscaledWidth;
    private float  _unscaledHeight;
    private float _height;
    private float _width;
    private TrayGridCell[,] _gridCells;
    private MainBoard _board;

    public void Init(TrayGridData data, MainBoard board)
    {
        _board = board;
        _rowCount = data.row;
        _columnCount = data.col;
        Construct(GameConfig.SlotWidth);

        Generate(data);
    }

    private void Construct(float cellSize)
    {
        _cellSize = cellSize;
        _trayMap = new Tray[_rowCount, _columnCount];
        _gridCells = new TrayGridCell[_rowCount, _columnCount];
        _unscaledHeight = _rowCount * _cellSize;
        _unscaledWidth = _columnCount * _cellSize;
        _height = _unscaledHeight * transform.localScale.z;
        _width = _unscaledWidth * transform.localScale.x;
        for (int row = 0; row < _rowCount; row++)
        {
            for (int column = 0; column < _columnCount; column++)
            {
                var cell = new TrayGridCell(row, column);
                _gridCells[row, column]  = cell;
                cell.localPosition = CalculateLocalPosition(cell);
            }
        }
    }
    private void Generate(TrayGridData data)
    {
        GameObjectPool.ClearManagedChild(gameObject);
        _trays = new List<Tray>();
        foreach (var positionData in data._gridData)
        {
            TrayGridCell cell = GetCell(positionData.row, positionData.column);
            Debug.Log($"cell {cell.Row}-{cell.Column}");
            if (cell == null || !cell.IsEmpty) continue;
            var prefabTray = GameConfig.GetTrayPrefab(positionData.type);
            if (!CheckAvailableSpace(prefabTray, cell, out List<TrayGridCell> cells)) continue;
            cells.Add(cell);
            Tray tray = GameObjectPool.CreateObject<Tray>(transform, prefabTray.gameObject);
            tray.Init(positionData);
            RegisterTray(tray, cells);
            Vector3 centerPosition = Vector3.zero;
            foreach (TrayGridCell gridCell in cells)
            {
                centerPosition += gridCell.localPosition;
            }
            centerPosition /= cells.Count;
            tray.SetPositionToCenter(ConvertToWorldSpace(centerPosition));
        }
    }
    private Vector3 ConvertToWorldSpace(Vector3 localPosition)
    {
        return transform.TransformPoint(localPosition);
    }

    private Vector3 ConvertToLocalSpace(Vector3 worldPos)
    {
        return transform.InverseTransformPoint(worldPos);
    }
    private Vector3 CalculateLocalPosition(TrayGridCell cell)
    {        
        float x = cell.Column * _cellSize - _unscaledWidth / 2 + _cellSize / 2;
        float z = _unscaledHeight / 2 - cell.Row * _cellSize - _cellSize / 2;
        return new Vector3(x, 0, z);
    }
    private bool IsInBound(int row, int column)
    {
        return  row >= 0 && row < _rowCount && column >= 0 && column < _columnCount;
    }

    private TrayGridCell GetCell(int row, int column)
    {
        if (!IsInBound(row, column)) return null;
        return _gridCells[row, column];
    }
    private TrayGridCell OffSetCellInverseCoord(TrayGridCell cell, Vector2Int offset)
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

    private bool RegisterTray(Tray tray, List<TrayGridCell> cells)
    {
        foreach (var cell in cells)
        {
            if (!cell.IsEmpty) return false;
            cell.AddTray(tray);
        }
        _trays.Add(tray);
        return true;
    }
    public void Remove(Tray tray)
    {
        bool dirty = false;
        foreach (TrayGridCell cell in _gridCells)
        {
            if (cell.Tray == tray)
            {
                cell.RemoveTray();
                dirty = true;
            }
        }

        if (dirty)
        {
            _trays.Remove(tray);
            if (_trays.IsNullOrEmpty())
            {
                _board.OnTrayGridEmpty(this);
            }
        }
    }
    // private List<TrayGridCell> _tempCells = new  List<TrayGridCell>();
    private bool CheckAvailableSpace(Tray tray, TrayGridCell cell, out List<TrayGridCell> cells)
    {
        if (tray == null)
        {
            cells = null;
            return false;
        }
        cells = new List<TrayGridCell>();
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

    public TrayGridCell GetCellNear(Vector3 worldPos, out Directions directionsFromPoint)
    {
        directionsFromPoint = default;
        Vector3 localPos = ConvertToLocalSpace(worldPos);
        
        float x = localPos.x + _unscaledWidth * 0.5f;
        float z = _unscaledHeight * 0.5f - localPos.z;

        int colIndex = Mathf.FloorToInt(x / _cellSize);
        int rowIndex = Mathf.FloorToInt(z / _cellSize);
        
        // int rowIndex = Mathf.FloorToInt(localPos.x / _cellSize);
        // int colIndex =  Mathf.FloorToInt(localPos.z / _cellSize);
        var cell = GetCell(Mathf.Clamp(rowIndex, 0, _rowCount - 1), 
                        Mathf.Clamp(colIndex, 0, _columnCount - 1));
        if (cell == null) return null;
        if (!Mathf.Approximately(localPos.z, cell.localPosition.z) &&
            localPos.x.IsInRange(cell.localPosition.x - _cellSize / 2, cell.localPosition.x + _cellSize / 2))
        {
            if (localPos.z > cell.localPosition.z)
            {
                directionsFromPoint |= Directions.Down;
            }
            else
            {
                directionsFromPoint |= Directions.Up;
            }
        }
        else if (!Mathf.Approximately(localPos.x, cell.localPosition.x) &&
            localPos.z.IsInRange(cell.localPosition.z - _cellSize / 2, cell.localPosition.z + _cellSize / 2))
        {
            if (localPos.x > cell.localPosition.x)
            {
                directionsFromPoint |= Directions.Left;
            }
            else
            {
                directionsFromPoint |= Directions.Right;
            }
        }

        return cell;
    }
    public Tray GetTrayAlong(TrayGridCell cell, Directions direction)
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
    public TrayGridCell OffsetCell(TrayGridCell cell, Directions direction, int offset = 1)
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
        // Generate(_testData);
    }

    [SerializeField] public bool drawDebug = false;
    [SerializeField] public bool drawCoord = false;
    [SerializeField] private float textSize = 0.3f;
    public override void DrawGizmos()
    {
        if (!drawDebug)
        {
            return;
        }
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

        if (drawCoord)
        {
            foreach (TrayGridCell cell in _gridCells)
            {
                // Draw.SphereOutline(ConvertToWorldSpace(cell.localPosition), 0.015f);
                Draw.Label3D(ConvertToWorldSpace(cell.localPosition),Quaternion.LookRotation(Vector3.down, Vector3.forward), $"{cell.Row}-{cell.Column}", textSize, LabelAlignment.Center);
            }
        }
    }
    // public void 

}
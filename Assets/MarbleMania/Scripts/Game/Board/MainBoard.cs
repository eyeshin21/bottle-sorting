using System;
using System.Collections.Generic;
using Anvil;
using Drawing;
using MarbleMania;
using MarbleMania.Scripts.Game;
using NaughtyAttributes;
using UnityEngine;

public class MainBoard : MonoBehaviourGizmos
{
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private TrayGrid _trayGridPrefab;
    [SerializeField] private Conveyor _conveyor;
    [SerializeField] private float _gridOffsetY;
    [SerializeField] private Vector2 _maxSize = new Vector2(100, 100);
    private List<TrayGrid> _grids = new List<TrayGrid>();
    private TrayGrid _currentGrid;
    [SerializeField, ReadOnly] private Rect _sizeRect;

    public Rect Size => _sizeRect;
    public List<TrayGrid> Grids  => _grids;
    private void Awake()
    {
        MainGameEventID.TrayFillComplete.AddEventListener<Tray>(OnTrayFilled);
    }

    private void OnDestroy()
    {
        MainGameEventID.TrayFillComplete.RemoveEventListener<Tray>(OnTrayFilled);
    }

    private void Update()
    {
        OnUpdate();
    }

    private void OnTrayFilled(Tray tray)
    {
        _currentGrid.Remove(tray);
        tray.Complete();
        if (_currentGrid.Trays.Count == 0)
        {
            _grids.Remove(_currentGrid);
            Destroy(_currentGrid.gameObject);
            if (_grids.Count > 0)
            {
                SetActiveGrid(0);
            }
            else
            {
                GameController.Instance.OnLevelComplete();
            }
        }
    }

    private void Construct()
    {
    }

    public void Init(List<TrayGridData> trayGridDatas)
    {
        _grids.Clear();
        _currentGrid = null;
        GameObjectPool.ClearManagedChild(gameObject);
        if (trayGridDatas.Count == 0) return;
        float y = 0;
        _sizeRect = new Rect(0, 0, 0, 0);
        for (var index = trayGridDatas.Count - 1; index >= 0; index--)
        {
            var trayGridData = trayGridDatas[index];
            var grid = GameObjectPool.CreateObject<TrayGrid>(transform, _trayGridPrefab.gameObject);
            grid.Init(trayGridData, this);
            grid.transform.localPosition = new Vector3(0, y, 0);
            y += _gridOffsetY;
            grid.drawDebug = false;
            _grids.Insert(0,grid);

            Rect size = grid.Size;
            if (size.width > _sizeRect.width) _sizeRect.width = size.width;
            if (size.height > _sizeRect.height) _sizeRect.height = size.height;
        }

        float xScale = _maxSize.x / _sizeRect.width;
        float yScale = _maxSize.y / _sizeRect.height;
        float scale = Mathf.Min(xScale, yScale);
        Debug.Log($"sclase {scale}, x {xScale}, y {yScale}");

        scale = Mathf.Min(scale, 1f);
        transform.localScale = Vector3.one * scale;

        _sizeRect.center = new Vector2(0, 0);
        SetActiveGrid(0);
    }

    public void Generate()
    {
    }


    private void OnUpdate()
    {
        if (_currentGrid == null) return;
        for (var i = 0; i < _conveyor.Slots.Length; i++)
        {
            var slot = _conveyor.Slots[i];
            if (slot.IsEmpty) continue;
            Bottle bottle = slot.bottle;
            bool taken = TryIntakeBottle(bottle);
            if (taken)
            {
                _conveyor.RemoveBottle(slot);
            }
        }
    }

    public bool TryIntakeBottle(Bottle bottle)
    {
        if (_currentGrid == null) return false;
        TrayGrid grid = _currentGrid;
        var cell = grid.GetCellNear(bottle.transform.position, out Directions direction);
        contactCells.Add(cell);
        if (direction.HasMultipleFlag()) return false;
        var tray = grid.GetTrayAlong(cell, direction);
        if (tray == null || !tray.IsCompatible(bottle))
        {
            return false;
        }

        return tray.IntakeBottle(bottle);
    }


    private List<GridCell> contactCells = new List<GridCell>();

    public override void DrawGizmos()
    {
        if (_currentGrid == null) return;
        foreach (GridCell cell in contactCells)
        {
            Draw.SolidBox(_currentGrid.transform.TransformPoint(cell.localPosition), Vector3.one * 0.1f, Color.blue);
        }

        contactCells.Clear();

        Vector3 topLeftMax = new Vector3(-_maxSize.x / 2, 0, _maxSize.y / 2);
        Vector3 bottomRightMax = new Vector3(_maxSize.x / 2, 0, -_maxSize.y / 2);
        // Draw.SolidBox(topLeftMax, Vector3.one * 0.2f, Color.red);
        // Draw.SolidBox(bottomRightMax, Vector3.one * 0.2f, Color.red);
    }

    public void OnTrayGridEmpty(TrayGrid trayGrid)
    {
        if (_currentGrid != trayGrid) return;
        int index = _grids.IndexOf(trayGrid);
        if (index <= 0) return;
        _currentGrid = _grids[index - 1];
        _currentGrid.drawDebug = true;
        GameObjectPool.RemoveObject(trayGrid.gameObject);
    }

    public void SetActiveGrid(int index)
    {
        _currentGrid = _grids[index];
        _currentGrid.drawDebug = true;
        _currentGrid.gameObject.SetActive(true);
        foreach (TrayGrid trayGrid in _grids)
        {
            if (trayGrid == _currentGrid) continue;
            trayGrid.gameObject.SetActive(false);
            trayGrid.drawDebug = false;
        }
    }

    public TrayGrid GenerateLayer(int layerIndex, int row, int col)
    {
        TrayGrid grid = _grids.TryGet(layerIndex);
        if (grid == null)
        {
            grid = GameObjectPool.CreateObject<TrayGrid>(transform, _trayGridPrefab.gameObject);
            _grids.Add(grid);
        }
        grid.Init(row, col, this);
        UpdateScale();
        return grid;
    }

    public void UpdateScale()
    {
        foreach (var grid in _grids)
        {
            Rect size = grid.Size;
            if (size.width > _sizeRect.width) _sizeRect.width = size.width;
            if (size.height > _sizeRect.height) _sizeRect.height = size.height;
        }

        float xScale = _maxSize.x / _sizeRect.width;
        float yScale = _maxSize.y / _sizeRect.height;
        float scale = Mathf.Min(xScale, yScale);
        Debug.Log($"sclase {scale}, x {xScale}, y {yScale}");

        scale = Mathf.Min(scale, 1f);
        transform.localScale = Vector3.one * scale;
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _grids.Clear();
    }
}
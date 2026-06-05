using System;
using System.Collections.Generic;
using Anvil.Legacy;
using Drawing;
using MarbleMania.Scripts.Game;
using UnityEngine;

public class MainBoard : MonoBehaviourGizmos
{
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private TrayGrid _trayGridPrefab;
    [SerializeField] private Conveyor _conveyor;
    [SerializeField] private float _gridOffsetY;
    private List<TrayGrid> _grids  = new List<TrayGrid>();
    private TrayGrid _currentGrid;
    private void Awake()
    {
        MainGameEventType.TrayFillComplete.AddListener<Tray>(OnTrayFilled);
    }

    private void OnTrayFilled(Tray tray)
    {
        _currentGrid.Remove(tray);
        tray.Complete();
    }

    private void Construct()
    {
    }

    public void Init(List<TrayGridData> trayGridDatas)
    {
        _grids.Clear();
        GameObjectPool.ClearManagedChild(gameObject);
        if (trayGridDatas.Count ==0) return;
        float y = 0;
        for (var index = 0; index < trayGridDatas.Count; index++)
        {
            var trayGridData = trayGridDatas[index];
            var grid = GameObjectPool.CreateObject<TrayGrid>(transform, _trayGridPrefab.gameObject);
            grid.Init(trayGridData, this);
            grid.transform.localPosition = new Vector3(0, y, 0);
             y += _gridOffsetY;
             grid.drawDebug = false;
            _grids.Add(grid);
        }
        _currentGrid = _grids[_grids.Count - 1];
        _currentGrid.drawDebug = true;
    }
    private void Update()
    {
        OnUpdate();
    }

    private void OnUpdate()
    {
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
        foreach (GridCell cell in contactCells)
        {
            Draw.SolidBox(_currentGrid.transform.TransformPoint(cell.localPosition), Vector3.one * 0.1f, Color.blue);
        }
        contactCells.Clear();
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
}
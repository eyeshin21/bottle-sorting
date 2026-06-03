using System;
using System.Collections.Generic;
using Drawing;
using MarbleMania.Scripts.Game;
using UnityEngine;

public class MainBoard : MonoBehaviourGizmos
{
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private TrayGrid _trayGrid;
    [SerializeField] private Conveyor _conveyor;

    private void Awake()
    {
        MainGameEventType.TrayFillComplete.AddListener<Tray>(OnTrayFilled);
    }

    private void OnTrayFilled(Tray tray)
    {
        _trayGrid.Remove(tray);
        tray.Complete();
    }

    private void Construct()
    {
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
        TrayGrid grid = _trayGrid;
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
            Draw.SolidBox(_trayGrid.transform.TransformPoint(cell.localPosition), Vector3.one * 0.1f, Color.blue);
        }
        contactCells.Clear();
    }
}
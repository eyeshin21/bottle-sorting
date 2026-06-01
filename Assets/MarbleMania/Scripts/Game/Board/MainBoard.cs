using System;
using UnityEngine;

public class MainBoard : MonoBehaviour
{
    [SerializeField] private int _rowCount;
    [SerializeField] private int _columnCount;
    [SerializeField] private TrayGrid _trayGrid;
    [SerializeField] private Conveyor _conveyor;
    private void Construct()
    {
    }

    private void Update()
    {
        OnUpdate();
    }

    private void OnUpdate()
    {
        foreach (Tray tray in _trayGrid.Trays)
        {
            if (tray == null) continue;
            for (var i = 0; i < _conveyor.Slots.Length; i++)
            {
                var slot = _conveyor.Slots[i];
                if (slot.IsEmpty) continue;
                bool taken = tray.TryIntake(slot.bottle);
                if (taken)
                {
                    slot.RemoveBottle();
                }
            }
        }
    }

    public bool TryIntakeBottle(Bottle bottle)
    {
        TrayGrid grid = _trayGrid;
        GridCell cell = grid.GetCellNear(bottle.transform.position, out Directions direction);
        {
            if (direction.HasMultipleFlag()) return false;
            
        }
    }
}
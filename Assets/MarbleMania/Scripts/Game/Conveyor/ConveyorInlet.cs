using System;
using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;

public class ConveyorInlet : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Conveyor _conveyor;
    [SerializeField] private List<Bottle> _intakeQueue = new  List<Bottle>();
    private float _entryDistance;
    public float  EntryDistance => _entryDistance;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _entryDistance = _conveyor.FindClosetDistanceFromWorldSpace(transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bottle bottle) && !_conveyor.HasBottle(bottle))
        {
            _intakeQueue.CheckAdd(bottle);
        }
    }

    private void Update()
    {
        if (_intakeQueue.Count == 0) return;
        ConveyorSlot slot = _conveyor.FindClosestSlotAt(_entryDistance);
        
        if (!slot.IsEmpty) return;
        var bottle = _intakeQueue[0];
        if (_conveyor.TryAddBottle(bottle, slot))
        {
            bottle.ActivePhysic(false);
            bottle.MoveTo(slot);
            _intakeQueue.RemoveAt(0);
        }
    }
}
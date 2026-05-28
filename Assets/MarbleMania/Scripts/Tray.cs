using Anvil;
using Anvil.Legacy;
using NaughtyAttributes;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [SerializeField] private ColorType _colorType;
    [SerializeField] private TraySlot[] _slots;
    [SerializeField] private Rect _boundingBox;
    
    [SerializeField, ReadOnly] private Vector3 _localCenter;
    private Vector3 _center => transform.TransformPoint(_localCenter);
    
    private void CalculateBoundingBox()
    {
        float top = 0f;
        float right = 0f;
        float bottom = 0f;
        float left = 0f;
        foreach (var slot in _slots)
        {
            Vector3 localPos = slot.transform.localPosition;
            if (localPos.x < left) left = localPos.x;
            if (localPos.x > right) right = localPos.x;
            if (localPos.y < bottom) bottom = localPos.y;
            if (localPos.y > top) top = localPos.y;
        }
        _boundingBox = new Rect(left, bottom, (right - left) + GameConfig.SlotWidth, (top - bottom) + GameConfig.SlotHeight);
        _localCenter = new Vector3(_boundingBox.center.x, _boundingBox.center.y, 0f);
    }

    public Directions CalculateDirectionOfPoint(Vector3 worldPoint)
    {         
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);
        if (!_boundingBox.Contains(localPoint)) return default;

        Directions result = default;
        if (localPoint.y > _boundingBox.yMax && localPoint.x.IsInRange(_boundingBox.xMin, _boundingBox.xMax)) // up
            result |= Directions.Up;
        if (localPoint.y < _boundingBox.yMin && localPoint.x.IsInRange(_boundingBox.xMin, _boundingBox.xMax)) // down
            result |= Directions.Down;
        if (localPoint.x < _boundingBox.xMin && localPoint.y.IsInRange(_boundingBox.yMin, _boundingBox.yMax)) // left
            result |= Directions.Left;
        if (localPoint.x > _boundingBox.xMax && localPoint.y.IsInRange(_boundingBox.yMin, _boundingBox.yMax)) // right
            result |= Directions.Right;
        return result;
    }
    private void BrowseEmptySlot(System.Func<TraySlot, bool> func)
    {
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty) continue;
            if (!func.Invoke(slot)) return;
        }
    }

    public bool GetEmptySlotAtDirection(Directions direction, out TraySlot slot)
    {
        slot = default;
        BrowseEmptySlot(slot =>
        {
            if (slot.HasDirection(direction))
            {
                slot = slot;
                return false;
            }

            return true;
        });
        if (slot.transform != null)
        {
            return true;
        }
        return false;
    }

    [Button]
    private void Generate()
    {
        
    }

    public bool TryIntake(Bottle bottle)
    {
        Directions directions = CalculateDirectionOfPoint(bottle.transform.position);
        if (directions == default) return false;
        if(!GetEmptySlotAtDirection(directions, out var slot)) 
            return false;
        if (slot.IsNull) return false;
        slot.bottle = bottle;
        bottle.MoveTo(GetSlotPosition(slot));

        return true;
    }
    public Vector3 GetSlotPosition(TraySlot slot)
    {
        return transform.TransformPoint(slot.localPosition);
    }
}
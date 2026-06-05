using System;
using System.Collections.Generic;
using Anvil;
using Anvil.Legacy;
using Drawing;
using MarbleMania.Scripts.Game;
using NaughtyAttributes;
using UnityEngine;
using IAnimationController = Anvil.IAnimationController;

public class Tray : MonoBehaviourGizmos
{
    [SerializeField] private ColorType _colorType;
    [SerializeField] private TraySlot[] _slots;
    [SerializeField] private Rect _boundingBox;
    [SerializeField] private List<Vector2Int> _gridShape;
    [SerializeField, ReadOnly] private Vector3 _localCenter;
    [SerializeField] private Vector3 _centerOffset;
    [SerializeField] private Transform _bottleParent;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField, ElementName(typeof(ColorType))] private List<Material> _colorMaterials;
    private IAnimationController _animationController;
    private HashSet<Bottle> _bottles = new HashSet<Bottle>();
    private Vector3 _center => transform.TransformPoint(_localCenter);

    public List<Vector2Int> ShapeProfile => _gridShape;

    private void Awake()
    {
        // _animationController = gameObject.CreateAnimationController();
    }

    private void Start()
    {
        CacheOffset();
    }
    private void CacheOffset()
    {
        _centerOffset = transform.InverseTransformDirection(_localCenter).normalized;
    }
    public void Init(TrayPositionData data)
    {
        InitColor(data.trayColor);
    }

    private void InitColor(ColorType color)
    {
        _colorType = color;
        _renderer.material = _colorMaterials.TryGet((int)color);
    }

    [Button]
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
            if (localPos.z < bottom) bottom = localPos.z;
            if (localPos.z > top) top = localPos.z;
        }

        Debug.Log($"left {left} , right {right}, top {top}, bottom {bottom}");
        _boundingBox = new Rect(left, bottom, (right - left) + GameConfig.SlotWidth, (top - bottom) + GameConfig.SlotHeight);
        _localCenter = new Vector3(_boundingBox.center.x, _boundingBox.center.y, 0f);
        CacheOffset();
    }

    public Directions CalculateDirectionOfPoint(Vector3 worldPoint)
    {         
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

        Directions result = default;
        if (localPoint.z > _boundingBox.yMax && localPoint.x.IsInRange(_boundingBox.xMin, _boundingBox.xMax)) // up
            result |= Directions.Up;
        if (localPoint.z < _boundingBox.yMin && localPoint.x.IsInRange(_boundingBox.xMin, _boundingBox.xMax)) // down
            result |= Directions.Down;
        if (localPoint.x < _boundingBox.xMin && localPoint.z.IsInRange(_boundingBox.yMin, _boundingBox.yMax)) // left
            result |= Directions.Left;
        if (localPoint.x > _boundingBox.xMax && localPoint.z.IsInRange(_boundingBox.yMin, _boundingBox.yMax)) // right
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
        slot = null;
        TraySlot result = null;
        BrowseEmptySlot(emptySlot =>
        {
            if (emptySlot.HasDirection(direction))
            {
                result = emptySlot;
                return false;
            }

            return true;
        });
        if (result == null) return false;
        if (result.transform != null)
        {
            slot = result;
            return true;
        }
        return false;
    }

    public TraySlot GetEmptySlot()
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty) return slot;
        }

        return null;
    }

    public bool TryIntake(Bottle bottle)
    {
        Directions directions = CalculateDirectionOfPoint(bottle.transform.position);
        if (directions == default) return false;
        var slot = GetEmptySlot();
        if(slot == null) 
            return false;
        slot.RegisterBottle(bottle);
        bottle.MoveTo(GetSlotPosition(slot));
        bottle.transform.SetParent(_bottleParent != null ? _bottleParent : transform);
        return true;
    }

    private int GetEmptySlotCount()
    {
        return _slots.Length - _bottles.Count;
    }

    public bool IsCompatible(Bottle bottle)
    {
        return bottle.ColorType == _colorType;
    }
    public bool IntakeBottle(Bottle bottle)
    {
        var slot = GetEmptySlot();
        if(slot == null) 
            return false;
        slot.RegisterBottle(bottle);
        _bottles.Add(bottle);
        bottle.transform.SetParent(_bottleParent != null ? _bottleParent : transform, true);
        bottle.MoveTo(GetSlotPosition(slot));
        if (GetEmptySlotCount() <= 0)
        {
            MainGameEventType.TrayFillComplete.Broadcast(this);
        }
        return true;
    }
    
    public Vector3 GetSlotPosition(BottleSlot slot)
    {
        return transform.TransformPoint(slot.localPosition);
    }

    public void OccupyPosition(Vector3 position)
    {
        
    }
    

    [SerializeField] private Transform _slotContainer;
    [Button]
    private void Generate()
    {
        _slots = new TraySlot[_slotContainer.childCount];
        foreach (Transform child in _slotContainer)
        {
            _slots[child.GetSiblingIndex()] = new TraySlot()
            {
                transform = child,
                localPosition = child.localPosition,
                directions = default
            };
        }
        CalculateBoundingBox();
    }

    [Button]
    private void GenerateShapeProfile()
    {
        // start from first slot
        TraySlot origin = _slots[0];
        _gridShape.Clear();
        float slotSize = GameConfig.SlotWidth;
        for (var i = 1; i < _slots.Length; i++)
        {
            TraySlot slot = _slots[i];
            Vector3 offset = slot.transform.localPosition - origin.transform.localPosition;
            Vector2Int gridOffset = new Vector2Int(Mathf.RoundToInt(offset.x / slotSize), Mathf.RoundToInt(offset.z / slotSize));
            _gridShape.Add(gridOffset);
        }
    }

    [SerializeField] private float _debugDrawSize = 0.15f;

    public override void DrawGizmos()
    {
        // Gizmos.color = Color.white;
        // foreach (var slot in _slots)
        // {
        //     if (slot.IsEmpty) 
        //         Draw.WireBox(GetSlotPosition(slot), Vector3.one * _debugDrawSize);
        //     else
        //         Draw.SolidBox(GetSlotPosition(slot), Vector3.one * _debugDrawSize);
        // }
    }

    public void SetPositionToCenter(Vector3 location)
    {
        transform.position = location - _centerOffset;
    }

    public void Complete()
    {
        GameObjectPool.RemoveObject(gameObject);
    }

    private void PlayAnimation(string name, Action callback)
    {
        
    }
}
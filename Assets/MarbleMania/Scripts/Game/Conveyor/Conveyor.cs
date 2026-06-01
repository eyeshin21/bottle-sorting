using System;
using System.Collections.Generic;
using Anvil.Legacy;
using Drawing;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class ConveyorSlot : BottleSlot, ITargetDesignator
{
    public float DistanceAlongConveyor;
    public Transform masterTransform;

    public GameObject GetTargetObject()
    {
        return null;
    }

    public Vector3 CalculateTargetPosition()
    {
        return masterTransform.TransformPoint(LocalPosition);
    }

    public Vector3 GetTargetPosition()
    {
        return CalculateTargetPosition();
    }
}

public class Conveyor : MonoBehaviourGizmos
{
#region Segment

    private interface IConveyorSegment
    {
        float Length { get; }

        Vector3 Evaluate(float t);
    }

    private class LineSegment : IConveyorSegment
    {
        private readonly Vector3 _a;
        private readonly Vector3 _b;

        public float Length { get; }

        public LineSegment(Vector3 a, Vector3 b)
        {
            _a = a;
            _b = b;

            Length = Vector3.Distance(a, b);
        }

        public Vector3 Evaluate(float t)
        {
            return Vector3.Lerp(_a, _b, t);
        }
    }

    private class QuadraticBezierSegment : IConveyorSegment
    {
        private readonly Vector3 _a;
        private readonly Vector3 _b;
        private readonly Vector3 _c;

        public float Length { get; }

        public QuadraticBezierSegment(
            Vector3 a,
            Vector3 b,
            Vector3 c)
        {
            _a = a;
            _b = b;
            _c = c;

            Length = ApproximateLength();
        }

        public Vector3 Evaluate(float t)
        {
            float u = 1f - t;

            return
                u * u * _a +
                2f * u * t * _b +
                t * t * _c;
        }

        private float ApproximateLength()
        {
            const int samples = 10;

            float length = 0f;

            Vector3 prev = Evaluate(0f);

            for (int i = 1; i <= samples; i++)
            {
                float t = i / (float)samples;

                Vector3 current = Evaluate(t);

                length += Vector3.Distance(prev, current);

                prev = current;
            }

            return length;
        }
    }

#endregion

    [SerializeField] private List<Vector3> _waypoints = new();
    [SerializeField] private float _cornerRadius = 1f;
    [SerializeField] private int _slotCount = 30;
    [SerializeField] private ConveyorSlot[] _slots;

    [SerializeField] [InspectorName("Speed (Unit per sec)")]
    private float _speed;

    private readonly List<IConveyorSegment> _segments = new();

    [SerializeField, ReadOnly] private float _totalLength;
    [SerializeField, ReadOnly] private float _distanceSpeed;
    [SerializeField, ReadOnly] private float _slotOffset;
    [SerializeField] private MainBoard _board;
    HashSet<Bottle> _bottlesOnConveyor = new();
    
    
    public float TotalLength => _totalLength;
    public ConveyorSlot[] Slots => _slots;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateConveyorMovement();
        UpdateContentMovement();
        
        UpdateBottleIntake();
    }

    private List<Bottle> _bottlesToRemove = new List<Bottle>();
    private void UpdateBottleIntake()
    {
        foreach (Bottle bottle in _bottlesOnConveyor)
        {
            if (_board.TryIntakeBottle(bottle))
            {
                _bottlesToRemove.Add(bottle);
            }
        }

        foreach (var bottle in _bottlesToRemove)
        {
            _bottlesOnConveyor.Remove(bottle);
        }
        _bottlesToRemove.Clear();
    }

    private void UpdateContentMovement()
    {
        foreach (ConveyorSlot slot in _slots)
        {
            if (slot.IsEmpty) continue;
            slot.bottle.transform.position = transform.TransformPoint(slot.LocalPosition);
        }
    }

    private void UpdateConveyorMovement()
    {
        float increment = _distanceSpeed * Time.deltaTime;
        _slotOffset = Mathf.Repeat(_slotOffset + increment, TotalLength);
        foreach (ConveyorSlot slot in _slots)
        {
            slot.DistanceAlongConveyor += increment;
            slot.LocalPosition = EvaluateDistance(slot.DistanceAlongConveyor);
        }
    }

    [Button]
    public void Init()
    {
        BuildSegments();
        _distanceSpeed = _speed;
        _slotOffset = 0f;
        InitSlots();
    }

    [Button]
    private void InitSlots()
    {
        float distance = 0f;
        _slots = new ConveyorSlot[_slotCount];
        for (int i = 0; i < _slotCount; i++)
        {
            ConveyorSlot slot = new ConveyorSlot();
            slot.masterTransform = transform;
            slot.LocalPosition = EvaluateDistance(distance);
            slot.DistanceAlongConveyor = distance;
            _slots[i] = slot;
            distance += TotalLength / _slotCount;
        }
    }

    private void BuildSegments()
    {
        _segments.Clear();
        _totalLength = 0f;
        if (_waypoints == null || _waypoints.Count < 3)
            return;

        int count = _waypoints.Count;

        for (int i = 0; i < count; i++)
        {
            Vector3 prev = _waypoints[(i - 1 + count) % count];
            Vector3 current = _waypoints[i];
            Vector3 next = _waypoints[(i + 1) % count];

            Vector3 dirToPrev = (prev - current).normalized;
            Vector3 dirToNext = (next - current).normalized;

            float prevDist = Vector3.Distance(prev, current);
            float nextDist = Vector3.Distance(current, next);

            float radius = Mathf.Min(
                _cornerRadius,
                prevDist * 0.5f,
                nextDist * 0.5f
            );

            Vector3 entryPoint = current + dirToPrev * radius;
            Vector3 exitPoint = current + dirToNext * radius;

            // LINE FROM PREVIOUS CORNER EXIT TO THIS ENTRY
            Vector3 prevPrev = _waypoints[(i - 2 + count) % count];
            Vector3 prevDir = (prevPrev - prev).normalized;

            float prevPrevDist = Vector3.Distance(prevPrev, prev);

            float prevRadius = Mathf.Min(
                _cornerRadius,
                prevPrevDist * 0.5f,
                prevDist * 0.5f
            );

            Vector3 prevExit = prev + (current - prev).normalized * prevRadius;

            AddSegment(new LineSegment(prevExit, entryPoint));

            // CORNER CURVE
            AddSegment(new QuadraticBezierSegment(
                entryPoint,
                current,
                exitPoint
            ));
        }
    }

    private void AddSegment(IConveyorSegment segment)
    {
        _segments.Add(segment);
        _totalLength += segment.Length;
    }

    /// <summary>
    ///  return local space
    /// </summary>
    public Vector3 EvaluateDistance(float distance)
    {
        if (_segments.Count == 0)
            return transform.position;

        distance = Mathf.Repeat(distance, _totalLength);

        float accumulated = 0f;

        foreach (var segment in _segments)
        {
            if (distance <= accumulated + segment.Length)
            {
                float localDistance = distance - accumulated;

                float t = localDistance / segment.Length;

                return segment.Evaluate(t);
            }

            accumulated += segment.Length;
        }

        return _segments[0].Evaluate(0f);
    }

    public float Advance(float currentDistance, float speed)
    {
        currentDistance += speed * Time.deltaTime;
        return Mathf.Repeat(currentDistance, _totalLength);
    }

    [SerializeField] private int _distanceSearchSamples = 200;

    public float FindClosetDistanceFromWorldSpace(Vector3 worldPosition)
    {
        return FindClosestDistance(transform.InverseTransformPoint(worldPosition), _distanceSearchSamples);
    }

    public float FindClosestDistance(Vector3 position, int samples = 200)
    {
        float bestDistance = 0f;
        float bestSqr = float.MaxValue;

        for (int i = 0; i <= samples; i++)
        {
            float d = (i / (float)samples) * _totalLength;

            Vector3 point = EvaluateDistance(d);

            float sqr = (position - point).sqrMagnitude;

            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                bestDistance = d;
            }
        }

        return bestDistance;
    }

    [SerializeField] private float _slotSearchRange;

    public ConveyorSlot FindClosestSlotAt(float distance)
    {
        distance = Mathf.Repeat(distance - _slotOffset, _totalLength);
        float approximateIndex = distance / TotalLength * _slotCount;
        int centerIndex = Mathf.RoundToInt(approximateIndex) % _slotCount;
        if (centerIndex >= _slotCount) return _slots[0];
        return _slots[centerIndex];
    }

    public bool HasBottle(Bottle bottle)
    {
        return _bottlesOnConveyor.Contains(bottle);
    }
    public bool TryAddBottle(Bottle bottle, ConveyorSlot slot)
    {
        if (!slot.IsEmpty) return false;
        if (!_bottlesOnConveyor.Add(bottle)) return false;
        slot.RegisterBottle(bottle);
        return true;
    }

    //=================================================  DEBUG  ==================================================================

    [Header("Debug")] [SerializeField] private bool _drawDebug = true;
    [SerializeField] private int _curveResolution = 12;
    [SerializeField] private float _slotDrawSize = 0.5f;
    [SerializeField] private ConveyorInlet _inlet;

    private void OnValidate()
    {
        BuildSegments();
    }

    public override void DrawGizmos()
    {
        if (!_drawDebug)
            return;

        if (_waypoints == null || _waypoints.Count < 3)
            return;


        // DRAW WAYPOINTS
        Gizmos.color = Color.yellow;
        foreach (var point in _waypoints)
        {
            Draw.SphereOutline(point, 0.15f);
        }

        // DRAW SEGMENTS
        Gizmos.color = Color.cyan;

        foreach (var segment in _segments)
        {
            Vector3 prev = segment.Evaluate(0f);

            for (int i = 1; i <= _curveResolution; i++)
            {
                float t = i / (float)_curveResolution;

                Vector3 current = segment.Evaluate(t);

                Draw.Line(prev, current);

                prev = current;
            }
        }

        ConveyorSlot intakeSlot = null;
        if (_inlet != null)
        {
            intakeSlot = FindClosestSlotAt(_inlet.EntryDistance);
        }

        // Draw slot
        for (var i = 0; i < _slots.Length; i++)
        {
            Color color = i == 0 ? Color.yellow : Color.red;

            var slot = _slots[i];
            if (slot == null) continue;
            if (intakeSlot != null && slot == intakeSlot)
                color = Color.green;
            Vector3 worldPos = transform.TransformPoint(slot.LocalPosition);
            if (slot.IsEmpty)
                Draw.WireBox(worldPos, Vector3.one * _slotDrawSize, color: color);
            else
                Draw.SolidBox(worldPos, Vector3.one * _slotDrawSize, color: color);
            
        }
    }
}
using System.Collections.Generic;
using Drawing;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PipeGenerator : MonoBehaviourGizmos
{
    [System.Serializable]
    public class PipeControlPoint
    {
        public Vector3 point;
        public Vector3 prevHandle;
        public Vector3 nextHandle;
    }

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
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public float Length { get; }

        public QuadraticBezierSegment(
            Vector3 a,
            Vector3 b,
            Vector3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;

            Length = ApproximateLength();
        }

        public Vector3 Evaluate(float t)
        {
            float u = 1f - t;

            return
                u * u * a +
                2f * u * t * b +
                t * t * c;
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

    [SerializeField] [InspectorName("Speed (Unit per sec)")]
    private float _speed;

    private readonly List<IConveyorSegment> _segments = new();

    [SerializeField, ReadOnly] private float _totalLength;
    [SerializeField] private float _paddingH;
    [SerializeField] private float _paddingV;

    [Header("Spline")] public List<PipeControlPoint> controlPoints = new();

    [Header("Pipe")] public float radius = 2f;
    public int radialSegments = 16;
    public int lengthSegmentsPerCurve = 24;

    [Header("Debug")] public bool realtimeUpdate = true;

    private Mesh mesh;

    public float TotalLength => _totalLength;

    [Button]
    public void Init()
    {
        BuildSegments();
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
            // Vector3 prev = transform.TransformPoint(_waypoints[(i - 1 + count) % count]);
            // Vector3 current = transform.TransformPoint(_waypoints[i]);
            // Vector3 next = transform.TransformPoint(_waypoints[(i + 1) % count]);
            // local space
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

    public Vector3 EvaluateDistanceWorld(float distance)
    {
        return transform.TransformPoint(EvaluateDistance(distance));
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
            Draw.SphereOutline(transform.TransformPoint(point), 0.15f);
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
    }


    private void Start()
    {
        Generate();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!realtimeUpdate)
            return;

        // RefreshControlPoints();
        // Generate();
    }
#endif

    // void RefreshControlPoints()
    // {
    //     controlPoints.Clear();
    //
    //     foreach (Transform child in transform)
    //     {
    //         Transform prev = child.Find("c1");
    //         Transform next = child.Find("c2");
    //
    //         if (prev == null)
    //         {
    //             GameObject go = new GameObject("c1");
    //             go.transform.SetParent(child);
    //             go.transform.localPosition = Vector3.left * 2f;
    //
    //             prev = go.transform;
    //         }
    //
    //         if (next == null)
    //         {
    //             GameObject go = new GameObject("c2");
    //             go.transform.SetParent(child);
    //             go.transform.localPosition = Vector3.right * 2f;
    //
    //             next = go.transform;
    //         }
    //
    //         controlPoints.Add(new PipeControlPoint()
    //         {
    //             point = child,
    //             prevHandle = prev,
    //             nextHandle = next
    //         });
    //     }
    // }
[ContextMenu("Generate")]
[Button]
public void Generate()
{
    if (_waypoints == null || _waypoints.Count < 3)
        return;

    BuildSegments();

    if (_totalLength <= 0f)
        return;

    if (mesh == null)
    {
        mesh = new Mesh();
        mesh.name = "Pipe Mesh";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    else
    {
        mesh.Clear();
    }

    BuildMesh();
}

void BuildMesh()
{
    List<Vector3> vertices = new();
    List<int> triangles = new();
    List<Vector2> uvs = new();

    int totalLengthSegments =
        Mathf.Max(
            4,
            Mathf.CeilToInt(
                _totalLength * lengthSegmentsPerCurve
            )
        );

    int ringVertexCount =
        radialSegments + 1;

    for (int i = 0; i <= totalLengthSegments; i++)
    {
        float distance =
            (i / (float)totalLengthSegments)
            * _totalLength;

        Vector3 center =
            EvaluateDistance(distance);

        Vector3 forward =
            GetTangent(distance);

        if (forward.sqrMagnitude < 0.0001f)
            forward = Vector3.forward;

        Vector3 up = Vector3.up;

        if (Mathf.Abs(Vector3.Dot(forward, up)) > 0.98f)
            up = Vector3.right;

        Vector3 right =
            Vector3.Cross(up, forward).normalized;

        up =
            Vector3.Cross(forward, right).normalized;

        for (int j = 0; j <= radialSegments; j++)
        {
            float angle =
                (j / (float)radialSegments)
                * Mathf.PI * 2f;

            Vector3 offset =
                right * Mathf.Cos(angle) * radius +
                up * Mathf.Sin(angle) * radius;

            vertices.Add(center + offset);

            uvs.Add(new Vector2(
                j / (float)radialSegments,
                distance / _totalLength
            ));
        }
    }

    for (int i = 0; i < totalLengthSegments; i++)
    {
        int ring =
            i * ringVertexCount;

        int nextRing =
            (i + 1) * ringVertexCount;

        for (int j = 0; j < radialSegments; j++)
        {
            int a = ring + j;
            int b = ring + j + 1;
            int c = nextRing + j;
            int d = nextRing + j + 1;

            triangles.Add(a);
            triangles.Add(c);
            triangles.Add(b);

            triangles.Add(b);
            triangles.Add(c);
            triangles.Add(d);
        }
    }

    mesh.SetVertices(vertices);
    mesh.SetTriangles(triangles, 0);
    mesh.SetUVs(0, uvs);

    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
}

Vector3 GetPoint(float distance)
{
    return EvaluateDistance(distance);
}

Vector3 GetTangent(float distance)
{
    float delta =
        Mathf.Max(
            0.01f,
            _totalLength * 0.001f
        );

    Vector3 p1 =
        EvaluateDistance(
            Mathf.Max(0f, distance - delta)
        );

    Vector3 p2 =
        EvaluateDistance(
            Mathf.Min(
                _totalLength,
                distance + delta
            )
        );

    return (p2 - p1).normalized;
}

    Vector3 CubicBezier(
        Vector3 a,
        Vector3 b,
        Vector3 c,
        Vector3 d,
        float t)
    {
        float u = 1f - t;

        return
            (u * u * u * a) +
            (3f * u * u * t * b) +
            (3f * u * t * t * c) +
            (t * t * t * d);
    }

    // ---------------------------------------------------
    // DEBUG
    // ---------------------------------------------------

    private void OnDrawGizmos()
    {
        if (controlPoints == null ||
            controlPoints.Count < 2)
            return;

        Gizmos.color = Color.cyan;

        Vector3 prev = GetPoint(0);

        int steps = 200;

        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;

            Vector3 current = GetPoint(t);

            Gizmos.DrawLine(prev, current);

            prev = current;
        }

        foreach (var cp in controlPoints)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(
                transform.TransformPoint(cp.point),
                0.2f
            );

            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(
                cp.point,
                cp.prevHandle
            );

            Gizmos.DrawSphere(
                cp.prevHandle,
                0.12f
            );

            Gizmos.DrawLine(
                cp.point,
                cp.nextHandle
            );

            Gizmos.DrawSphere(
                cp.nextHandle,
                0.12f
            );
        }
    }
}
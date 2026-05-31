using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PipeGenerator : MonoBehaviour
{
    [System.Serializable]
    public class PipeControlPoint
    {
        public Transform point;
        public Transform prevHandle;
        public Transform nextHandle;
    }

    [Header("Spline")]
    public List<PipeControlPoint> controlPoints = new();

    [Header("Pipe")]
    public float radius = 2f;
    public int radialSegments = 16;
    public int lengthSegmentsPerCurve = 24;

    [Header("Debug")]
    public bool realtimeUpdate = true;

    private Mesh mesh;

    private void Start()
    {
        Generate();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!realtimeUpdate)
            return;

        RefreshControlPoints();
        Generate();
    }
#endif

    void RefreshControlPoints()
    {
        controlPoints.Clear();

        foreach (Transform child in transform)
        {
            Transform prev = child.Find("c1");
            Transform next = child.Find("c2");

            if (prev == null)
            {
                GameObject go = new GameObject("c1");
                go.transform.SetParent(child);
                go.transform.localPosition = Vector3.left * 2f;

                prev = go.transform;
            }

            if (next == null)
            {
                GameObject go = new GameObject("c2");
                go.transform.SetParent(child);
                go.transform.localPosition = Vector3.right * 2f;

                next = go.transform;
            }

            controlPoints.Add(new PipeControlPoint()
            {
                point = child,
                prevHandle = prev,
                nextHandle = next
            });
        }
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        if (controlPoints.Count < 2)
        {
            return;
        }

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

        int totalCurveCount = controlPoints.Count - 1;
        int totalLengthSegments =
            totalCurveCount * lengthSegmentsPerCurve;

        int ringVertexCount = radialSegments + 1;

        for (int i = 0; i <= totalLengthSegments; i++)
        {
            float t = i / (float)totalLengthSegments;

            // avoid exact 1.0 tangent collapse
            t = Mathf.Min(t, 0.9999f);

            Vector3 center = GetPoint(t);
            Vector3 forward = GetTangent(t);

            // fallback if tangent dies
            if (forward.sqrMagnitude < 0.0001f)
            {
                forward = Vector3.forward;
            }

            // Simple frame generation
            Vector3 up = Vector3.up;

            if (Mathf.Abs(Vector3.Dot(forward, up)) > 0.99f)
            {
                up = Vector3.right;
            }

            Vector3 right =
                Vector3.Cross(up, forward).normalized;

            up =
                Vector3.Cross(forward, right).normalized;

            for (int j = 0; j <= radialSegments; j++)
            {
                float angle =
                    (j / (float)radialSegments) *
                    Mathf.PI * 2f;

                Vector3 localPos =
                    Mathf.Cos(angle) * right * radius +
                    Mathf.Sin(angle) * up * radius;

                vertices.Add(center + localPos);

                uvs.Add(new Vector2(
                    j / (float)radialSegments,
                    t * 5f
                ));
            }
        }

        // INSIDE PIPE TRIANGLES
        for (int i = 0; i < totalLengthSegments; i++)
        {
            int ringStart = i * ringVertexCount;
            int nextRingStart =
                (i + 1) * ringVertexCount;

            for (int j = 0; j < radialSegments; j++)
            {
                int a = ringStart + j;
                int b = ringStart + j + 1;
                int c = nextRingStart + j;
                int d = nextRingStart + j + 1;

                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(c);

                triangles.Add(b);
                triangles.Add(d);
                triangles.Add(c);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // ---------------------------------------------------
    // SPLINE
    // ---------------------------------------------------

    Vector3 GetPoint(float t)
    {
        t = Mathf.Clamp01(t);

        int segmentCount = controlPoints.Count - 1;

        // Prevent final segment collapse
        if (t >= 1f)
        {
            return controlPoints[segmentCount]
                .point.position;
        }

        float scaledT = t * segmentCount;

        int segment = Mathf.FloorToInt(scaledT);

        float localT = scaledT - segment;

        PipeControlPoint p0 =
            controlPoints[segment];

        PipeControlPoint p1 =
            controlPoints[segment + 1];

        Vector3 a = p0.point.position;
        Vector3 b = p0.nextHandle.position;
        Vector3 c = p1.prevHandle.position;
        Vector3 d = p1.point.position;

        return CubicBezier(
            a,
            b,
            c,
            d,
            localT
        );
    }

    Vector3 GetTangent(float t)
    {
        float delta = 0.001f;

        float t1 = Mathf.Clamp01(t);
        float t2 = Mathf.Clamp01(t + delta);

        // sample backwards near end
        if (t2 >= 1f)
        {
            t1 = Mathf.Clamp01(t - delta);
            t2 = t;
        }

        Vector3 p1 = GetPoint(t1);
        Vector3 p2 = GetPoint(t2);

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
            if (cp.point == null)
                continue;

            Gizmos.color = Color.red;

            Gizmos.DrawSphere(
                cp.point.position,
                0.2f
            );

            Gizmos.color = Color.yellow;

            if (cp.prevHandle != null)
            {
                Gizmos.DrawLine(
                    cp.point.position,
                    cp.prevHandle.position
                );

                Gizmos.DrawSphere(
                    cp.prevHandle.position,
                    0.12f
                );
            }

            if (cp.nextHandle != null)
            {
                Gizmos.DrawLine(
                    cp.point.position,
                    cp.nextHandle.position
                );

                Gizmos.DrawSphere(
                    cp.nextHandle.position,
                    0.12f
                );
            }
        }
    }
}
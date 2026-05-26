#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        static void OnLineRendererChanged(LineRenderer lineRenderer)
        {
            var debugLineRenderer = lineRenderer.GetComponent<DebugLineRenderer>();
            if (debugLineRenderer != null)
            {
                debugLineRenderer.OnLineRendererChanged(lineRenderer);
            }
        }

        [MenuItem("CONTEXT/LineRenderer/Log Points")]
        static void LineRendererLogPoints(MenuCommand menuCommand)
        {
            var lineRenderer = menuCommand.To<LineRenderer>();
            int pointCount = lineRenderer.positionCount;
            var points = new Vector3[pointCount];
            pointCount = lineRenderer.GetPositions(points);
            for (int i = 0; i < pointCount; i++)
            {
                LegacyLog.Debug($"Point {i}: {points[i]}");
            }
        }

        [MenuItem("CONTEXT/LineRenderer/Log Local Points")]
        static void LineRendererLogLocalPoints(MenuCommand menuCommand)
        {
            var lineRenderer = menuCommand.To<LineRenderer>();
            int pointCount = lineRenderer.positionCount;
            var points = new Vector3[pointCount];
            pointCount = lineRenderer.GetPositions(points);
            if (lineRenderer.useWorldSpace)
            {
                var pos = lineRenderer.transform.position;
                for (int i = 0; i < pointCount; i++)
                {
                    points[i] -= pos;
                }
            }
            for (int i = 0; i < pointCount; i++)
            {
                LegacyLog.Debug($"Point {i}: {points[i]}");
            }
        }

        [MenuItem("CONTEXT/LineRenderer/Copy Local Points")]
        static void LineRendererCopyLocalPoints(MenuCommand menuCommand)
        {
            var lineRenderer = menuCommand.To<LineRenderer>();
            int pointCount = lineRenderer.positionCount;
            var points = new Vector3[pointCount];
            pointCount = lineRenderer.GetPositions(points);
            if (lineRenderer.useWorldSpace)
            {
                var pos = lineRenderer.transform.position;
                for (int i = 0; i < pointCount; i++)
                {
                    points[i] -= pos;
                }
            }
            EditorContext.SetPoints(points, pointCount);
        }

        [MenuItem("CONTEXT/LineRenderer/Paste Local Points")]
        static void LineRendererPasteLocalPoints(MenuCommand menuCommand)
        {
            var points = EditorContext.Points;
            if (points == null)
            {
                LegacyLog.Warning("Points required!");
                return;
            }

            var lineRenderer = menuCommand.To<LineRenderer>();
            int pointCount = points.Count;
            lineRenderer.positionCount = pointCount;
            if (lineRenderer.useWorldSpace)
            {
                var pos = lineRenderer.transform.position;
                for (int i = 0; i < pointCount; i++)
                {
                    lineRenderer.SetPosition(i, pos + points[i]);
                }
            }
            else
            {
                for (int i = 0; i < pointCount; i++)
                {
                    lineRenderer.SetPosition(i, points[i]);
                }
            }
            OnLineRendererChanged(lineRenderer);
        }

        //[MenuItem("CONTEXT/LineRenderer/Localize")]
        //static void LineRendererLocalize(MenuCommand menuCommand)
        //{
        //    var lineRenderer = menuCommand.To<LineRenderer>();
        //    int pointCount = lineRenderer.positionCount;
        //    if (pointCount > 0)
        //    {
        //        var pos = lineRenderer.transform.position;
        //        if (!pos.Equals(Vector3.zero))
        //        {
        //            var points = new Vector3[pointCount];
        //            lineRenderer.GetPositions(points);
        //            for (int i = 0; i < pointCount; i++)
        //            {
        //                points[i] -= pos;
        //            }
        //            lineRenderer.SetPositions(points);
        //        }
        //    }
        //}

        [MenuItem("CONTEXT/LineRenderer/Reverse")]
        static void LineRendererReverse(MenuCommand menuCommand)
        {
            var lineRenderer = menuCommand.To<LineRenderer>();
            int pointCount = lineRenderer.positionCount;
            if (pointCount > 1)
            {
                var points = new Vector3[pointCount];
                lineRenderer.GetPositions(points);
                int i = 0, j = pointCount - 1;
                do
                {
                    var tmp = points[i];
                    points[i] = points[j];
                    points[j] = tmp;
                    i++;
                    j--;
                }
                while (i < j);
                lineRenderer.SetPositions(points);
                OnLineRendererChanged(lineRenderer);
            }
        }

        [MenuItem("CONTEXT/LineRenderer/Smooth Points...")]
        static void LineRendererSmoothPoints(MenuCommand menuCommand)
        {
            GUIHelper.ShowInputFloat("Smooth Points", "Segment Length", 0.1f, segmentLength =>
            {
                if (segmentLength > 0)
                {
                    var lineRenderer = menuCommand.To<LineRenderer>();
                    int pointCount = lineRenderer.positionCount;
                    var points = new Vector3[pointCount];
                    lineRenderer.GetPositions(points);
                    var smoothPoints = SmoothLine(points, segmentLength);
                    //Log.Warning($"{pointCount} vs {smoothPoints.Count}");
                    pointCount = smoothPoints.Count;
                    lineRenderer.positionCount = pointCount;
                    if (lineRenderer.useWorldSpace)
                    {
                        var pos = lineRenderer.transform.position;
                        for (int i = 0; i < pointCount; i++)
                        {
                            lineRenderer.SetPosition(i, pos + smoothPoints[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pointCount; i++)
                        {
                            lineRenderer.SetPosition(i, smoothPoints[i]);
                        }
                    }
                    OnLineRendererChanged(lineRenderer);
                    return true;
                }
                return false;
            });
        }

        static List<Vector3> SmoothLine(Vector3[] inputPoints, float segmentSize)
        {
            //create curves
            AnimationCurve curveX = new AnimationCurve();
            AnimationCurve curveY = new AnimationCurve();
            AnimationCurve curveZ = new AnimationCurve();

            //create keyframe sets
            Keyframe[] keysX = new Keyframe[inputPoints.Length];
            Keyframe[] keysY = new Keyframe[inputPoints.Length];
            Keyframe[] keysZ = new Keyframe[inputPoints.Length];

            //set keyframes
            for (int i = 0; i < inputPoints.Length; i++)
            {
                keysX[i] = new Keyframe(i, inputPoints[i].x);
                keysY[i] = new Keyframe(i, inputPoints[i].y);
                keysZ[i] = new Keyframe(i, inputPoints[i].z);
            }

            //apply keyframes to curves
            curveX.keys = keysX;
            curveY.keys = keysY;
            curveZ.keys = keysZ;

            //smooth curve tangents
            for (int i = 0; i < inputPoints.Length; i++)
            {
                curveX.SmoothTangents(i, 0);
                curveY.SmoothTangents(i, 0);
                curveZ.SmoothTangents(i, 0);
            }

            //list to write smoothed values to
            List<Vector3> lineSegments = new List<Vector3>();

            //find segments in each section
            for (int i = 0; i < inputPoints.Length; i++)
            {
                //add first point
                lineSegments.Add(inputPoints[i]);

                //make sure within range of array
                if (i + 1 < inputPoints.Length)
                {
                    //find distance to next point
                    float distanceToNext = Vector3.Distance(inputPoints[i], inputPoints[i + 1]);

                    //number of segments
                    int segments = (int)(distanceToNext / segmentSize);

                    //add segments
                    for (int s = 1; s < segments; s++)
                    {
                        //interpolated time on curve
                        float time = ((float)s / (float)segments) + (float)i;

                        //sample curves to find smoothed position
                        Vector3 newSegment = new Vector3(curveX.Evaluate(time), curveY.Evaluate(time), curveZ.Evaluate(time));

                        //add to list
                        lineSegments.Add(newSegment);
                    }
                }
            }

            return lineSegments;
        }
    }
}
#endif
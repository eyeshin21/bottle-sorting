#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    [RequireComponent(typeof(LineRenderer))]
    public class DebugLineRenderer : MonoBehaviour
    {
        [SerializeField] Color _color = Color.green;
        [SerializeField, OnValueChanged("OnPointCountChanged")] int _pointCount = -1;
        [SerializeField] bool _drawPoints = true;
        [SerializeField] bool _drawSelected = true;

        LineRenderer _lineRenderer;
        List<Vector3> _points;

        public void OnLineRendererChanged(LineRenderer lineRenderer)
        {
            if (this.CheckGetComponent(ref _lineRenderer))
            {
                if (lineRenderer == _lineRenderer)
                {
                    _points = lineRenderer.GetPositions2();
                    _pointCount = -1;
                    OnPointCountChanged();
                }
            }
        }

        void OnPointCountChanged()
        {
            if (_points == null)
            {
                if (this.CheckGetComponent(ref _lineRenderer))
                {
                    _points = _lineRenderer.GetPositions2();
                }
            }

            int pointCount = _points.GetCount();
            if (pointCount == 0) return;

            if (_pointCount < -1)
            {
                _pointCount = -1;
            }
            else if (_pointCount > pointCount)
            {
                _pointCount = pointCount;
            }
            else
            {
                _lineRenderer.SetPositions(_points, _pointCount);
            }
        }

        void Reset()
        {
            _lineRenderer = null;
            _points = null;
        }

        void OnDrawGizmos()
        {
            if (!_drawSelected)
            {
                DrawLine();
            }
        }

        void OnDrawGizmosSelected()
        {
            if (_drawSelected)
            {
                DrawLine();
            }
        }

        void DrawLine()
        {
            if (!this.CheckGetComponent(ref _lineRenderer)) return;

            int pointCount = _lineRenderer.positionCount;
            if (pointCount == 0) return;

            bool worldSpace = _lineRenderer.useWorldSpace;
            var worldPos = transform.position;
            var prevPos = _lineRenderer.GetPosition(0);
            if (!worldSpace)
            {
                prevPos += worldPos;
            }

            if (_drawPoints)
            {
                GizmosHelper.DrawPoint(prevPos, _color);
            }

            if (pointCount > 1)
            {
                for (int i = 1; i < pointCount; i++)
                {
                    var pos = _lineRenderer.GetPosition(i);
                    if (!worldSpace)
                    {
                        pos += worldPos;
                    }

                    if (_drawPoints)
                    {
                        GizmosHelper.DrawPoint(pos, Color.white);
                    }

                    if (i == 1)
                    {
                        GizmosHelper.DrawArrow(prevPos, pos, _color);
                    }
                    else
                    {
                        GizmosHelper.DrawLine(prevPos, pos, _color);
                    }

                    prevPos = pos;
                }
            }
        }
    }
}
#endif
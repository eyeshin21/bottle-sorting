using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public class PointsEditor : ITouchHandler, IKeyListener, IData
    {
        protected static readonly int DefaultMaxPoint = 128;

        protected Vector3[] _points; // Local
        protected int _maxPoint;
        protected int _pointCount;
        protected int _currentIndex = -1;
        protected Vector3 _pressedPos;

        public float TouchRadius { get; set; } = 0.15f;
        public float InsertDistance { get; set; } = 0.15f;
        public float RemoveDistance { get; set; } = 0.15f;
        public float FixEpsilon { get; set; } = 0.1f;
        public bool EditEnabled { get; set; } = true;

        public Vector3[] Points => _points;
        public int PointCount => _pointCount;

        public PointsEditor() : this(DefaultMaxPoint)
        {

        }

        public PointsEditor(int maxPoint)
        {
            _maxPoint = maxPoint;
            _points = new Vector3[maxPoint];
        }

        public PointsEditor(params Vector3[] points)
        {
            _pointCount = points.Length;
            _maxPoint = Mathf.Max(_pointCount, DefaultMaxPoint);
            _points = new Vector3[_maxPoint];
            for (int i = 0; i < _pointCount; i++)
            {
                _points[i] = points[i];
            }
        }

        protected void AddPoint(Vector3 pos)
        {
            CheckAddPoint();
            _points[_pointCount++] = pos;
            SelectPoint(_pointCount - 1, pos);
        }

        protected void InsertPoint(int index, Vector3 pos)
        {
            CheckAddPoint();

            for (int i = _pointCount - 1; i >= index; i--)
            {
                _points[i + 1] = _points[i];
            }
            _points[index] = pos;
            _pointCount++;

            SelectPoint(index, pos);
        }

        protected void RemovePoints(int fromIndex, int toIndex)
        {
            int count = toIndex - fromIndex + 1;
            for (int index = toIndex + 1; index < _pointCount; index++)
            {
                _points[fromIndex++] = _points[index];
            }
            _pointCount -= count;
        }

        protected void CheckAddPoint()
        {
            if (_pointCount == _maxPoint)
            {
                _maxPoint += 32;
                Array.Resize(ref _points, _maxPoint);
            }
        }

        protected void SelectPoint(int index, Vector3 pressedPos)
        {
            _currentIndex = index;
            _pressedPos = pressedPos;
        }

        public void Shift(float deltaX, float deltaY)
        {
            for (int i = 0; i < _pointCount; i++)
            {
                _points[i].x += deltaX;
                _points[i].y += deltaY;
            }
        }

        public void Fix()
        {
            if (_pointCount < 2) return;

            float x1 = _points[0].x;
            float y1 = _points[0].y;
            for (int i = 1; i < _pointCount; i++)
            {
                float x2 = _points[i].x;
                float y2 = _points[i].y;
                if (Mathf.Abs(x2 - x1) < FixEpsilon)
                {
                    _points[i].x = x1;
                }
                else
                {
                    x1 = x2;
                }

                if (Mathf.Abs(y2 - y1) < FixEpsilon)
                {
                    _points[i].y = y1;
                }
                else
                {
                    y1 = y2;
                }
            }
        }

        public bool OnTouchPressed(Vector3 pos)
        {
            if (_pointCount == 0)
            {
                AddPoint(pos);
                return true;
            }

            // Try to select point
            for (int i = _pointCount - 1; i >= 0; i--)
            {
                var point = _points[i];
                if (Helper.IsInsideCircle(point, TouchRadius, pos))
                {
                    SelectPoint(i, pos);
                    return true;
                }
            }

            // Try to insert point
            if (_pointCount > 1)
            {
                int insertIndex = -1;
                float minDistance = InsertDistance;
                for (int i = 1; i < _pointCount; i++)
                {
                    var point1 = _points[i - 1];
                    var point2 = _points[i];
                    bool inside = true;
                    if (pos.x < point1.x)
                    {
                        if (pos.x < point2.x)
                        {
                            inside = false;
                        }
                    }
                    else
                    {
                        if (pos.x > point2.x)
                        {
                            inside = false;
                        }
                    }

                    if (inside)
                    {
                        float distance = Mathf.Abs(Helper.GetCrossProduct(point1, point2, pos));
                        if (distance < minDistance)
                        {
                            insertIndex = i;
                            minDistance = distance;
                        }
                    }
                }

                if (insertIndex > 0)
                {
                    InsertPoint(insertIndex, pos);
                    return true;
                }
            }

            // Add point
            if (_pointCount == 1)
            {
                AddPoint(pos);
            }
            else
            {
                if (Helper.GetDistanceSquare(pos, _points[0]) < Helper.GetDistanceSquare(pos, _points[_pointCount - 1]))
                {
                    InsertPoint(0, pos);
                }
                else
                {
                    AddPoint(pos);
                }
            }

            return true;
        }

        public bool OnTouchMoved(Vector3 pos)
        {
            var currentPoint = _points[_currentIndex];
            currentPoint.x += pos.x - _pressedPos.x;
            currentPoint.y += pos.y - _pressedPos.y;
            _points[_currentIndex] = currentPoint;

            _pressedPos = pos;
            return true;
        }

        public void OnTouchReleased(Vector3 pos)
        {
            // Try to remove points
            if (_pointCount > 1)
            {
                var currentPoint = _points[_currentIndex];
                currentPoint.x += pos.x - _pressedPos.x;
                currentPoint.y += pos.y - _pressedPos.y;

                int removeToIndex = -1;
                float minDistanceSquare = RemoveDistance * RemoveDistance;
                for (int i = 0; i < _pointCount; i++)
                {
                    if (i != _currentIndex)
                    {
                        var point = _points[i];
                        float deltaX = currentPoint.x - point.x;
                        float deltaY = currentPoint.y - point.y;
                        float distanceSquare = deltaX * deltaX + deltaY * deltaY;
                        if (distanceSquare < minDistanceSquare)
                        {
                            removeToIndex = i;
                            minDistanceSquare = distanceSquare;
                        }
                    }
                }

                if (removeToIndex >= 0)
                {
                    if (_currentIndex < removeToIndex)
                    {
                        RemovePoints(_currentIndex, removeToIndex - 1);
                    }
                    else
                    {
                        RemovePoints(removeToIndex + 1, _currentIndex);
                    }
                }
            }

            _currentIndex = -1;
        }

        public bool OnKeyPressed(KeyCode keyCode)
        {
            if (keyCode == KeyCode.F)
            {
                Fix();
                return true;
            }
            return false;
        }

        public bool UpdateKey()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Fix();
                return true;
            }

            return false;
        }

        public string Serialize()
        {
            //return JsonHelper.Serialize(_points, _pointCount);
            return "";
        }

        public void Deserialize(string json)
        {
            //JsonHelper.Deserialize(json, out _points);
            //_pointCount = _points.Length;
            //_maxPoint = _pointCount;
            //_currentIndex = -1;
        }

#if UNITY_EDITOR
        public float DrawRadius { get; set; } = 0.05f;
        public Color LineColor { get; set; } = Color.green;
        public Color PointColor { get; set; } = Color.yellow;

        public void DrawGizmos(Transform transform, bool debug = false)
        {
            if (_pointCount == 0) return;

            var gizmosColor = Gizmos.color;

            // Convert to world
            var pos = transform.position;
            for (int i = 0; i < _pointCount; i++)
            {
                _points[i].x += pos.x;
                _points[i].y += pos.y;
            }

            // Polyline
            Gizmos.color = LineColor;
            for (int i = 1; i < _pointCount; i++)
            {
                Gizmos.DrawLine(_points[i - 1], _points[i]);
            }

            // Points
            Gizmos.color = PointColor;
            for (int i = 0; i < _pointCount; i++)
            {
                GizmosHelper.FillCircle(_points[i], DrawRadius);
            }

            if (EditEnabled)
            {
                // Touch
                if (_currentIndex < 0)
                {
                    Gizmos.color = Color.gray;
                    for (int i = 0; i < _pointCount; i++)
                    {
                        GizmosHelper.DrawCircle(_points[i], TouchRadius);
                    }
                }
                else
                {
                    Gizmos.color = Color.red;
                    for (int i = 0; i < _pointCount; i++)
                    {
                        if (i != _currentIndex)
                        {
                            GizmosHelper.DrawCircle(_points[i], RemoveDistance);
                        }
                    }

                    //Gizmos.color = Color.yellow;
                    //GizmosHelper.DrawCircle(_points[_currentIndex], TouchRadius);
                }

                if (debug)
                {
                    // Number
                    for (int i = 0; i < _pointCount; i++)
                    {
                        var textPos = _points[i];
                        GizmosHelper.DrawText($"{i + 1}", textPos, Color.white, 100);
                    }
                }
            }

            // Convert to local
            for (int i = 0; i < _pointCount; i++)
            {
                _points[i].x -= pos.x;
                _points[i].y -= pos.y;
            }

            Gizmos.color = gizmosColor;
        }
#endif
    }
}
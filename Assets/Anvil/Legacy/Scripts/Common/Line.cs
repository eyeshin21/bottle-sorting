using UnityEngine;

namespace Anvil.Legacy
{
    public class Line
    {
        float _x1, _y1;
        float _x2, _y2;
        float _a, _b;
        float _length;

        float _c => _a * _x1 + _b * _y1;

        public Vector3 StartPos => new Vector3(_x1, _y1);
        public Vector3 EndPos => new Vector3(_x2, _y2);

        public float Length
        {
            get
            {
                if (_length <= 0)
                {
                    _length = Mathf.Sqrt(_a * _a + _b * _b);
                }
                return _length;
            }
        }

        /// <summary>
        /// Returns angle in degrees.
        /// </summary>
        public float Angle
        {
            get
            {
                if (Mathf.Approximately(Mathf.Abs(_a), 0))
                {
                    return _x2 >= _x1 ? 0 : 180;
                }

                if (Mathf.Approximately(Mathf.Abs(_b), 0))
                {
                    return _y2 >= _y1 ? 90 : 270;
                }

                return Mathf.Atan2(_a, -_b) * Mathf.Rad2Deg;
            }
        }

        public Direction4 Direction => Angle.ToDirection();

        /// <summary>
        /// Returns angle in degrees.
        /// </summary>
        public float ReverseAngle
        {
            get
            {
                if (Mathf.Approximately(Mathf.Abs(_a), 0))
                {
                    return _x2 < _x1 ? 0 : 180;
                }

                if (Mathf.Approximately(Mathf.Abs(_b), 0))
                {
                    return _y2 < _y1 ? 90 : 270;
                }

                return Helper.ClampAngle(Mathf.Atan2(_a, -_b) * Mathf.Rad2Deg + 180);
            }
        }

        public Line()
        {

        }

        public Line(float x1, float y1, float x2, float y2)
        {
            Construct(x1, y1, x2, y2);
        }

        public Line(Vector3 pos1, Vector3 pos2)
        {
            Construct(pos1.x, pos1.y, pos2.x, pos2.y);
        }

        public void Construct(float x1, float y1, float x2, float y2)
        {
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
            _length = 0;
            UpdateAB();
        }

        public void Construct(Vector3 pos1, Vector3 pos2)
        {
            Construct(pos1.x, pos1.y, pos2.x, pos2.y);
        }

        public void Construct(Line line, float deltaX, float deltaY)
        {
            _x1 = line._x1 + deltaX;
            _y1 = line._y1 + deltaY;
            _x2 = line._x2 + deltaX;
            _y2 = line._y2 + deltaY;
            _a = line._a;
            _b = line._b;
            _length = line._length;
        }

        void UpdateAB()
        {
            _a = _y2 - _y1;
            _b = _x1 - _x2;
        }

        public void Translate(float deltaX, float deltaY)
        {
            _x1 += deltaX;
            _y1 += deltaY;
            _x2 += deltaX;
            _y2 += deltaY;
        }

        public void SetYs(float y1, float y2)
        {
            if (_a != 0)
            {
                float c = _c;
                _x1 = (c - y1 * _b) / _a;
                _x2 = (c - y2 * _b) / _a;
            }
            _y1 = y1;
            _y2 = y2;

            _length = 0;
            UpdateAB();
        }

        public bool GetX(float y, out float x)
        {
            if (_a != 0)
            {
                x = (_c - y * _b) / _a;
                return true;
            }

            x = 0;
            return false;
        }

        public bool GetY(float x, out float y)
        {
            if (_b != 0)
            {
                y = (_c - _a * x) / _b;
                return true;
            }

            y = 0;
            return false;
        }

        public float GetX(float y, float defaultX)
        {
            if (_a != 0)
            {
                return (_c - y * _b) / _a;
            }

            return defaultX;
        }

        public float GetY(float x, float defaultY)
        {
            if (_b != 0)
            {
                return (_c - _a * x) / _b;
            }

            return defaultY;
        }

        public float GetNextX(float length)
        {
            return _x1 - _b * length / Length;
        }

        public float GetNextY(float length)
        {
            return _y1 + _a * length / Length;
        }

        public Vector3 GetPositionByX(float x, float defaultY)
        {
            float y = _b != 0 ? (_c - _a * x) / _b : defaultY;
            return new Vector3(x, y);
        }

        public Vector3 GetPositionByY(float y, float defaultX)
        {
            float x = _a != 0 ? (_c - y * _b) / _a : defaultX;
            return new Vector3(x, y);
        }

        /// <summary>
        /// Returns next position from (x1,y1).
        /// </summary>
        public Vector3 GetNextPosition(float length)
        {
            float amount = length / Length;
            float x = _x1 - _b * amount;
            float y = _y1 + _a * amount;
            return new Vector3(x, y);
        }

        public Vector3 GetNextPosition(Vector3 middlePos, float length)
        {
            float amount = length / Length;
            float x = middlePos.x - _b * amount;
            float y = middlePos.y + _a * amount;
            return new Vector3(x, y);
        }

        public bool GetIntersect(Line line, out Vector3 point)
        {
            float a1 = _a;
            float b1 = _b;
            float a2 = line._a;
            float b2 = line._b;
            float det = a1 * b2 - a2 * b1;
            if (det != 0)
            {
                float c1 = _c;
                float c2 = line._c;
                float x = (b2 * c1 - b1 * c2) / det;
                float y = (a1 * c2 - a2 * c1) / det;
                point = new Vector3(x, y);
                return true;
            }

            point = Vector3.zero;
            return false;
        }

        public static Line CreateHorizontal(float y, float x1, float x2)
        {
            var line = new Line();
            line.Construct(x1, y, x2, y);
            return line;
        }

        public static Line CreateVertical(float x, float y1, float y2)
        {
            var line = new Line();
            line.Construct(x, y1, x, y2);
            return line;
        }

#if UNITY_EDITOR
        public void DrawGizmos(Color? color = null)
        {
            GizmosHelper.DrawLine(new Vector3(_x1, _y1), new Vector3(_x2, _y2), color);
        }

        public void DrawGizmos(float thickness, Color? color = null)
        {
            GizmosHelper.DrawLine(new Vector3(_x1, _y1), new Vector3(_x2, _y2), thickness, color);
        }
#endif
    }
}
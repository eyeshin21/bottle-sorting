using UnityEngine;

namespace Anvil.Legacy
{
    public class TouchReceiverQuadrilateral : ITouchReceiver
    {
        Vector3 _pos1, _pos2, _pos3, _pos4;
        float _left, _top, _right, _bottom;

        /// <summary>
        /// Angle in degrees.
        /// </summary>
        public void Construct(float width, float height, float angle)
        {
            angle *= Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * width * 0.5f;
            float y = Mathf.Sin(angle) * width * 0.5f;
            angle += Mathf.PI * 0.5f;
            float deltaX = Mathf.Cos(angle) * height * 0.5f;
            float deltaY = Mathf.Sin(angle) * height * 0.5f;
            _pos1 = new Vector3(-x + deltaX, -y + deltaY);
            _pos2 = new Vector3(x + deltaX, y + deltaY);
            _pos3 = new Vector3(x - deltaX, y - deltaY);
            _pos4 = new Vector3(-x - deltaX, -y - deltaY);
            _left = Mathf.Min(_pos1.x, Mathf.Min(_pos2.x, Mathf.Min(_pos3.x, _pos4.x)));
            _right = Mathf.Max(_pos1.x, Mathf.Max(_pos2.x, Mathf.Max(_pos3.x, _pos4.x)));
            _bottom = Mathf.Min(_pos1.y, Mathf.Min(_pos2.y, Mathf.Min(_pos3.y, _pos4.y)));
            _top = Mathf.Max(_pos1.y, Mathf.Max(_pos2.y, Mathf.Max(_pos3.y, _pos4.y)));
        }

        public void SetScale(float scale)
        {
            _pos1.x *= scale;
            _pos1.y *= scale;
            _pos2.x *= scale;
            _pos2.y *= scale;
            _pos3.x *= scale;
            _pos3.y *= scale;
            _pos4.x *= scale;
            _pos4.y *= scale;
            _left *= scale;
            _top *= scale;
            _right *= scale;
            _bottom *= scale;
        }

        public void GetSize(out float width, out float height)
        {
            width = _right - _left;
            height = _top - _bottom;
        }

        public bool Contains(Vector3 pos)
        {
            if (pos.x >= _left && pos.x <= _right && pos.y >= _bottom && pos.y <= _top)
            {
                return Helper.IsInsideTriangle(pos, _pos1, _pos2, _pos3) || Helper.IsInsideTriangle(pos, _pos1, _pos3, _pos4);
            }
            return false;
        }

#if DEBUG_MODE
        public void DrawGizmos(Vector3 pos, Color color)
        {
            GizmosHelper.DrawQuadrilateral(pos, _pos1, _pos2, _pos3, _pos4, color);
            //GizmosHelper.DrawLine(pos + _pos1, pos + _pos3, color);
        }
#endif

        #region Pool
        public void ReturnToPool()
        {
            _pool.Return(this);
        }

        static Pool<TouchReceiverQuadrilateral> _pool = new();
        public static TouchReceiverQuadrilateral Create(float width, float height, float angle)
        {
            var quadrilateral = _pool.Get();
            quadrilateral.Construct(width, height, angle);
            return quadrilateral;
        }
        #endregion
    }
}
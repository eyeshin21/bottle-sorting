using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class AABB
    {
        float _left, _top, _right, _bottom;

        public float Left
        {
            get => _left;
            set => _left = value;
        }

        public float Top
        {
            get => _top;
            set => _top = value;
        }

        public float Right
        {
            get => _right;
            set => _right = value;
        }

        public float Bottom
        {
            get => _bottom;
            set => _bottom = value;
        }

        public Vector3 CenterPosition => new Vector3((_left + _right) * 0.5f, (_bottom + _top) * 0.5f, 0);

        static AABB _default;
        public static AABB Default => _default ??= new();

        public AABB()
        {

        }

        public AABB(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public AABB(Vector3 topLeft, Vector3 bottomRight)
        {
            _left = topLeft.x;
            _top = topLeft.y;
            _right = bottomRight.x;
            _bottom = bottomRight.y;
        }

        public AABB(AABB aabb)
        {
            _left = aabb._left;
            _top = aabb._top;
            _right = aabb._right;
            _bottom = aabb._bottom;
        }

        public AABB(Camera camera)
        {
            if (camera != null)
            {
                camera.GetAABB(out _left, out _top, out _right, out _bottom);
            }
            else
            {
                LegacyLog.Warning("Camera is null");
            }
        }

        public void Construct(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public void Construct(Vector3 centerPos, float width, float height, float extra)
        {
            _left = centerPos.x - width * 0.5f - extra;
            _right = centerPos.x * 2 - _left;
            _bottom = centerPos.y - height * 0.5f - extra;
            _top = centerPos.y * 2 - _bottom;
        }

        public void Get(out float left, out float top, out float right, out float bottom)
        {
            left = _left;
            top = _top;
            right = _right;
            bottom = _bottom;
        }

        public void GetSize(out float width, out float height)
        {
            width = Mathf.Abs(_right - _left);
            height = Mathf.Abs(_top - _bottom);
        }

        public bool Contains(Vector3 pos)
        {
            return pos.x >= _left && pos.x <= _right && pos.y >= _bottom && pos.y <= _top;
        }

        public bool Approximately(float left, float top, float right, float bottom, float epsilon)
        {
            return Mathf.Abs(_left - left) <= epsilon && Mathf.Abs(_top - top) <= epsilon && Mathf.Abs(_right - right) <= epsilon && Mathf.Abs(_bottom - bottom) <= epsilon;
        }

        public bool Approximately(AABB aabb, float epsilon)
        {
            return Mathf.Abs(_left - aabb._left) <= epsilon && Mathf.Abs(_top - aabb._top) <= epsilon && Mathf.Abs(_right - aabb._right) <= epsilon && Mathf.Abs(_bottom - aabb._bottom) <= epsilon;
        }

        public void Clamp(ref Vector3 pos)
        {
            if (pos.x < _left)
            {
                pos.x = _left;
            }
            else if (pos.x > _right)
            {
                pos.x = _right;
            }

            if (pos.y < _bottom)
            {
                pos.y = _bottom;
            }
            else if (pos.y > _top)
            {
                pos.y = _top;
            }
        }

        public void Merge(float left, float top, float right, float bottom)
        {
            _left = Mathf.Min(_left, left);
            _top = Mathf.Max(_top, top);
            _right = Mathf.Max(_right, right);
            _bottom = Mathf.Min(_bottom, bottom);
        }

        public void Merge(AABB aabb)
        {
            if (aabb != null)
            {
                _left = Mathf.Min(_left, aabb._left);
                _top = Mathf.Max(_top, aabb._top);
                _right = Mathf.Max(_right, aabb._right);
                _bottom = Mathf.Min(_bottom, aabb._bottom);
            }
        }

        public void Translate(float deltaX, float deltaY)
        {
            _left += deltaX;
            _right += deltaX;
            _bottom += deltaY;
            _top += deltaY;
        }

        public void Scale(float scale)
        {
            _left *= scale;
            _top *= scale;
            _right *= scale;
            _bottom *= scale;
        }

        //public void Update(float offsetX, float offsetY, float scale)
        //{
        //    _left = (_left + offsetX) * scale;
        //    _top = (_top + offsetY) * scale;
        //    _right = (_right + offsetX) * scale;
        //    _bottom = (_bottom + offsetY) * scale;
        //}

        public void Clear()
        {
            _left = _top = _right = _bottom = 0;
        }

        public override string ToString()
        {
            return $"({_left:0.00}, {_top:0.00}, {_right:0.00}, {_bottom:0.00})";
        }

        #region Pool
        static Pool<AABB> _pool;
        static Pool<AABB> Pool => _pool ??= new();

        static Pool<List<AABB>> _poolList;
        static Pool<List<AABB>> PoolList => _poolList ??= new();

        public void ReturnPool()
        {
            Pool.Return(this);
        }

        public static AABB Get()
        {
            return Pool.Get();
        }

        public static List<AABB> GetList()
        {
            return PoolList.Get();
        }

        public static void Return(List<AABB> list)
        {
            if (list != null)
            {
                Pool.Return(list);
                PoolList.Return(list);
            }
        }
        #endregion

#if UNITY_EDITOR || DEBUG_MODE
        public Color Color { get; set; }

        public void DrawDebug(Color color, float duration)
        {
            var pos1 = new Vector3(_left, _bottom);
            var pos2 = new Vector3(_left, _top);
            var pos3 = new Vector3(_right, _top);
            var pos4 = new Vector3(_right, _bottom);
            Debug.DrawLine(pos1, pos2, color, duration);
            Debug.DrawLine(pos2, pos3, color, duration);
            Debug.DrawLine(pos3, pos4, color, duration);
            Debug.DrawLine(pos4, pos1, color, duration);
        }

        public void DrawGizmos(Color? color = null, bool fill = false)
        {
            if (fill)
            {
                GizmosHelper.FillAABB(_left, _top, _right, _bottom, color);
            }
            else
            {
                GizmosHelper.DrawAABB(_left, _top, _right, _bottom, color);
            }
        }

        public void DrawGizmos(Vector3 centerPos, Color? color = null, bool fill = false)
        {
            if (fill)
            {
                GizmosHelper.FillAABB(centerPos.x + _left, centerPos.y + _top, centerPos.x + _right, centerPos.y + _bottom, color);
            }
            else
            {
                GizmosHelper.DrawAABB(centerPos.x + _left, centerPos.y + _top, centerPos.x + _right, centerPos.y + _bottom, color);
            }
        }
#endif
    }
}
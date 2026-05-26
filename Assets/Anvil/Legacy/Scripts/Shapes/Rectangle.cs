using UnityEngine;

namespace Anvil.Legacy
{
    public class Rectangle : BaseShape
    {
        float _width;
        float _height;

        public void Construct(float width, float height)
        {
            _width = width;
            _height = height;
        }

        public override bool Contains(Vector3 pos)
        {
            float left = _localPos.x - _width * 0.5f;
            if (pos.x < left || pos.x > left + _width) return false;
            float bottom = _localPos.y - _height * 0.5f;
            return pos.y >= bottom && pos.y <= bottom + _height;
        }

#if DEBUG_MODE
        public override void DrawGizmos(Vector3 pos, Color? color)
        {
            GizmosHelper.DrawRect(pos + _localPos, _width, _height, color);
        }
#endif

        #region Pool
        static Pool<Rectangle> _pool;
        static Pool<Rectangle> Pool => _pool ??= new();

        public override void ReturnToPool()
        {
            Pool.Return(this);
        }

        public static Rectangle Create(float width, float height)
        {
            var rect = Pool.Get();
            rect.Construct(width, height);
            return rect;
        }
        #endregion
    }
}
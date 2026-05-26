using UnityEngine;

namespace Anvil.Legacy
{
    public class TouchReceiverCircle : ITouchReceiver
    {
        float _radius;

        public void Construct(float radius)
        {
            _radius = radius;
        }

        public void SetScale(float scale)
        {
            _radius *= scale;
        }

        public void GetSize(out float width, out float height)
        {
            width = height = _radius * 2;
        }

        public bool Contains(Vector3 pos)
        {
            return pos.x * pos.x + pos.y * pos.y <= _radius * _radius;
        }

#if DEBUG_MODE
        public void DrawGizmos(Vector3 pos, Color color)
        {
            GizmosHelper.DrawCircle(pos, _radius, 3, color);
        }
#endif

        #region Pool
        public void ReturnToPool()
        {
            _pool.Return(this);
        }

        static Pool<TouchReceiverCircle> _pool = new();
        public static TouchReceiverCircle Create(float radius)
        {
            var circle = _pool.Get();
            circle.Construct(radius);
            return circle;
        }
        #endregion
    }
}
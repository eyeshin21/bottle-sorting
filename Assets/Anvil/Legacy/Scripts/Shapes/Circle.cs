using UnityEngine;

namespace Anvil.Legacy
{
    public class Circle : BaseShape
    {
        float _radius;

        public void Construct(float radius)
        {
            _radius = radius;
        }

        public override bool Contains(Vector3 pos)
        {
            float deltaX = pos.x - _localPos.x;
            float deltaY = pos.y - _localPos.y;
            return deltaX * deltaX + deltaY * deltaY <= _radius * _radius;
        }

#if DEBUG_MODE
        public override void DrawGizmos(Vector3 pos, Color? color)
        {
            GizmosHelper.DrawCircle(pos + _localPos, _radius, 3, color);
        }
#endif

        #region Pool
        static Pool<Circle> _pool;
        static Pool<Circle> Pool => _pool ??= new();

        public override void ReturnToPool()
        {
            Pool.Return(this);
        }

        public static Circle Create(float radius)
        {
            var circle = Pool.Get();
            circle.Construct(radius);
            return circle;
        }
        #endregion
    }
}
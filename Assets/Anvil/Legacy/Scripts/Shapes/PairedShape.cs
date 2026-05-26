using UnityEngine;

namespace Anvil.Legacy
{
    public class PairedShape : BaseShape
    {
        IShape _shape1, _shape2;

        public void Construct(IShape shape1, IShape shape2)
        {
            _shape1 = shape1;
            _shape2 = shape2;
        }

        public override bool Contains(Vector3 pos)
        {
            pos -= _localPos;

            if (_shape1 != null && _shape1.Contains(pos))
            {
                return true;
            }
            return _shape2 != null && _shape2.Contains(pos);
        }

#if DEBUG_MODE
        public override void DrawGizmos(Vector3 pos, Color? color)
        {
            pos -= _localPos;
            _shape1?.DrawGizmos(pos, color);
            _shape2?.DrawGizmos(pos, color);
        }
#endif

        #region Pool
        static Pool<PairedShape> _pool;
        static Pool<PairedShape> Pool => _pool ??= new();

        public override void ReturnToPool()
        {
            if (_shape1 != null)
            {
                _shape1.ReturnToPool();
                _shape1 = null;
            }
            if (_shape2 != null)
            {
                _shape2.ReturnToPool();
                _shape2 = null;
            }
            Pool.Return(this);
        }

        public static PairedShape Create(IShape shape1, IShape shape2)
        {
            var shape = Pool.Get();
            shape.Construct(shape1, shape2);
            return shape;
        }
        #endregion
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class AnimationCurveMoveController : LerpMoveController
    {
        private AnimationCurve _curve;

        public AnimationCurve Curve
        {
            get => _curve;
            set => _curve = value;
        }

        protected override float GetLerp(float t)
        {
            return _curve != null ? _curve.Evaluate(t) : t;
        }

        static Pool<AnimationCurveMoveController> _pool;
        static Pool<AnimationCurveMoveController> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new Pool<AnimationCurveMoveController>();
                }
                return _pool;
            }
        }

        public static AnimationCurveMoveController Create()
        {
            return Pool.Get();
        }

        public override void UpdateConfig(MoveConfig moveConfig)
        {
            base.UpdateConfig(moveConfig);
            Curve = moveConfig.AnimationCurve;
        }

        public override void ReturnPool()
        {
            base.ReturnPool();
            Pool.Return(this);
        }
    }
}
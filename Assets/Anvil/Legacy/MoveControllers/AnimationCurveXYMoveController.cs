using UnityEngine;

namespace Anvil.Legacy
{
    public class AnimationCurveXYMoveController : LerpMoveController
    {
        private AnimationCurve _curve;

        public AnimationCurve Curve
        {
            get => _curve;
            set => _curve = value;
        }

        public override void Update(float deltaTime)
        {
            if (!_isFinished)
            {
                _time += deltaTime;
                if (_time < _duration)
                {
                    float t = _time / _duration;
                    float x = _startPos.x + (_endPos.x - _startPos.x) * t;
                    float y = _startPos.y + (_endPos.y - _startPos.y) * _curve.Evaluate(t);
                    Current = new Vector3(x, y, 0);
                }
                else
                {
                    Current = _endPos;
                    OnFinish();
                }
            }
        }

        static Pool<AnimationCurveXYMoveController> _pool;
        static Pool<AnimationCurveXYMoveController> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new Pool<AnimationCurveXYMoveController>();
                }
                return _pool;
            }
        }

        public static AnimationCurveXYMoveController Create()
        {
            return Pool.Get();
        }

        public override void UpdateConfig(MoveConfig moveConfig)
        {
            base.UpdateConfig(moveConfig);
            Curve = moveConfig.AnimationCurveXY;
        }

        public override void ReturnPool()
        {
            base.ReturnPool();
            Pool.Return(this);
        }
    }
}
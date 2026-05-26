using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public class CompositeMoveController : BaseMoveController
    {
        private LinearMoveController _linearMoveController;
        private SmoothStepMoveController _smoothStepMoveController;
        private EasingMoveController _easingMoveController;
        private AnimationCurveMoveController _animationCurveMoveController;
        private AnimationCurveXYMoveController _animationCurveXYMoveController;
        private QuadBezierMoveController _quadBezierMoveController;
        private CubicBezierMoveController _cubicBezierMoveController;
        private BaseMoveController _currentMoveController;

        LinearMoveController LinearMoveController
        {
            get
            {
                if (_linearMoveController == null)
                {
                    _linearMoveController = LinearMoveController.Create();
                }
                return _linearMoveController;
            }
        }

        SmoothStepMoveController SmoothStepMoveController
        {
            get
            {
                if (_smoothStepMoveController == null)
                {
                    _smoothStepMoveController = SmoothStepMoveController.Create();
                }
                return _smoothStepMoveController;
            }
        }

        EasingMoveController EasingMoveController
        {
            get
            {
                if (_easingMoveController == null)
                {
                    _easingMoveController = EasingMoveController.Create();
                }
                return _easingMoveController;
            }
        }

        AnimationCurveMoveController AnimationCurveMoveController
        {
            get
            {
                if (_animationCurveMoveController == null)
                {
                    _animationCurveMoveController = AnimationCurveMoveController.Create();
                }
                return _animationCurveMoveController;
            }
        }

        AnimationCurveXYMoveController AnimationCurveXYMoveController
        {
            get
            {
                if (_animationCurveXYMoveController == null)
                {
                    _animationCurveXYMoveController = AnimationCurveXYMoveController.Create();
                }
                return _animationCurveXYMoveController;
            }
        }

        QuadBezierMoveController QuadBezierMoveController
        {
            get
            {
                if (_quadBezierMoveController == null)
                {
                    _quadBezierMoveController = QuadBezierMoveController.Create();
                }
                return _quadBezierMoveController;
            }
        }

        CubicBezierMoveController CubicBezierMoveController
        {
            get
            {
                if (_cubicBezierMoveController == null)
                {
                    _cubicBezierMoveController = CubicBezierMoveController.Create();
                }
                return _cubicBezierMoveController;
            }
        }

        BaseMoveController CurrentMoveController
        {
            set
            {
                _currentMoveController = value;
                if (_currentMoveController != null)
                {
                    _currentMoveController.transform = _transform;
                }
            }
        }

        public override Transform transform
        {
            set
            {
                _transform = value;
                if (_currentMoveController != null)
                {
                    _currentMoveController.transform = value;
                }
            }
        }

        public override bool Local
        {
            set
            {
                _isLocal = value;
                if (_currentMoveController != null)
                {
                    _currentMoveController.Local = value;
                }
            }
        }

        public override void MoveTo(Vector3 endPos, Action callback = null)
        {
            base.MoveTo(endPos);
            if (_currentMoveController == null)
            {
                _currentMoveController = LinearMoveController;
            }
            _currentMoveController.MoveTo(endPos, callback);
        }

        public void SmoothStepMoveTo(Vector3 endPos, float duration)
        {
            var moveController = SmoothStepMoveController;
            moveController.MoveTo(endPos, duration);
            CurrentMoveController = moveController;
        }

        public void SmoothStepMoveToSpeed(Vector3 endPos, float speed)
        {
            var moveController = SmoothStepMoveController;
            moveController.MoveToSpeed(endPos, speed);
            CurrentMoveController = moveController;
        }

        public void EasingMoveTo(Vector3 endPos, EaseType easeType, float duration)
        {
            var moveController = EasingMoveController;
            moveController.EaseType = easeType;
            moveController.MoveTo(endPos, duration);
            CurrentMoveController = moveController;
        }

        public void EasingMoveToSpeed(Vector3 endPos, EaseType easeType, float speed)
        {
            var moveController = EasingMoveController;
            moveController.EaseType = easeType;
            moveController.MoveToSpeed(endPos, speed);
            CurrentMoveController = moveController;
        }

        public void AnimationCurveMoveTo(Vector3 endPos, AnimationCurve curve, float duration)
        {
            var moveController = AnimationCurveMoveController;
            moveController.Curve = curve;
            moveController.MoveTo(endPos, duration);
            CurrentMoveController = moveController;
        }

        public void AnimationCurveMoveToSpeed(Vector3 endPos, AnimationCurve curve, float speed)
        {
            var moveController = AnimationCurveMoveController;
            moveController.Curve = curve;
            moveController.MoveToSpeed(endPos, speed);
            CurrentMoveController = moveController;
        }

        public void AnimationCurveXYMoveTo(Vector3 endPos, AnimationCurve curve, float duration)
        {
            var moveController = AnimationCurveXYMoveController;
            moveController.Curve = curve;
            moveController.MoveTo(endPos, duration);
            CurrentMoveController = moveController;
        }

        public void AnimationCurveXYMoveToSpeed(Vector3 endPos, AnimationCurve curve, float speed)
        {
            var moveController = AnimationCurveXYMoveController;
            moveController.Curve = curve;
            moveController.MoveToSpeed(endPos, speed);
            CurrentMoveController = moveController;
        }

        public void QuadBezierMoveTo(Vector3 endPos, Vector3 controlPos, float duration)
        {
            var moveController = QuadBezierMoveController;
            moveController.MoveTo(endPos, controlPos, duration);
            CurrentMoveController = moveController;
        }

        public void CubicBezierMoveTo(Vector3 endPos, Vector3 controlPos, Vector3 control2Pos, float duration)
        {
            var moveController = CubicBezierMoveController;
            moveController.MoveTo(endPos, controlPos, control2Pos, duration);
            CurrentMoveController = moveController;
        }

        public override void ForceFinish()
        {
            if (!_isFinished)
            {
                _isFinished = true;
                if (_currentMoveController != null)
                {
                    _currentMoveController.ForceFinish();
                }
                else
                {
                    Current = _endPos;
                }
            }
        }

        public override void Update(float deltaTime)
        {
            if (!_isFinished)
            {
                if (_currentMoveController != null)
                {
                    _currentMoveController.Update(deltaTime);
                    if (_currentMoveController.Finished)
                    {
                        OnFinish();
                    }
                }
            }
        }

        static Pool<CompositeMoveController> _pool;
        static Pool<CompositeMoveController> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new Pool<CompositeMoveController>();
                }
                return _pool;
            }
        }

        public static CompositeMoveController Create()
        {
            return Pool.Get();
        }

        BaseMoveController GetMoveController(MoveType moveType)
        {
            if (moveType == MoveType.Linear) return LinearMoveController;
            if (moveType == MoveType.SmoothStep) return SmoothStepMoveController;
            if (moveType == MoveType.Easing) return EasingMoveController;
            if (moveType == MoveType.AnimationCurve) return AnimationCurveMoveController;
            if (moveType == MoveType.AnimationCurveXY) return AnimationCurveXYMoveController;
            if (moveType == MoveType.QuadBezier) return QuadBezierMoveController;
            if (moveType == MoveType.CubicBezier) return CubicBezierMoveController;

            Assert.Todo(moveType);
            return null;
        }

        public void SetMoveController(MoveType moveType, MoveConfig moveConfig)
        {
            var moveController = GetMoveController(moveType);
            if (moveController != null)
            {
                moveController.UpdateConfig(moveConfig);
                CurrentMoveController = moveController;
            }
            else
            {
                LegacyLog.Warning($"Can't set move controller for {moveType}!");
            }
        }

        public override void UpdateConfig(MoveConfig moveConfig)
        {
            var moveController = GetMoveController(moveConfig.MoveType);
            if (moveController != null)
            {
                moveController.UpdateConfig(moveConfig);
                CurrentMoveController = moveController;
            }
        }

        public override void ReturnPool()
        {
            base.ReturnPool();

            if (_linearMoveController != null)
            {
                _linearMoveController.ReturnPool();
                _linearMoveController = null;
            }

            if (_smoothStepMoveController != null)
            {
                _smoothStepMoveController.ReturnPool();
                _smoothStepMoveController = null;
            }

            if (_easingMoveController != null)
            {
                _easingMoveController.ReturnPool();
                _easingMoveController = null;
            }

            if (_animationCurveMoveController != null)
            {
                _animationCurveMoveController.ReturnPool();
                _animationCurveMoveController = null;
            }

            if (_animationCurveXYMoveController != null)
            {
                _animationCurveXYMoveController.ReturnPool();
                _animationCurveXYMoveController = null;
            }

            if (_quadBezierMoveController != null)
            {
                _quadBezierMoveController.ReturnPool();
                _quadBezierMoveController = null;
            }

            if (_cubicBezierMoveController != null)
            {
                _cubicBezierMoveController.ReturnPool();
                _cubicBezierMoveController = null;
            }

            _currentMoveController = null;

            Pool.Return(this);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            var name = _currentMoveController != null ? _currentMoveController.ToString() : base.ToString();
            int index = name.LastIndexOf('.');
            if (index > 0)
            {
                name = name.Substring(index + 1);
            }
            return name;
        }
#endif
    }
}
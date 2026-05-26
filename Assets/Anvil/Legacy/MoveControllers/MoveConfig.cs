using UnityEngine;

namespace Anvil.Legacy
{
    [System.Serializable]
    public class MoveConfig
    {
        [SerializeField] MoveType _moveType = MoveType.Linear;
        // Easing
        [SerializeField, ConditionalShow("_moveType", (int)MoveType.Easing)]
        EaseType _easeType = EaseType.Linear;

        //[SerializeField, ConditionalShow("_moveType", (int)MoveType.Easing)]
        //float _customEpsilon = 0f;

        // AnimationCurve
        [SerializeField, ConditionalShow("_moveType", (int)MoveType.AnimationCurve)]
        AnimationCurve _animationCurve = Defaults.AnimationCurve;
        // AnimationCurveXY
        [SerializeField, ConditionalShow("_moveType", (int)MoveType.AnimationCurveXY)]
        AnimationCurve _animationCurveXY = Defaults.AnimationCurve;
        // QuadBezier
        [SerializeField, ConditionalShow("_moveType", (int)MoveType.QuadBezier)]
        QuadBezierConfig2D _quadBezier;
        // CubicBezier
        [SerializeField, ConditionalShow("_moveType", (int)MoveType.CubicBezier)]
        CubicBezierConfig2D _cubicBezier;

        // Movement
        [SerializeField] MovementType _movementType = MovementType.Duration;
        [SerializeField, ConditionalShow("_movementType", (int)MovementType.Duration)]
        float _duration = 1;
        [SerializeField, ConditionalShow("_movementType", (int)MovementType.Speed)]
        float _speed = 1;

        public MoveType MoveType => _moveType;
        public EaseType EaseType => _easeType;
        public AnimationCurve AnimationCurve => _animationCurve;
        public AnimationCurve AnimationCurveXY => _animationCurveXY;
        public QuadBezierConfig2D QuadBezierConfig2D => _quadBezier;
        public CubicBezierConfig2D CubicBezierConfig2D => _cubicBezier;

        public MovementType MovementType => _movementType;
        public float Duration => _duration;
        public float Speed => _speed;

        public void SetDuration(float duration)
        {
            _movementType = MovementType.Duration;
            _duration = duration;
        }

        public MoveConfig Clone()
        {
            MoveConfig clone = new MoveConfig();
            clone._moveType = _moveType;
            clone._easeType = _easeType;
            clone._animationCurve = _animationCurve;
            clone._animationCurveXY = _animationCurveXY;
            clone._quadBezier = _quadBezier.Clone();
            clone._cubicBezier = _cubicBezier.Clone();
            clone._movementType = _movementType;
            clone._duration = _duration;
            clone._speed = _speed;
            return clone;
        }
        public IMoveController CreateMoveController()
        {
            IMoveController moveController = null;

            if (_moveType == MoveType.SmoothStep)
            {
                moveController = SmoothStepMoveController.Create();
            }
            else if (_moveType == MoveType.Easing)
            {
                moveController = EasingMoveController.Create();
            }
            else if (_moveType == MoveType.AnimationCurve)
            {
                moveController = AnimationCurveMoveController.Create();
            }
            else if (_moveType == MoveType.AnimationCurveXY)
            {
                moveController = AnimationCurveXYMoveController.Create();
            }
            else if (_moveType == MoveType.QuadBezier)
            {
                moveController = QuadBezierMoveController.Create();
            }
            else if (_moveType == MoveType.CubicBezier)
            {
                moveController = CubicBezierMoveController.Create();
            }
            else
            {
#if UNITY_EDITOR || DEBUG_MODE
                if (_moveType != MoveType.Linear)
                {
                    Assert.Todo(_moveType);
                }
#endif
                moveController = LinearMoveController.Create();
            }

            if (moveController != null)
            {
                moveController.UpdateConfig(this);
            }

            return moveController;
        }

#if UNITY_EDITOR
        public void DrawGizmos(Vector3 startPos, Vector3 endPos, Color? color = null)
        {
            if (_moveType == MoveType.AnimationCurveXY)
            {
                GizmosHelper.DrawCurve(startPos, endPos, _animationCurveXY, color);
            }
            else if (_moveType == MoveType.QuadBezier)
            {
                _quadBezier.DrawGizmos(startPos, endPos, color);
            }
            else if (_moveType == MoveType.CubicBezier)
            {
                _cubicBezier.DrawGizmos(startPos, endPos, color);
            }
            else
            {
                GizmosHelper.DrawLine(startPos, endPos, color);
                var textPos = (startPos + endPos) * 0.5f;
                string text;
                if (_moveType == MoveType.Easing)
                {
                    text = _easeType.ToString();
                }
                else
                {
                    text = _moveType.ToString();
                }
                GizmosHelper.DrawText(text, textPos, Color.yellow);
            }
        }
#endif
    }
}

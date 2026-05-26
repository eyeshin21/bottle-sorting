using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Scale : ActionX
    {
        Transform _transform;
        FloatController _xController;
        FloatController _yController;

        public override float Duration
        {
            get
            {
                float xDuration = _xController.Duration;
                if (xDuration < 0) return xDuration;
                float yDuration = _yController.Duration;
                if (yDuration < 0) return yDuration;

                return Mathf.Max(xDuration, yDuration);
            }
        }

        public override GameObject Target
        {
            set
            {
                _target = value;
                _transform = value.transform;
            }
        }

        public void Construct(Transform transform, FloatController xController, FloatController yController)
        {
            _transform = transform;
            _xController = xController;
            _yController = yController;
            _target = transform?.gameObject;
            _Construct();
        }

        public override void Reset()
        {
            base.Reset();
            _xController.Reset();
            if (_yController != _xController)
            {
                _yController.Reset();
            }
        }

        protected override bool OnPlay()
        {
            float scaleX = _xController.Value;
            if (_yController == _xController)
            {
                _transform.localScale = new Vector3(scaleX, scaleX, 1f);
                return _xController.Finished;
            }

            float scaleY = _yController.Value;
            _transform.localScale = new Vector3(scaleX, scaleY, 1f);
            return _xController.Finished && _yController.Finished;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                var scale = _transform.localScale;

                if (_xController.GetEnd(out float scaleX))
                {
                    scale.x = scaleX;
                    if (_yController == _xController)
                    {
                        scale.y = scaleX;
                    }
                }

                if (_yController != _xController)
                {
                    if (_yController.GetEnd(out float scaleY))
                    {
                        scale.y = scaleY;
                    }
                }

                _transform.localScale = scale;
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            float scaleX = _xController.Update(deltaTime);
            if (_yController == _xController)
            {
                if (_transform != null)
                {
                    _transform.localScale = new Vector3(scaleX, scaleX, 1f);
                }
                return _xController.Finished;
            }

            float scaleY = _yController.Update(deltaTime);
            if (_transform != null)
            {
                _transform.localScale = new Vector3(scaleX, scaleY, 1f);
            }

            return _xController.Finished && _yController.Finished;
        }

        public static Scale Create(GameObject go, FloatController controller)
        {
            return Create(go?.transform, controller);
        }
        public static Scale Create(Component component, FloatController controller)
        {
            return Create(component?.transform, controller);
        }
        public static Scale Create(Transform transform, FloatController controller)
        {
            return Create(transform, controller, controller);
        }

        public static Scale Create(GameObject go, FloatController xController, FloatController yController)
        {
            return Create(go?.transform, xController, yController);
        }
        public static Scale Create(Component component, FloatController xController, FloatController yController)
        {
            return Create(component?.transform, xController, yController);
        }
        public static Scale Create(Transform transform, FloatController xController, FloatController yController)
        {
            var action = new Scale();
            action.Construct(transform, xController, yController);
            return action;
        }

        public static Scale CreateLerp(GameObject go, float start, float end, float duration, EaseType easeType)
        {
            return CreateLerp(go?.transform, start, end, duration, easeType);
        }
        public static Scale CreateLerp(Component component, float start, float end, float duration, EaseType easeType)
        {
            return CreateLerp(component?.transform, start, end, duration, easeType);
        }
        public static Scale CreateLerp(Transform transform, float start, float end, float duration, EaseType easeType)
        {
            return Create(transform, FloatController.CreateLerp(start, end, duration, easeType));
        }

        public static Scale CreateLerp(GameObject go, float start, float end, float duration, Easer easer = null)
        {
            return CreateLerp(go?.transform, start, end, duration, easer);
        }
        public static Scale CreateLerp(Component component, float start, float end, float duration, Easer easer = null)
        {
            return CreateLerp(component?.transform, start, end, duration, easer);
        }
        public static Scale CreateLerp(Transform transform, float start, float end, float duration, Easer easer = null)
        {
            return Create(transform, FloatController.CreateLerp(start, end, duration, easer));
        }

        public static Scale CreateLerp(GameObject go, float start, float end, float duration, AnimationCurve curve)
        {
            return CreateLerp(go?.transform, start, end, duration, curve);
        }
        public static Scale CreateLerp(Component component, float start, float end, float duration, AnimationCurve curve)
        {
            return CreateLerp(component?.transform, start, end, duration, curve);
        }
        public static Scale CreateLerp(Transform transform, float start, float end, float duration, AnimationCurve curve)
        {
            return Create(transform, FloatController.CreateLerp(start, end, duration, curve));
        }

        public static Scale CreateAnimationCurve(GameObject go, AnimationCurve curve, float duration = -1)
        {
            return CreateAnimationCurve(go?.transform, curve, duration);
        }
        public static Scale CreateAnimationCurve(Component component, AnimationCurve curve, float duration = -1)
        {
            return CreateAnimationCurve(component?.transform, curve, duration);
        }
        public static Scale CreateAnimationCurve(Transform transform, AnimationCurve curve, float duration = -1)
        {
            return Create(transform, FloatController.CreateAnimationCurve(curve, duration));
        }
    }
}
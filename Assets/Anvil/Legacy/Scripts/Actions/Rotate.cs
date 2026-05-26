using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Rotate : ActionX
    {
        const bool DefaultLocal = true;

        Transform _transform;
        FloatController _controller;
        bool _isLocal = DefaultLocal;

        public override float Duration => _controller.Duration;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _transform = value.transform;
            }
        }

        public void Construct(Transform transform, FloatController controller, bool local)
        {
            _transform = transform;
            _controller = controller;
            _isLocal = local;
            _target = transform?.gameObject;
            _Construct();
        }

        void SetAngle(float angle)
        {
            if (_isLocal)
            {
                var eulerAngles = _transform.localEulerAngles;
                eulerAngles.z = angle;
                _transform.localEulerAngles = eulerAngles;
            }
            else
            {
                var eulerAngles = _transform.eulerAngles;
                eulerAngles.z = angle;
                _transform.eulerAngles = eulerAngles;
            }
        }

        public override void Reset()
        {
            base.Reset();
            _controller.Reset();
        }

        protected override bool OnPlay()
        {
            SetAngle(_controller.Value);
            return _controller.Finished;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                if (_controller.GetEnd(out float end))
                {
                    SetAngle(end);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            SetAngle(_controller.Update(deltaTime));
            return _controller.Finished;
        }

        public static Rotate Create(GameObject go, FloatController controller, bool local = DefaultLocal)
        {
            return Create(go?.transform, controller, local);
        }
        public static Rotate Create(Component component, FloatController controller, bool local = DefaultLocal)
        {
            return Create(component?.transform, controller, local);
        }
        public static Rotate Create(Transform transform, FloatController controller, bool local = DefaultLocal)
        {
            var action = new Rotate();
            action.Construct(transform, controller, local);
            return action;
        }

        public static Rotate CreateLerp(GameObject go, float startAngle, float endAngle, float duration, bool local = DefaultLocal)
        {
            return CreateLerp(go?.transform, startAngle, endAngle, duration, local);
        }
        public static Rotate CreateLerp(Component component, float startAngle, float endAngle, float duration, bool local = DefaultLocal)
        {
            return CreateLerp(component?.transform, startAngle, endAngle, duration, local);
        }
        public static Rotate CreateLerp(Transform transform, float startAngle, float endAngle, float duration, bool local = DefaultLocal)
        {
            return Create(transform, FloatController.CreateLerp(startAngle, endAngle, duration), local);
        }

        public static Rotate CreateLerp(GameObject go, float startAngle, float endAngle, float duration, EaseType easeType, bool local = DefaultLocal)
        {
            return CreateLerp(go?.transform, startAngle, endAngle, duration, local);
        }
        public static Rotate CreateLerp(Component component, float startAngle, float endAngle, float duration, EaseType easeType, bool local = DefaultLocal)
        {
            return CreateLerp(component?.transform, startAngle, endAngle, duration, local);
        }
        public static Rotate CreateLerp(Transform transform, float startAngle, float endAngle, float duration, EaseType easeType, bool local = DefaultLocal)
        {
            return Create(transform, FloatController.CreateLerp(startAngle, endAngle, duration, easeType), local);
        }

        public static Rotate CreateLerp(GameObject go, float startAngle, float endAngle, float duration, Easer easer, bool local = DefaultLocal)
        {
            return CreateLerp(go?.transform, startAngle, endAngle, duration, easer, local);
        }
        public static Rotate CreateLerp(Component component, float startAngle, float endAngle, float duration, Easer easer, bool local = DefaultLocal)
        {
            return CreateLerp(component?.transform, startAngle, endAngle, duration, easer, local);
        }
        public static Rotate CreateLerp(Transform transform, float startAngle, float endAngle, float duration, Easer easer, bool local = DefaultLocal)
        {
            return Create(transform, FloatController.CreateLerp(startAngle, endAngle, duration, easer), local);
        }

        public static Rotate CreateLerp(GameObject go, float startAngle, float endAngle, float duration, AnimationCurve curve, bool local = DefaultLocal)
        {
            return CreateLerp(go?.transform, startAngle, endAngle, duration, curve, local);
        }
        public static Rotate CreateLerp(Component component, float startAngle, float endAngle, float duration, AnimationCurve curve, bool local = DefaultLocal)
        {
            return CreateLerp(component?.transform, startAngle, endAngle, duration, curve, local);
        }
        public static Rotate CreateLerp(Transform transform, float startAngle, float endAngle, float duration, AnimationCurve curve, bool local = DefaultLocal)
        {
            return Create(transform, FloatController.CreateLerp(startAngle, endAngle, duration, curve), local);
        }

        public static Rotate CreateAnimationCurve(GameObject go, AnimationCurve curve, float duration = -1, bool local = DefaultLocal)
        {
            return CreateAnimationCurve(go?.transform, curve, duration, local);
        }
        public static Rotate CreateAnimationCurve(Component component, AnimationCurve curve, float duration = -1, bool local = DefaultLocal)
        {
            return CreateAnimationCurve(component?.transform, curve, duration, local);
        }
        public static Rotate CreateAnimationCurve(Transform transform, AnimationCurve curve, float duration = -1, bool local = DefaultLocal)
        {
            return Create(transform, FloatController.CreateAnimationCurve(curve, duration), local);
        }
    }
}
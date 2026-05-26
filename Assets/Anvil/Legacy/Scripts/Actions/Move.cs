using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Move : ActionX
    {
        Transform _transform;
        Vector3Controller _controller;
        bool _isLocal;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _transform = value.transform;
            }
        }

        public override float Duration => _controller.Duration;

        public void Construct(Transform transform, Vector3Controller controller, bool local)
        {
            _transform = transform;
            _controller = controller;
            _isLocal = local;
            _target = transform?.gameObject;
            _Construct();
        }

        public void SetEnd(Vector3 end)
        {
            _controller.SetEnd(end);
        }

        public override void Reset()
        {
            base.Reset();
            _controller.Reset();
        }

        protected override bool OnPlay()
        {
            _transform.SetPosition(_controller.Value, _isLocal);
            return _controller.Finished;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                if (_controller.GetEnd(out Vector3 end))
                {
                    _transform.SetPosition(end, _isLocal);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            var pos = _controller.Update(deltaTime);
            _transform.SetPosition(pos, _isLocal);
            return _controller.Finished;
        }

        public static Move Create(GameObject go, Vector3Controller controller, bool local = false)
        {
            return Create(go?.transform, controller, local);
        }
        public static Move Create(Component component, Vector3Controller controller, bool local = false)
        {
            return Create(component?.transform, controller, local);
        }
        public static Move Create(Transform transform, Vector3Controller controller, bool local = false)
        {
            var action = new Move();
            action.Construct(transform, controller, local);
            return action;
        }

        #region Lerp
        // End
        public static Move CreateLerp(GameObject go, Vector3 endPos, float duration, bool local = false)
        {
            return CreateLerp(go?.transform, endPos, duration, local);
        }
        public static Move CreateLerp(Component component, Vector3 endPos, float duration, bool local = false)
        {
            return CreateLerp(component?.transform, endPos, duration, local);
        }
        public static Move CreateLerp(Transform transform, Vector3 endPos, float duration, bool local = false)
        {
            return CreateLerp(transform, transform.GetPosition(local), endPos, duration, local);
        }

        // Start + End
        public static Move CreateLerp(GameObject go, Vector3 startPos, Vector3 endPos, float duration, bool local = false)
        {
            return CreateLerp(go?.transform, startPos, endPos, duration, local);
        }
        public static Move CreateLerp(Component component, Vector3 startPos, Vector3 endPos, float duration, bool local = false)
        {
            return CreateLerp(component?.transform, startPos, endPos, duration, local);
        }
        public static Move CreateLerp(Transform transform, Vector3 startPos, Vector3 endPos, float duration, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateLerp(startPos, endPos, duration), local);
        }

        // End + Ease
        public static Move CreateLerp(GameObject go, Vector3 endPos, float duration, EaseType easeType, bool local = false)
        {
            return CreateLerp(go?.transform, endPos, duration, Easers.GetEaser(easeType), local);
        }
        public static Move CreateLerp(Component component, Vector3 endPos, float duration, EaseType easeType, bool local = false)
        {
            return CreateLerp(component?.transform, endPos, duration, Easers.GetEaser(easeType), local);
        }
        public static Move CreateLerp(Transform transform, Vector3 endPos, float duration, EaseType easeType, bool local = false)
        {
            return CreateLerp(transform, endPos, duration, Easers.GetEaser(easeType), local);
        }

        // Start + End + Ease
        public static Move CreateLerp(GameObject go, Vector3 startPos, Vector3 endPos, float duration, EaseType easeType, bool local = false)
        {
            return CreateLerp(go?.transform, startPos, endPos, duration, Easers.GetEaser(easeType), local);
        }
        public static Move CreateLerp(Component component, Vector3 startPos, Vector3 endPos, float duration, EaseType easeType, bool local = false)
        {
            return CreateLerp(component?.transform, startPos, endPos, duration, Easers.GetEaser(easeType), local);
        }
        public static Move CreateLerp(Transform transform, Vector3 startPos, Vector3 endPos, float duration, EaseType easeType, bool local = false)
        {
            return CreateLerp(transform, startPos, endPos, duration, Easers.GetEaser(easeType), local);
        }

        // End + Easer
        public static Move CreateLerp(GameObject go, Vector3 endPos, float duration, Easer easer, bool local = false)
        {
            return CreateLerp(go?.transform, endPos, duration, easer, local);
        }
        public static Move CreateLerp(Component component, Vector3 endPos, float duration, Easer easer, bool local = false)
        {
            return CreateLerp(component?.transform, endPos, duration, easer, local);
        }
        public static Move CreateLerp(Transform transform, Vector3 endPos, float duration, Easer easer, bool local = false)
        {
            return CreateLerp(transform, transform.GetPosition(local), endPos, duration, easer, local);
        }

        // Start + End + Easer
        public static Move CreateLerp(GameObject go, Vector3 startPos, Vector3 endPos, float duration, Easer easer, bool local = false)
        {
            return CreateLerp(go?.transform, startPos, endPos, duration, easer, local);
        }
        public static Move CreateLerp(Component component, Vector3 startPos, Vector3 endPos, float duration, Easer easer, bool local = false)
        {
            return CreateLerp(component?.transform, startPos, endPos, duration, easer, local);
        }
        public static Move CreateLerp(Transform transform, Vector3 startPos, Vector3 endPos, float duration, Easer easer, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateLerp(startPos, endPos, duration, easer), local);
        }

        // End + Curve
        public static Move CreateLerp(GameObject go, Vector3 endPos, float duration, AnimationCurve curve, bool local = false)
        {
            return CreateLerp(go?.transform, endPos, duration, curve, local);
        }
        public static Move CreateLerp(Component component, Vector3 endPos, float duration, AnimationCurve curve, bool local = false)
        {
            return CreateLerp(component?.transform, endPos, duration, curve, local);
        }
        public static Move CreateLerp(Transform transform, Vector3 endPos, float duration, AnimationCurve curve, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateLerp(transform.GetPosition(local), endPos, duration, curve), local);
        }

        // Start + End + Curve
        public static Move CreateLerp(GameObject go, Vector3 startPos, Vector3 endPos, float duration, AnimationCurve curve, bool local = false)
        {
            return CreateLerp(go?.transform, startPos, endPos, duration, curve, local);
        }
        public static Move CreateLerp(Component component, Vector3 startPos, Vector3 endPos, float duration, AnimationCurve curve, bool local = false)
        {
            return CreateLerp(component?.transform, startPos, endPos, duration, curve, local);
        }
        public static Move CreateLerp(Transform transform, Vector3 startPos, Vector3 endPos, float duration, AnimationCurve curve, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateLerp(startPos, endPos, duration, curve), local);
        }
        #endregion

        #region Bezier
        // Control + End
        public static Move CreateQuadBezier(GameObject go, Vector3 controlPos, Vector3 endPos, float duration, bool local = false)
        {
            return CreateQuadBezier(go?.transform, controlPos, endPos, duration, local);
        }
        public static Move CreateQuadBezier(Component component, Vector3 controlPos, Vector3 endPos, float duration, bool local = false)
        {
            return CreateQuadBezier(component?.transform, controlPos, endPos, duration, local);
        }
        public static Move CreateQuadBezier(Transform transform, Vector3 controlPos, Vector3 endPos, float duration, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateQuadBezier(transform.GetPosition(local), controlPos, endPos, duration), local);
        }

        // Start + Control + End
        public static Move CreateQuadBezier(GameObject go, Vector3 startPos, Vector3 controlPos, Vector3 endPos, float duration, bool local = false)
        {
            return CreateQuadBezier(go?.transform, startPos, controlPos, endPos, duration, local);
        }
        public static Move CreateQuadBezier(Component component, Vector3 startPos, Vector3 controlPos, Vector3 endPos, float duration, bool local = false)
        {
            return CreateQuadBezier(component?.transform, startPos, controlPos, endPos, duration, local);
        }
        public static Move CreateQuadBezier(Transform transform, Vector3 startPos, Vector3 controlPos, Vector3 endPos, float duration, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateQuadBezier(startPos, controlPos, endPos, duration), local);
        }
        #endregion

        #region AnimationCurve
        // End + Curve
        public static Move CreateAnimationCurve(GameObject go, Vector3 endPos, AnimationCurve curve, float duration, bool local = false)
        {
            return CreateAnimationCurve(go?.transform, endPos, curve, duration, local);
        }
        public static Move CreateAnimationCurve(Component component, Vector3 endPos, AnimationCurve curve, float duration, bool local = false)
        {
            return CreateAnimationCurve(component?.transform, endPos, curve, duration, local);
        }
        public static Move CreateAnimationCurve(Transform transform, Vector3 endPos, AnimationCurve curve, float duration, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateAnimationCurve(transform.GetPosition(local), endPos, curve, duration), local);
        }

        // Start + End + Curve
        public static Move CreateAnimationCurve(GameObject go, Vector3 startPos, Vector3 endPos, AnimationCurve curve, float duration, bool local = false)
        {
            return CreateAnimationCurve(go?.transform, startPos, endPos, curve, duration, local);
        }
        public static Move CreateAnimationCurve(Component component, Vector3 startPos, Vector3 endPos, AnimationCurve curve, float duration, bool local = false)
        {
            return CreateAnimationCurve(component?.transform, startPos, endPos, curve, duration, local);
        }
        public static Move CreateAnimationCurve(Transform transform, Vector3 startPos, Vector3 endPos, AnimationCurve curve, float duration, bool local = false)
        {
            return Create(transform, Vector3Controller.CreateAnimationCurve(startPos, endPos, curve, duration), local);
        }
        #endregion
    }
}
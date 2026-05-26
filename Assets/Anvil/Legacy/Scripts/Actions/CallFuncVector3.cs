using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class CallFuncVector3 : ActionX
    {
        Vector3Controller _controller;
        Callback<Vector3> _callback;

        public override float Duration => _controller.Duration;

        public void Construct(Vector3Controller controller, Callback<Vector3> callback)
        {
            _controller = controller;
            _callback = callback;
            _Construct();
        }

        protected override bool OnPlay()
        {
            _callback?.Invoke(_controller.Value);
            return _controller.Finished;
        }

        public override void Reset()
        {
            base.Reset();
            _controller.Reset();
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                if (_controller.GetEnd(out Vector3 end))
                {
                    _callback?.Invoke(end);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            var value = _controller.Update(deltaTime);
            _callback?.Invoke(value);
            return _controller.Finished;
        }

        public static CallFuncVector3 Create(Vector3Controller controller, Callback<Vector3> callback)
        {
            var action = new CallFuncVector3();
            action.Construct(controller, callback);
            return action;
        }
    }
}
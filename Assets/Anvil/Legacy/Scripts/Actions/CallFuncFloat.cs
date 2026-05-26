using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class CallFuncFloat : ActionX
    {
        FloatController _controller;
        Callback<float> _callback;

        public override float Duration => _controller.Duration;

        public void Construct(FloatController controller, Callback<float> callback)
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
                if (_controller.GetEnd(out float end))
                {
                    _callback?.Invoke(end);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            float value = _controller.Update(deltaTime);
            _callback?.Invoke(value);
            return _controller.Finished;
        }

        public static CallFuncFloat Create(FloatController controller, Callback<float> callback)
        {
            var action = new CallFuncFloat();
            action.Construct(controller, callback);
            return action;
        }
    }
}
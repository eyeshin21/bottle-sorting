using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class DelayCallFunc : ActionX
    {
        float _delay;
        Callback _callback;
        float _time;

        public override float Duration => _delay;

        public void Construct(float delay, Callback callback)
        {
            _delay = delay;
            _callback = callback;
            _Construct();
        }

        protected override bool OnPlay()
        {
            _time = 0;
            if (_time >= _delay)
            {
                _callback?.Invoke();
                return true;
            }
            return false;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                _time = _delay;
                _callback?.Invoke();
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time >= _delay)
            {
                _callback?.Invoke();
                return true;
            }

            return false;
        }

        public static DelayCallFunc Create(float delay, Callback callback)
        {
            var action = new DelayCallFunc();
            action.Construct(delay, callback);
            return action;
        }
    }
}
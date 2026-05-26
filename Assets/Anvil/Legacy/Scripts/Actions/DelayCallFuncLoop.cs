using UnityEngine;
using System;

namespace Anvil.Legacy.Actions
{
    public class DelayCallFuncLoop : ActionX
    {
        private float _delay;
        private Action _callback;

        private float _time;

        public override float Duration => -1;

        public void Construct(float delay, Action callback)
        {
            _Construct();

            _delay = delay;
            _callback = callback;
        }

        protected override bool OnPlay()
        {
            _time = 0;
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
                _time -= _delay;
                _callback?.Invoke();
            }

            return false;
        }

        public static DelayCallFuncLoop Create(float delay, Action callback)
        {
            var action = new DelayCallFuncLoop();
            action.Construct(delay, callback);
            return action;
        }
    }
}
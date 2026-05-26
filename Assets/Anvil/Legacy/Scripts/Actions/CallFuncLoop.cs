using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class CallFuncLoop : ActionX
    {
        Callback _callback;

        public override float Duration => -1;

        public void Construct(Callback callback)
        {
            _callback = callback;
            _Construct();
        }

        protected override bool OnPlay()
        {
            if (_callback != null)
            {
                _callback();
                return false;
            }
            return true;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                _callback?.Invoke();
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            if (_callback != null)
            {
                _callback();
                return false;
            }
            return true;
        }

        public static CallFuncLoop Create(Callback callback)
        {
            var action = new CallFuncLoop();
            action.Construct(callback);
            return action;
        }
    }
}
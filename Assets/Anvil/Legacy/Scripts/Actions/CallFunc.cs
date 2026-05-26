using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class CallFunc : ActionX
    {
        Callback _callback;

        public void Construct(Callback callback)
        {
            _callback = callback;
            _Construct();
        }

        protected override bool OnPlay()
        {
            _callback?.Invoke();
            return true;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                _callback?.Invoke();
            }
        }

        public static CallFunc Create(Callback callback)
        {
            var action = new CallFunc();
            action.Construct(callback);
            return action;
        }
    }
}
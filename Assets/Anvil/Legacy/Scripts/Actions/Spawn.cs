using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Spawn : ActionX
    {
        ActionX _action;
        Callback _callback;

        public override float Duration => _action.Duration;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _action.Target = value;
            }
        }

        public void Construct(ActionX action, Callback callback)
        {
            _action = action;
            _callback = callback;
            _Construct();
        }

        public override void Play()
        {
            if (_isFinished) return;

            _action?.Play();
            _callback?.Invoke();

            if (_action == null || _action.Finished)
            {
                _isFinished = true;
            }
        }

        public override void Reset()
        {
            base.Reset();
            _action?.Reset();
            _callback?.Invoke();
        }

        public override void Stop(bool forceEnd)
        {
            if (_isFinished) return;

            _action?.Stop(forceEnd);
            if (forceEnd)
            {
                _callback?.Invoke();
            }
            _isFinished = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_isFinished) return true;

            if (_action != null)
            {
                _action.Update(deltaTime);
                if (_action.Finished)
                {
                    _isFinished = true;
                }
            }
            else
            {
                _isFinished = true;
            }
            _callback?.Invoke();

            return _isFinished;
        }

        public static Spawn Create(ActionX action, Callback callback)
        {
            var action2 = new Spawn();
            action2.Construct(action, callback);
            return action2;
        }
    }
}
using System;

namespace Anvil
{
    public class ScriptedTimeDelayEvent : ScriptedEvent
    {
        private float _delay;
        private float _timer;
        private bool _running;

        public ScriptedTimeDelayEvent(ScriptedEvent scriptedEvent, float delay, Action callback = null)
        {
            _delay = delay;
            _primaryAction = scriptedEvent.Execute;
            _callback = callback;
        }
        public ScriptedTimeDelayEvent(Action<Action> primaryAction, float delay, Action callback = null) : base(primaryAction, callback)
        {
            _delay = delay;
        }

        public ScriptedTimeDelayEvent()
        {
            _delay = 0;
            _timer = 0;
            _running = false;
        }
        public override void Execute(bool includeCallback = true)
        {
            if (!_running)
            {
                _running = true;
                _timer = 0;
                MonoBehaviourMessageForwarder.RegisterCommonUpdate(OnUpdate);
            }
            // if (_timer < _delay)
            // {
            //     return;
            // }
            // base.Execute(includeCallback);

        }
        private void ExecuteImidiate()
        {

            if (_primaryAction == null)
            {
                OnComplete();
                return;
            }

            Action callback = OnComplete;

            _primaryAction(callback);
            return;
        }

        private void OnUpdate()
        {
            if (!_running)
            {
                return;
            }
            _timer += UnityEngine.Time.deltaTime;
            if (_timer >= _delay)
            {
                MonoBehaviourMessageForwarder.UnRegisterCommonClockUpdate(OnUpdate);
                _running = false;
                ExecuteImidiate();
            }
        }
    }
}

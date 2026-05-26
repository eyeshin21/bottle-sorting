using System;
using UnityEditor;

namespace Anvil
{
    public class ScriptedFrameDelayEvent : ScriptedEvent
    {
        private int _delayFrame;
        private float _frameCount;
        private bool _running;

        public ScriptedFrameDelayEvent(int delayFrame)
        {
            _delayFrame = delayFrame;
        }
        public ScriptedFrameDelayEvent(ScriptedEvent scriptedEvent, int delayFrameFrame, Action callback = null)
        {
            _delayFrame = delayFrameFrame;
            _primaryAction = scriptedEvent.Execute;
            _callback = callback;
        }
        public ScriptedFrameDelayEvent(Action<Action> primaryAction, int delayFrameFrame, Action callback = null) : base(primaryAction, callback)
        {
            _delayFrame = delayFrameFrame;
        }


        public ScriptedFrameDelayEvent()
        {
            _delayFrame = 0;
            _frameCount = 0;
            _running = false;
        }
        public override void Execute(bool includeCallback = true)
        {
            if (!_running)
            {
                _running = true;
                _frameCount = 0;
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
            _frameCount += 1;
            if (_frameCount >= _delayFrame)
            {
                MonoBehaviourMessageForwarder.UnRegisterCommonClockUpdate(OnUpdate);
                _running = false;
                ExecuteImidiate();
            }
        }
    }
}

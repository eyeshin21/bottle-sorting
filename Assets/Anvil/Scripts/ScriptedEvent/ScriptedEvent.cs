using System;
using UnityEditor;
using UnityEngine;

namespace Anvil
{
    // a system.action wraper.
    public interface IScriptedEvent
    {
        void Execute(Action callback = null);
        // void OnComplete();
    }

    public class ScriptedEvent : IScriptedEvent
    {
        protected Action<Action> _primaryAction;
        protected Action _callback;

        public ScriptedEvent()
        {

        }

        public ScriptedEvent(Action<Action> primaryAction, Action callback = null)
        {
            SetPrimaryAction(primaryAction);
            _callback = callback;
        }
        public virtual void Execute(Action callback)
        {
            _callback = callback;
            Execute(true);
        }
        public virtual void Execute(bool includeCallback = true)
        {
            if (_primaryAction == null)
            {
                OnComplete();
                return;
            }

            Action callback = OnComplete;
            if (!includeCallback)
            {
                callback = null;
            }

            _primaryAction(callback);
            return;
        }

        public virtual void OnComplete()
        {
            Action callback = _callback;
            _callback = null;
            callback?.Invoke();

            // _callback?.Invoke();
        }

        public void SetPrimaryAction(Action<Action> action)
        {
            _primaryAction = action;
        }

        public void SetPrimaryAction(Action action)
        {
            _primaryAction = (callback) =>
            {
                action();
                // OnComplete();
            };
        }
    }
    public class ScriptedEventComponent : MonoBehaviour , IScriptedEvent
    {
        public virtual void Execute(Action callback = null)
        {
            
        }
    }
}

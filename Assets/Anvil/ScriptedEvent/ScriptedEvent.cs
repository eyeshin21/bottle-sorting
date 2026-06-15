using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Anvil
{
    // a system.action wraper.
    public interface IScriptedEvent
    {
        public bool IsCompleted { get; }
        void Execute(Action callback = null);
        // void OnComplete();
    }
    public interface ScriptedRoutine
    {
        IEnumerator ExecuteCoroutine(Action callback = null);
    }

    public class ScriptedEvent : IScriptedEvent
    {
        public virtual bool IsCompleted { get; private set; } = false;

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
        public void InsertCallback(Action callback)
        {
            _callback += callback;
        }


        public virtual void Execute(Action callback)
        {
            // _callback = callback;
            InsertCallback(callback);
            Execute(true);
        }

        public virtual IEnumerator ExecuteCoroutine(Action callback = null)
        {
            Execute(callback);
            while (!IsCompleted)
            {
                yield return null;
            }
        }
        public virtual void Execute(bool includeCallback = true)
        {
            IsCompleted = false;
            
            if (_primaryAction == null)
            {
                IsCompleted = true;
                OnComplete();
                return;
            }

            Action callback = null;
            if (includeCallback)
            {
                callback = OnComplete;
            }

            _primaryAction(() =>
            {
                IsCompleted = true;
                callback?.Invoke();
            });
            return;
        }

        protected virtual void OnComplete()
        {
            Action callback = _callback;
            _callback = null;
            callback?.Invoke();
        }
        
        public void ForceComplete()
        {
            OnComplete();
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
    // public class ScriptedEventComponent : MonoBehaviour , IScriptedEvent
    // {
    //     public virtual void Execute(Action callback = null)
    //     {
    //         
    //     }
    // }
}

#if UNITY_EDITOR || DEBUG_MODE
#define DEBUG_ANIMATION_EVENT_LISTENER
#endif
using UnityEngine;
#if DEBUG_ANIMATION_EVENT_LISTENER
using System.Collections.Generic;
#endif

namespace Anvil.Legacy
{
    public class AnimationEventHandler : MonoBehaviour
    {
        event Listener<string> _onTrigger;
#if DEBUG_ANIMATION_EVENT_LISTENER
        List<Listener<string>> _listeners = new();
#endif

        public void AddListener(Listener<string> listener)
        {
            if (listener != null)
            {
                _onTrigger += listener;
#if DEBUG_ANIMATION_EVENT_LISTENER
                if (_listeners.Contains(listener))
                {
                    LegacyLog.Warning($"Listener already added!");
                }
                else
                {
                    _listeners.Add(listener);
                }
#endif
            }
        }

        public void RemoveListener(Listener<string> listener)
        {
            if (listener != null)
            {
                _onTrigger -= listener;
#if DEBUG_ANIMATION_EVENT_LISTENER
                if (_listeners.Contains(listener))
                {
                    _listeners.Remove(listener);
                }
                else
                {
                    LegacyLog.Warning($"Listener not added!");
                }
#endif
            }
        }

        public void ClearListeners()
        {
            _onTrigger = null;
#if DEBUG_ANIMATION_EVENT_LISTENER
            _listeners.Clear();
#endif
        }

        public void OnTrigger(string name)
        {
            _onTrigger?.Invoke(name);
        }
    }
}
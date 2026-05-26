using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

namespace Anvil.Legacy
{
    public static class ListenerChecker
    {
        static Dictionary<object, List<Listener>> _objectListeners;
        static Dictionary<object, List<Listener>> ObjectListeners => _objectListeners ??= new();

        [Conditional("DEBUG_MODE")]
        public static void CheckAddListener(object obj, Listener listener)
        {
            //Log.Debug($"CheckAddListener: obj={obj}, listener={listener}");
            var dict = ObjectListeners;
            if (dict.TryGetValue(obj, out List<Listener> listeners))
            {
                if (listeners.Contains(listener))
                {
                    LegacyLog.Warning($"[{obj}]: Listener {listener} already added!");
                }
                listeners.Add(listener);
            }
            else
            {
                listeners = new List<Listener>();
                listeners.Add(listener);
                dict.Add(obj, listeners);
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void CheckRemoveListener(object obj, Listener listener)
        {
            //Log.Debug($"CheckRemoveListener: obj={obj}, listener={listener}");
            var dict = ObjectListeners;
            if (dict.TryGetValue(obj, out List<Listener> listeners))
            {
                if (listeners.Contains(listener))
                {
                    listeners.Remove(listener);
                }
                else
                {
                    LegacyLog.Warning($"[{obj}]: Listener {listener} not added!");
                }
            }
            else
            {
                LegacyLog.Warning($"[{obj}]: Listener {listener} not added!");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void RemoveObject(object obj)
        {
            //Log.Debug($"RemoveObject: obj={obj}");
            if (_objectListeners != null)
            {
                _objectListeners.Remove(obj);
            }
        }
    }
}
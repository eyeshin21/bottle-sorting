#if DEBUG_MODE
//#define LOG_ADD_LISTENER
//#define LOG_REMOVE_LISTENER
//#define LOG_BROADCAST
#define CHECK_ADD_REMOVE_LISTENER
#define CHECK_ADD_LISTENER
#define CHECK_REMOVE_LISTENER
#define CHECK_BROADCAST
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anvil.Legacy
{
    public class EventHandler<E> where E : struct
    {
        Delegate[] _events = new Delegate[Helper.GetEnumCount<E>()];
#if CHECK_ADD_REMOVE_LISTENER
        Dictionary<E, List<Delegate>> _checkEvents = new Dictionary<E, List<Delegate>>();
#endif
        #region AddListener
        public void AddListener(E eventID, Action listener)
        {
            AddListener(eventID, listener, d => { return (Action)d + listener; });
        }

        public void AddListener<T>(E eventID, Action<T> listener)
        {
            AddListener(eventID, listener, d => { return (Action<T>)d + listener; });
        }

        public void AddListener<T, U>(E eventID, Action<T, U> listener)
        {
            AddListener(eventID, listener, d => { return (Action<T, U>)d + listener; });
        }

        public void AddListener<T, U, V>(E eventID, Action<T, U, V> listener)
        {
            AddListener(eventID, listener, d => { return (Action<T, U, V>)d + listener; });
        }

        public void InsertFirstListener(E eventID, Action listener)
        {
            AddListener(eventID, listener, d => { return listener + (Action)d; });
        }

        public void InsertFirstListener<T>(E eventID, Action<T> listener)
        {
            AddListener(eventID, listener, d => { return listener + (Action<T>)d; });
        }

        public void InsertFirstListener<T, U>(E eventID, Action<T, U> listener)
        {
            AddListener(eventID, listener, d => { return listener + (Action<T, U>)d; });
        }

        public void InsertFirstListener<T, U, V>(E eventID, Action<T, U, V> listener)
        {
            AddListener(eventID, listener, d => { return listener + (Action<T, U, V>)d; });
        }

        public void AddListener(E eventID, Delegate listener, Func<Delegate, Delegate> func)
        {
#if LOG_ADD_LISTENER
           LegacyLog.Debug($"Add \"{eventID}\": {listener.Target}->{listener.Method}");
#endif

#if CHECK_ADD_REMOVE_LISTENER
            if (_checkEvents.ContainsKey(eventID))
            {
                var listeners = _checkEvents[eventID];
                Assert.NotContains(listeners, listener, $"[{eventID}] Listener added: {listener.Target}->{listener.Method}");
                listeners.Add(listener);
            }
            else
            {
                var listeners = new List<Delegate>();
                listeners.Add(listener);
                _checkEvents.Add(eventID, listeners);
            }
#endif
            int index = Convert.ToInt32(eventID);
            Delegate d = _events[index];
            if (d != null)
            {
#if CHECK_ADD_LISTENER
                var type1 = d.GetType();
                var type2 = listener.GetType();
                Assert.IsTrue(type1 == type2, $"Invalid type: {type1.Name} vs {type2.Name}");
#endif
                _events[index] = func(d);
            }
            else
            {
                _events[index] = listener;
            }
        }
        #endregion

        #region RemoveListener
        public void RemoveListener(E eventID, Action listener)
        {
            RemoveListener(eventID, listener, d => { return (Action)d - listener; });
        }

        public void RemoveListener<T>(E eventID, Action<T> listener)
        {
            RemoveListener(eventID, listener, d => { return (Action<T>)d - listener; });
        }

        public void RemoveListener<T, U>(E eventID, Action<T, U> listener)
        {
            RemoveListener(eventID, listener, d => { return (Action<T, U>)d - listener; });
        }

        public void RemoveListener<T, U, V>(E eventID, Action<T, U, V> listener)
        {
            RemoveListener(eventID, listener, d => { return (Action<T, U, V>)d - listener; });
        }

        public void RemoveListener(E eventID, Delegate listener, Func<Delegate, Delegate> func)
        {
#if LOG_REMOVE_LISTENER
           LegacyLog.Debug($"Remove \"{eventID}\": {listener.Target}->{listener.Method}");
#endif

#if CHECK_ADD_REMOVE_LISTENER
            if (_checkEvents.ContainsKey(eventID))
            {
                var listeners = _checkEvents[eventID];
                //Assert.Contains(listeners, listener, $"[{eventID}] Listener not added: {listener.Target}->{listener.Method}");
                if (listeners.Remove(listener) && listeners.Count == 0)
                {
                    _checkEvents.Remove(eventID);
                }
            }
#endif
            int index = Convert.ToInt32(eventID);
            Delegate d = _events[index];
            if (d != null)
            {
#if CHECK_REMOVE_LISTENER
                var type1 = d.GetType();
                var type2 = listener.GetType();
                Assert.IsTrue(type1 == type2, $"Invalid type: {type1.Name} vs {type2.Name}");
#endif
                _events[index] = func(d);
            }
        }
        #endregion

        #region Broadcast
        public void Broadcast(E eventID)
        {
#if LOG_BROADCAST
           LegacyLog.Debug($"Broadcast \"{eventID}\"");
#endif
            Broadcast(eventID, d =>
            {
                Action callback = d as Action;
#if CHECK_BROADCAST
                Assert.IsNotNull(callback, $"Invalid event \"{eventID}\"");
#endif
                callback();
            });
        }

        public void Broadcast<T>(E eventID, T arg)
        {
#if LOG_BROADCAST
           LegacyLog.Debug($"Broadcast \"{eventID}\": arg={arg}");
#endif
            Broadcast(eventID, d =>
            {
                Action<T> callback = d as Action<T>;
#if CHECK_BROADCAST
                Assert.IsNotNull(callback, $"Invalid event \"{eventID}\": arg={arg}");
#endif
                callback(arg);
            });
        }

        public void Broadcast<T, U>(E eventID, T arg1, U arg2)
        {
#if LOG_BROADCAST
           LegacyLog.Debug($"Broadcast \"{eventID}\": arg1={arg1}, arg2={arg2}");
#endif
            Broadcast(eventID, d =>
            {
                Action<T, U> callback = d as Action<T, U>;
#if CHECK_BROADCAST
                Assert.IsNotNull(callback, $"Invalid event \"{eventID}\": arg1={arg1}, arg2={arg2}");
#endif
                callback(arg1, arg2);
            });
        }

        public void Broadcast<T, U, V>(E eventID, T arg1, U arg2, V arg3)
        {
#if LOG_BROADCAST
           LegacyLog.Debug($"Broadcast \"{eventID}\": arg1={arg1}, arg2={arg2}, arg3={arg3}");
#endif
            Broadcast(eventID, d =>
            {
                Action<T, U, V> callback = d as Action<T, U, V>;
#if CHECK_BROADCAST
                Assert.IsNotNull(callback, $"Invalid event \"{eventID}\": arg1={arg1}, arg2={arg2}, arg3={arg3}");
#endif
                callback(arg1, arg2, arg3);
            });
        }

        void Broadcast(E eventID, Action<Delegate> callback)
        {
            int index = Convert.ToInt32(eventID);
            Delegate d = _events[index];
            if (d != null)
            {
                callback(d);
            }
        }
        #endregion

        public bool Empty
        {
            get
            {
                int count = _events.Length;
                for (int i = 0; i < count; i++)
                {
                    if (_events[i] != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void Clear()
        {
            //Log.Debug("Clear events");
            int count = _events.Length;
            for (int i = 0; i < count; i++)
            {
                _events[i] = null;
            }
#if CHECK_ADD_REMOVE_LISTENER
            _checkEvents.Clear();
#endif
        }

#if DEBUG_MODE
        public void LogListeners()
        {
#if CHECK_ADD_REMOVE_LISTENER
            foreach (var entry in _checkEvents)
            {
                var eventID = entry.Key;
                var listeners = entry.Value;
                var s = $"<b><{eventID}></b> ({listeners.Count}):";
                foreach (var listener in entry.Value)
                {
                    s = $"{s}\n{Helper.GetClassName(listener.Target)}->{listener.Method}";
                }
               LegacyLog.Debug(s);
            }
#endif
        }
#endif
    }
}
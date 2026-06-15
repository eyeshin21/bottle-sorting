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

namespace Anvil
{
    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, U>(T arg1, U arg2);
    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

    public class EventManager<E> where E : struct
    {
        private Delegate[] _events = new Delegate[Enum.GetNames(typeof(E)).Length];
#if CHECK_ADD_REMOVE_LISTENER
        private Dictionary<E, List<Delegate>> _checkEvents = new Dictionary<E, List<Delegate>>();
#endif
        #region AddListener
        public void AddListener(E eventID, Callback listener)
        {
            AddListener(eventID, listener, d => { return (Callback)d + listener; });
        }

        public void AddListener<T>(E eventID, Callback<T> listener)
        {
            AddListener(eventID, listener, d => { return (Callback<T>)d + listener; });
        }

        public void AddListener<T, U>(E eventID, Callback<T, U> listener)
        {
            AddListener(eventID, listener, d => { return (Callback<T, U>)d + listener; });
        }

        public void AddListener<T, U, V>(E eventID, Callback<T, U, V> listener)
        {
            AddListener(eventID, listener, d => { return (Callback<T, U, V>)d + listener; });
        }

        public void InsertFirstListener(E eventID, Callback listener)
        {
            AddListener(eventID, listener, d => { return listener + (Callback)d; });
        }

        public void InsertFirstListener<T>(E eventID, Callback<T> listener)
        {
            AddListener(eventID, listener, d => { return listener + (Callback<T>)d; });
        }

        public void InsertFirstListener<T, U>(E eventID, Callback<T, U> listener)
        {
            AddListener(eventID, listener, d => { return listener + (Callback<T, U>)d; });
        }

        public void InsertFirstListener<T, U, V>(E eventID, Callback<T, U, V> listener)
        {
            AddListener(eventID, listener, d => { return listener + (Callback<T, U, V>)d; });
        }

        public void AddListener(E eventID, Delegate listener, Func<Delegate, Delegate> func)
        {
#if LOG_ADD_LISTENER
            Log.Debug($"Add \"{eventID}\": {listener.Target}->{listener.Method}");
#endif

#if CHECK_ADD_REMOVE_LISTENER
            if (_checkEvents.ContainsKey(eventID))
            {
                var listeners = _checkEvents[eventID];
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
        public void RemoveListener(E eventID, Callback listener)
        {
            RemoveListener(eventID, listener, d => { return (Callback)d - listener; });
        }

        public void RemoveListener<T>(E eventID, Callback<T> listener)
        {
            RemoveListener(eventID, listener, d => { return (Callback<T>)d - listener; });
        }

        public void RemoveListener<T, U>(E eventID, Callback<T, U> listener)
        {
            RemoveListener(eventID, listener, d => { return (Callback<T, U>)d - listener; });
        }

        public void RemoveListener<T, U, V>(E eventID, Callback<T, U, V> listener)
        {
            RemoveListener(eventID, listener, d => { return (Callback<T, U, V>)d - listener; });
        }

        public void RemoveListener(E eventID, Delegate listener, Func<Delegate, Delegate> func)
        {
#if LOG_REMOVE_LISTENER
            Log.Debug($"Remove \"{eventID}\": {listener.Target}->{listener.Method}");
#endif

#if CHECK_ADD_REMOVE_LISTENER
            if (_checkEvents.ContainsKey(eventID))
            {
                var listeners = _checkEvents[eventID];
                //Assert.Contains(listeners, listener, $"{listener.Target}->{listener.Method}");
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
#endif
                _events[index] = func(d);
            }
        }
        #endregion

        #region Broadcast
        public void Broadcast(E eventID)
        {
#if LOG_BROADCAST
            Log.Debug($"Broadcast \"{eventID}\"");
#endif
            Broadcast(eventID, d =>
            {
                Callback callback = d as Callback;
#if CHECK_BROADCAST
#endif
                callback();
            });
        }

        public void Broadcast<T>(E eventID, T arg)
        {
#if LOG_BROADCAST
            Log.Debug($"Broadcast \"{eventID}\": arg={arg}");
#endif
            Broadcast(eventID, d =>
            {
                Callback<T> callback = d as Callback<T>;
#if CHECK_BROADCAST
#endif
                callback(arg);
            });
        }

        public void Broadcast<T, U>(E eventID, T arg1, U arg2)
        {
#if LOG_BROADCAST
            Log.Debug($"Broadcast \"{eventID}\": arg1={arg1}, arg2={arg2}");
#endif
            Broadcast(eventID, d =>
            {
                Callback<T, U> callback = d as Callback<T, U>;
#if CHECK_BROADCAST
#endif
                callback(arg1, arg2);
            });
        }

        public void Broadcast<T, U, V>(E eventID, T arg1, U arg2, V arg3)
        {
#if LOG_BROADCAST
            Log.Debug($"Broadcast \"{eventID}\": arg1={arg1}, arg2={arg2}, arg3={arg3}");
#endif
            Broadcast(eventID, d =>
            {
                Callback<T, U, V> callback = d as Callback<T, U, V>;
#if CHECK_BROADCAST
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
    }
}
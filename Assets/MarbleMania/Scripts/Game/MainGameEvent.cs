using System;
using Anvil;

namespace MarbleMania
{
    public enum MainGameEventID
    {
        TrayFillComplete
    }

    public static partial class MainGameEventExtension
    {
        static EventManager<MainGameEventID> _maingameEventManager = new EventManager<MainGameEventID>();

        public static void ClearAll()
        {
            _maingameEventManager.Clear();
        }
        public static void AddEventListener(this MainGameEventID eventID, Callback listener)
        {
            _maingameEventManager.AddListener(eventID, listener);
        }

        public static void AddEventListener<T>(this MainGameEventID eventID, Callback<T> listener)
        {
            _maingameEventManager.AddListener(eventID, listener);
        }
        public static void AddEventListener<T1, T2>(this MainGameEventID eventID, Callback<T1, T2> listener)
        {
            _maingameEventManager.AddListener(eventID, listener);
        }
        public static void AddEventListener<T1, T2, T3>(this MainGameEventID eventID, Callback<T1, T2, T3> listener)
        {
            _maingameEventManager.AddListener(eventID, listener);
        }

        public static void BroadcastEvent(this MainGameEventID eventID)
        {
            _maingameEventManager.Broadcast(eventID);
        }

        public static void BroadcastEvent<T>(this MainGameEventID eventID, T value)
        {
            _maingameEventManager.Broadcast(eventID, value);
        }

        public static void BroadcastEvent<T1, T2>(this MainGameEventID eventID, T1 value1, T2 value2)
        {
            _maingameEventManager.Broadcast(eventID, value1, value2);
        }
        public static void BroadcastEvent<T1, T2, T3>(this MainGameEventID eventID, T1 value1, T2 value2, T3 value3)
        {
            _maingameEventManager.Broadcast(eventID, value1, value2, value3);
        }
        
        public static void RemoveEventListener(this MainGameEventID eventID, Callback callback)
        {
            _maingameEventManager.RemoveListener(eventID, callback);
        }

        public static void RemoveEventListener<T>(this MainGameEventID eventID, Callback<T> callback)
        {
            _maingameEventManager.RemoveListener(eventID, callback);
        }
        public static void RemoveEventListener<T1, T2>(this MainGameEventID eventID, Callback<T1, T2> callback)
        {
            _maingameEventManager.RemoveListener(eventID, callback);
        }
        public static void RemoveEventListener<T1, T2, T3>(this MainGameEventID eventID, Callback<T1, T2, T3> callback)
        {
            _maingameEventManager.RemoveListener(eventID, callback);
        }
    }
}
using System;

namespace MarbleMania.Scripts.Game
{
    public enum MainGameEventType
    {
        None,
        TrayFillComplete,
    }
    public static class MainGameEvent
    {
        private static Anvil.Legacy.EventHandler<MainGameEventType> _eventHandler = new Anvil.Legacy.EventHandler<MainGameEventType>();

        public static void AddListener(this MainGameEventType eventType, Action callback)
        {
            _eventHandler.AddListener(eventType, callback);
        }
        public static void AddListener<T>(this MainGameEventType eventType, Action<T> callback)
        {
            _eventHandler.AddListener(eventType, callback);
        }

        public static void RemoveListener(this MainGameEventType eventType, Action callback)
        {
            _eventHandler.RemoveListener(eventType, callback);
        }
        public static void Broadcast(this MainGameEventType eventType)
        {
            _eventHandler.Broadcast(eventType);
        }
        public static void Broadcast<T>(this MainGameEventType eventType, T arg1)
        {
            _eventHandler.Broadcast(eventType, arg1);
        }
    }
}
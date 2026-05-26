using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class AppsFlyerHelper
    {
        class PendingEvent
        {
            public string eventName;
            public Dictionary<string, string> eventValues;

            public PendingEvent(string eventName, Dictionary<string, string> eventValues)
            {
                this.eventName = eventName;
                this.eventValues = eventValues;
            }
        }
        static Dictionary<string, string> _eventValues = new();
        static List<PendingEvent> _pendingEvents = new();
        static void LogPendingEvents()
        {
            for (int i = 0, count = _pendingEvents.Count; i < count; i++)
            {
                var eventData = _pendingEvents[i];
                LogEvent(eventData.eventName, eventData.eventValues);
            }
            _pendingEvents.Clear();
        }
        public static void LogEvent(string eventName)
        {
            _eventValues.Clear();
            LogEvent(eventName, _eventValues);
        }

        public static void LogEvent(string eventName, string paramName, int paramValue)
        {
            _eventValues.Clear();
            _eventValues.Add(paramName, paramValue.ToString());
            LogEvent(eventName, _eventValues);
        }

        public static void LogEvent(string eventName, string paramName, float paramValue)
        {
            _eventValues.Clear();
            _eventValues.Add(paramName, paramValue.ToStringInvariantCulture());
            LogEvent(eventName, _eventValues);
        }

        public static void LogEvent(string eventName, string paramName, string paramValue)
        {
            _eventValues.Clear();
            _eventValues.Add(paramName, paramValue);
            LogEvent(eventName, _eventValues);
        }

        public static void LogEvent(string eventName, string paramName, string paramValue, string paramName2, int paramValue2)
        {
            _eventValues.Clear();
            _eventValues.Add(paramName, paramValue);
            _eventValues.Add(paramName2, paramValue2.ToString());
            LogEvent(eventName, _eventValues);
        }

        public static void LogEvent(string eventName, string paramName, int paramValue, string paramName2, int paramValue2)
        {
            _eventValues.Clear();
            _eventValues.Add(paramName, paramValue.ToString());
            _eventValues.Add(paramName2, paramValue2.ToString());
            LogEvent(eventName, _eventValues);
        }

        public static void LogEvent(string eventName, string paramName, string paramValue, string paramName2, string paramValue2)
        {
            _eventValues.Clear();
            _eventValues.Add(paramName, paramValue);
            _eventValues.Add(paramName2, paramValue2);
            LogEvent(eventName, _eventValues);
        }

        public static void LogEvent(string eventName, Action<Dictionary<string, string>> callback)
        {
            _eventValues.Clear();
            callback(_eventValues);
            LogEvent(eventName, _eventValues);
        }

        static void LogEvent(string eventName, Dictionary<string, string> eventValues)
        {
#if DEBUG_MODE
            if (DebugManager.LogAppsFlyerEvents)
            {
                Assert.IsTrue(eventName.Length <= 100 && eventName.IndexOf(' ') < 0, eventName);
               LegacyLog.Warning($"[AppsFlyer]: <b>{eventName}</b> ({eventValues.ToString2()})");
            }
#elif !UNITY_EDITOR && APPSFLYER_ENABLED
            if (!_initialized)
            {
                _pendingEvents.Add(new PendingEvent(eventName, eventValues));
            }
            else
            {
                AppsFlyerSDK.AppsFlyer.sendEvent(eventName, eventValues);
            }
#endif
        }
    }
}
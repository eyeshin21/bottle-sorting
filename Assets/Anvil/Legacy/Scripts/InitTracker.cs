using UnityEngine;
using System.Diagnostics;
#if DEBUG_INITIALIZE
using System.Collections.Generic;
#endif

namespace Anvil
{
    public static class InitTracker
    {
#if DEBUG_INITIALIZE
        static float CurrentTime => Time.realtimeSinceStartup;

        class Tracker
        {
            string _name;
            float _initTime;

            public string Name => _name;
            public float StartTime => _initTime;

            public Tracker(string name)
            {
                _name = name;
                _initTime = CurrentTime;
            }

            public override string ToString()
            {
                return $"{_name}: initTime={_initTime:0.000}s";
            }
        }

        static Dictionary<string, Tracker> _trackers = new();
#endif

        [Conditional("DEBUG_INITIALIZE")]
        public static void Track(object obj)
        {
            Track(obj.GetType().ToString());
        }

        [Conditional("DEBUG_INITIALIZE")]
        public static void Track(string name)
        {
#if DEBUG_INITIALIZE
            UnityEngine.Debug.Log($"Initialize <b>\"{name}\"</b>");
            Assert.NotContainsKey(_trackers, name);
            _trackers.Add(name, new Tracker(name));
#endif
        }

        [Conditional("DEBUG_INITIALIZE")]
        public static void OnGUI(bool debugTab = false)
        {
#if DEBUG_INITIALIZE
            GUIHelper.LazyInit();

            if (!debugTab)
            {
                var rect = GUIHelper.ScreenRect;
                GUIHelper.Box(rect);
                GUILayout.Space(rect.y);
            }

            int trackCount = _trackers.Count;
            if (trackCount > 0)
            {
                foreach (var value in _trackers.Values)
                {
                    GUILayout.Label(value.ToString());
                }
            }
#endif
        }
    }
}
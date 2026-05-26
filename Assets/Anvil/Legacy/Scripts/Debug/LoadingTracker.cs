using UnityEngine;
using System.Diagnostics;
#if DEBUG_LOADING
using System.Collections.Generic;
#endif

namespace Anvil.Legacy
{
    public static class LoadingTracker
    {
#if DEBUG_LOADING
    static float CurrentTime => Time.realtimeSinceStartup;

    class Tracker
    {
        string _name;
        float _startTime;
        float _endTime;
        bool _isCallback;
        bool _isFinished;

        public string Name => _name;
        public float StartTime => _startTime;
        public float Time => (_isFinished ? _endTime : CurrentTime) - _startTime;

        public Tracker(string name, bool callback)
        {
            _name = name;
            _startTime = CurrentTime;
            _isCallback = callback;
        }

        public void Finish()
        {
            Assert.IsFalse(_isFinished);
            _isFinished = true;
            _endTime = CurrentTime;
        }

        public override string ToString()
        {
            var name = _isCallback ? $"[{_name}]" : $"{_name}";
            var duration = _isFinished ? $"{Time:0.000}s" : $"{Time:0.000}s...";
            return $"{name}: startTime={_startTime:0.000}s ({duration})";
        }
    }

    static Dictionary<string, Tracker> _trackers = new();
#endif

        [Conditional("DEBUG_LOADING")]
        public static void StartTrack(string name, bool callback = false)
        {
#if DEBUG_LOADING
            UnityEngine.Debug.Log($"Start track <b>\"{name}\"</b>");
            Assert.NotContainsKey(_trackers, name);
            _trackers.Add(name, new Tracker(name, callback));
#endif
        }

        [Conditional("DEBUG_LOADING")]
        public static void EndTrack(string name)
        {
#if DEBUG_LOADING
            if (_trackers.TryGetValue(name, out Tracker tracker))
            {
                tracker.Finish();
                UnityEngine.Debug.Log($"End track <b>\"{name}\"</b> ({tracker.Time:0.000}s)");
            }
            else
            {
               LegacyLog.Error($"End track <b>\"{name}\"</b>: Not found!");
            }
#endif
        }

        [Conditional("DEBUG_LOADING")]
        public static void Track(string name, ref Callback callback)
        {
            StartTrack(name, true);
            Callback tmp = callback;
            callback = () =>
            {
                EndTrack(name);
                tmp?.Invoke();
            };
        }

        [Conditional("DEBUG_LOADING")]
        public static void Track<T>(string name, ref Callback<T> callback)
        {
            StartTrack(name, true);
            Callback<T> tmp = callback;
            callback = (value) =>
            {
                EndTrack(name);
                tmp?.Invoke(value);
            };
        }

        [Conditional("DEBUG_LOADING")]
        public static void OnGUI(bool debugTab = false)
        {
#if DEBUG_LOADING
            GUIHelper.LazyInit();

            if (!debugTab)
            {
                var rect = GUIHelper.ScreenRect;
                GUIHelper.Box(rect);
                GUILayout.Space(rect.y);
            }

            //if (UserPrefs.Inited)
            //{
            //    GUILayout.Label($"Session: {UserPrefs.SessionCount}");
            //}

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
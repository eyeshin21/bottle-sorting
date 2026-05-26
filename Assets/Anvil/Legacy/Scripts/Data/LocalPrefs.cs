#if DEBUG_MODE
#define DEBUG_LOCAL_PREFS
#endif
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class LocalPrefs
    {
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.DeleteKey(key);
#endif
        }

        public static bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) > 0;
        }

        public static void SetBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.SetBool(key);
#endif
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.SetInt(key);
#endif
        }

        public static long GetLong(string key, long defaultValue = 0)
        {
            if (PlayerPrefs.HasKey(key))
            {
                if (long.TryParse(PlayerPrefs.GetString(key), out long value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        public static void SetLong(string key, long value)
        {
            PlayerPrefs.SetString(key, value.ToString());
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.SetLong(key);
#endif
        }

        public static float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.SetFloat(key);
#endif
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.SetString(key);
#endif
        }

        public static DateTime? GetDateTime(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key).ToDateTime2();
            }
            return null;
        }

        public static DateTime GetDateTimeOrCurrentTime(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var dateTime = PlayerPrefs.GetString(key).ToDateTime2();
                if (dateTime.HasValue)
                {
                    return dateTime.Value;
                }
            }
            return TimeHelper.CurrentDateTime;
        }

        public static void SetDateTime(string key, DateTime? value)
        {
            if (value.HasValue)
            {
                PlayerPrefs.SetString(key, value.Value.ToSaveString());
#if DEBUG_LOCAL_PREFS
                LocalPrefsDebugger.SetDateTime(key);
#endif
            }
            else
            {
                DeleteKey(key);
            }
        }

        public static void SetDateTime(string key, DateTime value)
        {
            PlayerPrefs.SetString(key, value.ToSaveString());
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.SetDateTime(key);
#endif
        }

        #region Create
        static List<IBaseDataController> _controllers;
        public static void AddController(IBaseDataController controller)
        {
            if (_controllers == null)
            {
                _controllers = new List<IBaseDataController>();
            }
            Assert.NotContains(_controllers, controller);
            _controllers.Add(controller);
        }

        public static LocalBoolDataController CreateBoolController(string key, bool defaultValue = false)
        {
            AddLocalKey(key);
            var controller = new LocalBoolDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }
        public static LocalBoolDataController CreateBoolController(string key, string name, bool defaultValue = false)
        {
            AddLocalKey(key, name);
            var controller = new LocalBoolDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }

        public static LocalIntDataController CreateIntController(string key, int defaultValue = 0)
        {
            AddLocalKey(key);
            var controller = new LocalIntDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }
        public static LocalIntDataController CreateIntController(string key, string name, int defaultValue = 0)
        {
            AddLocalKey(key, name);
            var controller = new LocalIntDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }

        public static LocalLongDataController CreateLongController(string key, long defaultValue = 0)
        {
            AddLocalKey(key);
            var controller = new LocalLongDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }
        public static LocalLongDataController CreateLongController(string key, string name, long defaultValue = 0)
        {
            AddLocalKey(key, name);
            var controller = new LocalLongDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }

        public static LocalFloatDataController CreateFloatController(string key, float defaultValue = 0)
        {
            AddLocalKey(key);
            var controller = new LocalFloatDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }
        public static LocalFloatDataController CreateFloatController(string key, string name, float defaultValue = 0)
        {
            AddLocalKey(key, name);
            var controller = new LocalFloatDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }

        public static LocalStringDataController CreateStringController(string key, string defaultValue = "")
        {
            AddLocalKey(key);
            var controller = new LocalStringDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }
        public static LocalStringDataController CreateStringControllerWithName(string key, string name, string defaultValue)
        {
            AddLocalKey(key, name);
            var controller = new LocalStringDataController(key, defaultValue);
            AddController(controller);
            return controller;
        }

        public static LocalDateTimeDataController CreateDateTimeController(string key)
        {
            AddLocalKey(key);
            var controller = new LocalDateTimeDataController(key);
            AddController(controller);
            return controller;
        }
        public static LocalDateTimeDataController CreateDateTimeController(string key, string name)
        {
            AddLocalKey(key, name);
            var controller = new LocalDateTimeDataController(key);
            AddController(controller);
            return controller;
        }

        [Conditional("DEBUG_MODE")]
        static void AddLocalKey(string key)
        {
#if DEBUG_MODE
            LocalKeys.AddKey(key, Helper.GetNicifyName(key));
#endif
        }

        [Conditional("DEBUG_MODE")]
        static void AddLocalKey(string key, string name)
        {
#if DEBUG_MODE
            LocalKeys.AddKey(key, name);
#endif
        }
        #endregion

#if UNITY_EDITOR
        [MenuItem("Debug/Log Local Prefs")]
#endif
        public static void LogConsole()
        {
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.LogConsole();
#endif
        }

#if UNITY_EDITOR
        [MenuItem("Debug/Clear Local Prefs")]
#endif
        public static void Clear()
        {
            PlayerPrefs.DeleteAll();
#if DEBUG_LOCAL_PREFS
            LocalPrefsDebugger.Clear();
#endif
            // SessionTracker.Clear();
            if (_controllers != null)
            {
                foreach (var controller in _controllers)
                {
                    //Log.Warning($"Clear {controller.Key}");
                    controller.Clear();
                }
            }
        }
    }
}
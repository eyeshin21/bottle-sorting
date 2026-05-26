#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class LocalPrefsDebugger
    {
        static readonly string FileName = "DebugLocalPrefs.data";
        static string Path => $"{Application.persistentDataPath}/{FileName}";

        enum ValueType
        {
            None = -1,
            Bool,
            Int,
            Long,
            Float,
            String,
            DateTime
        }

        class KeyValueType
        {
            string _key;
            ValueType _valueType = ValueType.None;

            public string Key => _key;

            public ValueType ValueType
            {
                get => _valueType;
                set
                {
                    if (_valueType != value)
                    {
                        if (_valueType != ValueType.None)
                        {
                           LegacyLog.Warning($"{_key}: {_valueType} => {value}");
                        }
                        _valueType = value;
                    }
                }
            }

            private KeyValueType()
            {

            }

            public KeyValueType(string key, ValueType valueType)
            {
                _key = key;
                _valueType = valueType;
            }

            public string Serialize()
            {
                return $"{_key}_{(int)_valueType}";
            }

            public static KeyValueType Deserialize(string json)
            {
                var keyValueType = new KeyValueType();
                if (!string.IsNullOrEmpty(json))
                {
                    int index = json.LastIndexOf('_');
                    if (index > 0)
                    {
                        keyValueType._key = json.Substring(0, index);
                        keyValueType._valueType = (ValueType)json.Substring(index + 1).ToInt();
                    }
                    else
                    {
                       LegacyLog.Warning($"Cannot parse \"{json}\"!");
                    }
                }

                return keyValueType;
            }

            public void LogConsole()
            {
                if (_valueType == ValueType.Bool)
                {
                    LogDebug(LocalPrefs.GetBool(_key));
                }
                else if (_valueType == ValueType.Int)
                {
                    LogDebug(LocalPrefs.GetInt(_key));
                }
                else if (_valueType == ValueType.Long)
                {
                    LogDebug(LocalPrefs.GetLong(_key));
                }
                else if (_valueType == ValueType.Float)
                {
                    LogDebug(LocalPrefs.GetFloat(_key));
                }
                else if (_valueType == ValueType.String)
                {
                    LogDebug(LocalPrefs.GetString(_key));
                }
                else if (_valueType == ValueType.DateTime)
                {
                    LogDebug(LocalPrefs.GetDateTime(_key));
                }
                else
                {
                   LegacyLog.Debug(this);
                }
            }

            void LogDebug(object value)
            {
                var name = KeyNameHelper.GetName(_key);
                //if (name != _key)
                //{
                //   LegacyLog.Debug($"<b>\"{name} ({_key})\"</b> = {value} <{_valueType}>");
                //}
                //else
                //{
                //   LegacyLog.Debug($"<b>\"{_key}\"</b> = {value} <{_valueType}>");
                //}
                if (name != _key)
                {
                   LegacyLog.Debug($"<b>\"{name} ({_key})\"</b>: {value}");
                }
                else
                {
                   LegacyLog.Debug($"<b>\"{_key}\"</b>: {value}");
                }
            }

            public override string ToString()
            {
                return $"{_key} <{_valueType}>";
            }
        }

        static Dictionary<string, KeyValueType> _dict = new Dictionary<string, KeyValueType>();
        static bool _isInited;
        static bool _isDirty;
        static float _dirtyTime;

        public static void SetBool(string key)
        {
            Set(key, ValueType.Bool);
        }

        public static void SetInt(string key)
        {
            Set(key, ValueType.Int);
        }

        public static void SetLong(string key)
        {
            Set(key, ValueType.Long);
        }

        public static void SetFloat(string key)
        {
            Set(key, ValueType.Float);
        }

        public static void SetString(string key)
        {
            Set(key, ValueType.String);
        }

        public static void SetDateTime(string key)
        {
            Set(key, ValueType.DateTime);
        }

        static void Set(string key, ValueType valueType)
        {
            LazyInit();

            if (!_dict.TryGetValue(key, out KeyValueType keyValueType))
            {
                keyValueType = new KeyValueType(key, valueType);
                _dict.Add(key, keyValueType);
                OnDirty();
            }
            else if (keyValueType.ValueType != valueType)
            {
                keyValueType.ValueType = valueType;
                OnDirty();
            }
        }

        public static void DeleteKey(string key)
        {
            LazyInit();

            if (_dict.ContainsKey(key))
            {
                _dict.Remove(key);
                OnDirty();
            }
        }

        public static void Clear()
        {
            LazyInit();

            if (_dict.Count > 0)
            {
                _dict.Clear();
                OnDirty();
            }
        }

        static void LazyInit()
        {
            if (_isInited) return;
            _isInited = true;

            var json = FileHelper.LoadBinary<string>(Path, true);
            if (!string.IsNullOrEmpty(json))
            {
                var subJsons = json.Split('&');
                foreach (var subJson in subJsons)
                {
                    var keyValueType = KeyValueType.Deserialize(subJson);
                    var key = keyValueType.Key;
                    if (string.IsNullOrEmpty(key))
                    {
                       LegacyLog.Warning($"Key required \"{subJson}\"!");
                    }
                    else if (_dict.ContainsKey(key))
                    {
                       LegacyLog.Warning($"Key existed \"{key}\"!");
                    }
                    else
                    {
                        _dict.Add(key, keyValueType);
                    }
                }
            }
        }

        static void OnDirty()
        {
            if (!_isDirty)
            {
                _isDirty = true;
                Save(); //TODO
            }
        }

        static string Serialize()
        {
            int count = _dict.Count;
            if (count == 0) return "";

            string json = "";
            bool isFirst = true;
            foreach (var keyValueType in _dict.Values)
            {
                if (isFirst)
                {
                    json = keyValueType.Serialize();
                    isFirst = false;
                }
                else
                {
                    json = $"{json}&{keyValueType.Serialize()}";
                }
            }

            return json;
        }

        public static void Save()
        {
            if (_isDirty)
            {
                var json = Serialize();
                if (FileHelper.SaveBinary(json, Path, true))
                {
                    _isDirty = false;
                    _dirtyTime = 0;
                }
            }
        }

        public static void Update(float deltaTime)
        {
            if (_isDirty)
            {
                _dirtyTime += deltaTime;
                if (_dirtyTime > 1)
                {
                    _dirtyTime = 0;
                    Save();
                }
            }
        }

        public static void LogConsole()
        {
            LazyInit();
            foreach (var keyValueType in _dict.Values)
            {
                keyValueType.LogConsole();
            }
        }
    }
}
#endif
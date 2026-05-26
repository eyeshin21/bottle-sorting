using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anvil
{
    public class LocalHashSetDataController<T> : LocalDataController<HashSet<T>>
    {
        public LocalHashSetDataController(string key) : base(key, new HashSet<T>())
        {
#if DEBUG_MODE
            Label = key;
#endif
        }
        public LocalHashSetDataController(string key, int priority) : base(key, new HashSet<T>(), priority)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }

        public override HashSet<T> Read()
        {
            string saved = UserDataSerializer.GetValue(_key);

            if (string.IsNullOrEmpty(saved))
            {
                
                return new HashSet<T>();
            }
            else
            {
                var result = DeserializeSet(saved);
                return result;
            }
        }


        public override void Write()
        {
            var data = SerializeSet();
            UserDataSerializer.SaveValue(_key, data);
            UserDataSerializer.IncreaseSavePoint(_priority);
        }

        private string SerializeSet()
        {
            if (_value == null)
            {
                return string.Empty;
            }

            List<string> parts = new List<string>();
            foreach (T item in _value)
            {
                parts.Add(item.ToString());
            }

            string data = string.Join(';', parts);
            return data;
        }

        private HashSet<T> DeserializeSet(string saved)
        {
            if (saved.IsNullOrEmpty())
            {
                return new HashSet<T>();
            }

            string[] parts = saved.Split(';');
            HashSet<T> result = new HashSet<T>();
            foreach (string part in parts)
            {
                if (!Parse(part, out T value)) continue;
                result.Add(value);
            }

            return result;
        }

        protected virtual bool Parse(string part, out T value)
        {
            if (string.IsNullOrWhiteSpace(part))
            {
                value = default;
                return false;
            }

            Type type = typeof(T);

            try
            {
                // common primitives
                if (type == typeof(int)) value = (T)(object)int.Parse(part);
                else if (type == typeof(long)) value = (T)(object)long.Parse(part);
                else if (type == typeof(float)) value = (T)(object)float.Parse(part);
                else if (type == typeof(double)) value = (T)(object)double.Parse(part);
                else if (type == typeof(bool)) value = (T)(object)bool.Parse(part);
                else if (type == typeof(decimal)) value = (T)(object)decimal.Parse(part);
                else if (type == typeof(DateTime)) value = (T)(object)DateTime.Parse(part);
                else if (type == typeof(Guid)) value = (T)(object)Guid.Parse(part);
                else if (type.IsEnum) value = (T)Enum.Parse(type, part);
                else if (type == typeof(string)) value = (T)(object)part;
                else
                {
                    // Fallback for anything else (Byte, SByte, Int16...)
                    value = (T)Convert.ChangeType(part, type);
                }
        
                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }

        protected override bool Equals(HashSet<T> value1, HashSet<T> value2)
        {
            return value1.SetEquals(value2);
        }

#if DEBUG_MODE
        string _text;
        bool _isGUIInited;

        protected override void Set(HashSet<T> value)
        {
            base.Set(value);
            _text = _value.ToString();
        }

        public string Label { get; set; }
        public float TextWidth { get; set; } = 120;

        public override void OnGUI(Action<HashSet<T>> setHandler = null)
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = SerializeSet();
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label($"{Label}: ");
                if (TextWidth > 0)
                {
                    _text = GUILayout.TextField(_text, GUILayout.Width(TextWidth));
                }
                else
                {
                    _text = GUILayout.TextField(_text);
                }

                GUILayout.Space(5);
                bool guiEnabled = GUI.enabled;
                var value = DeserializeSet(_text);
                GUI.enabled = guiEnabled && value != _value;
                if (GUILayout.Button("Set"))
                {
                    Value = value;
                }

                GUI.enabled = guiEnabled;
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }
}
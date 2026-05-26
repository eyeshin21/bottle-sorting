using UnityEngine;
using System;
using Anvil.Legacy;

namespace Anvil
{
    public abstract class LocalDataController<T> : IDataController<T>
    {
        protected string _key;
        protected T _defaultValue;
        protected T _value;
        private bool _isInited;
        protected int _priority;

        public string Key => _key;

        public LocalDataController(string key, T defaultValue)
        {
            _key = key;
            _defaultValue = defaultValue;
            UserDataSerializer.RegisterDataController(this);
        }
        public LocalDataController(string key, T defaultValue, int priority) : this(key, defaultValue)
        {
            _priority = priority;
        }

        public T Value
        {
            get
            {
                if (!_isInited)
                {
                    _isInited = true;
                    Init(Read());
                }

                return _value;
            }
            set
            {
                if (_isInited)
                {
                    if (!Equals(_value, value))
                    {
                        Set(value);
                        Write();
                    }
                }
                else
                {
                    _isInited = true;
                    Init(value);
                    Write();
                }
            }
        }

        protected virtual void Init(T value)
        {
            _value = value;
        }

        protected virtual void Set(T value)
        {
            _value = value;
        }

        public virtual void Reset()
        {
            Set(_defaultValue);
        }

        public virtual void ClearInitStatus()
        {
            _isInited = false;
        }

        public abstract T Read();
        public abstract void Write();
        protected abstract bool Equals(T value1, T value2);

#if DEBUG_MODE
        public abstract void OnGUI(Action<T> setHandler = null);
#endif
    }

    public class LocalBoolDataController : LocalDataController<bool>
    {
        public LocalBoolDataController(string key, bool defaultValue = false) : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }
        public LocalBoolDataController(string key, bool defaultValue, int priority) : base(key, defaultValue, priority)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }

        public override bool Read()
        {
            return UserDataSerializer.GetBool(_key, _defaultValue);
        }

        public override void Write()
        {
            UserDataSerializer.SetBool(_key, _value);
            UserDataSerializer.IncreaseSavePoint(_priority);
        }

        protected override bool Equals(bool value1, bool value2)
        {
            return value1 == value2;
        }


#if DEBUG_MODE
        bool _isGUIInited;

        public string Label { get; set; }

        public override void OnGUI(Action<bool> setHandler = null)
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _value = Value;
            }

            bool value = GUILayout.Toggle(Value, Label);
            if (setHandler != null)
            {
                if (value != _value)
                {
                    setHandler(value);
                }
            }
            else
            {
                Value = value;
            }
        }
#endif
    }

    public class LocalIntDataController : LocalDataController<int>
    {
        public LocalIntDataController(string key, int defaultValue = 0) : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }
        public LocalIntDataController(string key, int defaultValue, int priority) : base(key, defaultValue, priority)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }

        public override int Read()
        {
            return UserDataSerializer.GetInt(_key, _defaultValue);
        }

        public override void Write()
        {
            UserDataSerializer.SaveValue(_key, _value);
            UserDataSerializer.IncreaseSavePoint();
        }

        protected override bool Equals(int value1, int value2)
        {
            return value1 == value2;
        }

#if DEBUG_MODE
        string _text;
        bool _isGUIInited;

        protected override void Set(int value)
        {
            base.Set(value);
            _text = _value.ToString();
        }

        public string Label { get; set; }
        public float TextWidth { get; set; } = 120;

        public override void OnGUI(Action<int> setHandler = null)
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value.ToString();
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
                int value = _text.ToInt();
                GUI.enabled = guiEnabled && value != _value;
                if (GUILayout.Button("Set"))
                {
                    if (setHandler != null)
                    {
                        setHandler(value);
                        _text = _value.ToString(); // In case of value not changed (clamp, ...)
                    }
                    else
                    {
                        Value = value;
                    }
                }

                GUI.enabled = guiEnabled;
                if (GUILayout.Button("Clear"))
                {
                    Value = 0;
                    _text = _value.ToString();
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }

    public class LocalEnumHashSetDataController<TEnum> : LocalHashSetDataController<TEnum> where TEnum : struct, Enum
    {
        public LocalEnumHashSetDataController(string key) : base(key)
        {
        }
        public LocalEnumHashSetDataController(string key, int priority) : base(key, priority)
        {
        }

        protected override bool Parse(string part, out TEnum value)
        {
            bool ret = Enum.TryParse(part, out value);
            return ret;
        }
    }

    public class LocalStringDataController : LocalDataController<string>
    {
        public LocalStringDataController(string key, string defaultValue = "") : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }

        public LocalStringDataController(string key, string defaultValue, int priority) : base(key, defaultValue,
            priority)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }

        public override string Read()
        {
            return UserDataSerializer.GetValue(_key, _defaultValue);
        }

        public override void Write()
        {
            UserDataSerializer.SaveValue(_key, _value);
            UserDataSerializer.IncreaseSavePoint();
        }

        protected override bool Equals(string value1, string value2)
        {
            return value1 == value2;
        }
#if DEBUG_MODE
        string _text;
        bool _isGUIInited;
        public string Label { get; set; }
        public float TextWidth { get; set; } = 200;
        public override void OnGUI(Action<string> setHandler = null)
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value;
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
                GUI.enabled = guiEnabled && _text != _value;
                if (GUILayout.Button("Set"))
                {
                    if (setHandler != null)
                    {
                        setHandler(_text);
                        _text = _value; // In case of value not changed (clamp, ...)
                    }
                    else
                    {
                        Value = _text;
                    }
                }

                GUI.enabled = guiEnabled;
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
        
    }
}
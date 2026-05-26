#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class DebugGenericController<T>
    {
        protected string _key;
        protected T _defaultValue;
        protected T _value;
        private bool _isInited;

        public DebugGenericController(string key, T defaultValue)
        {
            _key = key;
            _defaultValue = defaultValue;
        }

        public T Value
        {
            get
            {
                if (!_isInited)
                {
                    _value = Read();
                    _isInited = true;
                }
                return _value;
            }
            set
            {
                if (_isInited)
                {
                    if (!Equals(_value, value))
                    {
                        _value = value;
                        Write();
                    }
                }
                else
                {
                    _value = value;
                    Write();
                    _isInited = true;
                }
            }
        }

        public void Reset()
        {
            Value = _defaultValue;
        }

        protected abstract T Read();
        protected abstract void Write();
        protected abstract bool Equals(T value1, T value2);
    }

    public class DebugBoolController : DebugGenericController<bool>
    {
        public DebugBoolController(string key, bool defaultValue = false) : base(key, defaultValue)
        {

        }

        protected override bool Read()
        {
            return DebugPrefs.GetBool(_key, _defaultValue);
        }

        protected override void Write()
        {
            DebugPrefs.SetBool(_key, _value);
        }

        protected override bool Equals(bool value1, bool value2)
        {
            return value1 == value2;
        }
    }

    public class DebugIntController : DebugGenericController<int>
    {
        public DebugIntController(string key, int defaultValue = 0) : base(key, defaultValue)
        {

        }

        protected override int Read()
        {
            return DebugPrefs.GetInt(_key, _defaultValue);
        }

        protected override void Write()
        {
            DebugPrefs.SetInt(_key, _value);
        }

        protected override bool Equals(int value1, int value2)
        {
            return value1 == value2;
        }
    }

    public class DebugFloatController : DebugGenericController<float>
    {
        public DebugFloatController(string key, float defaultValue = 0) : base(key, defaultValue)
        {

        }

        protected override float Read()
        {
            return DebugPrefs.GetFloat(_key, _defaultValue);
        }

        protected override void Write()
        {
            DebugPrefs.SetFloat(_key, _value);
        }

        protected override bool Equals(float value1, float value2)
        {
            return Mathf.Approximately(value1, value2);
        }
    }

    public class DebugStringController : DebugGenericController<string>
    {
        public DebugStringController(string key, string defaultValue = "") : base(key, defaultValue)
        {

        }

        protected override string Read()
        {
            return DebugPrefs.GetString(_key, _defaultValue);
        }

        protected override void Write()
        {
            DebugPrefs.SetString(_key, _value);
        }

        protected override bool Equals(string value1, string value2)
        {
            if (string.IsNullOrEmpty(value1))
            {
                return string.IsNullOrEmpty(value2);
            }

            return value1 == value2;
        }
    }
}
#endif
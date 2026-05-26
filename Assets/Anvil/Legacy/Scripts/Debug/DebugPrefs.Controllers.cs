#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class DebugPrefs
    {
        abstract class GenericController<T>
        {
            protected string _key;
            protected T _defaultValue;
            protected T _value;
            private bool _isInited;

            public GenericController(string key, T defaultValue)
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
                        _isInited = true;
                        _value = value;
                        Write();
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

        class BoolController : GenericController<bool>
        {
            public BoolController(string key, bool defaultValue = false) : base(key, defaultValue)
            {

            }

            protected override bool Read()
            {
                return GetBool(_key, _defaultValue);
            }

            protected override void Write()
            {
                SetBool(_key, _value);
            }

            protected override bool Equals(bool value1, bool value2)
            {
                return value1 == value2;
            }
        }

        class IntController : GenericController<int>
        {
            public IntController(string key, int defaultValue = 0) : base(key, defaultValue)
            {

            }

            protected override int Read()
            {
                return GetInt(_key, _defaultValue);
            }

            protected override void Write()
            {
                SetInt(_key, _value);
            }

            protected override bool Equals(int value1, int value2)
            {
                return value1 == value2;
            }
        }

        class FloatController : GenericController<float>
        {
            public FloatController(string key, float defaultValue = 0) : base(key, defaultValue)
            {

            }

            protected override float Read()
            {
                return GetFloat(_key, _defaultValue);
            }

            protected override void Write()
            {
                SetFloat(_key, _value);
            }

            protected override bool Equals(float value1, float value2)
            {
                return value1.IsEquals(value2);
            }
        }

        class StringController : GenericController<string>
        {
            public StringController(string key, string defaultValue = "") : base(key, defaultValue)
            {

            }

            protected override string Read()
            {
                return GetString(_key, _defaultValue);
            }

            protected override void Write()
            {
                SetString(_key, _value);
            }

            protected override bool Equals(string value1, string value2)
            {
                return value1.IsEquals(value2);
            }
        }
    }
}
#endif
using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public abstract class LocalDataController<T> :
#if DEBUG_MODE
        DebugDataController,
        IDebugGUIController,
#endif
        IDataController<T>
    {
        protected string _key;
        protected T _defaultValue;
        protected T _value;
        private bool _isInited;
        protected Handler<T> _setHandler;
#if DEBUG_MODE
        protected Handler<T> _logHandler;
        protected bool _isLogDebugSet; // Log debug when set value
#endif

        public string Key => _key;
        protected virtual T InitValue => _defaultValue;

        public LocalDataController(string key, T defaultValue)
        {
            _key = key;
            _defaultValue = defaultValue;
#if DEBUG_MODE
            _name = LocalKeys.GetName(key);
#endif
        }

        public T Value
        {
            get
            {
                if (!_isInited)
                {
                    _isInited = true;
                    if (Read(out T value))
                    {
                        Init(value);
                    }
                    else
                    {
                        Init(InitValue);
                    }
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
#if DEBUG_MODE
            if (_isLogDebugSet)
            {
               LegacyLog.Debug($"{_name}: {_value} => <b>{value}</b>");
            }
#endif
            _value = value;
        }

        /// <summary>
        /// Set to initial value.
        /// </summary>
        public virtual void Reset()
        {
            Set(InitValue);
        }

        public virtual void CopyLocal(string key)
        {
#if DEBUG_MODE
#endif
        }

        public virtual void Clear()
        {
            _isInited = false;
        }

        protected abstract bool Read(out T value);
        protected abstract T Read();
        protected abstract void Write();
        protected abstract bool Equals(T value1, T value2);

#if DEBUG_MODE
        public virtual IDataController<T> SetDebugSetHandler(Handler<T> setHandler)
        {
            _setHandler = setHandler;
            return this;
        }
        public virtual IDataController<T> SetDebugGUIController(IDebugGUIController guiController)
        {
            return this;
        }
        public override IDebugGUIController DebugGUIController => this;
        public virtual bool Contains(string search) => _name.Contains(search, StringComparison.OrdinalIgnoreCase);
        public abstract void OnGUI();
#endif
    }

    public class LocalBoolDataController : LocalDataController<bool>
    {
        Func<bool> _initFunc;

        public LocalBoolDataController SetInitFunc(Func<bool> initFunc)
        {
            _initFunc = initFunc;
            return this;
        }

        public LocalBoolDataController SetSetHandler(Handler<bool> handler)
        {
            _setHandler = handler;
            return this;
        }

        public LocalBoolDataController SetLogHandler(Handler<bool> handler)
        {
#if DEBUG_MODE
            _logHandler = handler;
#endif
            return this;
        }

        public LocalBoolDataController LogDebugSet()
        {
#if DEBUG_MODE
            _isLogDebugSet = true;
#endif
            return this;
        }

        protected override bool InitValue => _initFunc != null ? _initFunc() : _defaultValue;

        public LocalBoolDataController(string key, bool defaultValue = false) : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = _name;
#endif
        }

        protected override bool Read(out bool value)
        {
            if (LocalPrefs.HasKey(_key))
            {
                value = LocalPrefs.GetBool(_key, _defaultValue);
                return true;
            }
            value = false;
            return false;
        }

        protected override bool Read()
        {
            return LocalPrefs.GetBool(_key, _defaultValue);
        }

        protected override void Write()
        {
            LocalPrefs.SetBool(_key, _value);
        }

        protected override bool Equals(bool value1, bool value2)
        {
            return value1 == value2;
        }

#if DEBUG_MODE
        bool _isGUIInited;

        public new string Label { get; set; }

        public override void OnGUI()
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _value = Value;
            }

            bool value = Toggle(Label, Value);
            if (_setHandler != null)
            {
                if (value != _value)
                {
                    _setHandler(value);
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
        Func<int> _initFunc;

        public LocalIntDataController SetInitFunc(Func<int> initFunc)
        {
            _initFunc = initFunc;
            return this;
        }

        public LocalIntDataController SetSetHandler(Handler<int> handler)
        {
            _setHandler = handler;
            return this;
        }

        public LocalIntDataController SetLogHandler(Handler<int> handler)
        {
#if DEBUG_MODE
            _logHandler = handler;
#endif
            return this;
        }

        public LocalIntDataController LogDebugSet()
        {
#if DEBUG_MODE
            _isLogDebugSet = true;
#endif
            return this;
        }

        public LocalIntDataController SetGUIController<T>() where T : Enum
        {
#if DEBUG_MODE
            _guiController = new DebugFlagGUIController(Label, Enum.GetNames(typeof(T)), (bit) => FlagHelper.IsOn(Value, bit), (bit, value) => Value = FlagHelper.SetOn(Value, bit, value));
#endif
            return this;
        }

        protected override int InitValue => _initFunc != null ? _initFunc() : _defaultValue;

        public LocalIntDataController(string key, int defaultValue = 0) : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = _name;
#endif
        }

        protected override bool Read(out int value)
        {
            if (LocalPrefs.HasKey(_key))
            {
                value = LocalPrefs.GetInt(_key, _defaultValue);
                return true;
            }
            value = 0;
            return false;
        }

        protected override int Read()
        {
            return LocalPrefs.GetInt(_key, _defaultValue);
        }

        protected override void Write()
        {
            LocalPrefs.SetInt(_key, _value);
        }

        protected override bool Equals(int value1, int value2)
        {
            return value1 == value2;
        }

#if DEBUG_MODE
        DebugGUIController _guiController;
        string _text;
        bool _isGUIInited;

        protected override void Set(int value)
        {
            base.Set(value);
            _text = _value.ToString();
        }

        public new string Label { get; set; }
        public float TextWidth { get; set; } = 120;

        void OnSet(int value)
        {
            if (_setHandler != null)
            {
                _setHandler(value);
                _text = _value.ToString(); // In case of value not changed (clamp, ...)
            }
            else
            {
                Value = value;
            }
        }

        public override void OnGUI()
        {
            if (_guiController != null)
            {
                _guiController.OnGUI();
                return;
            }

            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value.ToString();
            }

            GUILayout.BeginHorizontal();
            {
                Label(Label);
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
                if (Button("Set"))
                {
                    OnSet(value);
                }

                GUI.enabled = guiEnabled && value != 0;
                if (Button("Clear"))
                {
                    _text = "";
                    OnSet(0);
                }

                GUI.enabled = guiEnabled;
                if (_logHandler != null)
                {
                    if (Button("Log"))
                    {
                        _logHandler(Value);
                    }
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }

    public class LocalLongDataController : LocalDataController<long>
    {
        Func<long> _initFunc;

        public LocalLongDataController SetInitFunc(Func<long> initFunc)
        {
            _initFunc = initFunc;
            return this;
        }

        public LocalLongDataController SetSetHandler(Handler<long> handler)
        {
            _setHandler = handler;
            return this;
        }

        public LocalLongDataController SetLogHandler(Handler<long> handler)
        {
#if DEBUG_MODE
            _logHandler = handler;
#endif
            return this;
        }

        public LocalLongDataController LogDebugSet()
        {
#if DEBUG_MODE
            _isLogDebugSet = true;
#endif
            return this;
        }

        protected override long InitValue => _initFunc != null ? _initFunc() : _defaultValue;

        public LocalLongDataController(string key, long defaultValue = 0) : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = _name;
#endif
        }

        protected override bool Read(out long value)
        {
            if (LocalPrefs.HasKey(_key))
            {
                value = LocalPrefs.GetLong(_key, _defaultValue);
                return true;
            }
            value = 0;
            return false;
        }

        protected override long Read()
        {
            return LocalPrefs.GetLong(_key, _defaultValue);
        }

        protected override void Write()
        {
            LocalPrefs.SetLong(_key, _value);
        }

        protected override bool Equals(long value1, long value2)
        {
            return value1 == value2;
        }

#if DEBUG_MODE
        string _text;
        bool _isGUIInited;

        protected override void Set(long value)
        {
            base.Set(value);
            _text = _value.ToString();
        }

        public new string Label { get; set; }
        public float TextWidth { get; set; } = 120;

        void OnSet(long value)
        {
            if (_setHandler != null)
            {
                _setHandler(value);
                _text = _value.ToString();
            }
            else
            {
                Value = value;
            }
        }

        public override void OnGUI()
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value.ToString();
            }

            GUILayout.BeginHorizontal();
            {
                Label(Label);
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
                long value = _text.ToLong();
                GUI.enabled = guiEnabled && value != _value;
                if (Button("Set"))
                {
                    OnSet(value);
                }

                GUI.enabled = guiEnabled && value != 0;
                if (Button("Clear"))
                {
                    _text = "";
                    OnSet(0);
                }

                GUI.enabled = guiEnabled;
                if (_logHandler != null)
                {
                    if (Button("Log"))
                    {
                        _logHandler(Value);
                    }
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }

    public class LocalFloatDataController : LocalDataController<float>
    {
        Func<float> _initFunc;

        public LocalFloatDataController SetInitFunc(Func<float> initFunc)
        {
            _initFunc = initFunc;
            return this;
        }

        public LocalFloatDataController SetSetHandler(Handler<float> handler)
        {
            _setHandler = handler;
            return this;
        }

        public LocalFloatDataController SetLogHandler(Handler<float> handler)
        {
#if DEBUG_MODE
            _logHandler = handler;
#endif
            return this;
        }

        public LocalFloatDataController LogDebugSet()
        {
#if DEBUG_MODE
            _isLogDebugSet = true;
#endif
            return this;
        }

        protected override float InitValue => _initFunc != null ? _initFunc() : _defaultValue;

        public LocalFloatDataController(string key, float defaultValue = 0) : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = _name;
#endif
        }

        protected override bool Read(out float value)
        {
            if (LocalPrefs.HasKey(_key))
            {
                value = LocalPrefs.GetFloat(_key, _defaultValue);
                return true;
            }
            value = 0;
            return false;
        }

        protected override float Read()
        {
            return LocalPrefs.GetFloat(_key, _defaultValue);
        }

        protected override void Write()
        {
            LocalPrefs.SetFloat(_key, _value);
        }

        protected override bool Equals(float value1, float value2)
        {
            return Mathf.Approximately(value1, value2);
        }

#if DEBUG_MODE
        string _text;
        bool _isGUIInited;

        protected override void Set(float value)
        {
            base.Set(value);
            _text = _value.ToString();
        }

        public new string Label { get; set; }
        public float TextWidth { get; set; } = 120;

        void OnSet(float value)
        {
            if (_setHandler != null)
            {
                _setHandler(value);
                _text = _value.ToString();
            }
            else
            {
                Value = value;
            }
        }

        public override void OnGUI()
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value.ToString();
            }

            GUILayout.BeginHorizontal();
            {
                Label(Label);
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
                float value = _text.ToFloat();
                GUI.enabled = guiEnabled && value != _value;
                if (Button("Set"))
                {
                    OnSet(value);
                }

                GUI.enabled = guiEnabled && value != 0;
                if (Button("Clear"))
                {
                    _text = "";
                    OnSet(0);
                }

                GUI.enabled = guiEnabled;
                if (_logHandler != null)
                {
                    if (Button("Log"))
                    {
                        _logHandler(Value);
                    }
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }

    public class LocalStringDataController : LocalDataController<string>
    {
        Func<string> _initFunc;

        public LocalStringDataController SetInitFunc(Func<string> initFunc)
        {
            _initFunc = initFunc;
            return this;
        }

        public LocalStringDataController SetSetHandler(Handler<string> handler)
        {
            _setHandler = handler;
            return this;
        }

        public LocalStringDataController SetLogHandler(Handler<string> handler)
        {
#if DEBUG_MODE
            _logHandler = handler;
#endif
            return this;
        }

        public LocalStringDataController SetPasteHandler(Handler<string> handler)
        {
#if DEBUG_MODE
            _pasteHandler = handler;
#endif
            return this;
        }

        public LocalStringDataController LogDebugSet()
        {
#if DEBUG_MODE
            _isLogDebugSet = true;
#endif
            return this;
        }

        protected override string InitValue => _initFunc != null ? _initFunc() : _defaultValue;

        public LocalStringDataController(string key, string defaultValue = "") : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = _name;
#endif
        }

        protected override bool Read(out string value)
        {
            if (LocalPrefs.HasKey(_key))
            {
                value = LocalPrefs.GetString(_key, _defaultValue);
                return true;
            }
            value = "";
            return false;
        }

        protected override string Read()
        {
            return LocalPrefs.GetString(_key, _defaultValue);
        }

        protected override void Write()
        {
            LocalPrefs.SetString(_key, _value);
        }

        protected override bool Equals(string value1, string value2)
        {
            return value1.IsEquals(value2);
        }

#if DEBUG_MODE
        protected Handler<string> _pasteHandler;
        string _text;
        bool _isGUIInited;

        protected override void Set(string value)
        {
            base.Set(value);
            _text = _value;
        }

        public new string Label { get; set; }
        public float TextWidth { get; set; } = 200;

        void OnSet(string text)
        {
            if (_setHandler != null)
            {
                _setHandler(text);
                _text = _value;
            }
            else
            {
                Value = text;
            }
        }

        public override void OnGUI()
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value;
            }

            GUILayout.BeginHorizontal();
            {
                Label(Label);
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
                if (Button("Set"))
                {
                    OnSet(_text);
                }

                GUI.enabled = guiEnabled && !string.IsNullOrEmpty(_text);
                if (Button("Clear"))
                {
                    _text = "";
                    OnSet(_text);
                }

                if (Button("Copy"))
                {
                    Value.CopyToClipboard();
                }

                GUI.enabled = guiEnabled;
                if (_logHandler != null)
                {
                    if (Button("Log"))
                    {
                        _logHandler(Value);
                    }
                }

                if (_pasteHandler != null)
                {
                    var clipboard = Helper.Clipboard;
                    GUI.enabled = guiEnabled && !string.IsNullOrEmpty(clipboard);
                    if (Button("Paste"))
                    {
                        _pasteHandler(clipboard);
                    }
                }

                GUI.enabled = guiEnabled;
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }

    public class LocalDateTimeDataController : LocalDataController<DateTime?>
    {
        Func<DateTime?> _initFunc;

        public LocalDateTimeDataController SetInitFunc(Func<DateTime?> initFunc)
        {
            _initFunc = initFunc;
            return this;
        }

        public LocalDateTimeDataController SetSetHandler(Handler<DateTime?> handler)
        {
            _setHandler = handler;
            return this;
        }

        public LocalDateTimeDataController SetLogHandler(Handler<DateTime?> handler)
        {
#if DEBUG_MODE
            _logHandler = handler;
#endif
            return this;
        }

        public LocalDateTimeDataController LogDebugSet()
        {
#if DEBUG_MODE
            _isLogDebugSet = true;
#endif
            return this;
        }

        protected override DateTime? InitValue => _initFunc != null ? _initFunc() : _defaultValue;

        public LocalDateTimeDataController(string key) : base(key, null)
        {
#if DEBUG_MODE
            Label = _name;
#endif
        }

        protected override bool Read(out DateTime? value)
        {
            if (LocalPrefs.HasKey(_key))
            {
                value = LocalPrefs.GetDateTime(_key);
                return true;
            }
            value = null;
            return false;
        }

        protected override DateTime? Read()
        {
            return LocalPrefs.GetDateTime(_key);
        }

        protected override void Write()
        {
            LocalPrefs.SetDateTime(_key, _value);
        }

        protected override bool Equals(DateTime? value1, DateTime? value2)
        {
            return value1.IsEquals(value2);
        }

#if DEBUG_MODE
        string _text;
        bool _isGUIInited;

        protected override void Set(DateTime? value)
        {
            base.Set(value);
            _text = _value.ToString2();
        }

        public new string Label { get; set; }
        public float TextWidth { get; set; } = 310;

        void OnSet(string text)
        {
            var value = text.ToDateTime2();
            if (_setHandler != null)
            {
                _setHandler(value);
                _text = _value.ToString2();
            }
            else
            {
                Value = value;
            }
        }

        public override void OnGUI()
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value.ToString2();
            }

            GUILayout.BeginHorizontal();
            {
                Label(Label);
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
                //GUI.enabled = guiEnabled && _text != _value;
                if (Button("Set"))
                {
                    OnSet(_text);
                }

                GUI.enabled = guiEnabled && !string.IsNullOrEmpty(_text);
                if (Button("Clear"))
                {
                    _text = "";
                    OnSet(_text);
                }

                GUI.enabled = guiEnabled;
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
#endif
    }
}
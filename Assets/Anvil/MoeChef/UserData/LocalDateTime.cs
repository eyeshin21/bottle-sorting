using UnityEngine;
using System;
using Anvil.Legacy;

namespace Anvil
{
    public class DateTimeDataController : LocalDataController<DateTime?>
    {
        public DateTimeDataController(string key, DateTime? defaultValue = null) : base(key, defaultValue)
        {
#if DEBUG_MODE
            Label = key;
#endif
        }

        public override DateTime? Read()
        {
            // string data = UserDataSerializer.GetValue(_key);
            // return string.IsNullOrEmpty(data) ? null : data.ToDateTime2();
            Debug.Log("Not Implemented in base Class");
            return null;
        }

        public override void Write()
        {
            // UserDataSerializer.SaveValue(_key, _value.ToString2());
        }

        protected override bool Equals(DateTime? value1, DateTime? value2)
        {
            if (value1 == null && value2 == null)
            {
                return true;
            }

            if (value1 == null || value2 == null)
            {
                return false;
            }

            return value1.Value.Equals(value2.Value);
        }

        protected override void Set(DateTime? value)
        {
            base.Set(value);
#if DEBUG_MODE
            _text = _value.ToString2();
#endif
        }
        private void OnSet(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Value = null;
            }
            else
            {
                Value = text.ToDateTime2();
            }
        }
        
#if DEBUG_MODE
        string _text;
        bool _isGUIInited;

        public float TextWidth { get; set; } = 310;
        public string Label { get; set; }

        public override void OnGUI(Action<DateTime?> setHandler = null)
        {
            if (!_isGUIInited)
            {
                _isGUIInited = true;
                _text = Value.ToString2();
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(Label);
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
                if (GUILayout.Button("Set"))
                {
                    OnSet(_text);
                }

                GUI.enabled = guiEnabled && !string.IsNullOrEmpty(_text);
                if (GUILayout.Button("Clear"))
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
    
    public class LocalDateTimeDataController : DateTimeDataController
    {
        public LocalDateTimeDataController(string key, DateTime? defaultValue = null) : base(key, defaultValue)
        {
        }
        public LocalDateTimeDataController(string key, DateTime? defaultValue, int priority) : base(key, defaultValue)
        {
            _priority = priority;
        }

        public override void Write()
        {
            UserDataSerializer.SaveValue(_key, _value.ToString2());
            UserDataSerializer.IncreaseSavePoint(_priority);
        }

        public override DateTime? Read()
        {
            string data = UserDataSerializer.GetValue(_key);
            return string.IsNullOrEmpty(data) ? null : data.ToDateTime2();
        }
    }
}
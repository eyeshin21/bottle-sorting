#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public abstract class DebugGUIController : IDebugGUIController
    {
        protected bool _skipGUI;

        protected static bool Portrait => Screen.width < Screen.height;

        public abstract bool Contains(string search);

        public virtual void OnGUI()
        {
            OnGUI(false);
        }

        public virtual void OnGUI(bool nested)
        {
            if (_skipGUI) return;

            if (nested)
            {
                OnGUICallback();
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    OnGUICallback();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
        }

        protected virtual void OnGUICallback() { }

        static Dictionary<string, GUILayoutOption> _labelWidths = new();
        static GUILayoutOption GetWidth(string label)
        {
            if (!_labelWidths.TryGetValue(label, out GUILayoutOption width))
            {
                width = GUILayout.Width(GUIHelper.GetLabelWidth(label));
                _labelWidths.Add(label, width);
            }
            return width;
        }

        protected static string GetLabel(string label)
        {
            if (!string.IsNullOrEmpty(label))
            {
                if (label[label.Length - 1] != ':')
                {
                    return $"{label}:";
                }
            }
            return label;
        }

        protected static void Label(string label)
        {
            GUILayout.Label(label);
        }

        protected static void Label(GUIContent content, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, options);
        }

        protected static void Label(string label, object value)
        {
            GUILayout.Label(label);
            GUILayout.Label($"{value}");
        }

        protected static void Label(string label, string value)
        {
            GUILayout.Label(label);
            GUILayout.Label(value);
        }

        protected static void TextField(string label, object value, ref string text, GUILayoutOption textWidth)
        {
            TextField(label, $"{value}", ref text, textWidth);
        }

        protected static void TextField(string label, string value, ref string text, GUILayoutOption textWidth)
        {
            GUILayout.Label(label, GUIHelper.LabelStyle, GetWidth(label));
            GUILayout.Label(value, GetWidth(value));
            text = GUILayout.TextField(text, textWidth);
        }

        //protected static void TextField(string label, string value, ref string text, GUILayoutOption textWidth, Action clickCallback)
        //{
        //    GUILayout.Label(label, GUIHelper.LabelStyle, GetWidth(label));
        //    if (GUILayout.Button(value, GUIHelper.ButtonLabelStyle, GetWidth(value)))
        //    {
        //        clickCallback?.Invoke();
        //    }
        //    text = GUILayout.TextField(text, textWidth);
        //}

        protected static bool Button(string label)
        {
            return GUILayout.Button(label);
        }

        protected static bool Button(string label, bool isOn)
        {
            if (isOn)
            {
                return GUILayout.Button(label, GUIHelper.SelectedFlagButtonStyle);
            }
            return GUILayout.Button(label);
        }

        protected static bool Toggle(string label, ref bool value)
        {
            bool newValue = GUILayout.Toggle(value, label);
            if (newValue != value)
            {
                value = newValue;
                return true;
            }
            return false;
        }
    }

    public class DebugTodoGUIController : DebugGUIController
    {
        string _name;

        public DebugTodoGUIController(string name)
        {
            _name = name;
        }

        public override bool Contains(string search)
        {
            return _name.Contains(search, StringComparison.InvariantCultureIgnoreCase);
        }

        protected override void OnGUICallback()
        {
            Label($"{_name}: Todo");
        }
    }

    public abstract class DebugGenericGUIController<T> : DebugGUIController
    {
        protected string _label;
        protected Getter<T> _getter;
        protected Setter<T> _setter;

        public DebugGenericGUIController(string label, Getter<T> getter, Setter<T> setter)
        {
            _label = GetLabel(label);
            _getter = getter;
            _setter = setter;
        }

        public override bool Contains(string search)
        {
            return _label.Contains(search, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public class DebugBoolGUIController : DebugGenericGUIController<bool>
    {
        bool _value;
        bool _isInited;

        public bool Value
        {
            get
            {
                if (!_isInited)
                {
                    _value = _getter();
                    _isInited = true;
                }
                return _value;
            }
            private set
            {
                _value = value;
                _isInited = true;
            }
        }

        public void SetValue(bool value)
        {
            _value = value;
            _setter?.Invoke(value);
        }

        public DebugBoolGUIController(string label, Getter<bool> getter, Setter<bool> setter) : base(label, getter, setter)
        {
            _label = label;
        }

        public DebugBoolGUIController(string label, DebugBoolController controller) : base(label, () => controller.Value, value => controller.Value = value)
        {
            _label = label;
        }

        public DebugBoolGUIController(string key, bool defaultValue = false) : this(Helper.GetNicifyName(key), new DebugBoolController(key, defaultValue))
        {

        }

        public DebugBoolGUIController(string label, string key, bool defaultValue = false) : this(label, new DebugBoolController(key, defaultValue))
        {

        }

        public DebugBoolGUIController SetSkipGUI()
        {
            _skipGUI = true;
            return this;
        }

        protected override void OnGUICallback()
        {
            Value = _getter();
            if (Toggle(_label, ref _value))
            {
                _setter(_value);
            }
        }
    }

    public class DebugIntGUIController : DebugGenericGUIController<int>
    {
        private GUILayoutOption _textWidth;
        private string _text;

        public DebugIntGUIController(string label, Getter<int> getter, Setter<int> setter, float textWidth = -1) : base(label, getter, setter)
        {
            _textWidth = GUILayout.Width(textWidth > 0 ? textWidth : 120);
        }

        public DebugIntGUIController(string key, int defaultValue = 0, float textWidth = -1) : this(Helper.GetNicifyName(key), new DebugIntController(key, defaultValue), textWidth)
        {

        }

        public DebugIntGUIController(string label, string key, int defaultValue = 0, float textWidth = -1) : this(label, new DebugIntController(key, defaultValue), textWidth)
        {

        }

        public DebugIntGUIController(string label, DebugIntController controller, float textWidth = -1) : this(label, () => controller.Value, value => controller.Value = value, textWidth)
        {

        }

        protected override void OnGUICallback()
        {
            var value = _getter();
            TextField(_label, value, ref _text, _textWidth);

            GUI.enabled = !string.IsNullOrWhiteSpace(_text);
            if (Button("Set"))
            {
                if (int.TryParse(_text, out int newValue))
                {
                    _text = "";
                    _setter(newValue);
                }
            }

            GUI.enabled = value != 0;
            if (Button("Clear"))
            {
                _text = "";
                _setter(0);
            }

            GUI.enabled = true;
        }
    }

    public class DebugLongGUIController : DebugGenericGUIController<long>
    {
        private GUILayoutOption _textWidth;
        private string _text;

        public DebugLongGUIController(string label, Getter<long> getter, Setter<long> setter, float textWidth = -1) : base(label, getter, setter)
        {
            _textWidth = GUILayout.Width(textWidth > 0 ? textWidth : 120);
        }

        //public DebugLongGUIController(string label, DebugLongController controller, float textWidth = -1) : base(label, () => controller.Value, value => controller.Value = value)
        //{
        //    _textWidth = GUILayout.Width(textWidth > 0 ? textWidth : 120);
        //}

        protected override void OnGUICallback()
        {
            var value = _getter();
            TextField(_label, value, ref _text, _textWidth);

            GUI.enabled = !string.IsNullOrWhiteSpace(_text);
            if (Button("Set"))
            {
                if (long.TryParse(_text, out long newValue))
                {
                    _text = "";
                    _setter(newValue);
                }
            }

            GUI.enabled = value != 0;
            if (Button("Clear"))
            {
                _text = "";
                _setter(0);
            }

            GUI.enabled = true;
        }
    }

    public class DebugFloatGUIController : DebugGenericGUIController<float>
    {
        private GUILayoutOption _textWidth;
        private string _text;

        public DebugFloatGUIController(string label, Getter<float> getter, Setter<float> setter, float textWidth = -1) : base(label, getter, setter)
        {
            _textWidth = GUILayout.Width(textWidth > 0 ? textWidth : 120);
        }

        public DebugFloatGUIController(string label, DebugFloatController controller, float textWidth = -1) : base(label, () => controller.Value, value => controller.Value = value)
        {
            _textWidth = GUILayout.Width(textWidth > 0 ? textWidth : 120);
        }

        protected override void OnGUICallback()
        {
            var value = _getter();
            TextField(_label, value, ref _text, _textWidth);

            GUI.enabled = !string.IsNullOrWhiteSpace(_text);
            if (Button("Set"))
            {
                if (float.TryParse(_text, out float newValue))
                {
                    _text = "";
                    _setter(newValue);
                }
            }

            GUI.enabled = value != 0;
            if (Button("Clear"))
            {
                _text = "";
                _setter(0);
            }

            GUI.enabled = true;
        }
    }

    public class DebugStringGUIController : DebugGenericGUIController<string>
    {
        private GUILayoutOption _textWidth;
        private string _text;

        public DebugStringGUIController(string label, Getter<string> getter) : base(label, getter, null)
        {

        }

        public DebugStringGUIController(string label, Getter<string> getter, Setter<string> setter, float textWidth = -1) : base(label, getter, setter)
        {
            _textWidth = GUILayout.Width(textWidth > 0 ? textWidth : 250);
        }

        public DebugStringGUIController(string key, float textWidth = -1) : this(Helper.GetNicifyName(key), new DebugStringController(key), textWidth)
        {

        }

        public DebugStringGUIController(string label, string key, string defaultValue = "", float textWidth = -1) : this(label, new DebugStringController(key, defaultValue), textWidth)
        {

        }

        public DebugStringGUIController(string label, DebugStringController controller, float textWidth = -1) : this(label, () => controller.Value, value => controller.Value = value, textWidth)
        {

        }

        protected override void OnGUICallback()
        {
            var value = _getter();
            // Check read only
            if (_setter == null)
            {
                Label(_label, value);
                return;
            }
            //TextField(_label, $"\"{Helper.Truncate(value, 32)}\"", ref _text, _textWidth, value.CopyToClipboard);
            TextField(_label, $"\"{Helper.Truncate(value, 32)}\"", ref _text, _textWidth);

            GUI.enabled = !string.IsNullOrWhiteSpace(_text);
            if (Button("Set"))
            {
                var newValue = _text;
                _text = "";
                _setter(newValue);
            }

            GUI.enabled = !string.IsNullOrEmpty(value);
            if (Button("Clear"))
            {
                _text = "";
                _setter("");
            }

            GUI.enabled = true;
        }
    }

    public class DebugFlagGUIController : DebugGUIController
    {
        class ButtonController
        {
            GUIContent _content;
            GUILayoutOption _optionWidth;
            bool _isNewRow;

            public bool NewRow => _isNewRow;

            public ButtonController(GUIContent content, float width, bool newRow)
            {
                _content = content;
                _optionWidth = GUILayout.Width(width);
                _isNewRow = newRow;
            }

            public bool OnGUI()
            {
                return GUILayout.Button(_content, _optionWidth);
            }
        }

        string _label;
        string[] _names;
        Getter<int, bool> _getter;
        Setter<int, bool> _setter;
        GUIContent _labelContent;
        GUILayoutOption[] _labelOptions;
        ButtonController[] _buttonControllers;
        bool _isMultipleRow;

        public DebugFlagGUIController(string label, string[] names, Getter<int, bool> getter, Setter<int, bool> setter)
        {
            _label = GetLabel(label);
            _names = names;
            _getter = getter;
            _setter = setter;
        }

        static GUIContent _separatorContent = new GUIContent("|");
        void Separator()
        {
            GUILayout.Label(_separatorContent);
        }

        public override bool Contains(string search)
        {
            if (_label.Contains(search, StringComparison.InvariantCultureIgnoreCase)) return true;
            for (int i = _names.Length - 1; i >= 0; i--)
            {
                if (_names[i].Contains(search, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnGUICallback()
        {
            int buttonCount = _names.Length;
            if (_buttonControllers == null)
            {
                _labelContent = new GUIContent(_label);
                float labelWidth = GUIHelper.GetLabelWidth(_labelContent);
                _labelOptions = new GUILayoutOption[] { GUILayout.Width(labelWidth) };
                float separatorWidth = GUIHelper.GetLabelWidth(_separatorContent);
                _buttonControllers = new ButtonController[buttonCount];

                float maxWidth = Screen.width;
                float width = labelWidth;
                _isMultipleRow = false;

                var buttonStyle = GUIHelper.SelectedFlagButtonStyle;
                for (int i = 0; i < buttonCount; i++)
                {
                    var content = new GUIContent(_names[i]);
                    float buttonWidth = Mathf.Max(buttonStyle.CalcSize(content).x, 150);
                    bool newRow = false;
                    width += buttonWidth;
                    if (i > 0)
                    {
                        width += separatorWidth;
                        if (width > maxWidth)
                        {
                            width = buttonWidth;
                            newRow = true;
                            _isMultipleRow = true;
                        }
                    }
                    _buttonControllers[i] = new ButtonController(content, buttonWidth, newRow);
                }
            }

            bool isFirst = true;
            if (_isMultipleRow)
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                for (int i = 0; i < buttonCount; i++)
                {
                    var buttonController = _buttonControllers[i];
                    if (buttonController.NewRow)
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                    }
                    if (isFirst)
                    {
                        Label(_labelContent, _labelOptions);
                        isFirst = false;
                    }
                    else
                    {
                        Separator();
                    }

                    bool isOn = _getter(i);
                    if (Button(_names[i], isOn))
                    {
                        _setter(i, !isOn);
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    for (int i = 0; i < buttonCount; i++)
                    {
                        var buttonController = _buttonControllers[i];
                        if (isFirst)
                        {
                            Label(_labelContent, _labelOptions);
                            isFirst = false;
                        }
                        else
                        {
                            Separator();
                        }

                        bool isOn = _getter(i);
                        if (Button(_names[i], isOn))
                        {
                            _setter(i, !isOn);
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
        }
    }

    public class DebugDateTimeGUIController : DebugGenericGUIController<DateTime?>
    {
        private GUILayoutOption _textWidth;
        private string _text;

        public bool ForceReloadScene { get; set; } = true;

        static string ReduceLabel(string label)
        {
            int length = label.Length;
            if (length > 15)
            {
                if (label.EndsWith("Time"))
                {
                    label = label.Substring(0, length - 4);
                }
            }
            return label;
        }

        public DebugDateTimeGUIController(string label, Getter<DateTime?> getter, Setter<DateTime?> setter, float textWidth = -1) : base(ReduceLabel(label), getter, setter)
        {
            _textWidth = GUILayout.Width(textWidth > 0 ? textWidth : 200);
        }

        void Set(DateTime? dateTime)
        {
            _setter(dateTime);

            //if (ForceReloadScene)
            //{
            //    Manager.HideDebugAndReloadScene();
            //}
        }

        protected override void OnGUICallback()
        {
            var value = _getter();
            //TextField(_label, value.ToString("dd/MM/yy HH:mm:ss"), ref _text, _textWidth);
            TextField(_label, value.ToStringDDMMYYHHMMSS2(), ref _text, _textWidth);

            if (string.IsNullOrWhiteSpace(_text))
            {
                if (Button("Now"))
                {
                    Set(TimeHelper.CurrentDateTime);
                }

                GUI.enabled = value.HasValue;
                if (Button("Clear"))
                {
                    _setter(null);
                }
                GUI.enabled = true;
            }
            else
            {
                if (Button("Set"))
                {
                    var newValue = TimeHelper.CurrentDateTime.AddSeconds(_text.ToSeconds());
                    {
                        _text = "";
                        Set(newValue);
                    }
                }

                if (Button("Add"))
                {
                    var seconds = _text.ToSeconds();
                    _text = "";
                    Set(TimeHelper.GetDateTimeOrCurrentDateTime(value).AddSeconds(seconds));
                }
            }
        }
    }

    public class DebugIntFlagGUIController : DebugGUIController
    {
        private string _label;
        private string[] _names;
        private Accepter<int> _accepter;
        private Getter<object> _getter;
        private Setter<int> _setter;

        public int ColumnCount { get; set; }

        public DebugIntFlagGUIController(string label, string[] names, Accepter<int> accepter, Getter<object> getter, Setter<int> setter)
        {
            _label = GetLabel(label);
            _names = names;
            _accepter = accepter;
            _getter = getter;
            _setter = setter;
        }

        public override bool Contains(string search)
        {
            if (_label.Contains(search, StringComparison.InvariantCultureIgnoreCase)) return true;
            for (int i = _names.Length - 1; i >= 0; i--)
            {
                if (_names[i].Contains(search, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnGUICallback()
        {
            Label(_label, _getter());
            GUILayout.Space(10);

            int bitCount = _names.Length;
            int columnCount = ColumnCount;
            if (columnCount > 1)
            {
                int rowCount = (bitCount - 1) / columnCount + 1;
                GUILayout.BeginVertical();
                {
                    int bit = 0;
                    for (int row = 0; row < rowCount; row++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            for (int column = 0; column < columnCount; column++)
                            {
                                if (_accepter(bit) && Button(_names[bit]))
                                {
                                    _setter(bit);
                                }

                                bit++;
                                if (bit == bitCount)
                                {
                                    break;
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            else
            {
                for (int bit = 0; bit < bitCount; bit++)
                {
                    if (_accepter(bit) && Button(_names[bit]))
                    {
                        _setter(bit);
                    }
                }
            }
        }
    }
}
#endif
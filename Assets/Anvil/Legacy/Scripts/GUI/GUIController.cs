using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public interface IGUIController : IGUI
    {
        //void OnGUI();
    }

    public interface ICheckGUIController
    {
        /// <summary>
        /// Returns true if value changed.
        /// </summary>
        bool CheckOnGUI();
    }

    public interface IBaseGUIController : IGUIController
    {
        float LabelWidth { get; set; }
        float ValueWidth { get; set; }
        float GUIWidth { get; }

        void SetSceneStyle();
    }

    public interface IGUIController<T> : IBaseGUIController, ICheckGUIController
    {
        T Value { get; set; }

        IGUIController<T> SetTooltip(string tooltip);
        IGUIController<T> SetLabelWidth(float width);
        IGUIController<T> SetValueWidth(float width);
    }

    public static class GUIController
    {
        public static BoolGUIController CreateBool(string label, bool value = false)
        {
            return new BoolGUIController(label, value);
        }

        public static IntGUIController CreateInt(string label, int value = 0)
        {
            return new IntGUIController(label, value);
        }

        public static FloatGUIController CreateFloat(string label, float value = 0)
        {
            return new FloatGUIController(label, value);
        }

        public static StringGUIController CreateString(string label, string value = "")
        {
            return new StringGUIController(label, value);
        }

        public static EnumGUIController<T> CreateEnum<T>(string label, T value = default) where T : Enum
        {
            return new EnumGUIController<T>(label, value);
        }

        public static ColorGUIController CreateColor(string label, Color? color = null)
        {
            return new ColorGUIController(label, color);
        }

        public static SliderColorGUIController CreateSliderColor(string label, Color? color = null)
        {
            return new SliderColorGUIController(label, color);
        }

        public static SizeGUIController CreateSize(string label, SizeInt size = default)
        {
            return new SizeGUIController(label, size);
        }

        public static RectGUIController CreateRect(string label, Rect? rect = null)
        {
            return new RectGUIController(label, rect);
        }

        public static LanguageGUIController CreateLanguage(string label, SystemLanguage? language = null)
        {
            return new LanguageGUIController(label, language);
        }
    }

    public class BaseGUIController<T> : IGUIController<T>
    {
        protected T _value;
        protected GUIContent _labelContent;
        protected GUIStyle _labelStyle;
        protected float? _labelWidth;
        protected float? _valueWidth;

        protected virtual float DefaultValueWidth => 50;

        public virtual T Value
        {
            get => _value;
            set => _value = value;
        }

        public virtual GUIContent LabelContent
        {
            get => _labelContent;
            set => _labelContent = value;
        }

        public virtual GUIStyle LabelStyle
        {
            get => _labelStyle;
            set => _labelStyle = value;
        }

        public virtual float LabelWidth
        {
            get
            {
                if (!_labelWidth.HasValue)
                {
                    _labelWidth = GUIHelper.GetLabelWidth(_labelContent);
                }
                return _labelWidth.Value;
            }
            set => _labelWidth = value;
        }

        public virtual float ValueWidth
        {
            get
            {
                if (!_valueWidth.HasValue)
                {
                    _valueWidth = DefaultValueWidth;
                    //Log.Warning($"{this}: valueWidth={_valueWidth}");
                }
                return _valueWidth.Value;
            }
            set => _valueWidth = value;
        }

        public virtual float GUIWidth => GUIHelper.GetWidth(LabelWidth, ValueWidth);

        public bool Nested { get; set; }

        public BaseGUIController(string label, T value = default)
        {
            if (label != null)
            {
                _labelContent = label.Length == 0 ? GUIContent.none : new GUIContent(label);
            }
            _value = value;
        }

        public BaseGUIController(string label, string tooltip, T value = default)
        {
            _labelContent = new GUIContent(label, tooltip);
            _value = value;
        }

        public virtual IGUIController<T> SetLabel(string label)
        {
            _labelContent.text = label;
            return this;
        }

        public virtual IGUIController<T> SetTooltip(string tooltip)
        {
            _labelContent.tooltip = tooltip;
            return this;
        }

        public virtual IGUIController<T> SetLabelWidth(float labelWidth)
        {
            _labelWidth = labelWidth;
            return this;
        }

        public virtual IGUIController<T> SetValueWidth(float valueWidth)
        {
            _valueWidth = valueWidth;
            return this;
        }

        public virtual IGUIController<T> SetNested()
        {
            Nested = true;
            return this;
        }

        public virtual void SetSceneStyle()
        {
            LabelStyle = GUIHelper.SceneLabelStyle;
        }

        public virtual void OnGUI()
        {
            if (Nested)
            {
                OnGUICallback();
            }
            else
            {
                GUIHelper.LayoutLeft(OnGUICallback);
            }
        }

        protected virtual void OnGUICallback()
        {
            GUIHelper.Label($"{GetType()}: OnGUICallback");
        }

        public virtual bool CheckOnGUI()
        {
            if (Nested)
            {
                return CheckOnGUICallback();
            }
            bool changed = false;
            GUIHelper.LayoutLeft(() => changed = CheckOnGUICallback());
            return changed;
        }

        protected virtual bool CheckOnGUICallback()
        {
            GUIHelper.Label($"{GetType()}: CheckOnGUICallback");
            return false;
        }

        protected static float GetGUIWidth(params IBaseGUIController[] controllers)
        {
            float width = 0;
            for (int i = controllers.Length - 1; i >= 0; i--)
            {
                width += controllers[i].GUIWidth;
            }
            return width;
        }

        public override string ToString()
        {
            string label = "";
            if (_labelContent != null)
            {
                var tooltip = _labelContent.tooltip;
                if (string.IsNullOrEmpty(tooltip))
                {
                    label = _labelContent.text;
                }
                else
                {
                    label = $"{_labelContent.text} ({tooltip})";
                }
            }
            return $"{label}: value={Value}, labelWidth={LabelWidth}, valueWidth={ValueWidth}, guiWidth={GUIWidth}, nested={Nested}";
        }
    }

    public class BoolGUIController : BaseGUIController<bool>
    {
        public BoolGUIController(string label, bool value = false) : base(label, value)
        {

        }

        public BoolGUIController(string label, string tooltip, bool value = false) : base(label, tooltip, value)
        {

        }

        //public override void SetSceneStyle()
        //{
        //    _labelStyle = GUIHelper.SceneToggleStyle;
        //}

        public override void OnGUI()
        {
            //GUIHelper.Toggle(ref _value, _labelContent, _labelStyle);
            _value = GUIHelper.BoolField(_labelContent, _labelStyle, LabelWidth, _value, Nested);
        }

        public override bool CheckOnGUI()
        {
            //GUIHelper.Toggle(ref _value, _labelContent, _labelStyle);
            bool value = GUIHelper.BoolField(_labelContent, _labelStyle, LabelWidth, _value, Nested);
            if (_value != value)
            {
                _value = value;
                return true;
            }
            return false;
        }
    }

    public class IntGUIController : BaseGUIController<int>
    {
        string _text;

        public override int Value
        {
            set
            {
                _value = value;
                _text = value.ToString();
            }
        }

        public IntGUIController(string label, int value = 0) : base(label, value)
        {
            _text = _value != 0 ? _value.ToString() : "";
        }

        public IntGUIController(string label, string tooltip, int value = 0) : base(label, tooltip, value)
        {
            _text = _value != 0 ? _value.ToString() : "";
        }

        public override void OnGUI()
        {
            var text = GUIHelper.TextField(_labelContent, _labelStyle, LabelWidth, _text, ValueWidth, Nested);
            if (text != _text)
            {
                _text = text;
                _value = text.ToInt();
            }
        }

        public override bool CheckOnGUI()
        {
            var text = GUIHelper.TextField(_labelContent, _labelStyle, LabelWidth, _text, ValueWidth, Nested);
            if (text != _text)
            {
                _text = text;
                int value = text.ToInt();
                if (_value != value)
                {
                    _value = value;
                    return true;
                }
            }
            return false;
        }
    }

    public class FloatGUIController : BaseGUIController<float>
    {
        string _text;

        public override float Value
        {
            set
            {
                _value = value;
                _text = value.ToString();
            }
        }

        public FloatGUIController(string label, float value = 0) : base(label, value)
        {
            _text = _value != 0 ? _value.ToString() : "";
        }

        public FloatGUIController(string label, string tooltip, float value = 0) : base(label, tooltip, value)
        {
            _text = _value != 0 ? _value.ToString() : "";
        }

        public override void OnGUI()
        {
            var text = GUIHelper.TextField(_labelContent, _labelStyle, LabelWidth, _text, ValueWidth, Nested);
            if (text != _text)
            {
                _text = text;
                _value = text.ToFloat();
            }
        }

        public override bool CheckOnGUI()
        {
            var text = GUIHelper.TextField(_labelContent, _labelStyle, LabelWidth, _text, ValueWidth, Nested);
            if (text != _text)
            {
                _text = text;
                float value = text.ToFloat();
                if (_value != value)
                {
                    _value = value;
                    return true;
                }
            }
            return false;
        }
    }

    public class StringGUIController : BaseGUIController<string>
    {
        protected override float DefaultValueWidth => 100;

        public StringGUIController(string label, string value = "") : base(label, value)
        {

        }

        public StringGUIController(string label, string tooltip, string value = "") : base(label, tooltip, value)
        {

        }

        public override void OnGUI()
        {
            //if (!ControlName.IsNullOrEmpty())
            //{
            //    GUIHelper.SetNextControlName(ControlName);
            //}
            _value = GUIHelper.TextField(_labelContent, _labelStyle, LabelWidth, _value, ValueWidth, Nested);
        }

        public override bool CheckOnGUI()
        {
            //if (!ControlName.IsNullOrEmpty())
            //{
            //    GUIHelper.SetNextControlName(ControlName);
            //}
            var value = GUIHelper.TextField(_labelContent, _labelStyle, LabelWidth, _value, ValueWidth, Nested);
            if (_value != value)
            {
                _value = value;
                return true;
            }
            return false;
        }
    }

    public class EnumGUIController<T> : BaseGUIController<T> where T : Enum
    {
        protected override float DefaultValueWidth => GUIHelper.GetEnumWidth<T>();

        public EnumGUIController() : base(Helper.GetNicifyName<T>(), default)
        {

        }

        public EnumGUIController(T value) : base("", value)
        {

        }

        public EnumGUIController(string label, T value = default) : base(label, value)
        {

        }

        public EnumGUIController(string label, string tooltip, T value = default) : base(label, tooltip, value)
        {

        }

        public override void OnGUI()
        {
            _value = GUIHelper.EnumPopup(_labelContent, _labelStyle, LabelWidth, _value, ValueWidth, Nested);
        }

        public override bool CheckOnGUI()
        {
            var value = GUIHelper.EnumPopup(_labelContent, _labelStyle, LabelWidth, _value, ValueWidth, Nested);
            if (!_value.Equals(value))
            {
                _value = value;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Options + Values
    /// </summary>
    public class EnumGUIController2<T> : BaseGUIController<T> where T : Enum
    {
        protected override float DefaultValueWidth => GUIHelper.GetEnumWidth<T>();

        string[] _options;
        int[] _values;

        public EnumGUIController2(string label, string[] options, int[] values) : base(label, (T)(object)values[0])
        {
            _options = options;
            _values = values;
        }

        public override void OnGUI()
        {
            _value = (T)(object)GUIHelper.IntPopup(_labelContent, _labelStyle, LabelWidth, (int)(object)_value, _options, _values, ValueWidth, Nested);
        }

        public override bool CheckOnGUI()
        {
            int intValue = (int)(object)_value;
            var value = GUIHelper.IntPopup(_labelContent, _labelStyle, LabelWidth, intValue, _options, _values, ValueWidth, Nested);
            if (value != intValue)
            {
                _value = (T)(object)value;
                return true;
            }
            return false;
        }
    }

    public class ColorGUIController : BaseGUIController<Color>
    {
        public ColorGUIController(string label, Color? color = null) : base(label, color.HasValue ? color.Value : Color.white)
        {

        }

        public ColorGUIController(string label, string tooltip, Color? color = null) : base(label, tooltip, color.HasValue ? color.Value : Color.white)
        {

        }

        public override void OnGUI()
        {
            _value = GUIHelper.ColorField(_labelContent, _labelStyle, LabelWidth, _value, ValueWidth, Nested);
        }

        public override bool CheckOnGUI()
        {
            var value = GUIHelper.ColorField(_labelContent, _labelStyle, LabelWidth, _value, ValueWidth, Nested);
            if (!_value.Equals(value))
            {
                _value = value;
                return true;
            }
            return false;
        }
    }

    public class SliderColorGUIController : BaseGUIController<Color>
    {
        SliderFloatGUIController _rController = new("R", 1f) { Nested = true };
        SliderFloatGUIController _gController = new("G", 1f) { Nested = true };
        SliderFloatGUIController _bController = new("B", 1f) { Nested = true };

        public override Color Value
        {
            set
            {
                _value = value;
                UpdateControllers();
            }
        }

        public override GUIStyle LabelStyle
        {
            set
            {
                _labelStyle = value;
                _rController.LabelStyle = value;
                _gController.LabelStyle = value;
                _bController.LabelStyle = value;
            }
        }

        public override float ValueWidth => GetGUIWidth(_rController, _gController, _bController);

        public SliderColorGUIController(string label, Color? color = null) : base(label, color.HasValue ? color.Value : Color.white)
        {
            UpdateControllers();
        }

        public SliderColorGUIController(string label, string tooltip, Color? color = null) : base(label, tooltip, color.HasValue ? color.Value : Color.white)
        {
            UpdateControllers();
        }

        void UpdateControllers()
        {
            _rController.Value = _value.r;
            _gController.Value = _value.g;
            _bController.Value = _value.b;
        }

        public override void OnGUI()
        {
            GUIHelper.LayoutLeft(() =>
            {
                GUIHelper.Label(_labelContent, LabelStyle, LabelWidth);
                _rController.OnGUI();
                _gController.OnGUI();
                _bController.OnGUI();

                _value.r = _rController.Value;
                _value.g = _gController.Value;
                _value.b = _bController.Value;
            });
        }
    }

    public class SizeGUIController : BaseGUIController<SizeInt>
    {
        IntGUIController _widthController = new("", 1) { ValueWidth = 30, Nested = true };
        IntGUIController _heightController = new("x", 1) { ValueWidth = 30, Nested = true };

        public override SizeInt Value
        {
            set
            {
                _value = value;
                UpdateControllers();
            }
        }

        public override GUIStyle LabelStyle
        {
            set
            {
                _labelStyle = value;
                _widthController.LabelStyle = value;
                _heightController.LabelStyle = value;
            }
        }

        public override float ValueWidth => GetGUIWidth(_widthController, _heightController);

        public SizeGUIController(string label, SizeInt size = default) : base(label, size)
        {
            UpdateControllers();
        }

        public SizeGUIController(string label, string tooltip, SizeInt size = default) : base(label, tooltip, size)
        {
            UpdateControllers();
        }

        void UpdateControllers()
        {
            _widthController.Value = _value.width;
            _heightController.Value = _value.height;
        }

        protected override void OnGUICallback()
        {
            GUIHelper.Label(_labelContent, LabelStyle, LabelWidth);
            _widthController.OnGUI();
            _heightController.OnGUI();
            _value.width = _widthController.Value;
            _value.height = _heightController.Value;
        }

        protected override bool CheckOnGUICallback()
        {
            bool changed = false;
            GUIHelper.Label(_labelContent, LabelStyle, LabelWidth);
            if (_widthController.CheckOnGUI())
            {
                changed = true;
            }
            if (_heightController.CheckOnGUI())
            {
                changed = true;
            }
            _value.width = _widthController.Value;
            _value.height = _heightController.Value;
            return changed;
        }
    }

    public class RectGUIController : BaseGUIController<Rect>
    {
        FloatGUIController _xController = new("X") { Nested = true };
        FloatGUIController _yController = new("Y", "Bottom") { Nested = true };
        FloatGUIController _widthController = new("W", 100) { Nested = true };
        FloatGUIController _heightController = new("H", 100) { Nested = true };

        public override Rect Value
        {
            set
            {
                _value = value;
                UpdateControllers();
            }
        }

        public override GUIStyle LabelStyle
        {
            set
            {
                _labelStyle = value;
                _xController.LabelStyle = value;
                _yController.LabelStyle = value;
                _widthController.LabelStyle = value;
                _heightController.LabelStyle = value;
            }
        }

        public override float ValueWidth => GetGUIWidth(_xController, _yController, _widthController, _heightController);

        public RectGUIController(string label, Rect? rect = null) : base(label, rect.HasValue ? rect.Value : new Rect(0, 0, 100, 100))
        {
            UpdateControllers();
        }

        public RectGUIController(string label, string tooltip, Rect? rect = null) : base(label, tooltip, rect.HasValue ? rect.Value : new Rect(0, 0, 100, 100))
        {
            UpdateControllers();
        }

        void UpdateControllers()
        {
            _xController.Value = _value.x;
            _yController.Value = _value.y;
            _widthController.Value = _value.width;
            _heightController.Value = _value.height;
        }

        public override void OnGUI()
        {
            GUIHelper.LayoutLeft(() =>
            {
                GUIHelper.Label(_labelContent, LabelStyle, LabelWidth);
                _xController.OnGUI();
                _yController.OnGUI();
                _widthController.OnGUI();
                _heightController.OnGUI();
                _value.x = _xController.Value;
                _value.y = _yController.Value;
                _value.width = _widthController.Value;
                _value.height = _heightController.Value;
            });
        }
    }

    public class LanguageGUIController : BaseGUIController<SystemLanguage>
    {
        protected override float DefaultValueWidth => GUIHelper.GetEnumWidth<SystemLanguage>();

        public SystemLanguage Language
        {
            get => _value;
            set => _value = value;
        }

        public LanguageGUIController(string label, SystemLanguage? language = null) : base(label, language.HasValue ? language.Value : SystemLanguage.English)
        {

        }

        public LanguageGUIController(string label, string tooltip, SystemLanguage? language = null) : base(label, tooltip, language.HasValue ? language.Value : SystemLanguage.English)
        {

        }

        public override void OnGUI()
        {
            _value = GUIHelper.EnumPopup(_labelContent, LabelStyle, LabelWidth, _value, ValueWidth, Nested);
        }

        public override bool CheckOnGUI()
        {
            var value = GUIHelper.EnumPopup(_labelContent, LabelStyle, LabelWidth, _value, ValueWidth, Nested);
            if (_value != value)
            {
                _value = value;
                return true;
            }
            return false;
        }
    }

    //public class ToggleGUIController : GUIController
    //{
    //    bool _value;
    //    GUIStyle _toggleStyle;

    //    public bool Value
    //    {
    //        get => _value;
    //        set => _value = value;
    //    }

    //    public GUIStyle ToggleStyle
    //    {
    //        get => _toggleStyle;
    //        set => _toggleStyle = value;
    //    }

    //    //public Listener<bool> OnValueChanged { get; set; }

    //    public ToggleGUIController(string label, bool defaultValue = false)
    //    {
    //        _label = label;
    //        _value = defaultValue;
    //    }

    //    public override void SetSceneStyle()
    //    {
    //        _toggleStyle = GUIHelper.SceneToggleStyle;
    //    }

    //    public override void OnGUI()
    //    {
    //        _value = GUIHelper.Toggle(_value, _label, _toggleStyle);
    //    }

    //    public void OnGUI(Callback trueCallback, Listener<bool> onValueChanged = null)
    //    {
    //        bool value = GUIHelper.Toggle(_value, _label, _toggleStyle);
    //        if (value != _value)
    //        {
    //            _value = value;
    //            onValueChanged?.Invoke(value);
    //        }
    //        if (_value)
    //        {
    //            GUIHelper.LayoutIndentVertical(trueCallback);
    //        }
    //    }
    //}

    //public class FoldoutGUIController : GUIController
    //{
    //    bool _value;
    //    GUIStyle _foldoutStyle;

    //    public bool Value
    //    {
    //        get => _value;
    //        set => _value = value;
    //    }

    //    public GUIStyle FoldoutStyle
    //    {
    //        get => _foldoutStyle;
    //        set => _foldoutStyle = value;
    //    }

    //    public FoldoutGUIController(string label, bool defaultValue = false)
    //    {
    //        _label = label;
    //        _value = defaultValue;
    //    }

    //    public override void SetSceneStyle()
    //    {
    //        _foldoutStyle = GUIHelper.SceneFoldoutStyle;
    //    }

    //    public override void OnGUI()
    //    {
    //        _value = GUIHelper.Foldout(_value, _label, _foldoutStyle);
    //    }

    //    public void OnGUI(Callback expandCallback)
    //    {
    //        _value = GUIHelper.Foldout(_value, _label, _foldoutStyle);
    //        if (_value)
    //        {
    //            GUIHelper.LayoutIndentVertical(expandCallback, GUIHelper.FoldoutIndent);
    //        }
    //    }
    //}

    public class SliderFloatGUIController : BaseGUIController<float>
    {
        float _min;
        float _max = 1;

        public override float Value
        {
            set => _value = Mathf.Clamp(value, _min, _max);
        }

        /// <summary>
        /// Range [0,1]
        /// </summary>
        public SliderFloatGUIController(string label, float value = 0) : this(label, 0, 1, value)
        {

        }

        public SliderFloatGUIController(string label, float min, float max, float value = 0) : base(label, value)
        {
            _min = min;
            _max = max;
            _value = Mathf.Clamp(value, _min, _max);
        }

        public override void OnGUI()
        {
            _value = GUIHelper.HorizontalSlider(_labelContent, _labelStyle, LabelWidth, _value, _min, _max, ValueWidth, Nested);
        }

        public override bool CheckOnGUI()
        {
            var value = GUIHelper.HorizontalSlider(_labelContent, _labelStyle, LabelWidth, _value, _min, _max, ValueWidth, Nested);
            if (_value != value)
            {
                _value = value;
                return true;
            }
            return false;
        }
    }
}
#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class DebugButtonCycleText : MonoBehaviour, IValue<int>
    {
        ITextController _textController;
        List<IntString> _valueTexts = new();
        int _count;
        int _index;
        int _startValue;
        int _value;
        Listener _onChanged;

        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    for (int i = 0; i < _count; i++)
                    {
                        var item = _valueTexts[i];
                        if (item.Int == value)
                        {
                            _value = value;
                            _textController.Text = item.String;
                            _onChanged?.Invoke();
                            return;
                        }
                    }
                   LegacyLog.Warning($"Value not found: {value}");
                }
            }
        }

        public bool ValueChanged => _value != _startValue;

        void Awake()
        {
            var button = gameObject.GetComponentInChildren<Button>();
            if (button != null)
            {
                button.AddListener(OnClickButton);
            }
            else
            {
               LegacyLog.Warning("Button not found!");
            }
            _textController = TextController.Create(gameObject);
        }

        //void Construct(Listener onChanged)
        //{
        //    _onChanged = onChanged;
        //    _count = _valueTexts.Count;
        //    _index = 0;
        //    var item = _valueTexts[0];
        //    _startValue = item.Int;
        //    _value = _startValue;

        //    // Calculate button size
        //    var rectTransform = transform as RectTransform;
        //    var buttonSize = rectTransform.sizeDelta;
        //    _textController.Text = item.String;
        //    var textSize = _textController.TextSize;
        //    for (int i = 1; i < _count; i++)
        //    {
        //        _textController.Text = _valueTexts[i].String;
        //        textSize.x = Mathf.Max(textSize.x, _textController.TextSize.x);
        //    }
        //    buttonSize.x = Mathf.Max(buttonSize.x, textSize.x - 20);
        //    rectTransform.sizeDelta = buttonSize;

        //    // Set text
        //    _textController.Text = item.String;
        //}

        //public void Construct(int value1, string text1, int value2, string text2, Listener onChanged)
        //{
        //    _valueTexts.Clear();
        //    _valueTexts.Add(new IntString(value1, text1));
        //    _valueTexts.Add(new IntString(value2, text2));
        //    Construct(onChanged);
        //}

        //public void Construct(int value1, string text1, int value2, string text2, int value3, string text3, Listener onChanged)
        //{
        //    _valueTexts.Clear();
        //    _valueTexts.Add(new IntString(value1, text1));
        //    _valueTexts.Add(new IntString(value2, text2));
        //    _valueTexts.Add(new IntString(value3, text3));
        //    Construct(onChanged);
        //}

        //public void Construct(int value1, string text1, int value2, string text2, int value3, string text3, int value4, string text4, Listener onChanged)
        //{
        //    _valueTexts.Clear();
        //    _valueTexts.Add(new IntString(value1, text1));
        //    _valueTexts.Add(new IntString(value2, text2));
        //    _valueTexts.Add(new IntString(value3, text3));
        //    _valueTexts.Add(new IntString(value4, text4));
        //    Construct(onChanged);
        //}

        public void Construct(List<IntString> valueTexts, int value, Listener onChanged)
        {
            _valueTexts = valueTexts;
            _startValue = _value = value;
            _onChanged = onChanged;

            _count = valueTexts.Count;
            _index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (valueTexts[i].Int == value)
                {
                    _index = i;
                    break;
                }
            }

            // Calculate button size
            var layoutElement = gameObject.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                float width = layoutElement.preferredWidth;
                for (int i = 0; i < _count; i++)
                {
                    _textController.Text = _valueTexts[i].String;
                    var textSize = _textController.TextSize;
                    width = Mathf.Max(width, textSize.x);
                }
                layoutElement.preferredWidth = width;
            }

            // Set text
            _textController.Text = _valueTexts[_index].String;
        }

        void OnClickButton()
        {
            SoundManager.PlaySoundButton();
            int nextIndex = (_index + 1) % _count;
            if (nextIndex != _index)
            {
                _index = nextIndex;
                var item = _valueTexts[_index];
                _value = item.Int;
                _textController.Text = item.String;
                _onChanged?.Invoke();
            }
        }
    }
}
#endif
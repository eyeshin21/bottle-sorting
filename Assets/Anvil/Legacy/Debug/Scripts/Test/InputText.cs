using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class InputText : MonoBehaviour
    {
        static readonly char StartChar = '[';
        static readonly char EndChar = ']';
        static readonly string BackgroundColor = "#ffffff";

        TextMeshProUGUI _textUGUI;
        TMP_InputField _inputField;
        TextMeshProUGUI _inputFieldTextUGUI;
        RectTransform _inputFieldRectTransform;
        string _first, _middle, _last, _start;
        Vector2 _startOffsetMin, _startOffsetMax;
        bool _isIndent;
        string _changedText; // Fix lineCount
        string _lastText; // Disable paste

        void Awake()
        {
            _textUGUI = GetComponentInChildren<TextMeshProUGUI>();
            if (_textUGUI == null)
            {
                LegacyLog.Warning("Missing TextMeshProUGUI");
                return;
            }

            _inputField = GetComponentInChildren<TMP_InputField>();
            if (_inputField == null)
            {
                LegacyLog.Warning("Missing TMP_InputField");
                return;
            }
            _inputField.onValueChanged.AddListener(OnTextChanged);
            _inputFieldTextUGUI = _inputField.textComponent.GetComponent<TextMeshProUGUI>();
            _inputFieldRectTransform = _inputField.GetRectTransform();
            _startOffsetMin = _inputFieldRectTransform.offsetMin;
            _startOffsetMax = _inputFieldRectTransform.offsetMax;

            //_textUGUI.AddComponent<DebugRectTransform>();
            //_inputField.AddComponent<DebugRectTransform>().SetColor(Color.blue);
        }

        public void Construct(string text)
        {
            if (_textUGUI == null || _inputField == null)
            {
                LegacyLog.Warning("Missing component");
                return;
            }

            _changedText = null;
            int startIndex = text.IndexOf(StartChar);
            if (startIndex >= 0)
            {
                int endIndex = text.IndexOf(EndChar, startIndex + 1);
                if (endIndex > startIndex)
                {
                    var first = text.GetSubstring(0, startIndex - 1);
                    var last = text.Substring(endIndex + 1);
                    _first = $"{first}<color={BackgroundColor}>";
                    _middle = text.GetSubstring(startIndex + 1, endIndex - 1);
                    _last = $"</color>{last}";
                    _start = $"{first}<u><color={BackgroundColor}>{_middle}</color></u>{last}";
                    _textUGUI.text = _start;
                    if (startIndex > 0)
                    {
                        _isIndent = true;
                        _textUGUI.ForceMeshUpdate();
                        var offsetMin = (_textUGUI.transform as RectTransform).offsetMin;
                        _startOffsetMin.x = _textUGUI.GetCharacterLeft(first.Length) - offsetMin.x;
                    }
                    else
                    {
                        _isIndent = false;
                        _startOffsetMin.x = 0;
                    }
                    _startOffsetMax.y = 0;
                    //Log.Debug($"startOffset={_startOffsetMin}");
                    _inputFieldRectTransform.offsetMin = _startOffsetMin;
                    _inputFieldRectTransform.offsetMax = _startOffsetMax;

                    _inputField.text = "";
                    _inputField.SetShow(true);
                    return;
                }
                else
                {
                    LegacyLog.Warning($"Missing end character '{EndChar}'");
                }
            }
            else
            {
                LegacyLog.Warning($"Missing start character '{StartChar}'");
            }

            _textUGUI.text = text;
            _inputField.SetShow(false);
        }

        void OnTextChanged(string text)
        {
            int length = text.Length;
            if (length > 0)
            {
                bool accept = true;
                char last = text[length - 1];
                if (last == '\t' || last == '\n' || last == '_')
                {
                    accept = false;
                }
                else if (last == ' ')
                {
                    if (length > 1 && text[length - 2] == ' ')
                    {
                        accept = false;
                    }
                }
                if (!accept)
                {
                    //Log.Warning($"Skip '{last}'");
                    _inputField.text = text.Substring(0, length - 1);
                    return;
                }
            }

            if (_isIndent)
            {
                _inputFieldRectTransform.offsetMin = _startOffsetMin;
                _inputFieldRectTransform.offsetMax = _startOffsetMax;

                if (length == 0)
                {
                    _textUGUI.text = _start;
                    _changedText = null;
                }
                else
                {
                    //_inputFieldTextUGUI.ForceMeshUpdate();
                    //if (_inputFieldTextUGUI.textInfo.lineCount > 1)
                    //{
                    //    float lineHeight = _textUGUI.textInfo.lineInfo[0].lineHeight;
                    //    //Log.Debug($"lineHeight={lineHeight}");
                    //    _inputFieldRectTransform.offsetMin = new Vector2(0, _startOffsetMin.y);
                    //    _inputFieldRectTransform.offsetMax = _startOffsetMax.AddDeltaY(-lineHeight);
                    //    _textUGUI.text = $"{_first}\n{text}{_last}";
                    //}
                    //else
                    //{
                    //    _textUGUI.text = $"{_first}{text}{_last}";
                    //}
                    _changedText = text;
                }
            }
            else
            {
                if (length == 0)
                {
                    _textUGUI.text = _start;
                }
                else
                {
                    _textUGUI.text = $"{_first}{text}{_last}";
                }
            }
        }

        void Update()
        {
            // Disable paste
            if (Input.GetKeyDown(KeyCode.V) &&
                (Input.GetKey(KeyCode.LeftCommand) ||
                Input.GetKey(KeyCode.RightCommand) ||
                Input.GetKey(KeyCode.LeftControl) ||
                Input.GetKey(KeyCode.RightControl)))
            {
                _inputField.text = _lastText;
            }
            else
            {
                _lastText = _inputField.text;
            }
        }

        void LateUpdate()
        {
            if (_changedText != null)
            {
                int lineCount = _inputFieldTextUGUI.textInfo.lineCount;
                //Log.Debug($"lineCount={lineCount}");
                if (lineCount > 1)
                {
                    float lineHeight = _textUGUI.textInfo.lineInfo[0].lineHeight;
                    //Log.Debug($"lineHeight={lineHeight}");
                    _inputFieldRectTransform.offsetMin = new Vector2(0, _startOffsetMin.y);
                    _inputFieldRectTransform.offsetMax = _startOffsetMax.AddDeltaY(-lineHeight);
                    _textUGUI.text = $"{_first}\n{_changedText}{_last}";
                }
                else
                {
                    _textUGUI.text = $"{_first}{_changedText}{_last}";
                }
                _changedText = null;
            }
        }

#if UNITY_EDITOR
        [SerializeField] string _text = "A: [Where] are you from?";

        void Start()
        {
            Test();
        }

        [ContextMenu("Test")]
        void Test()
        {
            if (!string.IsNullOrWhiteSpace(_text))
            {
                Construct(_text);
            }
        }

        void SetInputField(TMP_InputField inputField)
        {
            var tmpText = GetComponent<TMP_Text>();
            if (tmpText == null)
            {
                LegacyLog.Warning("TMP_Text not found");
                return;
            }

            // Set pivot top-left + top stretch
            var rectTransform = inputField.GetRectTransform();
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2(0, -1000);
            rectTransform.offsetMax = Vector2.zero;

            // Disable image
            inputField.DisableComponent<Image>();

            // Set text
            inputField.fontAsset = tmpText.font;
            inputField.pointSize = tmpText.fontSize;
            inputField.lineType = TMP_InputField.LineType.MultiLineNewline;

            // Text area
            var textArea = inputField.transform.GetChild("Text Area");
            if (textArea != null)
            {
                var rt = textArea.GetRectTransform();
                rt.offsetMin = rt.offsetMax = Vector2.zero;
                textArea.DisableComponent<RectMask2D>();

                // Placeholder
                var placeholder = textArea.GetChildComponent<TMP_Text>("Placeholder");
                if (placeholder != null)
                {
                    placeholder.text = "";
                }

                // Text
                var text = textArea.GetChildComponent<TMP_Text>("Text");
                if (text != null)
                {
                    text.fontStyle = FontStyles.Underline;
                    text.color = new Color(0, 0, 0.75f);
                }
            }
            else
            {
                LegacyLog.Warning("Text Area not found");
            }
        }

        [MenuItem("CONTEXT/InputText/Set InputField")]
        static void InputTextSetInputField(MenuCommand menuCommand)
        {
            var inputText = menuCommand.To<InputText>();
            var inputField = inputText.GetComponentInChildren<TMP_InputField>();
            if (inputField == null)
            {
                LegacyLog.Warning("TMP_InputField not found");
                return;
            }
            EditorHelper.Set(inputField, "Set InputField", () => inputText.SetInputField(inputField));
        }
#endif
    }
}
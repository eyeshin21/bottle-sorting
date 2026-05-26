using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        static bool _isInited;
        public static void LazyInit()
        {
            if (_isInited) return;
            _isInited = true;

            int fontSize = GetFontSize(32);
            GUI.skin.label.fontSize = fontSize;
            GUI.skin.textField.fontSize = fontSize;
            GUI.skin.toggle.fontSize = fontSize;
            GUI.skin.button.fontSize = fontSize;
        }

        static int GetFontSize(int fontSize)
        {
#if UNITY_EDITOR
            return fontSize;
#else
            return (int)(fontSize * (Screen.dpi / 326f));
#endif
        }

        static GUIContent _content = new GUIContent();
        public static GUIStyle LabelStyle => GUI.skin.label;
        public static GUIStyle ButtonStyle => GUI.skin.button;

        static Rect? _screenRect;
        public static Rect ScreenRect
        {
            get
            {
                if (!_screenRect.HasValue)
                {
                    var notchHeight = NotchHeight;
                    _screenRect = new Rect(0, notchHeight, Screen.width, Screen.height - notchHeight);
                }
                return _screenRect.Value;
            }
        }

        public static float LineHeight
        {
            get
            {
#if UNITY_EDITOR
                return EditorGUIUtility.singleLineHeight;
#else
                return 18;
#endif
            }
        }

        public static GUIContent GetContent(string label, out float labelWidth)
        {
            if (label.IsNullOrEmpty())
            {
                labelWidth = 0;
                return GUIContent.none;
            }

            var content = new GUIContent(label);
            labelWidth = GetLabelWidth(content);
            return content;
        }

        public static void OnGUI(Callback callback)
        {
            LazyInit();
            GUILayout.BeginArea(ScreenRect);
            {
                callback();
            }
            GUILayout.EndArea();
        }

        public static void OnGUILeft(Callback callback)
        {
            LazyInit();
            GUILayout.BeginArea(ScreenRect);
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    callback();
                    GUILayout.EndVertical();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        public static void Label(Texture image, string text, ImageLabelParams @params = null)
        {
            if (image == null)
            {
                Label(text);
                return;
            }

            //GUILayout.Label(new GUIContent(text, image));
            _Label(image, text, GUI.skin.label, @params);
        }

        public static void Label(Texture image, string text, GUIStyle textStyle, ImageLabelParams @params = null)
        {
            if (image == null)
            {
                Label(text, textStyle);
                return;
            }

            //GUILayout.Label(new GUIContent(text, image), textStyle);
            _Label(image, text, textStyle, @params);
        }

        static void _Label(Texture image, string text, GUIStyle textStyle, ImageLabelParams @params)
        {
            float leftPadding = 0;
            float space = 0;
            GUIDebugger debugger = null;
            if (@params != null)
            {
                leftPadding = @params.LeftPadding;
                space = @params.Space;
                debugger = @params.Debugger;
            }

            var imageWidth = image.width;
            var imageHeight = image.height;
            _content.text = text;
            var textSize = textStyle.CalcSize(_content);
            float textWidth = textSize.x;
            float textHeight = textSize.y;
            float width = leftPadding + imageWidth + space + textWidth;
            float height = Mathf.Max(imageHeight, textHeight);

            GUILayout.BeginHorizontal(GUILayout.Width(width), GUILayout.Height(height));
            {
                if (leftPadding != 0)
                {
                    GUILayout.Space(leftPadding);
                }

                //GUILayout.Label(image, LabelMiddleLeftStyle, GUILayout.Width(imageWidth), GUILayout.Height(imageHeight));
                //if (imageHeight < height)
                //{
                //    // Middle
                //    GUILayout.BeginVertical(GUILayout.Height(height));
                //    {
                //        GUILayout.FlexibleSpace();
                //        GUILayout.Label(image, GUILayout.Width(imageWidth), GUILayout.Height(imageHeight));
                //        debugger?.AddLastRect();
                //        GUILayout.FlexibleSpace();
                //    }
                //    GUILayout.EndVertical();
                //}
                //else
                {
                    GUILayout.Label(image, GUILayout.Width(imageWidth), GUILayout.Height(imageHeight));
                    debugger?.AddLastRect();
                }
                if (space != 0)
                {
                    GUILayout.Space(space);
                }
                GUILayout.Label(_content, textStyle, GUILayout.Width(textWidth), GUILayout.Height(height));
                debugger?.AddLastRect();
            }
            GUILayout.EndHorizontal();
            //debugger?.AddLastRect();
        }

        public static void FittedLabel(string text, GUIStyle style)
        {
            _content.text = text;
            var size = style.CalcSize(_content);
            GUILayout.Label(_content, style, GUILayout.Width(size.x));
        }

        public static bool BoolField(GUIContent labelContent, GUIStyle labelStyle, float labelWidth, bool value, bool nested = false)
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }
            Label(labelContent, labelStyle, labelWidth);
            value = GUILayout.Toggle(value, GUIContent.none, GUILayout.Width(ToggleWidth));
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }
            return value;
        }

        public static string TextField(string text, float textWidth)
        {
            if (textWidth > 0)
            {
                return GUILayout.TextField(text, GUILayout.Width(textWidth));
            }
            if (textWidth < 0)
            {
                return GUILayout.TextField(text, GUILayout.ExpandWidth(true));
            }
            return GUILayout.TextField(text);
        }

        public static string TextField(string label, float labelWidth, string text, float textWidth, bool nested = false)
        {
            return TextField(label, null, labelWidth, text, textWidth, nested);
        }

        public static string TextField(string label, GUIStyle labelStyle, float labelWidth, string text, float textWidth, bool nested = false)
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }
            Label(label, labelStyle, labelWidth);
            text = TextField(text, textWidth);
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return text;
        }

        public static string TextField(GUIContent labelContent, GUIStyle labelStyle, float labelWidth, string text, float textWidth, bool nested = false)
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }
            Label(labelContent, labelStyle, labelWidth);
            text = TextField(text, textWidth);
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return text;
        }

        static Color ColorField(Color color, float colorWidth)
        {
#if UNITY_EDITOR
            if (colorWidth > 0)
            {
                return EditorGUILayout.ColorField(color, GUILayout.Width(colorWidth));
            }
            if (colorWidth < 0)
            {
                return EditorGUILayout.ColorField(color, GUILayout.ExpandWidth(true));
            }
            return EditorGUILayout.ColorField(color);
#else
            return color;
#endif
        }

        public static Color ColorField(GUIContent labelContent, GUIStyle labelStyle, float labelWidth, Color color, float colorWidth, bool nested = false)
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }
            Label(labelContent, labelStyle, labelWidth);
            color = ColorField(color, colorWidth);
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return color;
        }

        static float HorizontalSlider(float value, float min, float max, float sliderWidth)
        {
            if (sliderWidth > 0)
            {
                return GUILayout.HorizontalSlider(value, min, max, GUILayout.Width(sliderWidth));
            }
            if (sliderWidth < 0)
            {
                return GUILayout.HorizontalSlider(value, min, max, GUILayout.ExpandWidth(true));
            }
            return GUILayout.HorizontalSlider(value, min, max);
        }

        public static float HorizontalSlider(GUIContent labelContent, GUIStyle labelStyle, float labelWidth, float value, float min, float max, float sliderWidth, bool nested = false)
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }
            Label(labelContent, labelStyle, labelWidth);
            value = HorizontalSlider(value, min, max, sliderWidth);
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return value;
        }

        static GUIStyle _searchStyle;
        static GUIStyle SearchStyle => _searchStyle ??=
#if UNITY_EDITOR
            EditorStyles.toolbarSearchField;
#else
            CreateTextFieldStyle();
#endif

        public static void Search(ref string text)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(7);
                text = GUILayout.TextField(text, SearchStyle, GUILayout.MaxWidth(200));
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public static string SearchField(string text, float textWidth = -1)
        {
#if UNITY_EDITOR
            //GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            {
                var style = SearchStyle;
                if (textWidth > 0)
                {
                    text = GUILayout.TextField(text, style, GUILayout.Width(textWidth));
                }
                else if (textWidth < 0)
                {
                    text = GUILayout.TextField(text, style, GUILayout.ExpandWidth(true));
                }
                else
                {
                    text = GUILayout.TextField(text, style);
                }

                //if (GUILayout.Button("x", EditorStyles.toolbarButton))
                if (GUILayout.Button("x", GUILayout.Width(18), GUILayout.Height(16)))
                {
                    text = "";
                    // Remove focus if cleared
                    GUI.FocusControl(null);
                }
            }
            GUILayout.EndHorizontal();
            return text;
#else
            return TextField(text, textWidth);
#endif
        }

        public static int IntPopup(int value, string[] options, int[] values, float width, bool nested = false)
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }

            int newValue;
#if UNITY_EDITOR
            if (width > 0)
            {
                newValue = EditorGUILayout.IntPopup(value, options, values, GUILayout.Width(width));
            }
            else if (width < 0)
            {
                newValue = EditorGUILayout.IntPopup(value, options, values, GUILayout.ExpandWidth(true));
            }
            else
            {
                newValue = EditorGUILayout.IntPopup(value, options, values);
            }
#else
            GUILayout.Label($"Todo: {value}");
            newValue = value;
#endif
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return newValue;
        }

        public static int IntPopup(GUIContent labelContent, GUIStyle labelStyle, float labelWidth, int value, string[] options, int[] values, float width, bool nested = false)
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }

            if (labelContent != null)
            {
                if (labelWidth > 0)
                {
                    if (labelStyle != null)
                    {
                        GUILayout.Label(labelContent, labelStyle, GUILayout.Width(labelWidth));
                    }
                    else
                    {
                        GUILayout.Label(labelContent, GUILayout.Width(labelWidth));
                    }
                }
                else
                {
                    if (labelStyle != null)
                    {
                        GUILayout.Label(labelContent, labelStyle);
                    }
                    else
                    {
                        GUILayout.Label(labelContent);
                    }
                }
            }

            int newValue;
#if UNITY_EDITOR
            if (width > 0)
            {
                newValue = EditorGUILayout.IntPopup(value, options, values, GUILayout.Width(width));
            }
            else if (width < 0)
            {
                newValue = EditorGUILayout.IntPopup(value, options, values, GUILayout.ExpandWidth(true));
            }
            else
            {
                newValue = EditorGUILayout.IntPopup(value, options, values);
            }
#else
            GUILayout.Label($"Todo: {value}");
            newValue = value;
#endif
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return newValue;
        }

        public static T EnumPopup<T>(T value, float enumWidth, bool nested = false) where T : Enum
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }

            T newValue;
#if UNITY_EDITOR
            if (enumWidth > 0)
            {
                newValue = (T)EditorGUILayout.EnumPopup(value, GUILayout.Width(enumWidth));
            }
            else if (enumWidth < 0)
            {
                newValue = (T)EditorGUILayout.EnumPopup(value, GUILayout.ExpandWidth(true));
            }
            else
            {
                newValue = (T)EditorGUILayout.EnumPopup(value);
            }
#else
            GUILayout.Label($"Todo: {value}");
            newValue = value;
#endif
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return newValue;
        }

        public static T EnumPopup<T>(string label, float labelWidth, T value, float enumWidth, bool nested = false) where T : Enum
        {
            return EnumPopup(label, null, labelWidth, value, enumWidth, nested);
        }

        public static T EnumPopup<T>(string label, GUIStyle labelStyle, float labelWidth, T value, float enumWidth, bool nested = false) where T : Enum
        {
            return EnumPopup(new GUIContent(label), labelStyle, labelWidth, value, enumWidth, nested);
        }

        public static T EnumPopup<T>(GUIContent labelContent, GUIStyle labelStyle, float labelWidth, T value, float enumWidth, bool nested = false) where T : Enum
        {
            if (!nested)
            {
                GUILayout.BeginHorizontal();
            }

            if (labelContent != null)
            {
                if (labelWidth > 0)
                {
                    if (labelStyle != null)
                    {
                        GUILayout.Label(labelContent, labelStyle, GUILayout.Width(labelWidth));
                    }
                    else
                    {
                        GUILayout.Label(labelContent, GUILayout.Width(labelWidth));
                    }
                }
                else
                {
                    if (labelStyle != null)
                    {
                        GUILayout.Label(labelContent, labelStyle);
                    }
                    else
                    {
                        GUILayout.Label(labelContent);
                    }
                }
            }

            T newValue;
#if UNITY_EDITOR
            if (enumWidth > 0)
            {
                newValue = (T)EditorGUILayout.EnumPopup(value, GUILayout.Width(enumWidth));
            }
            else if (enumWidth < 0)
            {
                newValue = (T)EditorGUILayout.EnumPopup(value, GUILayout.ExpandWidth(true));
            }
            else
            {
                newValue = (T)EditorGUILayout.EnumPopup(value);
            }
#else
            GUILayout.Label($"Todo: {value}");
            newValue = value;
#endif
            if (!nested)
            {
                GUILayout.EndHorizontal();
            }

            return newValue;
        }

        public static void Rect(float x, float y, float width, float height, Color color)
        {
            Rect(new Rect(x, y, width, height), color);
        }

        public static void Rect(Rect rect, Color color)
        {
            //var guiColor = GUI.color;
            //GUI.color = color;
            //GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            //GUI.color = guiColor;

            GUI.DrawTexture(rect, GetBackground(color));
        }

        public static void SetNextControlName(string name)
        {
            GUI.SetNextControlName(name);
        }

        public static void FocusControl(string name)
        {
            GUI.FocusControl(name);
        }

        public static void BeginChangeCheck()
        {
#if UNITY_EDITOR
            EditorGUI.BeginChangeCheck();
#endif
        }

        public static bool EndChangeCheck()
        {
#if UNITY_EDITOR
            return EditorGUI.EndChangeCheck();
#else
            return false;
#endif
        }
    }
}
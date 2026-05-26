using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        const float ToggleIndent = 18;
        public static readonly float FoldoutIndent = 15;
        static readonly float ListButtonWidth = 20;
        public static readonly float GUISpace = 2;
        static readonly GUILayoutOption LayoutListButtonWidth = GUILayout.Width(ListButtonWidth);

        public static readonly GUIContent MoveUpContent = new("\u2191");
        public static readonly GUIContent MoveDownContent = new("\u2193");

        //static readonly RectOffset RectOffsetTopBottom = new RectOffset(0, 0, 1, 1);

        static Dictionary<Color, Texture2D> _colorBackgrounds = new Dictionary<Color, Texture2D>();
        public static Texture2D GetBackground(int r, int g, int b) => GetBackground(Helper.CreateColor(r, g, b));
        public static Texture2D GetBackground(Color color) => _colorBackgrounds.GetValue(color, () => TextureHelper.CreateTexture(1, 1, color));

        public static void LayoutHorizontal(Callback callback)
        {
            GUILayout.BeginHorizontal();
            callback?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void LayoutLeft(Callback callback)
        {
            GUILayout.BeginHorizontal();
            callback?.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static T LayoutLeft<T>(Func<T> func)
        {
            GUILayout.BeginHorizontal();
            T result = func();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return result;
        }

        public static void LayoutLeft(float spacing, Callback callback)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spacing);
            callback?.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static T LayoutLeft<T>(float spacing, Func<T> func)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spacing);
            T result = func();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return result;
        }

        public static void LayoutLeftVertical(Callback callback)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            callback?.Invoke();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static void LayoutIndent(Callback callback, float indent = ToggleIndent)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(indent);
                callback?.Invoke();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public static void LayoutIndentVertical(Callback callback, float indent = ToggleIndent)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(indent);
                GUILayout.BeginVertical();
                {
                    callback?.Invoke();
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public static void LayoutCenter(Callback callback)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            callback?.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static T LayoutCenter<T>(Func<T> func)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            T result = func();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return result;
        }

        public static void LayoutRight(Callback callback)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                callback?.Invoke();
            }
            GUILayout.EndHorizontal();
        }

        public static void LayoutRight(float padding, Callback callback)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                callback?.Invoke();
                GUILayout.Space(padding);
            }
            GUILayout.EndHorizontal();
        }

        public static void LayoutLeftRight(Callback leftCallback, Callback rightCallback)
        {
            GUILayout.BeginHorizontal();
            {
                leftCallback?.Invoke();
                GUILayout.FlexibleSpace();
                rightCallback?.Invoke();
            }
            GUILayout.EndHorizontal();
        }

        public static void LayoutVertical(Callback callback)
        {
            GUILayout.BeginVertical();
            {
                callback?.Invoke();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutTop(Callback callback)
        {
            GUILayout.BeginVertical();
            {
                callback?.Invoke();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutTopCenter(Callback callback)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    callback?.Invoke();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutTop(float padding, Callback callback)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(padding);
                callback?.Invoke();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutBottom(Callback callback)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();
                callback?.Invoke();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutBottom(float padding, Callback callback)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();
                callback?.Invoke();
                GUILayout.Space(padding);
            }
            GUILayout.EndVertical();
        }

        public static void LayoutVertical(GUIStyle style, Callback callback)
        {
            GUILayout.BeginVertical(style);
            {
                callback?.Invoke();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutVertical(float width, Callback callback)
        {
            GUILayout.BeginVertical(GUILayout.Width(width));
            {
                callback?.Invoke();
            }
            GUILayout.EndVertical();
        }

        //public static void LayoutVertical(float padding, Callback callback)
        //{
        //    GUILayout.BeginHorizontal();
        //    {
        //        GUILayout.Space(padding);
        //        GUILayout.BeginVertical();
        //        {
        //            callback?.Invoke();
        //        }
        //        GUILayout.EndVertical();
        //        GUILayout.FlexibleSpace();
        //    }
        //    GUILayout.EndHorizontal();
        //}

        public static void LayoutVertical(float width, ref Vector2 scrollPos, Callback callback)
        {
            GUILayout.BeginVertical(GUILayout.Width(width));
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                {
                    callback?.Invoke();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutVertical(ref Vector2 scrollPos, Callback callback)
        {
            GUILayout.BeginVertical();
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                {
                    callback?.Invoke();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutVertical(ref Vector2 scrollPos, GUIStyle style, Callback callback, float width)
        {
            LayoutVertical(ref scrollPos, style, callback, GUILayout.Width(width));
        }

        public static void LayoutVertical(ref Vector2 scrollPos, GUIStyle style, Callback callback, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                {
                    callback?.Invoke();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        public static void LayoutInspectorBottom(Callback callback)
        {
            Line();
            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                callback?.Invoke();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public static void Toggle(string text, ref bool value, params GUILayoutOption[] options)
        {
            value = GUILayout.Toggle(value, text, options);
        }

        public static bool Toggle(bool value, string text, params GUILayoutOption[] options)
        {
            return GUILayout.Toggle(value, text, options);
        }

        public static void Toggle(ref bool value, string text, params GUILayoutOption[] options)
        {
            value = GUILayout.Toggle(value, text, options);
        }

        public static bool Toggle(bool value, string text, GUIStyle style, params GUILayoutOption[] options)
        {
            if (style == null)
            {
                return GUILayout.Toggle(value, text, options);
            }
            return GUILayout.Toggle(value, text, style, options);
        }

        public static void Toggle(ref bool value, string text, GUIStyle style, params GUILayoutOption[] options)
        {
            if (style == null)
            {
                value = GUILayout.Toggle(value, text, options);
            }
            else
            {
                value = GUILayout.Toggle(value, text, style, options);
            }
        }

        public static void Toggle(ref bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            if (style == null)
            {
                value = GUILayout.Toggle(value, content, options);
            }
            else
            {
                value = GUILayout.Toggle(value, content, style, options);
            }
        }

        /// <summary>
        /// Returns true if value changed.
        /// </summary>
        public static bool CheckToggle(ref bool value, string text, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, text, options);
            if (newValue != value)
            {
                value = newValue;
                return true;
            }
            return false;
        }

        public static bool Foldout(bool foldout, string text, GUIStyle style)
        {
#if UNITY_EDITOR
            return style != null ? EditorGUILayout.Foldout(foldout, text, true, style) : EditorGUILayout.Foldout(foldout, text, true);
#else
            return false;
#endif
        }

        public static void Space(float pixels)
        {
            GUILayout.Space(pixels);
        }

        static GUIStyle _separatorStyle;
        static GUIStyle SeparatorStyle => _separatorStyle ??= CreateLabelStyle().SetMarginHorizontal(0);

        public static void Separator()
        {
            GUILayout.Label("|", SeparatorStyle);
        }

        static GUIStyle _lineStyle;
        static GUIStyle LineStyle
        {
            get
            {
                if (_lineStyle == null)
                {
                    var style = new GUIStyle(GUI.skin.box);
                    var rectOffset = new RectOffset(0, 0, 1, 1);
                    style.border = rectOffset;
                    style.margin = rectOffset;
                    style.padding = rectOffset;
                    _lineStyle = style;
                }
                return _lineStyle;
            }
        }

        static Texture2D _lineBackground;
        static Texture2D LineBackground => _lineBackground ??= TextureHelper.CreateTexture(1, 1, new Color(0.5f, 0.5f, 0.5f, 0.9f));

        public static void Line(float width = -1, float height = 1, Texture2D background = null)
        {
            //GUILayout.Label("", GUI.skin.horizontalScrollbar);

            var content = GUIContent.none;
            var lineStyle = LineStyle;
            lineStyle.normal.background = background ?? LineBackground;

            if (width < 0)
            {
                GUILayout.Box(content, lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(height));
            }
            else if (width > 0)
            {
                GUILayout.Box(content, lineStyle, GUILayout.Width(width), GUILayout.Height(height));
            }
            else
            {
                GUILayout.Box(content, lineStyle, GUILayout.Height(height));
            }
        }

        static Texture2D _tabLineBackground;
        static Texture2D TabLineBackground => _tabLineBackground ??= TextureHelper.CreateTexture(1, 1, Color.yellow);

        public static void TabLine()
        {
            Line(-1, 3, TabLineBackground);
        }

        //static GUIStyle _colorBoxStyle;
        //static GUIStyle ColorBoxStyle => _colorBoxStyle ??= CreateBoxStyle();

        public static void LayoutBox(Rect rect, Color color)
        {
            //var style = ColorBoxStyle;
            //style.normal.background = GetBackground(color);
            //GUI.Box(rect, GUIContent.none, style);

            var guiColor = GUI.color;
            GUI.color = color;
            GUI.Box(rect, GUIContent.none);
            GUI.color = guiColor;
        }

        static GUIStyle _boxStyle;
        static Texture2D _boxBackground;
        public static void Box(Rect rect)
        {
            if (_boxStyle == null)
            {
                _boxStyle = new GUIStyle(GUI.skin.box);
            }

            if (_boxBackground == null)
            {
                _boxBackground = TextureHelper.CreateTexture(1, 1, new Color(0, 0, 0, 0.75f));
                _boxStyle.normal.background = _boxBackground;
            }

            GUI.Box(rect, "", _boxStyle);
        }

        public static void LayoutList<T>(string label, List<T> list, Callback copyPasteCallback) where T : IGUIController
        {
            LayoutList(label, list, () => Helper.GetDefault<T>(), 0, copyPasteCallback);
        }

        public static void LayoutList<T>(string label, List<T> list, float itemWidth = 0, Callback copyPasteCallback = null) where T : IGUIController
        {
            LayoutList(label, list, () => Helper.GetDefault<T>(), itemWidth, copyPasteCallback);
        }

        public static void LayoutList<T>(string label, List<T> list, Func<T> newFunc, float itemWidth = 0, Callback copyPasteCallback = null) where T : IGUIController
        {
            if (list.Count == 0)
            {
                GUILayout.BeginHorizontal();
                {
                    Label(label);
                    if (Button(AddContent, LayoutListButtonWidth))
                    {
                        list.Add(newFunc());
                    }
                    copyPasteCallback?.Invoke();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    Label(label);
                    copyPasteCallback?.Invoke();
                    if (ButtonClear())
                    {
                        list.Clear();
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            int count = list.Count;
            if (count == 0) return;

            float width = itemWidth;
            if (width > 0)
            {
                width += (ListButtonWidth + GUISpace) * 4;
            }

            bool guiEnabled = GUI.enabled;
            for (int i = 0; i < count; i++)
            {
                var item = list[i];
                if (width > 0)
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(width));
                }
                else
                {
                    GUILayout.BeginHorizontal();
                }
                {
                    // Move up
                    GUI.enabled = guiEnabled && i > 0;
                    if (Button(MoveUpContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i - 1, item);
                    }

                    // Move down
                    GUI.enabled = guiEnabled && i < count - 1;
                    if (Button(MoveDownContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i + 1, item);
                    }

                    // Item
                    GUI.enabled = guiEnabled;
                    item.OnGUI();

                    // Add
                    if (Button(AddContent, LayoutListButtonWidth))
                    {
                        var data = newFunc();
                        if (i < count - 1)
                        {
                            list.Insert(i + 1, data);
                        }
                        else
                        {
                            list.Add(data);
                        }
                    }

                    // Remove
                    if (Button(RemoveContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        GUILayout.EndHorizontal();
                        break;
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUI.enabled = guiEnabled;
        }

        public static void LayoutList<T>(ref Vector2 scrollPos, string label, List<T> list, Callback copyPasteCallback) where T : IGUIController
        {
            LayoutList(ref scrollPos, label, list, () => Helper.GetDefault<T>(), 0, copyPasteCallback);
        }

        public static void LayoutList<T>(ref Vector2 scrollPos, string label, List<T> list, float itemWidth = 0, Callback copyPasteCallback = null) where T : IGUIController
        {
            LayoutList(ref scrollPos, label, list, () => Helper.GetDefault<T>(), itemWidth, copyPasteCallback);
        }

        public static void LayoutList<T>(ref Vector2 scrollPos, string label, List<T> list, Func<T> newFunc, float itemWidth = 0, Callback copyPasteCallback = null) where T : IGUIController
        {
            if (list.Count == 0)
            {
                GUILayout.BeginHorizontal();
                {
                    Label(label);
                    if (Button(AddContent, LayoutListButtonWidth))
                    {
                        list.Add(newFunc());
                    }
                    copyPasteCallback?.Invoke();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    Label(label);
                    copyPasteCallback?.Invoke();
                    if (ButtonClear())
                    {
                        list.Clear();
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            int count = list.Count;
            if (count == 0) return;

            float width = itemWidth;
            if (width > 0)
            {
                width += (ListButtonWidth + GUISpace) * 4;
            }

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            bool guiEnabled = GUI.enabled;
            for (int i = 0; i < count; i++)
            {
                var item = list[i];
                if (width > 0)
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(width));
                }
                else
                {
                    GUILayout.BeginHorizontal();
                }
                {
                    // Move up
                    GUI.enabled = guiEnabled && i > 0;
                    if (Button(MoveUpContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i - 1, item);
                    }

                    // Move down
                    GUI.enabled = guiEnabled && i < count - 1;
                    if (Button(MoveDownContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i + 1, item);
                    }

                    // Item
                    GUI.enabled = guiEnabled;
                    Separator();
                    item.OnGUI();
                    Separator();

                    // Add
                    if (Button(AddContent, LayoutListButtonWidth))
                    {
                        var data = newFunc();
                        if (i < count - 1)
                        {
                            list.Insert(i + 1, data);
                        }
                        else
                        {
                            list.Add(data);
                        }
                    }

                    // Remove
                    if (Button(RemoveContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        GUILayout.EndHorizontal();
                        break;
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUI.enabled = guiEnabled;
            GUILayout.EndScrollView();
        }

        public static bool CheckLayoutList<T>(string label, List<T> list, Callback copyPasteCallback) where T : ICheckGUIController
        {
            return CheckLayoutList(label, list, () => Helper.GetDefault<T>(), 0, copyPasteCallback);
        }

        public static bool CheckLayoutList<T>(string label, List<T> list, float itemWidth = 0, Callback copyPasteCallback = null) where T : ICheckGUIController
        {
            return CheckLayoutList(label, list, () => Helper.GetDefault<T>(), itemWidth, copyPasteCallback);
        }

        public static bool CheckLayoutList<T>(string label, List<T> list, Func<T> newFunc, float itemWidth = 0, Callback copyPasteCallback = null) where T : ICheckGUIController
        {
            bool changed = false;
            if (list.Count == 0)
            {
                GUILayout.BeginHorizontal();
                {
                    Label(label);
                    if (Button(AddContent, LayoutListButtonWidth))
                    {
                        list.Add(newFunc());
                        changed = true;
                    }
                    copyPasteCallback?.Invoke();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    Label(label);
                    copyPasteCallback?.Invoke();
                    if (ButtonClear())
                    {
                        list.Clear();
                        changed = true;
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            int count = list.Count;
            if (count == 0) return changed;

            float width = itemWidth;
            if (width > 0)
            {
                width += (ListButtonWidth + GUISpace) * 4;
            }

            bool guiEnabled = GUI.enabled;
            for (int i = 0; i < count; i++)
            {
                var item = list[i];
                if (width > 0)
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(width));
                }
                else
                {
                    GUILayout.BeginHorizontal();
                }
                {
                    // Move up
                    GUI.enabled = guiEnabled && i > 0;
                    if (Button(MoveUpContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i - 1, item);
                        changed = true;
                    }

                    // Move down
                    GUI.enabled = guiEnabled && i < count - 1;
                    if (Button(MoveDownContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        list.Insert(i + 1, item);
                        changed = true;
                    }

                    // Item
                    GUI.enabled = guiEnabled;
                    Separator();
                    if (item.CheckOnGUI())
                    {
                        changed = true;
                    }
                    Separator();

                    // Add
                    if (Button(AddContent, LayoutListButtonWidth))
                    {
                        var data = newFunc();
                        if (i < count - 1)
                        {
                            list.Insert(i + 1, data);
                        }
                        else
                        {
                            list.Add(data);
                        }
                        changed = true;
                    }

                    // Remove
                    if (Button(RemoveContent, LayoutListButtonWidth))
                    {
                        list.RemoveAt(i);
                        changed = true;
                        GUILayout.EndHorizontal();
                        break;
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUI.enabled = guiEnabled;
            return changed;
        }

        public static bool GetLastRect(out Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                rect = GUILayoutUtility.GetLastRect();
                return true;
            }
            rect = default;
            return false;
        }

        public static bool GetLastRect(float margins, out Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                rect = GUILayoutUtility.GetLastRect();
                rect.x -= margins;
                rect.y -= margins;
                rect.width += margins * 2;
                rect.height += margins * 2;
                return true;
            }
            rect = default;
            return false;
        }
    }
}
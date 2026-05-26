#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Anvil.Legacy
{
    public class WindowBehaviour<T> : EditorWindow where T : EditorWindow
    {
        protected bool _isInited;

        protected float WindowWidth
        {
            get => position.width;
            set
            {
                var rect = position;
                rect.width = value;
                position = rect;
            }
        }

        protected float WindowHeight
        {
            get => position.height;
            set
            {
                var rect = position;
                rect.height = value;
                position = rect;
            }
        }

        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GetWindow<T>();
                    //Log.Warning(_instance);
                }
                return _instance;
            }
        }

        protected virtual void OnEnable()
        {
            //LogDebug("Enable");
            if (!_isInited)
            {
                _isInited = true;
                Init();
            }
        }

        protected virtual void Init()
        {
            //LogDebug("Init");
        }

        protected void SetWindowSize(float width, float height)
        {
            var rect = position;
            rect.width = width;
            rect.height = height;
            position = rect;
        }

        #region Get/Set
        protected static bool GetBool(string key, bool defaultValue = false)
        {
            return EditorPrefs.GetBool(key, defaultValue);
        }

        protected static void SetBool(string key, bool value)
        {
            EditorPrefs.SetBool(key, value);
        }

        protected static int GetInt(string key, int defaultValue = 0)
        {
            return EditorPrefs.GetInt(key, defaultValue);
        }

        protected static void SetInt(string key, int value)
        {
            EditorPrefs.SetInt(key, value);
        }

        protected static float GetFloat(string key, float defaultValue = 0)
        {
            return EditorPrefs.GetFloat(key, defaultValue);
        }

        protected static void SetFloat(string key, float value)
        {
            EditorPrefs.SetFloat(key, value);
        }

        protected static string GetString(string key, string defaultValue = "")
        {
            return EditorPrefs.GetString(key, defaultValue);
        }

        protected static void SetString(string key, string value)
        {
            EditorPrefs.SetString(key, value);
        }
        #endregion

        #region GUI
        protected static void LayoutHorizontal(Callback callback)
        {
            GUIHelper.LayoutHorizontal(callback);
        }

        protected static void Label(string text, float labelWidth)
        {
            GUIHelper.Label(text, labelWidth);
        }

        protected static void Label(string text, GUIStyle style, float labelWidth)
        {
            GUIHelper.Label(text, style, labelWidth);
        }

        protected static void Label(string text, params GUILayoutOption[] options)
        {
            GUILayout.Label(text, options);
        }

        protected static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUIHelper.Label(text, style, options);
        }

        protected static void Label(object label, params GUILayoutOption[] options)
        {
            GUILayout.Label($"{label}", options);
        }

        protected static bool Button(string text, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, options);
        }

        protected static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, style, options);
        }

        protected static bool Button(string text, ref float buttonWidth)
        {
            return GUIHelper.Button(text, ref buttonWidth);
        }

        protected static bool Button(GUIContent content, params GUILayoutOption[] options)
        {
            return GUILayout.Button(content, options);
        }

        protected static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            return GUILayout.Button(content, style, options);
        }

        protected static void Box(Rect rect, Color color)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(rect, GUIContent.none);
            GUI.backgroundColor = backgroundColor;
        }

        protected static void Space()
        {
            GUILayout.Space(10);
        }

        protected static void Line()
        {
            GUIHelper.Line();
        }
        #endregion

        #region Dialog
        protected static bool ShowMessage(string message)
        {
            return GUIHelper.ShowMessage(message);
        }

        protected static bool ShowError(string message)
        {
            return GUIHelper.ShowError(message);
        }

        protected static bool ShowMessage(string title, string message, string button = "OK")
        {
            return GUIHelper.ShowMessage(title, message, button);
        }

        protected static bool ShowConfirm(string message)
        {
            return GUIHelper.ShowConfirm(message);
        }

        protected static bool ShowConfirm(string title, string message)
        {
            return GUIHelper.ShowConfirm(title, message);
        }

        protected static bool ShowConfirm(string title, string message, string yes, string no)
        {
            return GUIHelper.ShowConfirm(title, message, yes, no);
        }
        #endregion

        #region Helper
        protected GUITabController CreateTabController<TEnum>(string key) where TEnum : Enum
        {
            var tabController = new GUITabController(typeof(TEnum));
            tabController.Window = this;
            tabController.CurrentTab = GetInt(key);
            tabController.onTabChanged += tab => SetInt(key, tab);
            return tabController;
        }
        #endregion

        protected static void ShowWindow(string title = "")
        {
            EditorHelper.ShowEditorWindow<T>(title);
        }

        protected static void ShowWindow(Rect rect)
        {
            ShowWindow("", rect);
        }

        protected static void ShowWindow(string title, Rect rect)
        {
            EditorHelper.ShowEditorWindow<T>(title, rect);
        }

        protected void LogDebug(object message)
        {
            LegacyLog.Debug($"[{Helper.GetClassName<T>()}] {message}");
        }
    }
}
#endif
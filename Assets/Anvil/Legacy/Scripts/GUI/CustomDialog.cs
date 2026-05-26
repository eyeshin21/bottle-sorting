#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace Anvil.Legacy
{
    public class CustomDialog : EditorWindow
    {
        private static readonly float WindowWidth = 300;
        private static readonly float WindowHeight = 200;
        private static readonly float LineHeight = 35;

        private string _title;
        private Action _guiCallback;
        private string _button1;
        private string _button2;
        private float _windowWidth;
        private float _windowHeight;
        private Action<int> _buttonCallback;
        private bool _isDirty;

        void Display(string title, Action guiCallback, string button1, string button2, float windowWidth, float windowHeight, Action<int> buttonCallback)
        {
            _title = title;
            _guiCallback = guiCallback;
            _button1 = button1;
            _button2 = button2;
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _buttonCallback = buttonCallback;
            _isDirty = true;

            ShowPopup();
        }

        void OnGUI()
        {
            if (_isDirty)
            {
                var screenWidth = Screen.width;
                var screenHeight = Screen.height;
                position = new Rect((screenWidth - _windowWidth) * 0.5f, (screenHeight - _windowHeight) * 0.5f, _windowWidth, _windowHeight);

                _isDirty = false;
            }

            if (!string.IsNullOrEmpty(_title))
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(_title);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUIHelper.Line();
                GUILayout.Space(10);
            }

            _guiCallback?.Invoke();

            GUILayout.Space(10);
            GUIHelper.Line();
            GUILayout.Space(10);

            int button = 0;
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                {
                    if (GUILayout.Button(_button1))
                    {
                        button = 1;
                    }

                    if (GUILayout.Button(_button2))
                    {
                        button = 2;
                    }
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            if (button > 0)
            {
                _buttonCallback?.Invoke(button);
            }
        }

        static float GetWindowHeight(int lineCount)
        {
            return 145 + lineCount * LineHeight;
        }

        /// <summary>
        /// buttonCallback(buttonId)
        /// </summary>
        public static void Show(string title, Action guiCallback, Action<int> buttonCallback)
        {
            Show(title, guiCallback, WindowWidth, WindowHeight, buttonCallback);
        }

        public static void Show(string title, Action guiCallback, int lineCount, Action<int> buttonCallback)
        {
            Show(title, guiCallback, WindowWidth, GetWindowHeight(lineCount), buttonCallback);
        }

        public static void Show(string title, Action guiCallback, float width, float height, Action<int> buttonCallback)
        {
            Show(title, guiCallback, "Ok", "Cancel", width, height, buttonCallback);
        }

        /// <summary>
        /// buttonCallback(1|2)
        /// </summary>
        public static void Show(string title, Action guiCallback, string button1, string button2, Action<int> buttonCallback)
        {
            Show(title, guiCallback, button1, button2, WindowWidth, WindowHeight, buttonCallback, false);
        }

        public static void Show(string title, Action guiCallback, int lineCount, string button1, string button2, Action<int> buttonCallback, bool autoClose = true)
        {
            Show(title, guiCallback, button1, button2, WindowWidth, GetWindowHeight(lineCount), buttonCallback, autoClose);
        }

        private static CustomDialog _currentDialog;
        public static void Show(string title, Action guiCallback, string button1, string button2, float width, float height, Action<int> buttonCallback, bool autoClose = true)
        {
            if (_currentDialog != null)
            {
                _currentDialog.Close();
            }
            else
            {
                var customDialog = GetWindow<CustomDialog>();
                if (customDialog != null)
                {
                    customDialog.Close();
                }
            }

            _currentDialog = CreateInstance<CustomDialog>();
            _currentDialog.Display(title, guiCallback, button1, button2, width, height, button =>
            {
                if (autoClose)
                {
                    CloseDialog();
                }

                buttonCallback?.Invoke(button);
            });
        }

        public static void CloseDialog()
        {
            if (_currentDialog != null)
            {
                _currentDialog.Close();
                _currentDialog = null;
            }
        }

        public static void TextField(string label, float labelWidth, ref string text, float textWidth)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label, GUILayout.Width(labelWidth));
                text = GUILayout.TextField(text, GUILayout.Width(textWidth));
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public static void TextField(GUIContent content, float labelWidth, ref string text, float textWidth)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(content, GUILayout.Width(labelWidth));
                text = GUILayout.TextField(text, GUILayout.Width(textWidth));
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        //static readonly float LineHeight = 18;
        static readonly float IntWidth = 100;
        static readonly float FloatWidth = 100;
        static readonly float TextFieldWidth = 180;
        static readonly float ColorWidth = 150;

        public static bool ShowMessage(string message)
        {
            return ShowMessage("Message", message);
        }

        public static bool ShowError(string message)
        {
            return ShowMessage("Error", message);
        }

        public static bool ShowMessage(string title, string message, string button = "OK")
        {
#if UNITY_EDITOR
            return EditorUtility.DisplayDialog(title, message, button);
#else
            return false;
#endif
        }

        public static bool ShowConfirm(string message)
        {
            return ShowConfirm("Confirm", message);
        }

        public static bool ShowConfirm(string title, string message)
        {
            return ShowConfirm(title, message, "Yes", "No");
        }

        public static bool ShowConfirm(string title, string message, string yes, string no)
        {
#if UNITY_EDITOR
            return EditorUtility.DisplayDialog(title, message, yes, no);
#else
            return false;
#endif
        }

        public static void ShowShortcutKeys(params (object, string)[] keyDescs)
        {
            var text = Helper.CreateString(sb =>
            {
                foreach (var keyDesc in keyDescs)
                {
                    sb.AppendLine($"[{keyDesc.Item1}]: {keyDesc.Item2}");
                }
            });
            ShowMessage("Shortcut Keys", text.Substring(0, text.Length - 1));
        }

        public static void ShowInputInt(string title, string label, int value, Callback<int> continueCallback, Callback cancelCallback = null)
        {
            ShowInputInt(title, label, value, "Continue", continueCallback, cancelCallback);
        }

        public static void ShowInputInt(string title, string label, int value, string button, Callback<int> continueCallback, Callback cancelCallback = null)
        {
            ShowInputInt(title, label, value, button, inputValue =>
            {
                continueCallback(inputValue);
                return true;
            }, cancelCallback);
        }

        public static void ShowInputInt(string title, string label, int value, Func<int, bool> acceptFunc, Callback cancelCallback = null)
        {
            ShowInputInt(title, label, value, "Continue", acceptFunc, cancelCallback);
        }

        public static void ShowInputInt(string title, string label, int value, string button, Func<int, bool> acceptFunc, Callback cancelCallback = null)
        {
            string text = value != 0 ? value.ToString() : "";

            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label);
                    text = GUILayout.TextField(text, GUILayout.Width(IntWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, button, "Cancel", button =>
            {
                if (button == 1)
                {
                    if (acceptFunc(text.ToInt()))
                    {
                        CustomDialog.CloseDialog();
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            });
        }

        public static void ShowInputIntInt(string title, string label, int value, string label2, int value2, Func<int, int, bool> acceptFunc, Action cancelCallback = null)
        {
            string text = value != 0 ? value.ToString() : "";
            string text2 = value2 != 0 ? value2.ToString() : "";
            float labelWidth = -1;

            void OnGUI()
            {
                if (labelWidth < 0)
                {
                    labelWidth = GetLabelWidth(label, label2);
                }

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label, GUILayout.Width(labelWidth));
                    text = GUILayout.TextField(text, GUILayout.Width(IntWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label2, GUILayout.Width(labelWidth));
                    text2 = GUILayout.TextField(text2, GUILayout.Width(IntWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, "Continue", "Cancel", button =>
            {
                if (button == 1)
                {
                    int newValue = text.ToInt();
                    int newValue2 = text2.ToInt();
                    if (acceptFunc(newValue, newValue2))
                    {
                        CustomDialog.CloseDialog();
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            });
        }

        public static void ShowInputFloat(string title, string label, float value, Func<float, bool> acceptFunc, Callback cancelCallback = null)
        {
            string text = value != 0 ? value.ToString() : "";

            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label);
                    text = GUILayout.TextField(text, GUILayout.Width(FloatWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, "Continue", "Cancel", button =>
            {
                if (button == 1)
                {
                    if (acceptFunc(text.ToFloat()))
                    {
                        CustomDialog.CloseDialog();
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            });
        }

        public static void ShowInputIntFloat(string title, string label, int value, string label2, float value2, Func<int, float, bool> acceptFunc, Action cancelCallback = null)
        {
            string text = value != 0 ? value.ToString() : "";
            string text2 = value2 != 0 ? value2.ToString() : "";
            float labelWidth = -1;

            void OnGUI()
            {
                if (labelWidth < 0)
                {
                    labelWidth = GetLabelWidth(label, label2);
                }

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label, GUILayout.Width(labelWidth));
                    text = GUILayout.TextField(text, GUILayout.Width(IntWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label2, GUILayout.Width(labelWidth));
                    text2 = GUILayout.TextField(text2, GUILayout.Width(FloatWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, "Continue", "Cancel", button =>
            {
                if (button == 1)
                {
                    int newValue = text.ToInt();
                    float newValue2 = text2.ToFloat();
                    if (acceptFunc(newValue, newValue2))
                    {
                        CustomDialog.CloseDialog();
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            });
        }

        public static void ShowInputString(string title, string label, string value, Func<string, bool> continueFunc, Action cancelCallback = null)
        {
            string text = value;

            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label);
                    text = GUILayout.TextField(text, GUILayout.Width(TextFieldWidth), GUILayout.Height(LineHeight));
                    var buffer = GUIUtility.systemCopyBuffer;

                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = guiEnabled && !string.IsNullOrEmpty(buffer);
                    if (GUILayout.Button("Paste"))
                    {
                        text = buffer;
                    }
                    GUI.enabled = guiEnabled;

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, "Continue", "Cancel", button =>
            {
                if (button == 1)
                {
                    if (text != value)
                    {
                        if (continueFunc(text))
                        {
                            CustomDialog.CloseDialog();
                        }
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            });
        }

        public static void ShowInputString(string title, string label, string value, string lastValue, Func<string, bool> continueFunc, Action cancelCallback = null)
        {
            string text = value;

            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label);
                    text = GUILayout.TextField(text, GUILayout.Width(TextFieldWidth));
                    var buffer = GUIUtility.systemCopyBuffer;

                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = guiEnabled && !string.IsNullOrEmpty(buffer);
                    if (GUILayout.Button("Paste"))
                    {
                        text = buffer;
                    }

                    GUI.enabled = guiEnabled && !string.IsNullOrEmpty(lastValue);
                    if (GUILayout.Button("Last"))
                    {
                        text = lastValue;
                    }
                    GUI.enabled = guiEnabled;

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, "Continue", "Cancel", 380, 120, button =>
            {
                if (button == 1)
                {
                    if (text != value)
                    {
                        if (continueFunc(text))
                        {
                            CustomDialog.CloseDialog();
                        }
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            });
        }

        public static void ShowInputString(string title, string label, string value, string lastValue, string label2, string value2, Func<string, string, bool> continueFunc, Action cancelCallback = null)
        {
            string text = value;
            string text2 = value2;

            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label);
                    text = GUILayout.TextField(text, GUILayout.Width(TextFieldWidth));
                    var buffer = GUIUtility.systemCopyBuffer;

                    bool guiEnabled = GUI.enabled;
                    GUI.enabled = guiEnabled && !string.IsNullOrEmpty(buffer);
                    if (GUILayout.Button("Paste"))
                    {
                        text = buffer;
                    }

                    GUI.enabled = guiEnabled && !string.IsNullOrEmpty(lastValue);
                    if (GUILayout.Button("Last"))
                    {
                        text = lastValue;
                    }
                    GUI.enabled = guiEnabled;

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label2);
                    text2 = GUILayout.TextField(text2, GUILayout.Width(TextFieldWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, "Continue", "Cancel", 360, 150, button =>
            {
                if (button == 1)
                {
                    if (text != value && text2 != value2)
                    {
                        if (continueFunc(text, text2))
                        {
                            CustomDialog.CloseDialog();
                        }
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            }, false);
        }

        public static void ShowInputDateTime(string title, string label, DateTime? dateTime, Action<DateTime> callback, Action cancelCallback = null)
        {
            ShowInputString(title, label, dateTime.ToString2(), (value) =>
            {
                var newDateTime = value.ToDateTime2();
                if (newDateTime.HasValue)
                {
                    callback?.Invoke(newDateTime.Value);
                    return true;
                }
                return false;
            }, cancelCallback);
        }

        public static void ShowInputColor(string title, Action<Color> callback)
        {
            ShowInputColor(title, "Color", Color.white, color =>
            {
                callback?.Invoke(color);
                return true;
            });
        }

        public static void ShowInputColor(string title, Func<Color, bool> continueFunc, Action cancelCallback = null)
        {
            ShowInputColor(title, "Color", Color.white, continueFunc, cancelCallback);
        }

        public static void ShowInputColor(string title, Color value, Func<Color, bool> continueFunc, Action cancelCallback = null)
        {
            ShowInputColor(title, "Color", value, continueFunc, cancelCallback);
        }

        public static void ShowInputColor(string title, string label, Color value, Func<Color, bool> continueFunc, Action cancelCallback = null)
        {
            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(label);
                    value = EditorGUILayout.ColorField(value, GUILayout.Width(ColorWidth));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show(title, OnGUI, "Continue", "Cancel", button =>
            {
                if (button == 1)
                {
                    if (continueFunc(value))
                    {
                        CustomDialog.CloseDialog();
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                    cancelCallback?.Invoke();
                }
            });
        }

        public static void ShowSwap(string labelPrefix, int item, Func<int, int, string> swapFunc)
        {
            ShowSwap(labelPrefix, item, item, -1, swapFunc);
        }

        public static void ShowSwap(string labelPrefix, int item, int max, Func<int, int, string> swapFunc)
        {
            ShowSwap(labelPrefix, item, item, max, swapFunc);
        }

        public static void ShowSwap(string labelPrefix, int item1, int item2, int max, Func<int, int, string> swapFunc)
        {
            string item1Text = item1.ToString();
            string item2Text = item2.ToString();

            void OnGUICallback()
            {
                var content1 = new GUIContent($"{labelPrefix} A");
                var content2 = new GUIContent($"{labelPrefix} B");
                float labelWidth = GetMaxLabelWidth(content1, content2);
                float textWidth = 80;
                CustomDialog.TextField(content1, labelWidth, ref item1Text, textWidth);
                CustomDialog.TextField(content2, labelWidth, ref item2Text, textWidth);
            }

            CustomDialog.Show($"Swap {labelPrefix}s", OnGUICallback, "Swap", "Cancel", button =>
            {
                if (button == 1)
                {
                    int value1 = item1Text.ToInt();
                    if (value1 > 0 && (max < 0 || value1 <= max))
                    {
                        int value2 = item2Text.ToInt();
                        if (value2 > 0 && (max < 0 || value2 <= max))
                        {
                            if (value1 != value2)
                            {
                                var message = swapFunc(value1, value2);
                                if (string.IsNullOrEmpty(message))
                                {
                                    CustomDialog.CloseDialog();
                                }
                                else
                                {
                                    ShowError(message);
                                }
                            }
                            else
                            {
                                ShowError($"Same {labelPrefix.ToLowerInvariant()}s");
                            }
                        }
                        else
                        {
                            ShowError($"Invalid {labelPrefix.ToLowerInvariant()} B!");
                        }
                    }
                    else
                    {
                        ShowError($"Invalid {labelPrefix.ToLowerInvariant()} A!");
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                }
            });
        }

        public static void ShowSelectSortingLayerID(Callback<int> setCallback, Callback cancelCallback = null)
        {
            var layers = SortingLayer.layers;
            int layerCount = layers.Length;
            var names = new string[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                names[i] = layers[i].name;
            }
            //int layerID = layers[0].id;
            //var layerValue = SortingLayer.GetLayerValueFromID(layerID);
            int layerIndex = 0;

            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Sorting Layer");
                    //layerValue = EditorGUILayout.Popup(layerValue, names);
                    layerIndex = EditorGUILayout.Popup(layerIndex, names);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show("Set Sorting Layer", OnGUI, "Continue", "Cancel", button =>
            {
                if (button == 1)
                {
                    setCallback?.Invoke(layers[layerIndex].id);
                }
                else
                {
                    cancelCallback?.Invoke();
                }
            });
        }
    }
}
#endif
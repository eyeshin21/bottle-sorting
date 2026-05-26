#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public static partial class DebugUIHelper
    {
        static DebugUIConfig _config;
        static DebugUIConfig Config => _config ??= DebugUIConfig.Instance;

        public static void ShowMessage(string title, string message, Callback doneCallback = null)
        {
            ShowMessage(title, message, "Ok", doneCallback);
        }

        public static void ShowMessage(string title, string message, string button, Callback doneCallback = null)
        {
            var popup = Config.PopupOption.CreateUI<DebugPopupOption>();
            if (popup != null)
            {
                popup.Show(title, message, button, buttonId =>
                {
                    doneCallback?.Invoke();
                });
            }
            else
            {
                doneCallback?.Invoke();
            }
        }

        public static void ShowConfirm(string title, string message, Callback<int> buttonCallback)
        {
            ShowOption(title, message, "Yes", "No", buttonCallback);
        }

        public static void ShowConfirm(string title, string message, string button1, string button2, Callback<int> buttonCallback)
        {
            ShowOption(title, message, button1, button2, buttonCallback);
        }

        public static DebugPopupOption ShowOption(string title, string message, string button1, string button2, Callback<int> buttonCallback)
        {
            var popup = Config.PopupOption.CreateUI<DebugPopupOption>();
            if (popup != null)
            {
                popup.Show(title, message, button1, button2, buttonCallback);
            }
            else
            {
                buttonCallback?.Invoke(0);
            }
            return popup;
        }

        public static DebugPopupOption ShowOption(string title, string message, string button1, string button2, string button3, Callback<int> buttonCallback)
        {
            var popup = Config.PopupOption.CreateUI<DebugPopupOption>();
            if (popup != null)
            {
                popup.Show(title, message, button1, button2, button3, buttonCallback);
            }
            else
            {
                buttonCallback?.Invoke(0);
            }
            return popup;
        }

        static string FixLabel(string label)
        {
            return label.EndsWith(':') ? label : $"{label}:";
        }

        /// <summary>
        /// continueCallback(input)
        /// </summary>
        public static void ShowPopupInt(string title, string label, int value, string continueButton, Callback<int> continueCallback)
        {
            var popup = Config.PopupInt.CreateUI<DebugPopupInt>();
            if (popup != null)
            {
                popup.Show(title, FixLabel(label), value, continueButton, continueCallback);
            }
        }

        /// <summary>
        /// callback(button, input) (button is 1 or 2)
        /// </summary>
        public static void ShowPopupInt(string title, string label, int value, string button1, string button2, Callback<int, int> callback)
        {
            var popup = Config.PopupInt.CreateUI<DebugPopupInt>();
            if (popup != null)
            {
                popup.Show(title, FixLabel(label), value, button1, button2, callback);
            }
        }

        public static void ShowPopupSettings(string title, Callback<DebugPopupSettings> createCallback, Callback saveCallback, Callback closeCallback = null)
        {
            ShowPopupSettings(title, "Save", createCallback, saveCallback, closeCallback);
        }

        public static void ShowPopupSettings(string title, string buttonSave, Callback<DebugPopupSettings> createCallback, Callback saveCallback, Callback closeCallback = null)
        {
            var popup = Config.PopupSettings.CreateUI<DebugPopupSettings>();
            if (popup != null)
            {
                createCallback?.Invoke(popup);
                popup.Show(title, buttonSave, saveCallback, closeCallback);
            }
            else
            {
               LegacyLog.Warning($"Can't create DebugPopupSettings!");
                closeCallback?.Invoke();
            }
        }

        public static void AddButtonSettingsTopLeft(Transform parent, Listener onClick)
        {
            var button = Config.ButtonSettings.Create<Button>(parent);
            button.AddListener(onClick);
            (button.transform as RectTransform).SetPositionTopLeft(20, 20);
        }
    }
}
#endif
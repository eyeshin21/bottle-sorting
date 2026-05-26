using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        //public static void AddListener(this Button button, UnityAction callback)
        //{
        //    if (button != null && callback != null)
        //    {
        //        button.onClick.AddListener(callback);
        //    }
        //}

        public static void AddListener(this Button button, Listener listener)
        {
            if (button != null && listener != null)
            {
                button.onClick.AddListener(() => listener?.Invoke());
            }
        }

        public static void AddCallback(this Button button, Callback callback)
        {
            if (button != null && callback != null)
            {
                button.onClick.AddListener(() => callback?.Invoke());
            }
        }
    }
}
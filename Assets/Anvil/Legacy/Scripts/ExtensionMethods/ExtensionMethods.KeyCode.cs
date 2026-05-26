using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static bool IsDelete(this KeyCode keyCode)
        {
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                return keyCode == KeyCode.Backspace;
            }
            return keyCode == KeyCode.Delete;
        }
    }
}
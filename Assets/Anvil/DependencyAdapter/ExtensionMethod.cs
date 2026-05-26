using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        public static bool IsActiveSafe(this GameObject gameObject)
        {
            return gameObject != null && gameObject.activeSelf;
        }
    }
}

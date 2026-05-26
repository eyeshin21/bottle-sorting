#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static void SetDebugDoubleClick(this GameObject gameObject, Callback callback)
        {
            if (gameObject != null)
            {
                gameObject.GetOrAddComponent<DebugDoubleClick>().OnDoubleClick = callback;
            }
            else
            {
               LegacyLog.Warning("SetDebugDoubleClick: GameObject is null!");
            }
        }
    }
}
#endif
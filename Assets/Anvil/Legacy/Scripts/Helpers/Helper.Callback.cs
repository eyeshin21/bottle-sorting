using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static void InvokeAndClear(ref Callback callback)
        {
            if (callback != null)
            {
                var tmp = callback;
                callback = null;
                tmp();
            }
        }
    }
}
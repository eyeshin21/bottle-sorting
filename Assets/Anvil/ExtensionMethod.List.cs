using System.Collections.Generic;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        public static void TrimNull<T>(this List<T> list) where T : class
        {
            if (list.IsNullOrEmpty())
            {
                return;
            }
            list.RemoveAll(item => item == null);
        }
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

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
        public static T ForceGet<T>(this List<T> list, int index)
        {
            if (list != null)
            {
                int count = list.Count;
                if (count > 0)
                {
                    return index <= 0 ? list[0] : list[Mathf.Min(index, count - 1)];
                }
            }
            return default;
        }
        public static T TryGet<T>(this List<T> list, int index)
        {
            if (list != null)
            {
                int count = list.Count;
                if (index >= 0 && index < count)
                {
                    return list[index];
                }
            }
            return default;
        }

        public static bool TryGetLast<T>(this List<T> list, out  T element)
        {
            if (list == null ||  list.Count == 0)
            {
                element =  default;
                return false;
            }
            element = list[list.Count - 1];
            return true;
        }

        public static int GetCount<T>(this List<T> list)
        {
            if (list == null) return 0;
            return list.Count;
        }

        public static void CheckAdd<T>(this List<T> list, T element)
        {
            if (list == null) return;
            if (list.Contains(element))  return;
            list.Add(element);
        }
    }
}

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static void AddOrReplaceAt<T>(this List<T> list, int index, T item)
        {
            if (index < 0)
            {
                Debug.LogError("List.AddAt Index negative");
                return;
            }

            if (index >= list.Count)
            {
                int diff = index + 1 - list.Count;
                for (int i = 0; i < diff; i++)
                {
                    list.Add(default(T));
                }
            }

            list[index] = item;
        }

        public static bool ContainIndex<T>(this List<T> list, int index)
        {
            int count = list.Count;
            return (index >= 0 && index < count);
        }
        public static void ClearOrConstruct<T>(this List<T> list)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            else
            {
                list.Clear();
            }
        }

        // public static List<T> Fisher_Yates_Shuffle<T>(this List<T> originalList)
        // {
        //     System.Random _random = new System.Random();
        //
        //     T temp;
        //
        //     int n = originalList.Count;
        //     for (int i = 0; i < n; i++)
        //     {
        //         int r = i + (int)(_random.NextDouble() * (n - i));
        //         temp = originalList[r];
        //         originalList[r] = originalList;
        //
        //         originalList = temp;
        //     }
        //
        //
        //     return originalList;
        // }
        public static void SimpleShuffle<T>(this IList<T> ts) {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i) {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}

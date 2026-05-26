using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static int GetCount<T>(this T[] a)
        {
            return a != null ? a.Length : 0;
        }

        public static int GetLength<T>(this T[] a)
        {
            return a != null ? a.Length : 0;
        }

        public static bool IsNullOrEmpty<T>(this T[] a)
        {
            return a == null || a.Length == 0;
        }

        public static string GetLengthOrNullString<T>(this T[] a)
        {
            return a != null ? a.Length.ToString() : StringNull;
        }

        public static T ForceGet<T>(this T[] a, int index)
        {
            if (a != null)
            {
                int length = a.Length;
                if (length > 0)
                {
                    return index <= 0 ? a[0] : (index >= length ? a[length - 1] : a[index]);
                }
            }
            return default;
        }

        public static T GetFirst<T>(this T[] a)
        {
            if (a != null)
            {
                int count = a.Length;
                if (count > 0)
                {
                    return a[0];
                }
            }
            return default;
        }

        public static T GetLast<T>(this T[] a)
        {
            if (a != null)
            {
                int count = a.Length;
                if (count > 0)
                {
                    return a[count - 1];
                }
            }
            return default;
        }

        public static T GetLastNotNull<T>(this T[] a)
        {
            if (a != null)
            {
                for (int i = a.Length - 1; i >= 0; i--)
                {
                    var item = a[i];
                    if (item != null)
                    {
                        return item;
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// Returns default if index out of range.
        /// </summary>
        public static T Get<T>(this T[] a, int index)
        {
            if (a != null)
            {
                int length = a.Length;
                if (index >= 0 && index < length)
                {
                    return a[index];
                }
            }
            return default;
        }

        public static T Get<T>(this T[] a, AcceptFunc<T> acceptFunc)
        {
            if (a != null)
            {
                int count = a.Length;
                for (int i = 0; i < count; i++)
                {
                    T item = a[i];
                    if (acceptFunc(item))
                    {
                        return item;
                    }
                }
            }
            return default;
        }

        public static bool TryGet<T>(this T[] a, AcceptFunc<T> acceptFunc, out T value)
        {
            if (a != null)
            {
                int count = a.Length;
                for (int i = 0; i < count; i++)
                {
                    if (acceptFunc(a[i]))
                    {
                        value = a[i];
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }

        public static void Swap<T>(this T[] a, int count = -1)
        {
            if (count < 0) count = a.GetLength();
            if (count < 2) return;

            T tmp;
            if (count == 2)
            {
                if (Helper.GetRandomBool())
                {
                    tmp = a[0];
                    a[0] = a[1];
                    a[1] = tmp;
                }
                return;
            }

            for (int i = 0; i < count; i++)
            {
                int j = Random.Range(0, count);
                if (i != j)
                {
                    tmp = a[i];
                    a[i] = a[j];
                    a[j] = tmp;
                }
            }
        }

        public static void ForEach<T>(this T[] a, Callback<T> callback)
        {
            if (a != null && callback != null)
            {
                int count = a.Length;
                for (int i = 0; i < count; i++)
                {
                    callback(a[i]);
                }
            }
        }

        public static void ForEach<T>(this T[] a, ContinueFunc<T> continueFunc)
        {
            if (a != null && continueFunc != null)
            {
                int count = a.Length;
                for (int i = 0; i < count; i++)
                {
                    if (!continueFunc(a[i]))
                    {
                        return;
                    }
                }
            }
        }

        public static bool ReturnForEach<T>(this T[] a, ChangedFunc<T> changedFunc)
        {
            bool changed = false;
            if (a != null && changedFunc != null)
            {
                int count = a.Length;
                for (int i = 0; i < count; i++)
                {
                    if (changedFunc(a[i]))
                    {
                        changed = true;
                    }
                }
            }
            return changed;
        }

        public static Vector3[] ToArrayVector3(this Vector2[] a)
        {
            int length = a.Length;
            var ret = new Vector3[length];
            for (int i = 0; i < length; i++)
            {
                ret[i] = a[i];
            }
            return ret;
        }

        public static int[] ToArrayInt(this ushort[] a)
        {
            int length = a.Length;
            var ret = new int[length];
            for (int i = 0; i < length; i++)
            {
                ret[i] = a[i];
            }
            return ret;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this TValue[] values, Func<TValue, TKey> keyFunc)
        {
            var dict = new Dictionary<TKey, TValue>();
            int count = values.GetCount();
            for (int i = 0; i < count; i++)
            {
                var value = values[i];
                var key = keyFunc(value);
                if (dict.ContainsKey(key))
                {
                    LegacyLog.Warning($"Key {key} existed!");
                }
                else
                {
                    dict.Add(key, value);
                }
            }
            return dict;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TItem, TKey, TValue>(this TItem[] items, Func<TItem, (TKey, TValue)> func)
        {
            var dict = new Dictionary<TKey, TValue>();
            int count = items.GetCount();
            for (int i = 0; i < count; i++)
            {
                var item = items[i];
                var keyValue = func(item);
                var key = keyValue.Item1;
                if (dict.ContainsKey(key))
                {
                    LegacyLog.Warning($"Key {keyValue} existed!");
                }
                else
                {
                    dict.Add(key, keyValue.Item2);
                }
            }
            return dict;
        }

        public static string ToString(this string[] a, string separator)
        {
            if (a == null) return StringNull;
            int count = a.Length;
            if (count == 0) return StringArrayEmpty;
            if (count == 1) return a[0];

            return StringHelper.GetString(sb =>
            {
                sb.Append(a[0]);
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{a[i]}");
                }
            });
        }

        /// <summary>
        /// Returns (null) or [] or [item1, item2, ...]
        /// </summary>
        public static string ToString<T>(this T[] a, Func<T, object> itemFunc, int count = -1)
        {
            if (a == null) return StringNull;
            if (count < 0) count = a.Length;
            if (count == 0) return StringArrayEmpty;
            if (count == 1) return $"[{itemFunc(a[0])}]";

            return StringHelper.GetString(sb =>
            {
                sb.Append($"[{itemFunc(a[0])}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($", {itemFunc(a[i])}");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// Returns (null) or [] or [item1, item2, ...]
        /// </summary>
        public static string ToString2<T>(this T[] a, int count = -1)
        {
            if (a == null) return StringNull;
            if (count < 0) count = a.Length;
            if (count == 0) return StringArrayEmpty;
            if (count == 1) return $"[{a[0]}]";

            return StringHelper.GetString(sb =>
            {
                sb.Append($"[{a[0]}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($", {a[i]}");
                }
                sb.Append("]");
            });
        }
    }
}
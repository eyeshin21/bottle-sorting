using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict == null || dict.Count == 0;
        }

        public static int GetCount<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict != null ? dict.Count : 0;
        }

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue> newFunc)
        {
            if (!dict.TryGetValue(key, out TValue value))
            {
                value = newFunc();
                dict.Add(key, value);
            }
            return value;
        }

        public static List<TKey> GetKeys<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict != null)
            {
                var keys = new List<TKey>();
                foreach (var key in dict.Keys)
                {
                    keys.Add(key);
                }
                return keys;
            }
            return default;
        }

        public static List<TKey> GetRandomKeys<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict != null)
            {
                var keys = new List<TKey>();
                foreach (var key in dict.Keys)
                {
                    keys.AddRandom(key);
                }
                return keys;
            }
            return default;
        }

        public static List<string> GetAscendingKeys<TValue>(this Dictionary<string, TValue> dict)
        {
            if (dict != null)
            {
                var keys = new List<string>();
                foreach (var key in dict.Keys)
                {
                    keys.InsertAsc(key);
                }
                return keys;
            }
            return default;
        }

        public static string GetStringOfKeys<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict != null ? dict.Keys.GetEnumerator().ToString2() : "";
        }

        public static string GetStringOfValues<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict != null ? dict.Values.GetEnumerator().ToString2() : "";
        }

        public static void AddValue(this Dictionary<int, int> dict, int key, int value = 1)
        {
            if (dict != null)
            {
                if (dict.TryGetValue(key, out int value2))
                {
                    dict[key] = value + value2;
                }
                else
                {
                    dict.Add(key, value);
                }
            }
            else
            {
                LegacyLog.Warning("Dictionary is null!");
            }
        }

        public static void AddValue<TKey>(this Dictionary<TKey, int> dict, TKey key, int value = 1)
        {
            if (dict != null)
            {
                if (dict.TryGetValue(key, out int value2))
                {
                    dict[key] = value + value2;
                }
                else
                {
                    dict.Add(key, value);
                }
            }
            else
            {
                LegacyLog.Warning("Dictionary is null!");
            }
        }

        public static string ToString2<TKey, TValue>(this Dictionary<TKey, TValue>.KeyCollection keys)
        {
            return keys.GetEnumerator().ToString2();
        }

        public static string ToString2<TKey, TValue>(this Dictionary<TKey, TValue>.ValueCollection values)
        {
            return values.GetEnumerator().ToString2();
        }

        /// <summary>
        /// Returns {"key1":value1, ...}
        /// </summary>
        public static string ToString2<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            int count = dict != null ? dict.Count : 0;
            if (count == 0) return "{}";

            return Helper.CreateString(sb =>
            {
                foreach (var entry in dict)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append($"{{\"{entry.Key}\":{entry.Value}");
                    }
                    else
                    {
                        sb.Append($", \"{entry.Key}\":{entry.Value}");
                    }
                }
                sb.Append("}");
            });
        }

        /// <summary>
        /// Returns {"key1":value1, ...}
        /// </summary>
        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<TKey, object> keyFunc)
        {
            int count = dict != null ? dict.Count : 0;
            if (count == 0) return "{}";

            return Helper.CreateString(sb =>
            {
                foreach (var entry in dict)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append($"{{\"{keyFunc(entry.Key)}\":{entry.Value}");
                    }
                    else
                    {
                        sb.Append($", \"{keyFunc(entry.Key)}\":{entry.Value}");
                    }
                }
                sb.Append("}");
            });
        }

        /// <summary>
        /// Returns {"key1":value1, ...}
        /// </summary>
        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<TValue, object> valueFunc)
        {
            int count = dict != null ? dict.Count : 0;
            if (count == 0) return "{}";

            return Helper.CreateString(sb =>
            {
                foreach (var entry in dict)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append($"{{\"{entry.Key}\":{valueFunc(entry.Value)}");
                    }
                    else
                    {
                        sb.Append($", \"{entry.Key}\":{valueFunc(entry.Value)}");
                    }
                }
                sb.Append("}");
            });
        }

        /// <summary>
        /// Returns {"key1":value1, ...}
        /// </summary>
        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<TKey, object> keyFunc, Func<TValue, object> valueFunc)
        {
            int count = dict != null ? dict.Count : 0;
            if (count == 0) return "{}";

            return Helper.CreateString(sb =>
            {
                foreach (var entry in dict)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append($"{{\"{keyFunc(entry.Key)}\":{valueFunc(entry.Value)}");
                    }
                    else
                    {
                        sb.Append($", \"{keyFunc(entry.Key)}\":{valueFunc(entry.Value)}");
                    }
                }
                sb.Append("}");
            });
        }
    }
}
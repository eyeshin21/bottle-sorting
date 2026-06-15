using System;
using System.Collections.Generic;
using System.Text;

namespace Anvil
{
    public class KeyCount<T>
    {
        private T _key;
        private int _count;

        public T Key => _key;
        public int Count => _count;

        public KeyCount(T key, int count)
        {
            _key = key;
            _count = count;
        }
    }

    public static partial class ExtensionMethods
    {
        public static int GetCount<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict != null ? dict.Count : 0;
        }

        public static string GetValue(this Dictionary<string, string> dict, string key, string defaultValue = "")
        {
            if (dict.TryGetValue(key, out string value))
            {
                return value;
            }

            return defaultValue;
        }

        public static bool GetBool(this Dictionary<string, string> dict, string key, bool defaultValue = false)
        {
            if (dict.TryGetValue(key, out string value))
            {
                return value.ToBool(defaultValue);
            }

            return defaultValue;
        }

        public static int GetInt(this Dictionary<string, string> dict, string key, int defaultValue = 0)
        {
            if (dict.TryGetValue(key, out string value))
            {
                return value.ToInt(defaultValue);
            }

            return defaultValue;
        }

        public static string GetString(this Dictionary<string, string> dict, string key, string defaultValue = "")
        {
            if (dict.TryGetValue(key, out string value))
            {
                return value;
            }

            return defaultValue;
        }

        public static DateTime? GetDateTime(this Dictionary<string, string> dict, string key, DateTime? defaultValue = null)
        {
            if (dict.TryGetValue(key, out string value))
            {
                var dateTime = value.ToDateTime2();
                return dateTime != null ? dateTime : defaultValue;
            }

            return defaultValue;
        }

        public static List<TKey> GetKeys<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            var keys = new List<TKey>(dict.Count);
            foreach (var key in dict.Keys)
            {
                keys.Add(key);
            }

            return keys;
        }

        //public static List<int> GetAscKeys<T>(this Dictionary<int, T> dict)
        //{
        //    var list = new List<int>(dict.Count);
        //    foreach (int key in dict.Keys)
        //    {
        //        list.InsertAsc(key);
        //    }

        //    return list;
        //}

        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        public static void RenameKey<T>(this Dictionary<string, T> dict, string key, string newKey)
        {
            if (dict.TryGetValue(key, out T value))
            {
                dict.Remove(key);
                dict.Add(newKey, value);
            }
        }

        public static bool TryRemove<T>(this Dictionary<string, T> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return dict.Remove(key);
            }

            return false;
        }

        public static bool IsEquals<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> dict2)
        {
            if (dict.Count != dict2.Count) return false;

            int equalCount = 0;
            foreach (var entry in dict)
            {
                var key = entry.Key;
                if (dict2.TryGetValue(key, out TValue value))
                {
                    if (entry.Value.Equals(value))
                    {
                        equalCount++;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return equalCount == dict2.Count;
        }

        public static Dictionary ToDictionary(this Dictionary<string, string> dict)
        {
            var dict2 = new Dictionary();
            foreach (var entry in dict)
            {
                dict2.Add(entry.Key, entry.Value);
            }
            return dict2;
        }

        public static string ToLogLines<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            int count = dict.Count;
            if (count == 0) return "";

            StringBuilder sb = null;
            foreach (var entry in dict)
            {
                if (sb == null)
                {
                    sb = new StringBuilder($"{entry.Key}: {entry.Value}");
                }
                else
                {
                    sb.Append($"\n{entry.Key}: {entry.Value}");
                }
            }

            return sb.ToString();
        }

        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<TValue, object> valueFunc)
        {
            int count = dict.Count;
            if (count == 0) return "{}";

            StringBuilder sb = null;
            foreach (var entry in dict)
            {
                if (sb == null)
                {
                    sb = new StringBuilder($"\"{entry.Key}\": {valueFunc(entry.Value)}");
                }
                else
                {
                    sb.Append($"\n\"{entry.Key}\": {valueFunc(entry.Value)}");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns {"key1":value1, ..., "keyN":valueN}
        /// </summary>
        public static string ToString2<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            int count = dict.Count;
            if (count == 0) return "{}";

            StringBuilder sb = null;
            foreach (var entry in dict)
            {
                if (sb == null)
                {
                    sb = new StringBuilder($"{{\"{entry.Key}\":{entry.Value}");
                }
                else
                {
                    sb.Append($", \"{entry.Key}\":{entry.Value}");
                }
            }

            sb.Append("}");

            return sb.ToString();
        }

        public static string ToString2<TKey, TValue>(this Dictionary<TKey, TValue>.KeyCollection keys)
        {
            if (keys.Count == 0) return "[]";

            StringBuilder sb = null;
            foreach (var key in keys)
            {
                if (sb == null)
                {
                    sb = new StringBuilder($"[{key}");
                }
                else
                {
                    sb.Append($", {key}");
                }
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Anvil;

namespace Anvil
{
    public interface ISerialize
    {
        string Serialize();
    }

    public interface IDeserialize
    {
        void Deserialize(string json);
    }
    public static partial class JsonSerializer
    {
        #region Pool
        static Stack<List<int>> _intLists = new Stack<List<int>>();
        static List<int> GetIntList()
        {
            if (_intLists.Count > 0)
            {
                var list = _intLists.Pop();
                return list;
            }

            return new List<int>();
        }

        static void ReturnPool(List<int> list)
        {
            list.Clear();
            _intLists.Push(list);
        }

        static Stack<List<string>> _stringLists = new Stack<List<string>>();
        static List<string> GetStringList()
        {
            if (_stringLists.Count > 0)
            {
                var list = _stringLists.Pop();
                return list;
            }

            return new List<string>();
        }

        static void ReturnPool(List<string> list)
        {
            list.Clear();
            _stringLists.Push(list);
        }

        static Stack<StringBuilder> _stringBuilders = new Stack<StringBuilder>();
        static string CreateString(Action<StringBuilder> callback)
        {
            StringBuilder sb;
            if (_stringBuilders.Count > 0)
            {
                sb = _stringBuilders.Pop();
            }
            else
            {
                sb = new StringBuilder();
            }

            callback(sb);
            var s = sb.ToString();
            sb.Clear();
            _stringBuilders.Push(sb);

            return s;
        }
        #endregion

        public static string AddOpenClose(string json)
        {
            return $"{Open}{json}{Close}";
        }

        public static string RemoveOpenClose(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                int length = json.Length;
                if (length > 1)
                {
                    if (json[0] == Open && json[length - 1] == Close)
                    {
                        return length > 2 ? json.Substring(1, length - 2) : "";
                    }
                }
            }

            return json;
        }

        public static bool IsDictionary(string json)
        {
            return !string.IsNullOrEmpty(json) && json[0] == Open;
        }

        public static void Split(string s, out string s1, out string s2, char separator = ParamSeparator)
        {
            s1 = "";
            s2 = "";

            if (!string.IsNullOrEmpty(s))
            {
                int index = s.IndexOf(separator);
                if (index > 0)
                {
                    s1 = s.Substring(0, index);
                    if (index < s.Length - 1)
                    {
                        s2 = s.Substring(index + 1);
                    }
                }
                else if (index < 0)
                {
                    s1 = s;
                }
                else
                {
                    if (index < s.Length - 1)
                    {
                        s2 = s.Substring(1);
                    }
                }
            }
        }

        public static void Split(string s, out int value1, out int value2, char separator = ParamSeparator)
        {
            Split(s, out string s1, out string s2, separator);
            value1 = s1.ToInt();
            value2 = s2.ToIntForce();
        }

        public static void Split(string s, out string s1, out string s2, out string s3, char separator = ParamSeparator)
        {
            s1 = "";
            s2 = "";
            s3 = "";

            if (!string.IsNullOrEmpty(s))
            {
                int index = s.IndexOf(separator);
                if (index > 0)
                {
                    s1 = s.Substring(0, index);
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(index + 1), out s2, out s3, separator);
                    }
                }
                else if (index < 0)
                {
                    s1 = s;
                }
                else
                {
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(1), out s2, out s3, separator);
                    }
                }
            }
        }

        public static void Split(string s, out int value1, out int value2, out int value3, char separator = ParamSeparator)
        {
            Split(s, out string s1, out string s2, out string s3, separator);
            value1 = s1.ToInt();
            value2 = s2.ToInt();
            value3 = s3.ToIntForce();
        }

        public static void Split(string s, out string s1, out string s2, out string s3, out string s4, char separator = ParamSeparator)
        {
            s1 = "";
            s2 = "";
            s3 = "";
            s4 = "";

            if (!string.IsNullOrEmpty(s))
            {
                int index = s.IndexOf(separator);
                if (index > 0)
                {
                    s1 = s.Substring(0, index);
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(index + 1), out s2, out s3, out s4, separator);
                    }
                }
                else if (index < 0)
                {
                    s1 = s;
                }
                else
                {
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(1), out s2, out s3, out s4, separator);
                    }
                }
            }
        }

        public static void Split(string s, out int value1, out int value2, out int value3, out int value4, char separator = ParamSeparator)
        {
            Split(s, out string s1, out string s2, out string s3, out string s4, separator);
            value1 = s1.ToInt();
            value2 = s2.ToInt();
            value3 = s3.ToInt();
            value4 = s4.ToIntForce();
        }

        public static void Split(string s, out string s1, out string s2, out string s3, out string s4, out string s5, char separator = ParamSeparator)
        {
            s1 = "";
            s2 = "";
            s3 = "";
            s4 = "";
            s5 = "";

            if (!string.IsNullOrEmpty(s))
            {
                int index = s.IndexOf(separator);
                if (index > 0)
                {
                    s1 = s.Substring(0, index);
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(index + 1), out s2, out s3, out s4, out s5, separator);
                    }
                }
                else if (index < 0)
                {
                    s1 = s;
                }
                else
                {
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(1), out s2, out s3, out s4, out s5, separator);
                    }
                }
            }
        }

        public static void Split(string s, out int value1, out int value2, out int value3, out int value4, out int value5, char separator = ParamSeparator)
        {
            Split(s, out string s1, out string s2, out string s3, out string s4, out string s5, separator);
            value1 = s1.ToInt();
            value2 = s2.ToInt();
            value3 = s3.ToInt();
            value4 = s4.ToInt();
            value5 = s5.ToIntForce();
        }

        public static void Split(string s, out string s1, out string s2, out string s3, out string s4, out string s5, out string s6, char separator = ParamSeparator)
        {
            s1 = "";
            s2 = "";
            s3 = "";
            s4 = "";
            s5 = "";
            s6 = "";

            if (!string.IsNullOrEmpty(s))
            {
                int index = s.IndexOf(separator);
                if (index > 0)
                {
                    s1 = s.Substring(0, index);
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(index + 1), out s2, out s3, out s4, out s5, out s6, separator);
                    }
                }
                else if (index < 0)
                {
                    s1 = s;
                }
                else
                {
                    if (index < s.Length - 1)
                    {
                        Split(s.Substring(1), out s2, out s3, out s4, out s5, out s6, separator);
                    }
                }
            }
        }

        public static void Split(string s, out int value1, out int value2, out int value3, out int value4, out int value5, out int value6, char separator = ParamSeparator)
        {
            Split(s, out string s1, out string s2, out string s3, out string s4, out string s5, out string s6, separator);
            value1 = s1.ToInt();
            value2 = s2.ToInt();
            value3 = s3.ToInt();
            value4 = s4.ToInt();
            value5 = s5.ToInt();
            value6 = s6.ToIntForce();
        }

        public static List<string> SplitItems(string json, char separator = ItemSeparator)
        {
            var list = Pools.GetListString();
            if (!string.IsNullOrEmpty(json))
            {
                int index = json.IndexOf(separator);
                if (index < 0)
                {
                    list.Add(json);
                }
                else
                {
                    list.Add(json.Substring(0, index));

                    int startIndex = index + 1;
                    do
                    {
                        index = json.IndexOf(separator, startIndex);
                        if (index < 0)
                        {
                            list.Add(json.Substring(startIndex));
                            break;
                        }

                        list.Add(json.Substring(startIndex, index - startIndex));
                        startIndex = index + 1;
                    }
                    while (true);
                }
            }

            return list;
        }

        /// <summary>
        /// json format: ["item1","item2",...] or [item1,item2,...]
        /// </summary>
        public static List<string> GetStrings(string json)
        {
            var list = Pools.GetListString();
            AddStrings(list, json);
            return list;
        }

        /// <summary>
        /// json format: ["item1","item2",...] or [item1,item2,...]
        /// </summary>
        public static void AddStrings(List<string> list, string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            int length = json.Length;
            int startIndex = json.IndexOf('"');

            // [item1,item2,...]
            if (startIndex < 0)
            {
                startIndex = json[0] == '[' ? 1 : 0;
                for (int i = startIndex + 1; i < length; i++)
                {
                    if (json[i] == ',')
                    {
                        if (i == startIndex)
                        {
                            list.Add("");
                        }
                        else
                        {
                            list.Add(json.Substring(startIndex, i - startIndex));
                        }
                        startIndex = i + 1;
                    }
                }
                int endIndex = json[length - 1] == ']' ? length - 1 : length;
                if (endIndex > startIndex)
                {
                    list.Add(json.Substring(startIndex, endIndex - startIndex));
                }
            }
            // ["item1","item2",...]
            else
            {
                for (int i = startIndex + 1; i < length; i++)
                {
                    if (json[i] == '"')
                    {
                        if (startIndex < 0)
                        {
                            startIndex = i;
                        }
                        else
                        {
                            list.Add(json.Substring(startIndex + 1, i - startIndex - 1));
                            startIndex = -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// json format: "[{"key1":"value1", "key2":value2},{...},...]"
        /// </summary>
        public static List<string> GetDictionaries(string json)
        {
            var list = Pools.GetListString();
            AddListOfDictionaries(list, json);
            return list;
        }

        /// <summary>
        /// json format: "[{"key1":"value1", "key2":value2},{...},...]"
        /// </summary>
        public static void AddListOfDictionaries(List<string> list, string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            int length = json.Length;
            int startIndex = -1;
            int openCount = 0;

            for (int i = 0; i < length; i++)
            {
                char c = json[i];
                if (startIndex < 0)
                {
                    if (c == '{')
                    {
                        startIndex = i;
                        openCount = 1;
                    }
                }
                else
                {
                    if (c == '{')
                    {
                        openCount++;
                    }
                    else if (c == '}')
                    {
                        openCount--;
                        if (openCount == 0)
                        {
                            list.Add(json.Substring(startIndex, i - startIndex + 1));
                            startIndex = -1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns "item1|item2|..."
        /// </summary>
        public static string Serialize<T>(T[] a, char separator = ItemSeparator) where T : ISerialize
        {
            int count = a != null ? a.Length : 0;
            if (count == 0) return "";
            if (count == 1)
            {
                var s = a[0].Serialize();
                return s;
            }

            return CreateString(sb =>
            {
                var s = a[0].Serialize();
                sb.Append(s);
                for (int i = 1; i < count; i++)
                {
                    s = a[i].Serialize();
                    sb.Append($"{separator}{s}");
                }
            });
        }


        /// <summary>
        /// Returns "value1_value2_..."
        /// </summary>
        public static string SerializeValues(Action<List<int>> callback, char separator = ValueSeparator)
        {
            return Serialize(callback, separator);
        }

        /// <summary>
        /// Returns "item1|item2|..."
        /// </summary>
        public static string Serialize(Action<List<int>> callback, char separator = ItemSeparator)
        {
            var list = GetIntList();
            callback(list);
            var json = Serialize(list, separator);
            ReturnPool(list);

            return json;
        }

        /// <summary>
        /// Json format: "value1_value2_..."
        /// </summary>
        public static void DeserializeValues(string json, Action<List<int>> callback, char separator = ValueSeparator)
        {
            Deserialize(json, callback, separator);
        }

        /// <summary>
        /// Json format: "item1|item2|..."
        /// </summary>
        public static void Deserialize(string json, Action<List<int>> callback, char separator = ItemSeparator)
        {
            var list = GetIntList();
            Deserialize(json, ref list, separator);
            callback(list);
            ReturnPool(list);
        }

        /// <summary>
        /// Json format: "item1|item2|..."
        /// </summary>
        public static void Deserialize(string json, ref List<int> items, char separator = ItemSeparator)
        {
            if (string.IsNullOrEmpty(json))
            {
                if (items != null)
                {
                    items.Clear();
                }
            }
            else
            {
                if (items != null)
                {
                    items.Clear();
                }
                else
                {
                    items = new List<int>();
                }

                int index = json.IndexOf(separator);
                if (index < 0)
                {
                    items.Add(json.ToInt());
                }
                else
                {
                    items.Add(json.Substring(0, index).ToInt());

                    int startIndex = index + 1;
                    do
                    {
                        index = json.IndexOf(separator, startIndex);
                        if (index < 0)
                        {
                            items.Add(json.Substring(startIndex).ToInt());
                            break;
                        }
                        items.Add(json.Substring(startIndex, index - startIndex).ToInt());
                        startIndex = index + 1;
                    }
                    while (true);
                }
            }
        }

        /// <summary>
        /// Returns "value1_value2_..."
        /// </summary>
        public static string SerializeValues(int[] values, char separator = ValueSeparator)
        {
            return Serialize(values, separator);
        }

        /// <summary>
        /// Returns "item1|item2|..."
        /// </summary>
        public static string Serialize(int[] items, char separator = ItemSeparator)
        {
            int count = items != null ? items.Length : 0;
            if (count == 0) return "";
            if (count == 1) return items[0].ToString();

            return CreateString(sb =>
            {
                sb.Append(items[0].ToString());
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{items[i]}");
                }
            });
        }

        /// <summary>
        /// Returns "value1_value2_..."
        /// </summary>
        public static string SerializeValues(List<int> values, char separator = ValueSeparator)
        {
            return Serialize(values, separator);
        }

        /// <summary>
        /// Returns "item1|item2|..."
        /// </summary>
        public static string Serialize(List<int> items, char separator = ItemSeparator)
        {
            int count = items != null ? items.Count : 0;
            if (count == 0) return "";
            if (count == 1) return items[0].ToString();

            return CreateString(sb =>
            {
                sb.Append(items[0].ToString());
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{items[i]}");
                }
            });
        }

        /// <summary>
        /// Returns "item1|item2|..." (integers)
        /// </summary>
        public static string SerializeEnums<T>(List<T> items, char separator = ItemSeparator) where T : Enum
        {
            int count = items != null ? items.Count : 0;
            if (count == 0) return "";
            if (count == 1) return ((int)(object)items[0]).ToString();

            return CreateString(sb =>
            {
                sb.Append(((int)(object)items[0]).ToString());
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{(int)(object)items[i]}");
                }
            });
        }

        /// <summary>
        /// Json format: "item1|item2|..." (integers)
        /// </summary>
        public static void DeserializeEnums<T>(string json, ref List<T> items, char separator = ItemSeparator) where T : Enum
        {
            if (string.IsNullOrEmpty(json))
            {
                if (items != null)
                {
                    items.Clear();
                }
            }
            else
            {
                if (items != null)
                {
                    items.Clear();
                }
                else
                {
                    items = new List<T>();
                }

                int index = json.IndexOf(separator);
                if (index < 0)
                {
                    items.Add((T)(object)json.ToInt());
                }
                else
                {
                    items.Add((T)(object)json.Substring(0, index).ToInt());

                    int startIndex = index + 1;
                    do
                    {
                        index = json.IndexOf(separator, startIndex);
                        if (index < 0)
                        {
                            items.Add((T)(object)json.Substring(startIndex).ToInt());
                            break;
                        }
                        items.Add((T)(object)json.Substring(startIndex, index - startIndex).ToInt());
                        startIndex = index + 1;
                    }
                    while (true);
                }
            }
        }

        /// <summary>
        /// Returns "item1|item2|..."
        /// </summary>
        public static string Serialize(Action<List<string>> callback, char separator = ItemSeparator)
        {
            var list = GetStringList();
            callback(list);
            var json = Serialize(list, separator);
            ReturnPool(list);
            return json;
        }

        /// <summary>
        /// Returns "value1_value2_..."
        /// </summary>
        public static string SerializeValues(List<string> values, char separator = ValueSeparator)
        {
            return Serialize(values, separator);
        }

        /// <summary>
        /// Returns "item1|item2|..."
        /// </summary>
        public static string Serialize(List<string> items, char separator = ItemSeparator)
        {
            int count = items != null ? items.Count : 0;
            if (count == 0) return "";
            if (count == 1)
            {
                var s = items[0];
                return s;
            }

            return CreateString(sb =>
            {
                var s = items[0];
                sb.Append(s);
                for (int i = 1; i < count; i++)
                {
                    s = items[i];
                    sb.Append($"{separator}{s}");
                }
            });
        }

        /// <summary>
        /// Returns "item1|item2|..." or "[[item1]|[item2]|...]"
        /// </summary>
        public static string Serialize<T>(List<T> list, char separator = ItemSeparator) where T : ISerialize
        {
            int count = list != null ? list.Count : 0;
            if (count == 0) return "";
            if (count == 1)
            {
                var s = list[0].Serialize();
                return s;
            }

            var json = CreateString(sb =>
            {
                var s = list[0].Serialize();
                sb.Append(s);
                for (int i = 1; i < count; i++)
                {
                    s = list[i].Serialize();
                    sb.Append($"{separator}{s}");
                }
            });

            // Check []
            int length = json.Length - 1;
            for (int i = 0; i < length; i++)
            {
                if (json[i] == Open)
                {
                    json = $"{Open}{json}{Close}";
                    break;
                }
            }

            return json;
        }

        /// <summary>
        /// Returns "item1|item2|..." or "[[item1]|[item2]|...]"
        /// </summary>
        public static string AddSerialize<T>(string json, T item, char separator = ItemSeparator) where T : ISerialize
        {
            var itemJson = item.Serialize();
            int length = json.GetLength();
            if (length == 0)
            {
                return itemJson;
            }

            if (json[length - 1] == Close)
            {
                return $"{json.Substring(0, length - 1)}{separator}{itemJson}{Close}";
            }

            json = $"{json}{separator}{itemJson}";
            if (itemJson.Contains(Open))
            {
                json = $"{Open}{json}{Close}";
            }
            return json;
        }

        public static void DeserializeValues(string json, Action<string> callback)
        {
            Deserialize(json, callback, ValueSeparator);
        }

        public static void DeserializeItems(string json, Action<string> callback)
        {
            Deserialize(json, callback, ItemSeparator);
        }

        static void Deserialize(string json, Action<string> callback, char separator)
        {
            if (string.IsNullOrEmpty(json) || callback == null) return;

            ReduceOpenClose(ref json);

            int index = json.IndexOf(separator);
            if (index < 0)
            {
                callback(json);
            }
            else
            {
                callback(json.Substring(0, index));

                int startIndex = index + 1;
                do
                {
                    index = json.IndexOf(separator, startIndex);
                    if (index < 0)
                    {
                        callback(json.Substring(startIndex));
                        break;
                    }
                    callback(json.Substring(startIndex, index - startIndex));
                    startIndex = index + 1;
                }
                while (true);
            }
        }


        public static void Deserialize(string json, Action<List<string>> callback, char separator = ItemSeparator)
        {
            var list = GetStringList();
            Deserialize(json, ref list, separator);
            callback(list);
            ReturnPool(list);
        }

        public static int[] DeserializeValues(string json, char separator = ValueSeparator)
        {
            var list = GetStringList();
            Deserialize(json, ref list, separator);
            int count = list.Count;
            var values = new int[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = list[i].ToInt();
            }
            ReturnPool(list);
            return values;
        }

        public static void DeserializeValues(string json, ref List<string> values, char separator = ValueSeparator)
        {
            Deserialize(json, ref values, separator);
        }

        public static void Deserialize(string json, ref List<string> items, char separator = ItemSeparator)
        {
            if (string.IsNullOrEmpty(json))
            {
                if (items != null)
                {
                    items.Clear();
                }
            }
            else
            {
                if (items != null)
                {
                    items.Clear();
                }
                else
                {
                    items = new List<string>();
                }

                int index = json.IndexOf(separator);
                if (index < 0)
                {
                    items.Add(json);
                }
                else
                {
                    items.Add(json.Substring(0, index));

                    int startIndex = index + 1;
                    do
                    {
                        index = json.IndexOf(separator, startIndex);
                        if (index < 0)
                        {
                            items.Add(json.Substring(startIndex));
                            break;
                        }
                        items.Add(json.Substring(startIndex, index - startIndex));
                        startIndex = index + 1;
                    }
                    while (true);
                }
            }
        }

        public static void Deserialize<T>(string json, ref List<T> list, char separator = ItemSeparator) where T : IDeserialize, new()
        {
            if (list == null)
            {
                list = new List<T>();
            }
            Deserialize(json, list, separator);
        }

        //public static void Deserialize<T>(string json, List<T> list, char separator, char oldSeparator) where T : IDeserialize, new()
        //{
        //    Deserialize(Helper.ReplaceCharacters(json, oldSeparator, separator), list, separator);
        //}

        public static void Deserialize<T>(string json, List<T> list, char separator = ItemSeparator) where T : IDeserialize, new()
        {
            list.Clear();
            if (!string.IsNullOrEmpty(json))
            {
                ReduceOpenClose(ref json);

                int index = json.IndexOf(separator);
                if (index < 0)
                {
                    var item = new T();
                    item.Deserialize(json);
                    list.Add(item);
                }
                else
                {
                    var item = new T();
                    item.Deserialize(json.Substring(0, index));
                    list.Add(item);

                    int startIndex = index + 1;
                    do
                    {
                        index = json.IndexOf(separator, startIndex);
                        if (index < 0)
                        {
                            item = new T();
                            item.Deserialize(json.Substring(startIndex));
                            list.Add(item);
                            break;
                        }
                        item = new T();
                        item.Deserialize(json.Substring(startIndex, index - startIndex));
                        list.Add(item);
                        startIndex = index + 1;
                    }
                    while (true);
                }
            }
        }

        public static bool TryParse(string s, out bool value)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = false;
                return true;
            }

            if (s.Length == 1)
            {
                if (int.TryParse(s, out int result))
                {
                    value = result > 0;
                    return true;
                }
            }
            else
            {
                return bool.TryParse(s, out value);
            }

            value = false;
            return false;
        }

        public static bool TryParse(string s, out int value)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = 0;
                return true;
            }
            return int.TryParse(s, out value);
        }

        public static bool TryParse(string s, out long value)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = 0;
                return true;
            }
            return long.TryParse(s, out value);
        }

        public static bool TryParse(string s, out float value)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = 0;
                return true;
            }
            return float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        public static bool TryParse<T>(string s, out object value)
        {
            if (!string.IsNullOrEmpty(s))
            {
                var type = typeof(T);
                if (type == TypeBool)
                {
                    // 0/1
                    if (s.Length == 1)
                    {
                        if (int.TryParse(s, out int result))
                        {
                            value = result > 0;
                            return true;
                        }
                    }
                    else
                    {
                        if (s == "True")
                        {
                            value = true;
                            return true;
                        }
                        if (s == "False")
                        {
                            value = false;
                            return true;
                        }
                    }
                }
                else if (type == TypeInt)
                {
                    if (int.TryParse(s, out int result))
                    {
                        value = result;
                        return true;
                    }
                }
                else if (type == TypeLong)
                {
                    if (long.TryParse(s, out long result))
                    {
                        value = result;
                        return true;
                    }
                }
                else if (type == TypeFloat)
                {
                    if (float.TryParse(s, out float result))
                    {
                        value = result;
                        return true;
                    }
                }
                else if (type == TypeString)
                {
                    value = s;
                    return true;
                }
                else if (type == typeof(DateTime?))
                {
                    if (TryParse(s, out DateTime? dateTime))
                    {
                        value = dateTime;
                        return true;
                    }
                }
                else
                {
                    LegacyLog.Todo($"Convert \"{s}\" to {type}");
                }
            }

            value = default;
            return false;
        }

        public static bool TryParse(string s, out DateTime? dateTime)
        {
            dateTime = s.ToDateTime2();
            return dateTime != null;
        }

        static int[] _indices = new int[10];
        public static bool TryAppend(string items, string item, out string result, int maxItem = -1)
        {
            if (string.IsNullOrEmpty(items))
            {
                result = item;
                return true;
            }

            if (string.IsNullOrEmpty(item))
            {
                result = items;
                return false;
            }

            if (maxItem > 0)
            {
                int itemCount = 1;
                int length = items.Length;
                for (int i = 0; i < length; i++)
                {
                    if (items[i] == ItemSeparator)
                    {
                        if (itemCount > _indices.Length)
                        {
                            Array.Resize(ref _indices, _indices.Length + 10);
                        }
                        _indices[itemCount - 1] = i;
                        itemCount++;
                    }
                }

                if (ContainItem(items, item))
                {
                    if (itemCount > maxItem)
                    {
                        int startIndex = _indices[0];
                        if (maxItem > 1)
                        {
                            int lastIndex = _indices[itemCount - maxItem];
                            result = $"{items.Substring(0, startIndex)}{items.Substring(lastIndex)}";
                        }
                        else
                        {
                            result = items.Substring(0, startIndex);
                        }
                        return true;
                    }
                }
                else
                {
                    if (maxItem == 1)
                    {
                        result = item;
                        return true;
                    }

                    maxItem--;
                    if (itemCount > maxItem)
                    {
                        int startIndex = _indices[0];
                        if (maxItem > 1)
                        {
                            int lastIndex = _indices[itemCount - maxItem];
                            items = $"{items.Substring(0, startIndex)}{items.Substring(lastIndex)}";
                        }
                        else
                        {
                            items = items.Substring(0, startIndex);
                        }
                    }

                    result = $"{items}{ItemSeparator}{item}";
                    return true;
                }
            }
            else
            {
                if (!ContainItem(items, item))
                {
                    result = $"{items}{ItemSeparator}{item}";
                    return true;
                }
            }

            result = items;
            return false;
        }

        public static bool ContainItem(string items, string item)
        {
            if (string.IsNullOrEmpty(items)) return false;

            int index = items.IndexOf(item, StringComparison.Ordinal);
            if (index < 0) return false;

            // Check "|item"
            if (index > 0 && items[index - 1] != ItemSeparator) return false;

            // Check "item|"
            index += item.Length;
            if (index < items.Length && items[index] != ItemSeparator) return false;

            return true;
        }

        public static string GetFirstItem(string items)
        {
            if (!string.IsNullOrEmpty(items))
            {
                int index = items.IndexOf(ItemSeparator);
                if (index > 0)
                {
                    return items.Substring(0, index);
                }
            }

            return items;
        }

        public static string GetLastItem(string items)
        {
            if (!string.IsNullOrEmpty(items))
            {
                int index = items.LastIndexOf(ItemSeparator);
                if (index > 0)
                {
                    return items.Substring(index + 1);
                }
            }

            return items;
        }

        
        /// <summary>
        /// Returns "item1.x_item1.y|item2.x_item2.y|..."
        /// </summary>
        public static string Serialize(List<Vector2Int> points, char valueSeparator = ValueSeparator,
            char itemSeparator = ItemSeparator)
        {
            int count = points != null ? points.Count : 0;
            if (count == 0) return "";
            if (count == 1) return $"{points[0].x}{valueSeparator}{points[0].y}";

            return CreateString(sb =>
            {
                sb.Append($"{points[0].x}{valueSeparator}{points[0].y}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{itemSeparator}{points[i].x}{valueSeparator}{points[i].y}");
                }
            });
        }


        public static void Deserialize(string json, ref List<Vector2Int> points, char separator = ItemSeparator, char valueSeparator = ValueSeparator)
        {
            if (string.IsNullOrEmpty(json))
            {
                if (points != null)
                {
                    points.Clear();
                }
            }
            else
            {
                if (points != null)
                {
                    points.Clear();
                }
                else
                {
                    points = new List<Vector2Int>();
                }

                int index = json.IndexOf(separator);
                if (index < 0)
                {
                    Split(json, out int x, out int y, valueSeparator);
                    points.Add(new Vector2Int(x, y));
                }
                else
                {
                    Split(json.Substring(0, index), out int x, out int y, valueSeparator);
                    points.Add(new Vector2Int(x, y));

                    int startIndex = index + 1;
                    do
                    {
                        index = json.IndexOf(separator, startIndex);
                        if (index < 0)
                        {
                            Split(json.Substring(startIndex), out x, out y, valueSeparator);
                            points.Add(new Vector2Int(x, y));
                            break;
                        }

                        Split(json.Substring(startIndex, index - startIndex), out x, out y, valueSeparator);
                        points.Add(new Vector2Int(x, y));
                        startIndex = index + 1;
                    } while (true);
                }
            }
        }
        
        // NOT READY
        private static string FastSerialize(List<Vector2Int> points, char separator = ValueSeparator)
        {
            int count = points != null ? points.Count : 0;
            if (count == 0) return "";
            if (count == 1) return $"{points[0].x}{separator}{points[0].y}";

            return CreateString(sb =>
            {
                sb.Append($"{points[0].x}{separator}{points[0].y}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{points[i].x}{separator}{points[i].y}");
                }
            });
        }
        private static void FastDeserialize(string json, ref List<Vector2Int> points, char separator = ValueSeparator)
        {
            if (string.IsNullOrEmpty(json))
            {
                if (points != null)
                {
                    points.Clear();
                }
            }
            else
            {
                if (points != null)
                {
                    points.Clear();
                }
                else
                {
                    points = new List<Vector2Int>();
                }

                int length = json.Length;
                int startIndex = 0;
                int x = 0;
                bool xFound = false;
                for (int i = 0; i < length; i++)
                {
                    if (json[i] == separator)
                    {
                        if (!xFound)
                        {
                            x = json.Substring(startIndex, i - startIndex).ToInt();
                            xFound = true;
                        }
                        else
                        {
                            int y = json.Substring(startIndex, i - startIndex).ToInt();
                            points.Add(new Vector2Int(x, y));
                            xFound = false;
                        }

                        startIndex = i + 1;
                    }
                }                    
            }
        }
    }
}
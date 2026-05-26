using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anvil.Legacy
{
    public class Dictionary : Dictionary<string, string>
    {
        private static readonly char Separator = ',';
        private static readonly char Space = ' ';

        static readonly int FloatToInt = 1000000;
        static readonly float IntToFloat = 0.000001f;

        public bool SkipReturnPool { get; set; }

        public Dictionary()
        {

        }

        public Dictionary(string json)
        {
            Deserialize(json);
        }

        public void Add(string key, bool value)
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            base.Add(key, value ? "1" : "0");
        }

        public void AddBool(string key, bool value)
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            base.Add(key, value ? "True" : "False");
        }

        public void Add(string key, int value)
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            base.Add(key, value.ToString());
        }

        public void Add(string key, float value)
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            int intValue = Mathf.RoundToInt(value * FloatToInt);
            value = intValue * IntToFloat;
            base.Add(key, value.ToString());
        }

        public new void Add(string key, string value)
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            base.Add(key, value);
        }

        public void CheckAdd(string key, int value1, int value2, char separator = JsonHelper.ParamSeparator)
        {
            if (value2 != 0)
            {
                base.Add(key, $"{value1}{separator}{value2}");
            }
            else if (value1 != 0)
            {
                base.Add(key, value1.ToString());
            }
        }

        public void CheckAdd(string key, int value1, int value2, int value3, char separator = JsonHelper.ParamSeparator)
        {
            if (value3 != 0)
            {
                base.Add(key, $"{value1}{separator}{value2}{separator}{value3}");
            }
            else
            {
                CheckAdd(key, value1, value2, separator);
            }
        }

        /// <summary>
        /// Adds string of date time's binary.
        /// </summary>
        public void Add(string key, DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                base.Add(key, dateTime.Value.ToSaveString());
            }
        }

        /// <summary>
        /// Adds integer value.
        /// </summary>
        public void Add<T>(string key, T value) where T : Enum
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            int intValue = (int)(object)value;
            base.Add(key, intValue.ToString());
        }

        public void Add(string key, object value)
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            base.Add(key, $"{value}");
        }

        public void AddWithDoubleQuote(string key, string value)
        {
            Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
            base.Add(key, $"\"{value}\"");
        }

        public void AddValues(string key, List<int> values, char separator = JsonHelper.ValueSeparator)
        {
            CheckAdd(key, JsonHelper.Serialize(values, separator));
        }

        public void Add<T>(string key, List<T> values, char separator = JsonHelper.ItemSeparator) where T : ISerialize
        {
            CheckAdd(key, JsonHelper.Serialize(values, separator));
        }

        public void GetValues(string key, ref List<int> values, char separator = JsonHelper.ValueSeparator)
        {
            JsonHelper.Deserialize(GetString(key), ref values, separator);
        }

        public void Get<T>(string key, List<T> values, char separator = JsonHelper.ItemSeparator) where T : IDeserialize, new()
        {
            JsonHelper.Deserialize(GetString(key), values, separator);
        }

        public void Get<T>(string key, ref List<T> values, char separator = JsonHelper.ItemSeparator) where T : IDeserialize, new()
        {
            JsonHelper.Deserialize(GetString(key), ref values, separator);
        }

        public void CheckAdd(string key, bool value)
        {
            if (value)
            {
                Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, "1");
            }
        }

        public void CheckAdd(string key, int value)
        {
            if (value != 0)
            {
                Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, value.ToString());
            }
        }

        public void CheckAdd(string key, int value, int defaultValue)
        {
            if (value != defaultValue)
            {
                Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, value.ToString());
            }
        }

        public void CheckAdd(string key, float value)
        {
            int intValue = Mathf.RoundToInt(value * FloatToInt);
            if (intValue != 0)
            {
                Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                value = intValue * IntToFloat;
                base.Add(key, value.ToString());
            }
        }

        public void CheckAdd(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, value);
            }
        }

        public void CheckAdd<T>(string key, T value) where T : Enum
        {
            int intValue = (int)(object)value;
            if (intValue != 0)
            {
                Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, intValue.ToString());
            }
        }

        public void CheckAddDoubleQuote(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Assert.IsTrue(value.IndexOf('"') < 0);
                Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                base.Add(key, $"\"{value}\"");
            }
        }

        /// <summary>
        /// Returns [items]
        /// </summary>
        public void CheckAddItems(string key, string items)
        {
            if (!string.IsNullOrEmpty(items))
            {
                base.Add(key, $"[{items}]");
            }
        }

        /// <summary>
        /// Value format [items]
        /// </summary>
        public string GetItems(string key)
        {
            var items = GetString(key);
            if (!string.IsNullOrEmpty(items))
            {
                int length = items.Length;
                if (length >= 2)
                {
                    if (items[0] == '[' && items[length - 1] == ']')
                    {
                        return length > 2 ? items.Substring(1, length - 2) : "";
                    }
                }

                return items;
            }

            return "";
        }

        public void CheckAdd(string key, DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                base.Add(key, dateTime.Value.ToSaveString());
            }
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            if (TryGetValue(key, out string value))
            {
                return value.ToBool(defaultValue);
            }

            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            if (TryGetValue(key, out string value))
            {
                return value.ToInt(defaultValue);
            }

            return defaultValue;
        }

        public int GetIntMin(string key, int minValue, int defaultValue = 0)
        {
            if (TryGetValue(key, out string value))
            {
                return Mathf.Max(value.ToInt(defaultValue), minValue);
            }

            return Mathf.Max(defaultValue, minValue);
        }

        public long GetLong(string key, long defaultValue = 0)
        {
            if (TryGetValue(key, out string value))
            {
                return value.ToLong(defaultValue);
            }

            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            if (TryGetValue(key, out string value))
            {
                return value.ToFloat(defaultValue);
            }

            return defaultValue;
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (TryGetValue(key, out string value))
            {
                // Try to remove double quote
                if (!string.IsNullOrEmpty(value))
                {
                    int length = value.Length;
                    if (length > 1)
                    {
                        if (value[0] == '"' && value[length - 1] == '"')
                        {
                            value = length > 2 ? value.Substring(1, length - 2) : "";
                        }
                    }
                }

                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// From string of date time's binary.
        /// </summary>
        public DateTime? GetDateTime(string key, DateTime? defaultValue = null)
        {
            if (TryGetValue(key, out string value))
            {
                return value.ToDateTime2();
            }

            return defaultValue;
        }

        public Vector3 GetPosition(string xKey, string yKey)
        {
            float x = GetFloat(xKey);
            float y = GetFloat(yKey);
            return new Vector3(x, y, 0);
        }

        public Vector2 GetOffset(string xKey, string yKey)
        {
            float x = GetFloat(xKey);
            float y = GetFloat(yKey);
            return new Vector2(x, y);
        }

        public bool TryGetInt(string key, out int value)
        {
            if (TryGetValue(key, out string s))
            {
                return int.TryParse(s, out value);
            }

            value = 0;
            return false;
        }

        public bool IsEquals(string key, int value)
        {
            return GetInt(key) == value;
        }

        public bool IsEquals(string key, DateTime? value)
        {
            return GetDateTime(key).IsEquals(value);
        }

        public string Serialize()
        {
            int count = Count;
            if (count == 0) return "";

            string json = "";
            bool isFirst = true;
            foreach (var entry in this)
            {
                if (isFirst)
                {
                    json = $"[{entry.Key}:{entry.Value}";
                    isFirst = false;
                }
                else
                {
                    json = $"{json},{entry.Key}:{entry.Value}";
                }
            }

            return json + "]";
        }

        /// <summary>
        /// Parses json string to dictionary.
        /// </summary>
        /// <param name="json">[key1:value1,key2:[key21:value21,key22:value22],...,keyN:valueN].</param>
        /// <param name="isSimpleJson">{"key1":value1,"key2":{"key21":value21,...},...,"keyN":valueN}</param>
        public Dictionary Deserialize(string json, bool isSimpleJson = false)
        {
            Clear();
            if (string.IsNullOrEmpty(json)) return this;

            char openChar = '[';
            char closeChar = ']';
            if (isSimpleJson)
            {
                openChar = '{';
                closeChar = '}';
            }
            else
            {
                JsonHelper.ReduceOpenClose(ref json);
                // Assert.IsFalse(JsonHelper.IsDoubleOpenClose(json), json);
            }

            int length = json.Length;
            string key = null;
            string value = null;
            int keyStartIndex = -1;
            int valueStartIndex = -1;
            bool isKeyQuoted = false;
            bool isStringValue = false;
            char nestedOpenChar = openChar;
            char nestedCloseChar = closeChar;
            int openCount = 0;

            for (int i = 0; i < length; i++)
            {
                char c = json[i];

                // Key
                if (key == null)
                {
                    // Start index
                    if (keyStartIndex < 0)
                    {
                        if (c != Space && c != openChar && c != Separator)
                        {
                            keyStartIndex = i;
                            isKeyQuoted = c == '"';
                        }
                    }
                    // End index
                    else
                    {
                        if (c == ':')
                        {
                            for (int j = i - 1; j >= keyStartIndex; j--)
                            {
                                if (json[j] != Space)
                                {
                                    if (isKeyQuoted)
                                    {
                                        key = json.Substring(keyStartIndex + 1, j - keyStartIndex - 1);
                                    }
                                    else
                                    {
                                        key = json.Substring(keyStartIndex, j - keyStartIndex + 1);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                // Value
                else
                {
                    // Start index
                    if (valueStartIndex < 0)
                    {
                        if (c != Space)
                        {
                            if (c == Separator || c == closeChar || i == length - 1)
                            {
                                value = "";
                            }
                            else
                            {
                                valueStartIndex = i;
                                openCount = 0;
                                if (c == '\"')
                                {
                                    isStringValue = true;
                                }
                                else
                                {
                                    isStringValue = false;

                                    if (c == '[')
                                    {
                                        nestedOpenChar = '[';
                                        nestedCloseChar = ']';
                                        openCount = 1;
                                    }
                                    else if (c == '{')
                                    {
                                        nestedOpenChar = '{';
                                        nestedCloseChar = '}';
                                        openCount = 1;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // String value
                        if (isStringValue)
                        {
                            if (c == '\"')
                            {
                                value = json.Substring(valueStartIndex + 1, i - valueStartIndex - 1); // Not get ""
                            }
                        }
                        else
                        {
                            // Nested json
                            if (openCount > 0)
                            {
                                if (c == nestedOpenChar)
                                {
                                    openCount++;
                                }
                                else if (c == nestedCloseChar)
                                {
                                    openCount--;
                                    if (openCount == 0)
                                    {
                                        value = json.Substring(valueStartIndex, i - valueStartIndex + 1);
                                    }
                                }
                            }
                            else
                            {
                                if (c == Separator || c == closeChar || i == length - 1)
                                {
                                    for (int j = i - 1; j >= valueStartIndex; j--)
                                    {
                                        if (json[j] != Space)
                                        {
                                            value = json.Substring(valueStartIndex, j - valueStartIndex + 1);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (value != null)
                    {
                        Assert.IsFalse(ContainsKey(key), $"{key}:{value}");
                        base.Add(key, value);

                        key = null;
                        value = null;
                        keyStartIndex = -1;
                        valueStartIndex = -1;
                    }
                }
            }

            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = null;
            if (Count > 0)
            {
                foreach (var entry in this)
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder($"{{{entry.Key}:{entry.Value}");
                    }
                    else
                    {
                        sb = sb.Append($",\n{entry.Key}:{entry.Value}");
                    }
                }
                sb.Append("}");
            }

            return sb != null ? sb.ToString() : "";
        }

        public static string Serialize(Action<Dictionary> callback)
        {
            var sharedDict = Shared.Dictionary;
            var dict = sharedDict.Value;
            callback?.Invoke(dict);
            var json = dict.Serialize();
            sharedDict.Return();

            return json;
        }

        public static void BrowseKeyValues(string json, Action<string, string> callback, bool simpleJson = false)
        {
            var sharedDict = Shared.Dictionary;
            var dict = sharedDict.Value;
            dict.Deserialize(json, simpleJson);
            foreach (var entry in dict)
            {
                callback(entry.Key, entry.Value);
            }
            sharedDict.Return();
        }

        public static Dictionary Create(string json, bool simpleJson = false)
        {
            var dict = new Dictionary();
            dict.Deserialize(json, simpleJson);
            return dict;
        }

        public static void Deserialize(string json, Action<Dictionary> callback, bool simpleJson = false)
        {
            var sharedDict = Shared.Dictionary;
            var dict = sharedDict.Value;
            dict.Deserialize(json, simpleJson);
            callback?.Invoke(dict);

            if (!dict.SkipReturnPool)
            {
                sharedDict.Return();
            }
        }

        public static bool IsEquals(string json, Func<Dictionary, bool> func, bool simpleJson = false)
        {
            var sharedDict = Shared.Dictionary;
            var dict = sharedDict.Value;
            dict.Deserialize(json, simpleJson);
            bool isEquals = func(dict);
            if (!dict.SkipReturnPool)
            {
                sharedDict.Return();
            }
            return isEquals;
        }

        /// <summary>
        /// Returns key1: value1,\nkey2: value2,\n...
        /// </summary>
        public static string GetLogString(Action<Dictionary> callback, bool singleLine = false)
        {
            var sharedDict = Shared.Dictionary;
            var dict = sharedDict.Value;
            callback?.Invoke(dict);

            StringBuilder sb = null;
            if (dict.Count > 0)
            {
                if (singleLine)
                {
                    foreach (var entry in dict)
                    {
                        if (sb == null)
                        {
                            sb = new StringBuilder($"{entry.Key}:{entry.Value}");
                        }
                        else
                        {
                            sb = sb.Append($", {entry.Key}:{entry.Value}");
                        }
                    }
                }
                else
                {
                    foreach (var entry in dict)
                    {
                        if (sb == null)
                        {
                            sb = new StringBuilder($"{entry.Key}: {entry.Value}");
                        }
                        else
                        {
                            sb = sb.Append($",\n{entry.Key}: {entry.Value}");
                        }
                    }
                }
            }

            string s = sb != null ? sb.ToString() : "";
            sharedDict.Return();

            return s;
        }

        /// <summary>
        /// Returns {key1:value1,\nkey2:value2,\n...}
        /// </summary>
        public static string ToString(Action<Dictionary> callback, char separator = '\n')
        {
            var sharedDict = Shared.Dictionary;
            var dict = sharedDict.Value;
            callback?.Invoke(dict);

            var s = "";
            if (dict.Count > 0)
            {
                s = Helper.CreateString(sb =>
                {
                    if (separator == '\n')
                    {
                        foreach (var entry in dict)
                        {
                            if (sb.Length == 0)
                            {
                                sb.Append($"<b>{entry.Key}</b>: {entry.Value}");
                            }
                            else
                            {
                                sb.Append($",\n<b>{entry.Key}</b>: {entry.Value}");
                            }
                        }
                    }
                    else
                    {
                        foreach (var entry in dict)
                        {
                            if (sb.Length == 0)
                            {
                                sb.Append($"{{<b>{entry.Key}</b>:{entry.Value}");
                            }
                            else
                            {
                                sb.Append($",{separator}<b>{entry.Key}</b>:{entry.Value}");
                            }
                        }
                        sb.Append("}");
                    }
                });
            }

            sharedDict.Return();
            return s;
        }

        /// <summary>
        /// Returns {key1:value1,\nkey2:value2,\n...}
        /// </summary>
        public static string ToString(Action<Dictionary> callback, Func<string, string> keyNameFunc, char separator = '\n')
        {
            var sharedDict = Shared.Dictionary;
            var dict = sharedDict.Value;
            callback?.Invoke(dict);

            var s = "";
            if (dict.Count > 0)
            {
                s = Helper.CreateString(sb =>
                {
                    foreach (var entry in dict)
                    {
                        if (sb.Length == 0)
                        {
                            sb.Append($"{{<b>{keyNameFunc(entry.Key)}</b>:{entry.Value}");
                        }
                        else
                        {
                            sb.Append($",{separator}<b>{keyNameFunc(entry.Key)}</b>:{entry.Value}");
                        }
                    }
                    sb.Append("}");
                });
            }

            sharedDict.Return();
            return s;
        }
    }

    public static partial class ExtensionMethods
    {
        public static bool IsNullOrEmpty(this Dictionary dictionary)
        {
            return dictionary == null || dictionary.Count == 0;
        }

        /// <summary>
        /// Check dictionary is null.
        /// </summary>
        public static string CheckGetString(this Dictionary dictionary, string key)
        {
            return dictionary != null ? dictionary.GetString(key) : "";
        }
    }
}
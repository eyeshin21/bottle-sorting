#if DEBUG_MODE
//#define TEST_SERIALIZE_DESERIALIZE
#endif
#if TEST_SERIALIZE_DESERIALIZE
#define LOG_DESERIALIZE
#endif
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static readonly char Open = '[';
        public static readonly char Close = ']';
        public const char ValueSeparator = '_';
        public const char ItemSeparator = ',';
        const char LineSeparator = '\n';

        /// <summary>
        /// "[...]" => "..."
        /// </summary>
        static string RemoveOpenClose(string s)
        {
            if (s != null)
            {
                int length = s.Length;
                if (length > 1)
                {
                    if (s[0] == Open && s[length - 1] == Close)
                    {
                        return s.Substring(1, length - 2);
                    }
                }
            }
            return s;
        }

        static void AddItems<T>(string s, List<T> list, Func<string, T> parseFunc, char separator)
        {
            if (s == null) return;

            int length = s.Length;
            if (length == 0) return;

            int lastIndex = -1;
            for (int i = 0; i < length; i++)
            {
                if (s[i] == separator)
                {
                    // Empty
                    if (i == lastIndex + 1)
                    {
                        list.Add(parseFunc(""));
                    }
                    else
                    {
                        list.Add(parseFunc(s.Substring(lastIndex + 1, i - lastIndex - 1)));
                    }
                    lastIndex = i;
                }
            }

            // Single item
            if (lastIndex < 0)
            {
                list.Add(parseFunc(s));
            }
            else if (lastIndex < length - 1)
            {
                list.Add(parseFunc(s.Substring(lastIndex + 1)));
            }
            // Last empty
            else
            {
                list.Add(parseFunc(""));
            }
        }

        /// <summary>
        /// Checks separator out of [...]
        /// </summary>
        static void CheckAddItems(string s, List<string> list, char separator)
        {
            if (s == null) return;

            int length = s.Length;
            if (length == 0) return;

            int lastIndex = -1;
            int openCount = 0;
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if (c == Open)
                {
                    openCount++;
                }
                else if (c == Close)
                {
                    Assert.IsPositive(openCount);
                    openCount--;
                }
                else if (c == separator)
                {
                    if (openCount == 0)
                    {
                        // Empty
                        if (i == lastIndex + 1)
                        {
                            list.Add("");
                        }
                        else
                        {
                            list.Add(s.Substring(lastIndex + 1, i - lastIndex - 1));
                        }
                        lastIndex = i;
                    }
                }
            }

            // Single item
            if (lastIndex < 0)
            {
                list.Add(s);
            }
            else if (lastIndex < length - 1)
            {
                list.Add(s.Substring(lastIndex + 1));
            }
            // Last empty
            else
            {
                list.Add("");
            }
        }

        /// <summary>
        /// Checks separator out of [...]
        /// </summary>
        static void CheckAddItems<T>(string s, List<T> list, Func<string, T> parseFunc, char separator)
        {
            if (s == null) return;

            int length = s.Length;
            if (length == 0) return;

            int lastIndex = -1;
            int openCount = 0;
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if (c == Open)
                {
                    openCount++;
                }
                else if (c == Close)
                {
                    Assert.IsPositive(openCount);
                    openCount--;
                }
                else if (c == separator)
                {
                    if (openCount == 0)
                    {
                        // Empty
                        if (i == lastIndex + 1)
                        {
                            list.Add(parseFunc(""));
                        }
                        else
                        {
                            list.Add(parseFunc(s.Substring(lastIndex + 1, i - lastIndex - 1)));
                        }
                        lastIndex = i;
                    }
                }
            }

            // Single item
            if (lastIndex < 0)
            {
                list.Add(parseFunc(s));
            }
            else if (lastIndex < length - 1)
            {
                list.Add(parseFunc(s.Substring(lastIndex + 1)));
            }
            // Last empty
            else
            {
                list.Add(parseFunc(""));
            }
        }

        /// <summary>
        /// "" or "[item1,...]"
        /// </summary>
        public static void SplitItems(string s, Callback<string> callback, char separator = ItemSeparator)
        {
            Split(RemoveOpenClose(s), separator, callback);
        }

        /// <summary>
        /// "" or "[item1,...]"
        /// </summary>
        public static void SplitItems(string s, ContinueFunc<string> continueFunc, char separator = ItemSeparator)
        {
            Split(RemoveOpenClose(s), separator, continueFunc);
        }

        public static void Split(string s, char separator, Callback<string> callback, bool skipOpenClose = false)
        {
            if (s == null) return;
            //#if UNITY_EDITOR
            //           LegacyLog.Debug(s);
            //            var callback2 = callback;
            //            callback = (item) =>
            //            {
            //               LegacyLog.Debug($"\"{item}\"");
            //                callback2(item);
            //            };
            //#endif

            int length = s.Length;
            if (length == 0) return;

            int lastIndex = -1;
            if (skipOpenClose)
            {
                for (int i = 0; i < length; i++)
                {
                    if (s[i] == separator)
                    {
                        // Empty
                        if (i == lastIndex + 1)
                        {
                            callback("");
                        }
                        else
                        {
                            callback(s.Substring(lastIndex + 1, i - lastIndex - 1));
                        }
                        lastIndex = i;
                    }
                }
            }
            else
            {
                int openCount = 0;
                for (int i = 0; i < length; i++)
                {
                    char c = s[i];
                    if (c == Open)
                    {
                        openCount++;
                    }
                    else if (c == Close)
                    {
                        Assert.IsPositive(openCount);
                        openCount--;
                    }
                    else if (c == separator)
                    {
                        if (openCount == 0)
                        {
                            // Empty
                            if (i == lastIndex + 1)
                            {
                                callback("");
                            }
                            else
                            {
                                callback(s.Substring(lastIndex + 1, i - lastIndex - 1));
                            }
                            lastIndex = i;
                        }
                    }
                }
            }

            // Single item
            if (lastIndex < 0)
            {
                callback(s);
            }
            else if (lastIndex < length - 1)
            {
                callback(s.Substring(lastIndex + 1));
            }
            // Last empty
            else
            {
                callback("");
            }
        }

        public static void Split(string s, char separator, ContinueFunc<string> continueFunc, bool skipOpenClose = false)
        {
            if (s == null) return;

            int length = s.Length;
            if (length == 0) return;

            int lastIndex = -1;
            if (skipOpenClose)
            {
                for (int i = 0; i < length; i++)
                {
                    if (s[i] == separator)
                    {
                        // Empty
                        if (i == lastIndex + 1)
                        {
                            if (!continueFunc(""))
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (!continueFunc(s.Substring(lastIndex + 1, i - lastIndex - 1)))
                            {
                                return;
                            }
                        }
                        lastIndex = i;
                    }
                }
            }
            else
            {
                int openCount = 0;
                for (int i = 0; i < length; i++)
                {
                    char c = s[i];
                    if (c == Open)
                    {
                        openCount++;
                    }
                    else if (c == Close)
                    {
                        Assert.IsPositive(openCount);
                        openCount--;
                    }
                    else if (c == separator)
                    {
                        if (openCount == 0)
                        {
                            // Empty
                            if (i == lastIndex + 1)
                            {
                                if (!continueFunc(""))
                                {
                                    return;
                                }
                            }
                            else
                            {
                                if (!continueFunc(s.Substring(lastIndex + 1, i - lastIndex - 1)))
                                {
                                    return;
                                }
                            }
                            lastIndex = i;
                        }
                    }
                }
            }

            // Single item
            if (lastIndex < 0)
            {
                continueFunc(s);
            }
            else if (lastIndex < length - 1)
            {
                continueFunc(s.Substring(lastIndex + 1));
            }
            // Last empty
            else
            {
                continueFunc("");
            }
        }

        /// <summary>
        /// Returns "" or "[item1,...]"
        /// </summary>
        public static string Serialize(List<int> list)
        {
            int count = list.GetCount();
            if (count == 0) return "";
            if (count == 1) return $"[{list[0]}]";

            return CreateString(sb =>
            {
                sb.Append($"[{list[0]}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{ItemSeparator}{list[i]}");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// "[item1,...]"
        /// </summary>
        public static void Deserialize(string json, List<int> list)
        {
            list.Clear();
            AddItems(RemoveOpenClose(json), list, item => item.ToInt(), ItemSeparator);
            LogDeserialize(json, list);
        }

        /// <summary>
        /// Returns "" or "item1_..."
        /// </summary>
        public static string Serialize(List<int> list, char separator)
        {
            int count = list.GetCount();
            if (count == 0) return "";
            if (count == 1) return list[0].ToString();

            return CreateString(sb =>
            {
                sb.Append(list[0].ToString());
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{list[i]}");
                }
            });
        }

        /// <summary>
        /// "item1_..."
        /// </summary>
        public static void Deserialize(string json, List<int> list, char separator)
        {
            list.Clear();
            AddItems(json, list, item => item.ToInt(), separator);
            LogDeserialize(json, list, separator);
        }

        /// <summary>
        /// Returns "" or "["item1",...]"
        /// </summary>
        public static string Serialize(List<string> list)
        {
            int count = list.GetCount();
            if (count == 0) return "";

            Assert.NotContains(list, item => item.Contains('"'));
            if (count == 1) return $"[\"{list[0]}\"]";

            return CreateString(sb =>
            {
                sb.Append($"[\"{list[0]}\"");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{ItemSeparator}\"{list[i]}\"");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// "["item1",...]"
        /// </summary>
        public static void Deserialize(string json, List<string> list)
        {
            list.Clear();

            int length = json.GetLength();
            if (length > 0)
            {
                int startIndex = -1;
                for (int i = 0; i < length; i++)
                {
                    char c = json[i];
                    if (c == '"')
                    {
                        if (startIndex < 0)
                        {
                            startIndex = i;
                        }
                        else
                        {
                            int itemLength = i - startIndex - 1;
                            if (itemLength > 0)
                            {
                                list.Add(json.Substring(startIndex + 1, itemLength));
                            }
                            else
                            {
                                list.Add("");
                            }
                            startIndex = -1;
                        }
                    }
                }
            }

            LogDeserialize(json, list);
        }

        /// <summary>
        /// Returns "" or "[intItem1,...]"
        /// </summary>
        public static string SerializeEnums<T>(List<T> list) where T : Enum
        {
            int count = list.GetCount();
            if (count == 0) return "";
            if (count == 1) return $"[{(int)(object)list[0]}]";

            return CreateString(sb =>
            {
                sb.Append($"[{(int)(object)list[0]}");
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{ItemSeparator}{(int)(object)list[i]}");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// "[intItem1,...]"
        /// </summary>
        public static void DeserializeEnums<T>(string json, List<T> list) where T : Enum
        {
            list.Clear();
            AddItems(RemoveOpenClose(json), list, item => (T)(object)item.ToInt(), ItemSeparator);
            LogDeserialize(json, list);
        }

        /// <summary>
        /// Returns "" or "intItem1_..."
        /// </summary>
        public static string SerializeEnums<T>(List<T> list, char separator) where T : Enum
        {
            int count = list.GetCount();
            if (count == 0) return "";
            if (count == 1) return ((int)(object)list[0]).ToString();

            return CreateString(sb =>
            {
                sb.Append(((int)(object)list[0]).ToString());
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{(int)(object)list[i]}");
                }
            });
        }

        /// <summary>
        /// "intItem1_..."
        /// </summary>
        public static void DeserializeEnums<T>(string json, List<T> list, char separator) where T : Enum
        {
            list.Clear();
            AddItems(json, list, item => (T)(object)item.ToInt(), separator);
            LogDeserialize(json, list, separator);
        }

        /// <summary>
        /// Returns "" or "item1..."
        /// </summary>
        public static string SerializeContinuous(List<int> list)
        {
            int count = list.GetCount();
            if (count == 0) return "";
            if (count == 1)
            {
                Assert.IsInRange(list[0], 0, 9);
                return list[0].ToString();
            }

            return CreateString(sb =>
            {
                for (int i = 0; i < count; i++)
                {
                    Assert.IsInRange(list[i], 0, 9);
                    sb.Append(list[i].ToString());
                }
            });
        }

        /// <summary>
        /// Returns "" or "intItem1..."
        /// </summary>
        public static string SerializeEnumsContinuous<T>(List<T> list) where T : Enum
        {
            int count = list.GetCount();
            if (count == 0) return "";
            if (count == 1)
            {
                int value = (int)(object)list[0];
                Assert.IsInRange(value, 0, 9);
                return value.ToString();
            }

            return CreateString(sb =>
            {
                for (int i = 0; i < count; i++)
                {
                    int value = (int)(object)list[i];
                    Assert.IsInRange(value, 0, 9);
                    sb.Append(value.ToString());
                }
            });
        }

        /// <summary>
        /// "item1..."
        /// </summary>
        public static void DeserializeContinuous(string json, List<int> list)
        {
            list.Clear();
            if (!string.IsNullOrEmpty(json))
            {
                int length = json.Length;
                for (int i = 0; i < length; i++)
                {
                    int value = json[i] - 48;
                    if (value >= 0 && value <= 9)
                    {
                        list.Add(value);
                    }
                    else
                    {
                        LegacyLog.Warning($"Skip \'{json[i]}\'");
                    }
                }
            }
        }

        /// <summary>
        /// "intItem1..."
        /// </summary>
        public static void DeserializeEnumsContinuous<T>(string json, List<T> list) where T : Enum
        {
            list.Clear();
            if (!string.IsNullOrEmpty(json))
            {
                int length = json.Length;
                for (int i = 0; i < length; i++)
                {
                    int value = json[i] - 48;
                    if (value >= 0 && value <= 9)
                    {
                        list.Add((T)(object)value);
                    }
                    else
                    {
                        LegacyLog.Warning($"Skip \'{json[i]}\'");
                    }
                }
            }
        }

        /// <summary>
        /// Returns "" or "item1_..."
        /// </summary>
        //public static string Serialize<T>(T[] a, char separator) where T : ISerialize
        //{
        //    int count = a.GetCount();
        //    if (count == 0) return "";
        //    if (count == 1)
        //    {
        //        var s = a[0].Serialize();
        //        CheckItem(s, separator);
        //        return s;
        //    }

        //    return CreateString(sb =>
        //    {
        //        var s = a[0].Serialize();
        //        CheckItem(s, separator);
        //        sb.Append(s);
        //        for (int i = 1; i < count; i++)
        //        {
        //            s = a[i].Serialize();
        //            CheckItem(s, separator);
        //            sb.Append($"{separator}{s}");
        //        }
        //    });
        //}

        /// <summary>
        /// Returns "" or "item1_..."
        /// </summary>
        //public static string Serialize<T>(List<T> list, char separator) where T : ISerialize
        //{
        //    int count = list.GetCount();
        //    if (count == 0) return "";
        //    if (count == 1)
        //    {
        //        var s = list[0].Serialize();
        //        //Assert.NotContains(s, separator);
        //        CheckItem(s, separator);
        //        return s;
        //    }

        //    return CreateString(sb =>
        //    {
        //        var s = list[0].Serialize();
        //        //Assert.NotContains(s, separator);
        //        CheckItem(s, separator);
        //        sb.Append(s);
        //        for (int i = 1; i < count; i++)
        //        {
        //            s = list[i].Serialize();
        //            //Assert.NotContains(s, separator);
        //            CheckItem(s, separator);
        //            sb.Append($"{separator}{s}");
        //        }
        //    });
        //}

        /// <summary>
        /// "item1_..."
        /// </summary>
        //public static void Deserialize<T>(string json, List<T> list, char separator) where T : IDeserialize, new()
        //{
        //    list.Clear();
        //    CheckAddItems(json, list, itemJson =>
        //    {
        //        var item = new T();
        //        if (itemJson.Length > 0)
        //        {
        //            item.Deserialize(itemJson);
        //        }
        //        return item;
        //    }, separator);
        //    LogDeserialize(json, list, separator);
        //}

        [Conditional("LOG_DESERIALIZE")]
        static void LogDeserialize<T>(string json, List<T> list)
        {
            LegacyLog.Warning($"Deserialize \"{json}\"");
            LegacyLog.Debug(list.ToString2());
        }

        [Conditional("LOG_DESERIALIZE")]
        static void LogDeserialize<T>(string json, List<T> list, char separator)
        {
            LegacyLog.Warning($"Deserialize \"{json}\" ('{separator}')");
            LegacyLog.Debug(list.ToString2());
        }

        [Conditional("DEBUG_MODE")]
        static void CheckItem(string s, char separator)
        {
            int length = s.GetLength();
            if (length > 0)
            {
                if (length > 1)
                {
                    if (s[0] == Open && s[length - 1] == Close)
                    {
                        return;
                    }
                }

                if (s.Contains(separator))
                {
                    LegacyLog.Error($"Invalid item \"{s}\" (\'{separator}\')");
                }
            }
        }

#if TEST_SERIALIZE_DESERIALIZE
        enum TestEnum
        {
            Type1,
            Type2,
            Type3
        }

        class TestClass : IData
        {
            string _name;
            int _age;

            public string Name
            {
                get => _name;
                set => _name = value;
            }

            public int Age
            {
                get => _age;
                set => _age = value;
            }

            public TestClass()
            {

            }

            public TestClass(string name, int age)
            {
                _name = name;
                _age = age;
            }

            public string Serialize()
            {
                return Dictionary.Serialize(dict =>
                {
                    dict.Add("a", _name);
                    dict.Add("b", _age);
                });
            }

            public void Deserialize(string json)
            {
                Dictionary.Deserialize(json, dict =>
                {
                    _name = dict.GetString("a");
                    _age = dict.GetInt("b");
                });
            }

            public override bool Equals(object obj)
            {
                var other = obj as TestClass;
                if (other != null)
                {
                    return other._name.IsEquals(_name) && other._age == _age;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return Dictionary.ToString(dict =>
                {
                    dict.Add("name", (object)_name);
                    dict.Add("age", _age);
                }, ", ");
            }
        }

        [UnityEditor.MenuItem("Test/Serialize-Deserialize")]
        static void TestSerializeDeserialize()
        {
            ClearLog();
           LegacyLog.Debug("Test Serialize-Deserialize");

            char separator = '_';
            //separator = ',';

            // Ints
            var intList = new List<int>() { 1, 2, 3 };
            var intList2 = new List<int>();
            var s = Serialize(intList);
            Deserialize(s, intList2);
            Assert.IsEquals(intList, intList2);

            s = Serialize(intList, separator);
            Deserialize(s, intList2, separator);
            Assert.IsEquals(intList, intList2);

            // Strings
            var stringList = new List<string>() { "one", "two", "three,four" };
            var stringList2 = new List<string>();
            s = Serialize(stringList);
            Deserialize(s, stringList2);
            Assert.IsEquals(stringList, stringList2);

            // Enums
            var enumList = new List<TestEnum>() { TestEnum.Type1, TestEnum.Type2, TestEnum.Type3 };
            var enumList2 = new List<TestEnum>();
            s = SerializeEnums(enumList);
            DeserializeEnums(s, enumList2);
            Assert.IsEquals(enumList, enumList2);

            s = SerializeEnums(enumList, separator);
            DeserializeEnums(s, enumList2, separator);
            Assert.IsEquals(enumList, enumList2);

            // Classes
            var classList = new List<TestClass>()
            {
                new TestClass("Tom", 1),
                new TestClass("Jerry", 2),
            };
            var classList2 = new List<TestClass>();
            s = Serialize(classList);
            Deserialize(s, classList2);
            Assert.IsEquals(classList, classList2);

            s = Serialize(classList, separator);
            Deserialize(s, classList2, separator);
            Assert.IsEquals(classList, classList2);

            // ','
            classList[0].Name = "A,B";
            s = Serialize(classList);
            Deserialize(s, classList2);
            Assert.IsEquals(classList, classList2);
        }
#endif
    }
}
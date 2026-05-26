using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static List<int> GetInts(string s, char separator = ItemSeparator)
        {
            var list = Pools.GetListInt();
            if (!s.IsNullOrEmpty())
            {
                AddItems(s, list, item => item.ToInt(), separator);
            }
            return list;
        }

        public static List<float> GetFloats(string s, char separator = ItemSeparator)
        {
            var list = Pools.GetListFloat();
            if (!s.IsNullOrEmpty())
            {
                AddItems(s, list, item => item.ToFloat(), separator);
            }
            return list;
        }

        public static List<string> GetStrings(string s, char separator = ItemSeparator)
        {
            var list = Pools.GetListString();
            if (!s.IsNullOrEmpty())
            {
                AddItems(s, list, item => item, separator);
            }
            return list;
        }

        /// <summary>
        /// Returns "item1\n..."
        /// </summary>
        public static string JoinItemsByLine<T>(List<T> list) where T : ISerialize
        {
            return JoinItems(list, LineSeparator);
        }

        /// <summary>
        /// Returns "item1{separator}..."
        /// </summary>
        public static string JoinItems(List<int> list, char separator)
        {
            int count = list != null ? list.Count : 0;
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
        /// Returns "item1{separator}..."
        /// </summary>
        public static string JoinItems<T>(List<T> list, char separator) where T : ISerialize
        {
            int count = list != null ? list.Count : 0;
            if (count == 0) return "";
            if (count == 1) return list[0].Serialize();
            return CreateString(sb =>
            {
                sb.Append(list[0].Serialize());
                for (int i = 1; i < count; i++)
                {
                    sb.Append($"{separator}{list[i].Serialize()}");
                }
            });
        }

        /// <summary>
        /// String format "item1\n..."
        /// </summary>
        public static void SplitItemsByLine<T>(string s, List<T> list) where T : IDeserialize, new()
        {
            SplitItems(s, list, LineSeparator);
        }

        /// <summary>
        /// String format "item1{separator}..."
        /// </summary>
        public static void SplitItems<T>(string s, List<T> list, char separator) where T : IDeserialize, new()
        {
            list.Clear();
            if (s.IsNullOrEmpty()) return;

            int length = s.Length;
            int lastIndex = -1;
            for (int i = 0; i < length; i++)
            {
                if (s[i] == separator)
                {
                    var item = new T();
                    if (i > lastIndex + 1)
                    {
                        item.Deserialize(s.Substring(lastIndex + 1, i - lastIndex - 1));
                    }
                    list.Add(item);
                    lastIndex = i;
                }
            }

            var lastItem = new T();
            if (lastIndex < 0)
            {
                lastItem.Deserialize(s);
            }
            else if (lastIndex < length - 1)
            {
                lastItem.Deserialize(s.Substring(lastIndex + 1));
            }
            list.Add(lastItem);
        }

#if UNITY_EDITOR
        //[UnityEditor.MenuItem("Test/Parse")]
        static void TestParse()
        {
            ClearLog();
            LegacyLog.Debug("Test Parse");

            var s = "";
            var list = GetFloats(s);
            Assert.IsEmpty(list);

            s = ",1.1,2.2";
            list = GetFloats(s);
            LegacyLog.Debug(list.ToString2());
            Assert.IsEquals(list, 0, 1.1f, 2.2f);

            s = "1.1,,2.2";
            list = GetFloats(s);
            LegacyLog.Debug(list.ToString2());
            Assert.IsEquals(list, 1.1f, 0, 2.2f);

            s = "1.1,2.2,";
            list = GetFloats(s);
            LegacyLog.Debug(list.ToString2());
            Assert.IsEquals(list, 1.1f, 2.2f, 0);
        }
#endif
    }
}
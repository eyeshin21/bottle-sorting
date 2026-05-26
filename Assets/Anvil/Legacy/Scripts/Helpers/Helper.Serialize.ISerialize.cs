using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        /// <summary>
        /// Returns "" or "[item1,...]"
        /// </summary>
        public static string Serialize<T>(T[] a, char separator = ItemSeparator) where T : ISerialize
        {
            int count = a.GetCount();
            if (count == 0) return "";
            if (count == 1)
            {
                var s = a[0].Serialize();
                CheckItem(s, separator);
                return $"[{s}]";
            }

            return CreateString(sb =>
            {
                var s = a[0].Serialize();
                CheckItem(s, separator);
                sb.Append($"[{s}");
                for (int i = 1; i < count; i++)
                {
                    s = a[i].Serialize();
                    CheckItem(s, separator);
                    sb.Append($"{separator}{s}");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// "[item1,...]"
        /// </summary>
        public static T[] DeserializeArray<T>(string json, char separator = ItemSeparator) where T : IDeserialize, new()
        {
            var list = new List<T>();
            _Deserialize(json, list, separator);
            return list.ToArray();
        }

        /// <summary>
        /// Returns "" or "[item1,...]"
        /// </summary>
        public static string Serialize<T>(List<T> list, char separator = ItemSeparator) where T : ISerialize
        {
            int count = list.GetCount();
            if (count == 0) return "";
            if (count == 1)
            {
                var s = list[0].Serialize();
                CheckItem(s, separator);
                return $"[{s}]";
            }

            return CreateString(sb =>
            {
                var s = list[0].Serialize();
                CheckItem(s, separator);
                sb.Append($"[{s}");
                for (int i = 1; i < count; i++)
                {
                    s = list[i].Serialize();
                    CheckItem(s, separator);
                    sb.Append($"{separator}{s}");
                }
                sb.Append("]");
            });
        }

        /// <summary>
        /// "[item1,...]"
        /// </summary>
        public static List<T> Deserialize<T>(string json, char separator = ItemSeparator) where T : IDeserialize, new()
        {
            var list = new List<T>();
            _Deserialize(json, list, separator);
            return list;
        }

        /// <summary>
        /// "[item1,...]"
        /// </summary>
        public static void Deserialize<T>(string json, List<T> list, char separator = ItemSeparator) where T : IDeserialize, new()
        {
            Assert.IsNotNull(list);
            list.Clear();
            _Deserialize(json, list, separator);
        }

        /// <summary>
        /// "[item1,...]"
        /// </summary>
        static void _Deserialize<T>(string json, List<T> list, char separator) where T : IDeserialize, new()
        {
            Assert.IsEmpty(list);
            CheckAddItems(RemoveOpenClose(json), list, itemJson =>
            {
                var item = new T();
                if (itemJson.Length > 0)
                {
                    item.Deserialize(itemJson);
                }
                return item;
            }, separator);
            LogDeserialize(json, list);
        }
    }
}
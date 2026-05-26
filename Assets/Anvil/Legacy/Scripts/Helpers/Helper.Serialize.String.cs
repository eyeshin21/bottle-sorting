using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        /// <summary>
        /// Returns "" or "[item1,...]"
        /// </summary>
        public static string Serialize(string[,] a, char separator = ItemSeparator)
        {
            if (a == null) return "";

            var s = CreateString(sb =>
            {
                a.GetSize(out int rowCount, out int columnCount);
                int remaining = rowCount * columnCount;
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        var item = a[row, column];
                        remaining--;
                        if (remaining > 0)
                        {
                            if (string.IsNullOrEmpty(item))
                            {
                                sb.Append(separator);
                            }
                            else
                            {
                                CheckItem(item, separator);
                                sb.Append($"{item}{separator}");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                CheckItem(item, separator);
                                sb.Append(item);
                            }
                        }
                    }
                }
            });
            return $"[{s}]";
        }

        /// <summary>
        /// "[item1,...]"
        /// </summary>
        public static string[,] GetStringArray(string s, int rowCount, int columnCount, char separator = ItemSeparator)
        {
            var list = Pools.GetListString();
            CheckAddItems(RemoveOpenClose(s), list, separator);

            int count = list.Count;
            int total = rowCount * columnCount;
            //if (count != total)
            //{
            //   LegacyLog.Warning($"count={count}, total={total}");
            //}
            if (count < total)
            {
                LegacyLog.Warning($"count={count}, total={total}");
                for (int i = count; i < total; i++)
                {
                    list.Add("");
                }
            }

            var a = new string[rowCount, columnCount];
            int index = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    a[row, column] = list[index++];
                }
            }

            Pools.Return(list);
            return a;
        }

        /// <summary>
        /// "1_2_..."
        /// </summary>
        public static int GetItem(string s, int index, char separator = '_')
        {
            if (index < 0)
            {
                LegacyLog.Warning($"Invalid index {index}");
                return 0;
            }

            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            //TODO:Optimize
            var items = s.Split(separator);
            int count = items.GetCount();
            return index < count ? items[index].ToInt() : 0;
        }

        public static string SetItem(string s, int index, int value, char separator = '_')
        {
            if (index < 0)
            {
                LegacyLog.Warning($"Invalid index {index}");
                return s;
            }

            var list = new List<int>();
            if (!string.IsNullOrEmpty(s))
            {
                var items = s.Split(separator);
                foreach (var item in items)
                {
                    list.Add(item.ToInt());
                }
            }
            int count = list.Count;
            if (index >= count)
            {
                for (int i = count; i <= index; i++)
                {
                    list.Add(0);
                }
            }
            list[index] = value;

            return JoinItems(list, separator);
        }
    }
}
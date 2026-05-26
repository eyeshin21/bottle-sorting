using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        /// <summary>
        /// "01..."
        /// </summary>
        public static bool IsSerializedArrayBool(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                for (int i = s.Length - 1; i >= 0; i--)
                {
                    char c = s[i];
                    if (c < '0' || c > '1')
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns "01..."
        /// </summary>
        public static string Serialize(bool[,] a)
        {
            if (a == null) return "";

            a.GetSize(out int rowCount, out int columnCount);
            int count = rowCount * columnCount;
            var chars = new char[count];
            int index = 0;
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    chars[index++] = a[row, column] ? '1' : '0';
                }
            }

            return new string(chars, 0, count);
        }

        /// <summary>
        /// "01..."
        /// </summary>
        public static bool[,] DeserializeArrayBool(string s, int rowCount, int columnCount)
        {
            var a = new bool[rowCount, columnCount];
            if (!string.IsNullOrEmpty(s))
            {
                int length = s.Length;
                int index = 0;
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        if (index < length)
                        {
                            a[row, column] = s[index++] != '0';
                        }
                        else
                        {
                            row = rowCount;
                            break;
                        }
                    }
                }
            }
            return a;
        }

        public static string[,] SerializeArrayString<T>(T[,] a, int rowCount, int columnCount) where T : ISerialize
        {
            Assert.IsValid(a, rowCount, columnCount);
            var jsons = new string[rowCount, columnCount];
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    var item = a[row, column];
                    jsons[row, column] = item != null ? item.Serialize() : "";
                }
            }
            return jsons;
        }

        public static void Deserialize<T>(T[,] a, string[,] jsons, int rowCount, int columnCount) where T : IDeserialize
        {
            Assert.IsValid(a, rowCount, columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    var item = a[row, column];
                    if (item != null)
                    {
                        item.Deserialize(jsons[row, column]);
                    }
                    else
                    {
                        LegacyLog.Warning($"Item [{row},{column}] is null!");
                    }
                }
            }
        }
    }
}
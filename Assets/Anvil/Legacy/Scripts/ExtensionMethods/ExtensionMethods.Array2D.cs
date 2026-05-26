using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static int GetRowCount<T>(this T[,] a)
        {
            return a.GetLength(0);
        }

        public static int GetColumnCount<T>(this T[,] a)
        {
            return a.GetLength(1);
        }

        public static bool IsSize<T>(this T[,] a, int rowCount, int columnCount)
        {
            return a != null ? rowCount == a.GetLength(0) && columnCount == a.GetLength(1) : false;
        }

        public static void GetSize<T>(this T[,] a, out int rowCount, out int columnCount)
        {
            if (a != null)
            {
                rowCount = a.GetLength(0);
                columnCount = a.GetLength(1);
            }
            else
            {
                rowCount = columnCount = 0;
            }
        }

        public static void SetValues<T>(this T[,] a, int rowCount, int columnCount, T value)
        {
            if (a != null)
            {
                rowCount = Mathf.Min(rowCount, a.GetLength(0));
                columnCount = Mathf.Min(columnCount, a.GetLength(1));
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        a[row, column] = value;
                    }
                }
            }
        }

        public static bool[,] ToBools<T>(this T[,] a, Func<T, bool> func)
        {
            if (a != null)
            {
                a.GetSize(out int rowCount, out int columnCount);
                var b = new bool[rowCount, columnCount];
                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        if (func(a[row, column]))
                        {
                            b[row, column] = true;
                        }
                    }
                }
                return b;
            }
            return null;
        }

        public static void OnGUI<T>(this T[,] a, int rowCount, int columnCount) where T : IGUI
        {
            Assert.IsValid(a, rowCount, columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    a[row, column]?.OnGUI();
                }
            }
        }

        public static void Clear<T>(this T[,] a, int rowCount, int columnCount) where T : IClear
        {
            Assert.IsValid(a, rowCount, columnCount);
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    a[row, column]?.Clear();
                }
            }
        }

        public static bool IsEquals(this string[,] a, string[,] a2)
        {
            if (a != null)
            {
                if (a2 != null)
                {
                    a.GetSize(out int rowCount, out int columnCount);
                    a2.GetSize(out int rowCount2, out int columnCount2);
                    if (rowCount != rowCount2 || columnCount != columnCount2)
                    {
                        return false;
                    }
                    for (int row = 0; row < rowCount; row++)
                    {
                        for (int column = 0; column < columnCount; column++)
                        {
                            var item = a[row, column];
                            var item2 = a2[row, column];
                            if (item != item2)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                return false;
            }
            return a2 == null;
        }

        public static void LogConsole(this string[,] a)
        {
            if (a == null)
            {
                LegacyLog.Debug("(null)");
                return;
            }
            a.GetSize(out int rowCount, out int columnCount);
            LegacyLog.Debug($"{columnCount}x{rowCount}");
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    LegacyLog.Debug($"\"{a[row, column]}\"");
                }
            }
        }

        public static string ToString2(this int[,] a, string separator = ",")
        {
            if (a == null) return StringNull;

            return Helper.CreateString(sb =>
            {
                a.GetSize(out int rowCount, out int columnCount);
                for (int row = 0; row < rowCount; row++)
                {
                    var line = Helper.CreateString(sb2 =>
                    {
                        sb2.Append(a[row, 0]);
                        for (int column = 1; column < columnCount; column++)
                        {
                            sb2.Append($"{separator}{a[row, column]}");
                        }
                    });
                    if (row == 0)
                    {
                        sb.Append(line);
                    }
                    else
                    {
                        sb.Append($"\n{line}");
                    }
                }
            });
        }
    }
}
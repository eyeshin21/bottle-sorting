#if DEBUG_MODE
//#define CHECK_PARSE
#endif
using UnityEngine;
using System.Globalization;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static int GetLength(this string s)
        {
            return s != null ? s.Length : 0;
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return s == null || s.Length == 0;
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            if (s != null)
            {
                int length = s.Length;
                if (length > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        if (s[i] != ' ')
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool IsFirstCharUppercase(this string s)
        {
            if (s != null)
            {
                int length = s.Length;
                if (length > 0)
                {
                    char c = s[0];
                    return c >= 'A' && c <= 'Z';
                }
            }
            return false;
        }

        /// <summary>
        /// Null is equals empty.
        /// </summary>
        public static bool IsEquals(this string s1, string s2)
        {
            if (IsNullOrEmpty(s1))
            {
                return IsNullOrEmpty(s2);
            }
            return s1 == s2;
        }

        public static string GetFirstWord(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                int index = s.IndexOf(' ');
                if (index > 0)
                {
                    return s.Substring(0, index);
                }
            }
            return s;
        }

        public static bool ToBool(this string s, bool defaultValue = false)
        {
            if (s != null)
            {
                if (s.Length == 1)
                {
                    return s[0] != '0';
                }
                if (bool.TryParse(s, out bool value))
                {
                    return value;
                }
            }
            return defaultValue;
        }

        public static int ToInt(this string s, int defaultValue = 0)
        {
#if CHECK_PARSE
            if (s != null)
            {
                int length = s.Length;
                if (length > 0)
                {
                    char c = s[0];
                    if (c == '-' || c >= '0' && c <= '9')
                    {
                        for (int i = 1; i < length; i++)
                        {
                            c = s[i];
                            if (c < '0' || c > '9')
                            {
                               LegacyLog.Warning($"Can't parse \"{s}\" to int!");
                                break;
                            }
                        }
                    }
                    else
                    {
                       LegacyLog.Warning($"Can't parse \"{s}\" to int!");
                    }
                }
            }
#endif
            if (int.TryParse(s, out int value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Try to convert first digits.
        /// </summary>
        public static int ToIntForce(this string s, int defaultValue = 0)
        {
            if (s != null)
            {
                int length = s.Length;
                if (length > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        char c = s[i];
                        if (c < '0' || c > '9')
                        {
                            if (i > 0 && int.TryParse(s.Substring(0, i), out int value))
                            {
                                return value;
                            }

                            return defaultValue;
                        }
                    }

                    if (int.TryParse(s, out int value2))
                    {
                        return value2;
                    }
                }
            }

            return defaultValue;
        }

        public static long ToLong(this string s, long defaultValue = 0)
        {
            if (long.TryParse(s, out long value))
            {
                return value;
            }
            return defaultValue;
        }

        public static float ToFloat(this string s, float defaultValue = 0)
        {
            if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }
            return defaultValue;
        }

        public static bool TryParse(this string s, out bool value)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Length == 1)
                {
                    value = s[0] == '1';
                    return true;
                }

                return bool.TryParse(s, out value);
            }

            value = false;
            return false;
        }

        public static bool TryParse(this string s, out int value)
        {
            return int.TryParse(s, out value);
        }

        public static bool TryParse(this string s, out float value)
        {
            if (!string.IsNullOrEmpty(s))
            {
                char last = s[s.Length - 1];
                if (last == '.')
                {
                    value = 0;
                    return false;
                }
                return float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
            }

            value = 0;
            return false;
        }

        public static string ToStringInvariantCulture(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static char GetLastChar(this string s)
        {
            if (s != null)
            {
                int length = s.Length;
                if (length > 0)
                {
                    return s[length - 1];
                }
            }
            return default;
        }

        public static string GetSubstring(this string s, int startIndex, int endIndex)
        {
            if (startIndex < endIndex)
            {
                int length = s != null ? s.Length : 0;
                if (length > 0)
                {
                    if (startIndex < length)
                    {
                        if (endIndex < length)
                        {
                            return s.Substring(startIndex, endIndex - startIndex + 1);
                        }
                        return s.Substring(startIndex);
                    }
                }
            }
            return "";
        }

        public static string ReplaceApostrophes(this string s)
        {
            if (s != null)
            {
                int length = s.Length;
                for (int i = 0; i < length; i++)
                {
                    char c = s[i];
                    if (c == 'ˈ' || c == '’')
                    {
                        return Helper.CreateString(s, chars =>
                        {
                            chars[i] = '\'';
                            for (int j = i + 1; j < length; j++)
                            {
                                c = chars[j];
                                if (c == 'ˈ' || c == '’')
                                {
                                    chars[j] = '\'';
                                }
                            }
                        });
                    }
                }
            }
            return s;
        }

        public static string RemoveLineBreaks(this string s)
        {
            return RemoveCharacters(s, '\n');
        }

        public static string RemoveCharacters(this string s, char character)
        {
            int length = s != null ? s.Length : 0;
            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    if (s[i] == character)
                    {
                        var chars = Helper.GetChars(length - 1);
                        for (int j = 0; j < i; j++)
                        {
                            chars[j] = s[j];
                        }
                        int index = i;
                        for (int j = i + 1; j < length; j++)
                        {
                            char c = s[j];
                            if (c != character)
                            {
                                chars[index++] = c;
                            }
                        }
                        return new string(chars, 0, index);
                    }
                }
            }
            return s;
        }

        public static string RemovePrefix(this string s, string prefix)
        {
            int length = GetLength(s);
            int length2 = GetLength(prefix);
            if (length > 0 && length2 > 0)
            {
                if (s.StartsWith(prefix))
                {
                    return GetSubstring(s, length2, length - 1);
                }
            }
            return s;
        }

        /// <summary>
        /// ""..."" => "..."
        /// </summary>
        public static string RemoveDoubleQuote(this string s)
        {
            if (s != null)
            {
                int length = s.Length;
                if (length > 1)
                {
                    if (s[0] == '"' && s[length - 1] == '"')
                    {
                        return s.Substring(1, length - 2);
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// "[...]" => "..."
        /// </summary>
        //public static string RemoveOpenClose(this string s)
        //{
        //    if (s != null)
        //    {
        //        int length = s.Length;
        //        if (length > 1)
        //        {
        //            if (s[0] == '[' && s[length - 1] == ']')
        //            {
        //                return s.Substring(1, length - 2);
        //            }
        //        }
        //    }
        //    return s;
        //}

        public static string RemoveCharacters(this string s, int index1, int index2)
        {
            int length = s != null ? s.Length : 0;
            if (length > 0)
            {
                if (index1 > 0)
                {
                    string s1 = GetSubstring(s, 0, index1 - 1);
                    if (index2 < length - 1)
                    {
                        string s2 = s.Substring(index2 + 1);
                        if (index2 > index1 + 1)
                        {
                            return $"{s1}{GetSubstring(s, index1 + 1, index2 - 1)}{s2}";
                        }
                        return $"{s1}{s2}";
                    }
                    if (index2 > index1 + 1)
                    {
                        return $"{s1}{GetSubstring(s, index1 + 1, index2 - 1)}";
                    }
                    return s1;
                }
                if (index2 < length - 1)
                {
                    string s2 = s.Substring(index2 + 1);
                    if (index2 > 1)
                    {
                        return $"{GetSubstring(s, 1, index2 - 1)}{s2}";
                    }
                    return s2;
                }
                if (index2 > 1)
                {
                    return GetSubstring(s, 1, index2 - 1);
                }
            }
            return s;
        }

        public static string AppendLine(this string s, string s2)
        {
            if (IsNullOrEmpty(s)) return s2;
            if (IsNullOrEmpty(s2)) return s;
            return $"{s}\n{s2}";
        }

        public static string[] ToLines(this string s, bool removeEmptyLines = false)
        {
            return s.Split('\n', removeEmptyLines ? System.StringSplitOptions.RemoveEmptyEntries : System.StringSplitOptions.None);
        }

        public static void CopyToClipboard(this string s)
        {
            GUIUtility.systemCopyBuffer = s;
        }

        public static string GetFileNameWithoutExt(this string path)
        {
            if (!path.IsNullOrEmpty())
            {
                int index = path.LastIndexOf(FileHelper.PathSeparator);
                int startIndex = index > 0 ? index + 1 : 0;
                index = path.LastIndexOf('.');
                int endIndex = index > 0 ? index - 1 : path.Length - 1;
                return path.Substring(startIndex, endIndex - startIndex + 1);
            }
            return path;
        }

        public static string RemoveFileName(this string s)
        {
            if (!s.IsNullOrEmpty())
            {
                int index = s.LastIndexOf(FileHelper.DirectorySeparatorChar);
                if (index > 0)
                {
                    return s.Substring(0, index);
                }
            }
            return s;
        }

        /// <summary>
        /// "{path}/{fileName}.xxx"
        /// </summary>
        public static void SplitPath(this string s, out string path, out string fileName)
        {
            path = s;
            fileName = "";

            if (!s.IsNullOrEmpty())
            {
                int index = s.LastIndexOf(FileHelper.DirectorySeparatorChar);
                if (index > 0)
                {
                    path = s.Substring(0, index);
                    int len = s.Length - index - 1;
                    if (len > 0)
                    {
                        fileName = s.Substring(index + 1);
                        index = fileName.IndexOf('.');
                        if (index > 0)
                        {
                            fileName = fileName.Substring(0, index);
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR || DEBUG_MODE
        /// <summary>
        /// Text format "?d?h?m?s *"
        /// </summary>
        public static int ToSeconds(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;

            if (text[0] == '-')
            {
                return text.Length > 1 ? -GetSeconds(text.Substring(1)) : 0;
            }

            return GetSeconds(text);
        }

        static int GetSeconds(string text)
        {
            int seconds = 0;
            int length = text.Length;
            if (length > 1)
            {
                int index = text.IndexOf('*');
                if (index > 0)
                {
                    return text.Substring(0, index).ToInt(1) * text.Substring(index + 1).ToInt(1);
                }

                int startIndex = 0;
                int endIndex = startIndex + 1;
                for (; endIndex < length; endIndex++)
                {
                    char c = text[endIndex];
                    if (c == 'd' || c == 'D')
                    {
                        seconds += text.Substring(startIndex, endIndex - startIndex).ToInt() * 24 * 60 * 60;
                        startIndex = endIndex + 1;
                    }
                    else if (c == 'h' || c == 'H')
                    {
                        seconds += text.Substring(startIndex, endIndex - startIndex).ToInt() * 60 * 60;
                        startIndex = endIndex + 1;
                    }
                    else if (c == 'm' || c == 'M')
                    {
                        seconds += text.Substring(startIndex, endIndex - startIndex).ToInt() * 60;
                        startIndex = endIndex + 1;
                    }
                    else if (c == 's' || c == 'S')
                    {
                        seconds += text.Substring(startIndex, endIndex - startIndex).ToInt();
                        startIndex = endIndex + 1;
                    }
                }

                if (startIndex < endIndex)
                {
                    seconds += text.Substring(startIndex, endIndex - startIndex).ToInt();
                }
            }
            else
            {
                seconds = text.ToInt();
            }

            return seconds;
        }
#endif
    }
}
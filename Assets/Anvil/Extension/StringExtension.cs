using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Anvil
{
    public static class StringExtension
    {
        public static void PurgeEmpty(this List<string> strs)
        {
            for (int i = strs.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(strs[i]))
                {
                    strs.RemoveAt(i);
                }
            }
        } 
        public static string Beautify(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input,
                @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])",
                " $1$2");
        }
        
          public static int GetLength(this string s)
        {
            return s != null ? s.Length : 0;
        }
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
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
        
        public static bool ToBool(this string s, bool defaultValue = false)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.Length == 1)
                {
                    return s[0] == '0' ? false : true;
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
            if (int.TryParse(s, out int value))
            {
                return value;
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


        public static string ToCoinString(this int coin)
        {
            if (coin < 1000) return coin.ToString();
            if (coin < 1000000)
            {
                int n = coin % 1000;
                coin /= 1000;
                return $"{coin} {n:000}";
            }

            int n1 = coin % 1000;
            coin /= 1000;
            int n2 = coin % 1000;
            coin /= 1000;
            return $"{coin} {n1:000} {n2:000}";
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


        public static string ToStringInvariantCulture(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static bool IsEquals(this string s, string s2)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.IsNullOrEmpty(s2);
            }

            return s2 == s;
        }

        /// <summary>
        /// From string of date time's binary.
        /// </summary>
        public static DateTime? ToDateTime(this string s)
        {
            if (long.TryParse(s, out long dateData))
            {
                if (dateData != 0)
                {
                    return DateTime.FromBinary(dateData);
                }
            }

            return null;
        }

        public static string ToID(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                int length = s.Length;
                var chars = new char[length];
                int index = 0;
                bool uppercase = true;
                for (int i = 0; i < length; i++)
                {
                    char c = s[i];
                    if (c == ' ')
                    {
                        uppercase = true;
                    }
                    else
                    {
                        if (uppercase)
                        {
                            uppercase = false;
                            if (c >= 'a' && c <= 'z')
                            {
                                c = (char)(c - 32);
                            }
                        }
                        chars[index++] = c;
                    }
                }

                Array.Resize(ref chars, index);
                return new string(chars);
            }

            return "";
        }

        public static string AppendLine(this string s, string s2)
        {
            if (string.IsNullOrEmpty(s)) return s2;
            if (string.IsNullOrEmpty(s2)) return s;
            return $"{s}\n{s2}";
        }

        public static void CopyToClipboard(this string s)
        {
            GUIUtility.systemCopyBuffer = s;
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
       
    }
}
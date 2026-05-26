using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static char[] _chars;
        public static char[] GetChars(int minLength)
        {
            if (_chars == null)
            {
                _chars = new char[Mathf.Max(minLength, 64)];
            }
            else if (_chars.Length < minLength)
            {
                Array.Resize(ref _chars, minLength);
            }

            return _chars;
        }

        public static char[] GetChars(string s, out int length)
        {
            length = s != null ? s.Length : 0;
            var chars = GetChars(length);
            for (int i = 0; i < length; i++)
            {
                chars[i] = s[i];
            }
            return chars;
        }

        public static string Truncate(string s, int maxLength)
        {
            if (s != null && maxLength >= 3)
            {
                int length = s.Length;
                if (length > maxLength)
                {
                    return $"{s.Substring(0, maxLength - 3)}...";
                }
            }
            return s;
        }

        public static string CreateString(char[] chars, int count = -1)
        {
            if (chars != null)
            {
                if (count < 0)
                {
                    count = chars.Length;
                }
                if (count > 0)
                {
                    return new string(chars, 0, count);
                }
            }
            return "";
        }

        public static string CreateString(string s, Callback<char[]> callback)
        {
            int length = s.Length;
            var chars = GetChars(length);
            for (int i = 0; i < length; i++)
            {
                chars[i] = s[i];
            }
            callback(chars);
            return new string(chars, 0, length);
        }

        static Stack<StringBuilder> _stringBuilders = new();
        public static string CreateString(Callback<StringBuilder> callback)
        {
            var stringBuilder = GetStringBuilder();
            callback(stringBuilder);
            var s = stringBuilder.ToString();
            ReturnStringBuilder(stringBuilder);
            return s;
        }

        public static StringBuilder GetStringBuilder()
        {
            StringBuilder stringBuilder;
            if (_stringBuilders.Count > 0)
            {
                stringBuilder = _stringBuilders.Pop();
                Assert.IsZero(stringBuilder.Length);
            }
            else
            {
                stringBuilder = new StringBuilder();
            }
            return stringBuilder;
        }

        public static void ReturnStringBuilder(StringBuilder stringBuilder)
        {
            stringBuilder.Clear();
            _stringBuilders.Push(stringBuilder);
        }

        /// <summary>
        /// Returns class name without namespace.
        /// </summary>
        public static string GetClassName<T>()
        {
            return GetClassName(typeof(T));
        }

        /// <summary>
        /// Returns class name without namespace.
        /// </summary>
        public static string GetClassName(object obj)
        {
            return obj != null ? GetClassName(obj.GetType()) : "(null)";
        }

        /// <summary>
        /// Returns class name without namespace.
        /// </summary>
        public static string GetClassName(Type type)
        {
            var name = type.Name;
            int index = name.LastIndexOf('.');
            return index > 0 ? name.Substring(index + 1) : name;
        }

        /// <summary>
        /// Returns file name without extension.
        /// </summary>
        public static string GetFileName(string path)
        {
            if (!path.IsNullOrEmpty())
            {
                int length = path.Length;
                int endIndex = length - 1;
                for (int i = length - 1; i >= 0; i--)
                {
                    if (path[i] == '.')
                    {
                        endIndex = i - 1;
                        break;
                    }
                }
                for (int i = endIndex - 1; i >= 0; i--)
                {
                    if (path[i] == Constants.PathSeparator)
                    {
                        return path.Substring(i + 1, endIndex - i);
                    }
                }
            }
            return path;
        }

        /// <summary>
        /// Folder1/Folder2/FileName.xxx => Folder1/Folder2
        /// </summary>
        public static string GetFolderPath(string path)
        {
            if (!path.IsNullOrEmpty())
            {
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    if (path[i] == Constants.PathSeparator)
                    {
                        return path.Substring(0, i);
                    }
                }
            }
            return path;
        }

        /// <summary>
        /// "AbcXyz" => "Abc Xyz"
        /// </summary>
        public static string GetNicifyName<T>() where T : Enum
        {
            return GetNicifyName(typeof(T).Name);
        }

        /// <summary>
        /// "AbcXyz" => "Abc Xyz"
        /// </summary>
        public static string GetNicifyName<T>(T value) where T : Enum
        {
            return GetNicifyName(value.ToString());
        }

        /// <summary>
        /// "AbcXyz" => "Abc Xyz"
        /// </summary>
        public static string GetNicifyName(string name)
        {
            int length = name.Length;
            if (length > 1)
            {
                var chars = StringHelper.GetChars(length + 3);
                int index = 0;
                bool checkDigit = true;

                for (int i = 0; i < length; i++)
                {
                    char c = name[i];
                    if (c == '_') continue;

                    if (index == 0)
                    {
                        if (c >= 'a' && c <= 'z')
                        {
                            c = c.ToUpper();
                        }
                    }
                    else
                    {
                        if (c >= '0' && c <= '9')
                        {
                            if (checkDigit)
                            {
                                checkDigit = false;
                                chars[index++] = ' ';
                            }
                        }
                        else
                        {
                            if (checkDigit)
                            {
                                if (c >= 'A' && c <= 'Z')
                                {
                                    chars[index++] = ' ';
                                }
                            }
                            else
                            {
                                checkDigit = true;
                                chars[index++] = ' ';
                            }
                        }
                    }

                    chars[index++] = c;
                }
                name = new string(chars, 0, index);
            }

            return name;
        }

        public static string GetLogString(params object[] @params)
        {
            int count = @params.Length;
            if (count == 0) return "{}";
            if (count == 1) return $"{{{@params[0]}}}]";

            return CreateString(sb =>
            {
                int count2 = count / 2;
                int index = 0;
                if (count % 2 == 0)
                {
                    sb.Append($"{{{@params[index++]}={@params[index++]}");
                }
                else
                {
                    sb.Append($"{{{@params[index++]}: {@params[index++]}={@params[index++]}");
                }
                for (int i = 1; i < count2; i++)
                {
                    sb.Append($", {@params[index++]}={@params[index++]}");
                }
                sb.Append("}");
            });
        }
    }
}
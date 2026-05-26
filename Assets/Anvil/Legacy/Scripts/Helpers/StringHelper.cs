//#if DEBUG_MODE
//#define DEBUG_STRING_HELPER
//#endif
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anvil.Legacy
{
    public static class StringHelper
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

        public static string GetString(int length, Action<char[]> callback)
        {
            if (callback != null)
            {
                var chars = GetChars(length);
                callback(chars);
                return new string(chars, 0, length);
            }
            return "";
        }

        static Stack<StringBuilder> _stringBuilders = new Stack<StringBuilder>();
        public static string GetString(Action<StringBuilder> callback)
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
            callback(stringBuilder);

            var s = stringBuilder.ToString();
            stringBuilder.Clear();
            _stringBuilders.Push(stringBuilder);

            return s;
        }

        public static void Browse(string s, char separator, Action<string> callback, bool skipEmpty = false)
        {
            Assert.IsNotNull(callback);
            if (s.IsNullOrEmpty()) return;

            int length = s.Length;
            int lastIndex = -1;
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if (c == separator)
                {
                    int len = i - lastIndex - 1;
                    if (len > 0)
                    {
                        callback(s.Substring(lastIndex + 1, len));
                    }
                    else if (!skipEmpty)
                    {
                        callback("");
                    }
                    lastIndex = i;
                }
            }

            if (lastIndex < 0)
            {
                callback(s);
            }
            else
            {
                if (lastIndex < length - 1)
                {
                    callback(s.Substring(lastIndex + 1));
                }
                else if (!skipEmpty)
                {
                    callback("");
                }
            }
        }
    }
}
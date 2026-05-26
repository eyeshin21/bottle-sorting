using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        /// <summary>
        /// Checks to create new list if it is null.
        /// </summary>
        public static void CheckAdd<T>(ref List<T> list, T item)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            list.Add(item);
        }

        public static List<T> CreateList<T>(T item)
        {
            return new List<T>() { item };
        }

        public static List<T> CreateOrClear<T>(ref List<T> list)
        {
            if (list == null)
            {
                list = new List<T>();
            }
            else
            {
                list.Clear();
            }
            return list;
        }

        /// <summary>
        /// Adds new item by default value or new object.
        /// </summary>
        public static void SetCount<T>(this List<T> list, int newCount)
        {
            int count = list.Count;
            int delta = newCount - count;
            if (delta < 0)
            {
                for (int i = count - 1; i >= newCount; i--)
                {
                    list.RemoveAt(i);
                }
            }
            else if (delta > 0)
            {
                var type = typeof(T);
                if (type.IsValueType || type == TypeString)
                {
                    T defaultValue = default;
                    for (int i = 0; i < delta; i++)
                    {
                        list.Add(defaultValue);
                    }
                }
                else
                {
                    for (int i = 0; i < delta; i++)
                    {
                        list.Add((T)Activator.CreateInstance(type));
                    }
                }
            }
        }

        /// <summary>
        /// If newFunc is null then adds new item by default value.
        /// </summary>
        public static void SetCount<T>(this List<T> list, int newCount, Func<T> newFunc)
        {
            int count = list.Count;
            int delta = newCount - count;
            if (delta < 0)
            {
                for (int i = count - 1; i >= newCount; i--)
                {
                    list.RemoveAt(i);
                }
            }
            else if (delta > 0)
            {
                if (newFunc != null)
                {
                    for (int i = 0; i < delta; i++)
                    {
                        list.Add(newFunc());
                    }
                }
                else
                {
                    T defaultValue = default;
                    for (int i = 0; i < delta; i++)
                    {
                        list.Add(defaultValue);
                    }
                }
            }
        }

        //public static void AddWords(string s, List<string> words, Func<string, bool> acceptFunc)
        //{
        //    if (s.IsNullOrEmpty()) return;

        //    int length = s.Length;
        //    int startIndex = -1;
        //    for (int i = 0; i < length; i++)
        //    {
        //        char c = s[i];
        //        if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c == '-' || c == '\'')
        //        {
        //            if (startIndex < 0)
        //            {
        //                startIndex = i;
        //            }
        //        }
        //        else
        //        {
        //            if (startIndex >= 0)
        //            {
        //                var word = s.Substring(startIndex, i - startIndex);
        //                if (acceptFunc(word))
        //                {
        //                    words.Add(word);
        //                }
        //                startIndex = -1;
        //            }
        //        }
        //    }

        //    if (startIndex >= 0)
        //    {
        //        var word = s.Substring(startIndex);
        //        if (acceptFunc(word))
        //        {
        //            words.Add(word);
        //        }
        //    }
        //}
    }
}
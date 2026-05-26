using System;
using System.Collections.Generic;
using System.Diagnostics;
using Anvil.Legacy.Actions;
using UnityEngine;

namespace Anvil.Legacy
{
    public static class Assert
    {
        #region bool
        [Conditional("DEBUG_MODE")]
        public static void IsTrue(bool condition, object message = null)
        {
            if (!condition)
            {
                Throw(message ?? "condition = False");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsFalse(bool condition, object message = null)
        {
            if (condition)
            {
                Throw(message ?? "condition = True");
            }
        }
        #endregion // bool

        #region int
        [Conditional("DEBUG_MODE")]
        public static void IsZero(int value, object message = null)
        {
            if (value != 0)
            {
                Throw(message ?? $"Not zero: {value}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsPositive(int value, object message = null)
        {
            if (value <= 0)
            {
                Throw(message ?? $"Not positive: {value}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsEquals(int value, int other, object message = null)
        {
            if (value != other)
            {
                Throw(message ?? $"Not equals: {value} vs {other}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotEquals(int value, int other, object message = null)
        {
            if (value == other)
            {
                Throw(message ?? $"Equals: {value}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsLessThan(int value, int max, object message = null)
        {
            if (value >= max)
            {
                Throw(message ?? $"Greater than or equals: {value} >= {max}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsLessThanOrEquals(int value, int max, object message = null)
        {
            if (value > max)
            {
                Throw(message ?? $"Greater than: {value} > {max}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsGreaterThan(int value, int min, object message = null)
        {
            if (value <= min)
            {
                Throw(message ?? $"Less than or equals: {value} <= {min}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsGreaterThanOrEquals(int value, int min, object message = null)
        {
            if (value < min)
            {
                Throw(message ?? $"Less than: {value} < {min}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsInRange(int value, int min, int max, object message = null)
        {
            if (value < min || value > max)
            {
                Throw(message ?? $"{value} out of range [{min},{max}]");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotInRange(int value, int min, int max, object message = null)
        {
            if (value >= min && value <= max)
            {
                Throw(message ?? $"{value} in range [{min},{max}]");
            }
        }
        #endregion // int

        #region float
        [Conditional("DEBUG_MODE")]
        public static void IsZero(float value, object message = null)
        {
            if (value != 0)
            {
                Throw(message ?? $"{value} != 0");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsPositive(float value, object message = null)
        {
            if (value <= 0)
            {
                Throw(message ?? $"{value} <= 0");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsEquals(float value, float other, object message = null)
        {
            if (value != other)
            {
                Throw(message ?? $"{value} != {other}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotEquals(float value, float other, object message = null)
        {
            if (value == other)
            {
                Throw(message ?? $"{value} = {other}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsLessThan(float value, float max, object message = null)
        {
            if (value >= max)
            {
                Throw(message ?? $"{value} >= {max}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsLessThanOrEquals(float value, float max, object message = null)
        {
            if (value > max)
            {
                Throw(message ?? $"{value} > {max}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsGreaterThan(float value, float min, object message = null)
        {
            if (value <= min)
            {
                Throw(message ?? $"{value} <= {min}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsGreaterThanOrEquals(float value, float min, object message = null)
        {
            if (value < min)
            {
                Throw(message ?? $"{value} < {min}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsInRange(float value, float min, float max, object message = null)
        {
            if (value < min || value > max)
            {
                Throw(message ?? $"{value} out of range [{min},{max}]");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotInRange(float value, float min, float max, object message = null)
        {
            if (value >= min && value <= max)
            {
                Throw(message ?? $"{value} in range [{min},{max}]");
            }
        }
        #endregion // float

        #region string
        [Conditional("DEBUG_MODE")]
        public static void IsNullOrEmpty(string s)
        {
            if (!s.IsNullOrEmpty())
            {
                Throw(s);
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotNullAndNotEmpty(string s)
        {
            if (s.IsNullOrEmpty())
            {
                Throw(s);
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void Contains(string s, char c, object message = null)
        {
            if (s != null)
            {
                for (int i = s.Length - 1; i >= 0; i--)
                {
                    if (s[i] == c)
                    {
                        return;
                    }
                }
            }

            Throw(message ?? $"\"{s}\" not contains '{c}'");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains(string s, char c, object message = null)
        {
            if (s != null)
            {
                for (int i = s.Length - 1; i >= 0; i--)
                {
                    if (s[i] == c)
                    {
                        Throw(message ?? $"\"{s}\" contains '{c}'");
                        return;
                    }
                }
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void Contains(string s, string value)
        {
            if (s == null || !s.Contains(value))
            {
                Throw(s);
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains(string s, string value)
        {
            if (s != null && s.Contains(value))
            {
                Throw(s);
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void EndsWith(string s, string suffix)
        {
            if (s == null || !s.EndsWith(suffix))
            {
                Throw(s);
            }
        }
        #endregion // string

        #region object
        [Conditional("DEBUG_MODE")]
        public static void IsNull(object obj, object message = null)
        {
            if (obj != null)
            {
                Throw(message ?? obj);
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotNull(object obj, object message = null)
        {
            if (obj == null)
            {
                Throw(message ?? "Object is null");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsEquals(object value, object other, object message = null)
        {
            if (!value.Equals(other))
            {
                Throw(message ?? $"{value} != {other}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotEquals(object value, object other, object message = null)
        {
            if (value.Equals(other))
            {
                Throw(message ?? $"{value} = {other}");
            }
        }
        #endregion // object

        #region Array
        [Conditional("DEBUG_MODE")]
        public static void Contains<T>(T[] array, T item, object message = null)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (array[i].Equals(item))
                {
                    return;
                }
            }

            Throw(message ?? $"Not contains {item}");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains<T>(T[] array, T item, object message = null)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (array[i].Equals(item))
                {
                    Throw(message ?? $"Contains {item}");
                    return;
                }
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains<T>(T[] array, int count, T item, object message = null)
        {
            for (int i = 0; i < count; i++)
            {
                if (array[i].Equals(item))
                {
                    Throw(message ?? $"Contains {item}");
                    return;
                }
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsUnique<T>(T[] array, object message = null)
        {
            T item = default;
            bool unique = true;
            int count = array.Length;
            for (int i = 0; i < count - 1; i++)
            {
                item = array[i];
                for (int j = i + 1; j < count; j++)
                {
                    if (item.Equals(array[j]))
                    {
                        unique = false;
                        i = count;
                        break;
                    }
                }
            }

            if (!unique)
            {
                Throw(message ?? $"Duplicate {item}");
            }
        }
        #endregion // Array

        #region Array-2D
        [Conditional("DEBUG_MODE")]
        public static void IsValid<T>(T[,] array, int rowCount, int columnCount, object message = null)
        {
            array.GetSize(out int _rowCount, out int _columnCount);
            if (rowCount > _rowCount || columnCount > _columnCount)
            {
                Throw(message ?? $"{_columnCount}x{_rowCount} vs {columnCount}x{rowCount}");
            }
        }
        #endregion

        #region List
        [Conditional("DEBUG_MODE")]
        public static void IsEmpty<T>(List<T> list, object message = null)
        {
            if (list.Count > 0)
            {
                Throw(message ?? $"count = {list.Count}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotEmpty<T>(List<T> list, object message = null)
        {
            if (list.Count == 0)
            {
                Throw(message ?? "List is empty");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void Contains<T>(List<T> list, T item, object message = null)
        {
            if (!list.Contains(item))
            {
                Throw(message ?? $"Not contains {item}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains<T>(List<T> list, T item, object message = null)
        {
            if (list.Contains(item))
            {
                Throw(message ?? $"Contains {item}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void Contains<T>(List<T> list, Func<T, bool> containFunc, object message = null)
        {
            T item = default;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                item = list[i];
                if (containFunc(item))
                {
                    return;
                }
            }

            Throw(message ?? $"Not contains {item}");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains<T>(List<T> list, Func<T, bool> containFunc, object message = null)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                T item = list[i];
                if (containFunc(item))
                {
                    Throw(message ?? $"Contains {item}");
                    return;
                }
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsUnique<T>(List<T> list, object message = null)
        {
            T item = default;
            bool unique = true;
            int count = list.Count;
            for (int i = 0; i < count - 1; i++)
            {
                item = list[i];
                for (int j = i + 1; j < count; j++)
                {
                    if (item.Equals(list[j]))
                    {
                        unique = false;
                        i = count;
                        break;
                    }
                }
            }

            if (!unique)
            {
                Throw(message ?? $"Duplicate {item}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsEquals<T>(List<T> list, params T[] items)
        {
            int count = list.Count;
            int length = items.Length;
            if (count != length)
            {
                Throw($"count={count} vs length={length}");
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var item1 = list[i];
                    var item2 = items[i];
                    if (!item1.Equals(item2))
                    {
                        Throw($"{item1} vs {item2}");
                    }
                }
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsEquals<T>(List<T> list1, List<T> list2)
        {
            bool equals = true;
            if (list1 != null)
            {
                if (list2 == null)
                {
                    equals = false;
                }
            }
            else
            {
                if (list2 != null)
                {
                    equals = false;
                }
            }

            if (equals)
            {
                int count1 = list1.Count;
                int count2 = list2.Count;
                if (count1 != count2)
                {
                    Throw($"count1={count1} vs count2={count2}");
                }
                else
                {
                    for (int i = 0; i < count1; i++)
                    {
                        var item1 = list1[i];
                        var item2 = list2[i];
                        if (!item1.Equals(item2))
                        {
                            Throw($"{item1} vs {item2}");
                        }
                    }
                }
            }
            else
            {
                Throw($"{list1} vs {list2}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsAscendingIndices(List<int> indices, object message = null)
        {
            bool ascending = false;
            int count = indices.Count;
            if (count > 0)
            {
                ascending = true;
                if (count > 1)
                {
                    IsGreaterThanOrEquals(indices[0], 0);
                    for (int i = 1; i < count; i++)
                    {
                        if (indices[i] <= indices[i - 1])
                        {
                            ascending = false;
                            break;
                        }
                    }
                }
            }

            if (!ascending)
            {
                Throw(message ?? indices.ToString2());
            }
        }
        #endregion // List

        #region Transform
        [Conditional("DEBUG_MODE")]
        public static void Contains(Transform transform, Transform child, object message = null)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (transform.GetChild(i) == child)
                {
                    return;
                }
            }

            Throw(message ?? $"Not contains {child}");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains(Transform transform, Transform child, object message = null)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (transform.GetChild(i) == child)
                {
                    Throw(message ?? $"Contains {child}");
                    return;
                }
            }
        }
        #endregion // Transform

        #region Stack
        [Conditional("DEBUG_MODE")]
        public static void Contains<T>(Stack<T> stack, T item, object message = null)
        {
            if (!stack.Contains(item))
            {
                Throw(message ?? $"Not contains {item}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContains<T>(Stack<T> stack, T item, object message = null)
        {
            if (stack.Contains(item))
            {
                Throw(message ?? $"Contains {item}");
            }
        }
        #endregion // Stack

        #region Dictionary
        [Conditional("DEBUG_MODE")]
        public static void IsEmpty<TKey, TValue>(Dictionary<TKey, TValue> dict, object message = null)
        {
            if (dict.Count > 0)
            {
                Throw(message ?? $"count = {dict.Count}");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void IsNotEmpty<TKey, TValue>(Dictionary<TKey, TValue> dict, object message = null)
        {
            if (dict.Count == 0)
            {
                Throw(message ?? "Dictionary is empty");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void ContainsKey<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, object message = null)
        {
            if (!dict.ContainsKey(key))
            {
                Throw(message ?? $"Not contains key \"{key}\"");
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void NotContainsKey<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, object message = null)
        {
            if (dict.ContainsKey(key))
            {
                Throw(message ?? $"Contains key \"{key}\"");
            }
        }
        #endregion // Dictionary

        #region Action
        [Conditional("DEBUG_MODE")]
        public static void IsNullOrFinished(ActionX action, object message = null)
        {
            if (action != null && !action.Finished)
            {
                Throw(message ?? "Action not finished");
            }
        }
        #endregion // Action

        #region Others
        [Conditional("DEBUG_MODE")]
        public static void Todo(object message)
        {
            Throw($"[TODO] {message}");
        }

        [Conditional("DEBUG_MODE")]
        public static void Todo(string message)
        {
            Throw($"[TODO] {message}");
        }

        [Conditional("DEBUG_MODE")]
        public static void Todo(string format, params object[] args)
        {
            Throw($"[TODO] {string.Format(format, args)}");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotSupported(object message)
        {
            Throw($"[NOT SUPPORTED] {message}");
        }

        [Conditional("DEBUG_MODE")]
        public static void Bug(object message)
        {
            Throw($"[BUG] {message}");
        }
        #endregion // Others

        static void Throw(object message)
        {
            //throw new Exception($"{message}");
            LegacyLog.Error(message);
        }
    }
}
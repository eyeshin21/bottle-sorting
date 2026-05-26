using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static List<IntString> CreateListIntString(params int[] ints)
        {
            int count = ints.GetLength();
            var list = new List<IntString>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(new IntString(ints[i]));
            }
            return list;
        }

        public static List<IntString> CreateListIntString(int int1, string string1, int int2, string string2, int int3, string string3)
        {
            return new List<IntString>()
            {
                new IntString(int1, string1),
                new IntString(int2, string2),
                new IntString(int3, string3),
            };
        }

        public static List<IntString> CreateListIntString<T>() where T : Enum
        {
            var type = typeof(T);
            var values = Enum.GetValues(type);
            var names = Enum.GetNames(type);
            int count = Mathf.Min(values.Length, names.Length);
            var list = new List<IntString>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(new IntString((int)values.GetValue(i), GetNicifyName(names[i])));
            }
            return list;
        }

        public static List<IntString> CreateListIntString<T>(T t1, string string1, T t2, string string2, T t3, string string3) where T : Enum
        {
            return new List<IntString>()
            {
                new IntString((int)(object)t1, string1),
                new IntString((int)(object)t2, string2),
                new IntString((int)(object)t3, string3),
            };
        }
    }
}
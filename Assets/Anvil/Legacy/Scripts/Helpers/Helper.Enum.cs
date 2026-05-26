using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static int GetEnumCount<T>() where T : struct
        {
            return Enum.GetNames(typeof(T)).Length;
        }

        public static T[] GetEnumValues<T>() where T : struct
        {
            var values = Enum.GetValues(typeof(T));
            int count = values.Length;
            var values2 = new T[count];
            int index = 0;
            foreach (var value in values)
            {
                values2[index++] = (T)value;
            }
            return values2;
        }

        public static void GetNamesValues<T>(out string[] names, out int[] values, bool skipFirst = false) where T : struct
        {
            var type = typeof(T);
            var enumNames = Enum.GetNames(type);
            var enumValues = Enum.GetValues(type);
            int length = enumNames.Length;
            int count = skipFirst ? length - 1 : length;
            names = new string[count];
            values = new int[count];
            if (skipFirst)
            {
                for (int i = 0; i < count; i++)
                {
                    names[i] = enumNames[i + 1];
                    values[i] = (int)enumValues.GetValue(i + 1);
                    //Log.Debug($"{names[i]}={values[i]} ({(T)(object)values[i]})");
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    names[i] = enumNames[i];
                    values[i] = (int)enumValues.GetValue(i);
                    //Log.Debug($"{names[i]}={values[i]} ({(T)(object)values[i]})");
                }
            }
        }
    }
}
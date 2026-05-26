using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static int ToInt<T>(this T value) where T : Enum
        {
            return (int)(object)value;
        }
    }
}
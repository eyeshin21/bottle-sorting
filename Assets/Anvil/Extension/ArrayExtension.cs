using System;

namespace Anvil.Extension
{
    public static class ArrayExtension
    {
        public static int GetLenght(this Array array)
        {
            if (array != null)
            {
                return array.Length;
            }
            return 0;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static string GetIntString(int value)
        {
            return value.ToString();
        }

        public static string GetNumberStringWithRotation(int number)
        {
            if (number == 6 || number == 9 || number == 66 || number == 99)
            {
                return $"<u>{number}</u>";
            }
            return number.ToString();
        }
    }
}
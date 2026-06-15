using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static bool IsLowercase(this char c)
        {
            return c >= 'a' && c <= 'z';
        }

        public static bool IsUppercase(this char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        public static bool IsVowel(this char c)
        {
            int code = c;
            // 'A'=65, 'U'=85
            if (code > 64 && code < 86)
            {
                // Lowercase
                code += 32;
            }
            // 'a'=97, 'u'=117
            if (code > 96 && code < 118)
            {
                return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
            }
            return false;
        }

        public static char ToLower(this char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return (char)(c + 32);
            }
            return c;
        }

        public static char ToUpper(this char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return (char)(c - 32);
            }
            return c;
        }
    }
}
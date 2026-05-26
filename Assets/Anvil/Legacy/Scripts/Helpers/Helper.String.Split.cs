using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static void Split(string s, out string s1, out string s2, char separator)
        {
            s1 = "";
            s2 = "";

            if (!s.IsNullOrEmpty())
            {
                int index = s.IndexOf(separator);
                if (index > 0)
                {
                    s1 = s.Substring(0, index);
                    if (index < s.Length - 1)
                    {
                        s2 = s.Substring(index + 1);
                    }
                }
                else if (index < 0)
                {
                    s1 = s;
                }
                else
                {
                    if (index < s.Length - 1)
                    {
                        s2 = s.Substring(1);
                    }
                }
            }
        }

        public static void Split(string s, out int value1, out int value2, char separator)
        {
            Split(s, out string s1, out string s2, separator);
            value1 = s1.ToInt();
            value2 = s2.ToIntForce();
        }

        public static bool SplitNameNumber(string s, out string name, out int number)
        {
            int length = s != null ? s.Length : 0;
            if (length >= 2)
            {
                int endIndex = length - 1;
                char c = s[endIndex];
                if (c >= '0' && c <= '9')
                {
                    for (int i = endIndex - 1; i >= 0; i--)
                    {
                        c = s[i];
                        if (c < '0' || c > '9')
                        {
                            int j = i;
                            while (j >= 0 && s[j] == ' ')
                            {
                                j--;
                            }

                            name = j < 0 ? "" : s.Substring(0, j + 1);
                            number = s.Substring(i + 1).ToInt();
                            return true;
                        }
                    }
                }
            }

            name = "";
            number = 0;
            return false;
        }
    }
}
using UnityEngine;

namespace Anvil
{
    public class IntString
    {
        public int Int { get; set; }
        public string String { get; set; }

        public IntString()
        {
        }

        public IntString(int i)
        {
            Int = i;
            String = i.ToString();
        }

        public IntString(int i, string s)
        {
            Int = i;
            String = s;
        }

        public override string ToString()
        {
            return $"{{{Int}:\"{String}\"}})";
        }
    }
}
#region
using System.Text;
#endregion

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static string TrimLowerCase(this string input)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] >= 'A' && input[i] <= 'Z')
                    sb.Append((char)(input[i]));
            }

            return sb.ToString();
        }
    }
}
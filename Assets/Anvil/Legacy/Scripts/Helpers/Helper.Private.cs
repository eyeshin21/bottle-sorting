using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static string GetHashName(string name)
        {
            return Mathf.Abs(Animator.StringToHash(name)).ToString();
        }
    }
}
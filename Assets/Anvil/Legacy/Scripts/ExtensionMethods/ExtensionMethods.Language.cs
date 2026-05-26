using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static string GetCode(this SystemLanguage language)
        {
            return Helper.GetLanguageCode(language);
        }
    }
}
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Anvil.Extension
{
    public static class DictionaryExtension
    {
        public static List<TKey> GetKeys<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            var keys = new List<TKey>(dict.Count);
            foreach (var key in dict.Keys)
            {
                keys.Add(key);
            }

            return keys;
        }

    }
}
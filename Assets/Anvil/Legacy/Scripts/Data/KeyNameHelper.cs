#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class KeyNameHelper
    {
        static Dictionary<string, string> _keyNames = new();

        public static string GetName(string key)
        {
            if (!_keyNames.TryGetValue(key, out string name))
            {
                name = key;
                _keyNames.Add(key, name);
            }
            return name;
        }
    }
}
#endif
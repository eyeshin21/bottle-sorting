#if DEBUG_MODE
#define DEBUG_LOCAL_KEYS
#endif
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class LocalKeys
    {
#if DEBUG_LOCAL_KEYS
        static Dictionary<string, string> _keyNames = new();
#endif

        public static string GetName(string key)
        {
#if DEBUG_LOCAL_KEYS
            if (_keyNames.TryGetValue(key, out string name))
            {
                return name;
            }

           LegacyLog.Warning($"Can't get name of key \"{key}\"!");
#endif
            return key;
        }

        public static string AddKey(string key, string name)
        {
#if DEBUG_LOCAL_KEYS
            if (_keyNames.ContainsKey(key))
            {
               LegacyLog.Error($"Duplicate key \"{key}\": current={_keyNames[key]}, new={name}");
            }
            else
            {
                _keyNames.Add(key, name);
            }
#endif
            return key;
        }
    }
}
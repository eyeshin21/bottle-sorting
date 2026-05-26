#if DEBUG_MODE
using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class DebugManager
    {
        static List<string> _keyBlockers = new();

        public static bool KeyBlocked => _keyBlockers.Count > 0;

        public static void AddKeyBlocker(string name)
        {
            if (_keyBlockers.Contains(name))
            {
               LegacyLog.Warning($"Multiple key blocker \"{name}\"!");
            }
            _keyBlockers.Add(name);
        }

        public static void RemoveKeyBlocker(string name)
        {
            if (!_keyBlockers.Remove(name))
            {
               LegacyLog.Warning($"Can't remove key blocker \"{name}\"!");
            }
        }

        static void OnGUIKeyBlockers()
        {
            int count = _keyBlockers.Count;
            if (count == 0) return;

            GUIHelper.Label("Key Blockers:");
            //TODO: Optimize
            for (int i = 0; i < count; i++)
            {
                GUIHelper.Label(_keyBlockers[i]);
            }
        }
    }
}
#endif
#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class DebugManager
    {
        static Vector2 _scrollPos;
        public static void OnGUIDebug()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            int count = _guiControllers.GetCount();
            for (int i = 0; i < count; i++)
            {
                _guiControllers[i].OnGUI();
            }
            GUILayout.EndScrollView();
        }

        //public static void OnGUI()
        //{
        //    OnGUIKeyBlockers();
        //}
    }
}
#endif
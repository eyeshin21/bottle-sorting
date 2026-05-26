#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class DebugPrefs
    {
        static IntController _debugTab = new("debugTab");
        public static int DebugTab
        {
            get => _debugTab.Value;
            set => _debugTab.Value = value;
        }

        //static BoolController _showSafeArea = new BoolController("showSafeArea");
        //public static bool ShowSafeArea
        //{
        //    get => _showSafeArea.Value;
        //    set => _showSafeArea.Value = value;
        //}
    }
}
#endif
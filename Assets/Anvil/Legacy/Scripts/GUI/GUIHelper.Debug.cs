using UnityEngine;
using System;

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        public static readonly int DebugWidth = 300;
        private static readonly GUILayoutOption TabWidth = GUILayout.MinWidth(150);
        static Rect? _debugRect;

        public static float NotchHeight
        {
            get
            {
#if UNITY_EDITOR
                // Check Simulator
                if (UnityEngine.Device.SystemInfo.deviceType != DeviceType.Desktop)
                {
                    return 110;
                }
                return 0;
#else
                //return Crystal.SafeArea.NotchHeight;
                return 110;
#endif
            }
        }

        public static bool ToggleDebug(bool show, float paddingTop = 0)
        {
            if (_debugRect == null)
            {
                _debugRect = new Rect(0, paddingTop, DebugWidth, 300 + NotchHeight);
            }
            return ToggleDebug(_debugRect.Value, show);
        }

        static float _toggleDebugLastTime = -1;
        public static bool ToggleDebug(Rect rect, bool show)
        {
            if (!show)
            {
                if (GUI.Button(rect, "", GUIStyle.none))
                {
                    if (Helper.CheckDoubleTouch(ref _toggleDebugLastTime))
                    {
                        show = !show;
                    }
                }
            }

            return show;
        }
    }
}
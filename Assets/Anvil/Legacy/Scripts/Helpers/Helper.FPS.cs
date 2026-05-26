#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static float _fpsDeltaTime;

        static GUIStyle _fpsStyle;
        static GUIStyle FpsStyle => _fpsStyle ??= GUIHelper.CreateLabelStyle().SetFontSize(40).SetTextColor(Color.red);

        public static void UpdateFPS()
        {
            _fpsDeltaTime += (Time.unscaledDeltaTime - _fpsDeltaTime) * 0.1f;
        }

        public static void OnGUIFPS()
        {
            GUIHelper.OnGUI(() =>
            {
                bool top = false;
                if (top)
                {
                    GUIHelper.LayoutTop(10, () =>
                    {
                        GUIHelper.LayoutRight(10, () => GUIHelper.Label($"{1 / _fpsDeltaTime:0.0}", FpsStyle));
                    });
                }
                else
                {
                    GUIHelper.LayoutBottom(20, () =>
                    {
                        GUIHelper.LayoutRight(10, () => GUIHelper.Label($"{1 / _fpsDeltaTime:0.0}", FpsStyle));
                    });
                }
            });
        }
    }
}
#endif
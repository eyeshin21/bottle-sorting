using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        public static float GetWidth(float labelWidth, float valueWidth)
        {
            return labelWidth > 0 ? labelWidth + GUISpace + valueWidth : valueWidth;
        }

        public static void SetSceneStyle(params IBaseGUIController[] controllers)
        {
            Assert.IsUnique(controllers);
            controllers.ForEach(controller => controller.SetSceneStyle());
        }

        public static void SetSceneStyle(IEnumerable<IBaseGUIController> controllers)
        {
            foreach (var controller in controllers)
            {
                controller.SetSceneStyle();
            }
        }

        public static void SetMaxLabelWidth(params IBaseGUIController[] controllers)
        {
            Assert.IsUnique(controllers);
            float? maxWidth = null;
            controllers.ForEach(controller =>
            {
                float width = controller.LabelWidth;
                if (maxWidth == null || maxWidth.Value < width)
                {
                    maxWidth = width;
                }
            });
            controllers.ForEach(controller => controller.LabelWidth = maxWidth.Value);
        }

        public static void SetMaxValueWidth(params IBaseGUIController[] controllers)
        {
            Assert.IsUnique(controllers);
            float? maxWidth = null;
            controllers.ForEach(controller =>
            {
                float width = controller.ValueWidth;
                if (maxWidth == null || maxWidth.Value < width)
                {
                    maxWidth = width;
                }
            });
            controllers.ForEach(controller => controller.ValueWidth = maxWidth.Value);
        }

        static GUIContent GUIControllerToggleSeparator = new("|");
        static GUILayoutOption _guiControllerToggleWidth;
        static GUILayoutOption GUIControllerToggleWidth => _guiControllerToggleWidth ??= GUILayout.Width(ToggleWidth + GetLabelWidth(GUIControllerToggleSeparator));

        public static void OnGUI<T>(BaseGUIController<T> controller, ref bool enabled)
        {
            if (enabled)
            {
                GUILayout.BeginHorizontal();
                {
                    enabled = GUILayout.Toggle(enabled, GUIControllerToggleSeparator, GUIControllerToggleWidth);
                    controller.OnGUI();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                var content = controller.LabelContent;
                var style = controller.LabelStyle;
                if (style != null)
                {
                    enabled = GUILayout.Toggle(enabled, content, style);
                }
                else
                {
                    enabled = GUILayout.Toggle(enabled, content);
                }
            }
        }
    }
}
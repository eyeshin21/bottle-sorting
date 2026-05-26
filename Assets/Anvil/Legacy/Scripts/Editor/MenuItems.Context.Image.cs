#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("CONTEXT/Image/Preserve Aspect")]
        static void SetPreserveAspect(MenuCommand menuCommand)
        {
            var image = menuCommand.To<Image>();
            if (!image.preserveAspect)
            {
                EditorHelper.Set(image, "Preserve Aspect", () => image.preserveAspect = true);
            }
        }

        [MenuItem("CONTEXT/Image/Set Raycast Extra")]
        static void SetRaycastExtra(MenuCommand menuCommand)
        {
            GUIHelper.ShowInputInt("Set Raycast Extra", "Extra", 10, "Set", extra =>
            {
                var image = menuCommand.To<Image>();
                float padding = -extra;
                EditorHelper.Set(image, "Set Raycast Extra", () => image.raycastPadding = new Vector4(padding, padding, padding, padding));
            });
        }
    }
}
#endif
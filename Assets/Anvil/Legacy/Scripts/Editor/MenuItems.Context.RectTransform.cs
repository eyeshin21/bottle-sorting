#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
#if DEBUG_MODE
        [MenuItem("CONTEXT/RectTransform/Debug")]
        static void RectTransformDebug(MenuCommand menuCommand)
        {
            var rectTransform = menuCommand.To<RectTransform>();
            rectTransform.CheckAddComponent<DebugRectTransform>();
        }

        [MenuItem("CONTEXT/RectTransform/Debug All")]
        static void RectTransformDebugAll(MenuCommand menuCommand)
        {
            var rectTransform = menuCommand.To<RectTransform>();
            rectTransform.ForEachTransform(transform =>
            {
                transform.CheckAddComponent<DebugRectTransform>();
            });
        }
#endif
    }
}
#endif
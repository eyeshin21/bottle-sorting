#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        const string GameObjectRectTransformPath = "GameObject/Gametamin/RectTransform/";
        const string ContextRectTransformSetPath = "CONTEXT/RectTransform/Set/";

        [MenuItem(GameObjectRectTransformPath + "Set Stretch", false, RectTransformPriority)]
        static void RectTransformSetStretch()
        {
            SetSelected<RectTransform>(rectTransform => rectTransform.SetStretch());
        }
        //[MenuItem(ContextRectTransformSetPath + "Stretch")]
        //static void RectTransformSetStretch(MenuCommand menuCommand)
        //{
        //    menuCommand.ToRectTransform().SetStretch();
        //}

        [MenuItem(GameObjectRectTransformPath + "Set Top", false, RectTransformPriority)]
        static void RectTransformSetTop()
        {
            SetSelected<RectTransform>(rectTransform => rectTransform.SetDefaultName("Top").SetTop());
        }
        [MenuItem(ContextRectTransformSetPath + "Top")]
        static void RectTransformSetTop(MenuCommand menuCommand)
        {
            menuCommand.ToRectTransform().SetTop();
        }

        [MenuItem(GameObjectRectTransformPath + "Set Bottom", false, RectTransformPriority)]
        static void RectTransformSetBottom()
        {
            SetSelected<RectTransform>(rectTransform => rectTransform.SetDefaultName("Bottom").SetBottom());
        }
        [MenuItem(ContextRectTransformSetPath + "Bottom")]
        static void RectTransformSetBottom(MenuCommand menuCommand)
        {
            menuCommand.ToRectTransform().SetBottom();
        }

        [MenuItem(GameObjectRectTransformPath + "Set Play Top-Left", false, RectTransformPriority)]
        static void RectTransformSetPlayTopLeft()
        {
            SetSelected<RectTransform>(rectTransform => rectTransform.SetDefaultName("PlayTopLeft").SetPlayTopLeft());
        }
        [MenuItem(ContextRectTransformSetPath + "Play Top-Left")]
        static void RectTransformSetPlayTopLeft(MenuCommand menuCommand)
        {
            menuCommand.ToRectTransform().SetPlayTopLeft();
        }

        [MenuItem(GameObjectRectTransformPath + "Set Play Bottom-Right", false, RectTransformPriority)]
        static void RectTransformSetPlayBottomRight()
        {
            SetSelected<RectTransform>(rectTransform => rectTransform.SetDefaultName("PlayBottomRight").SetPlayBottomRight());
        }
        [MenuItem(ContextRectTransformSetPath + "Play Bottom-Right")]
        static void RectTransformSetPlayBottomRight(MenuCommand menuCommand)
        {
            menuCommand.ToRectTransform().SetPlayBottomRight();
        }

        [MenuItem("CONTEXT/RectTransform/Set Stretch")]
        static void RectTransformSetStretch(MenuCommand menuCommand)
        {
            menuCommand.ToRectTransform().SetStretch();
        }

        [MenuItem("CONTEXT/RectTransform/Log AABB")]
        static void RectTransformLogAABB(MenuCommand menuCommand)
        {
            menuCommand.ToRectTransform().LogAABB();
        }

        [MenuItem("CONTEXT/RectTransform/Log")]
        static void RectTransformLog(MenuCommand menuCommand)
        {
            var rectTransform = menuCommand.ToRectTransform();
            LegacyLog.Debug($"anchorMin={rectTransform.anchorMin}, anchorMax={rectTransform.anchorMax}, offsetMin={rectTransform.offsetMin}, offsetMax={rectTransform.offsetMax}, anchoredPos={rectTransform.anchoredPosition}, sizeDelta={rectTransform.sizeDelta}, rectSize={rectTransform.rect.size}, pos={rectTransform.position}");
        }
    }
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        const string GameObjectCreatePath = "GameObject/Gametamin/Create/";
        const string GameObjectCreateRectTransformPath = GameObjectCreatePath + "RectTransform - ";

        [MenuItem(GameObjectCreatePath + "Sprite", false, CreatePriority)]
        static void CreateSprite()
        {
            CreateChild<SpriteRenderer>("Sprite");
        }

        [MenuItem(GameObjectCreatePath + "Custom Image", false, CreatePriority + 1)]
        static void CreateCustomImage()
        {
            CreateChild<CustomImage>("Image");
        }

        [MenuItem(GameObjectCreatePath + "SafeArea", false, CreatePriority + 2)]
        static void CreateSafeArea()
        {
            CreateChildRectTransform("SafeArea", rectTransform => rectTransform.SetStretch().AddComponent<SafeArea>());
        }

        #region RectTransform
        [MenuItem(GameObjectCreateRectTransformPath + "Stretch", false, CreatePriority + 3)]
        static void CreateRectTransformStretch()
        {
            CreateChildRectTransform("Stretch", rectTransform => rectTransform.SetStretch());
        }

        [MenuItem(GameObjectCreateRectTransformPath + "Top", false, CreatePriority + 4)]
        static void CreateRectTransformTop()
        {
            CreateChildRectTransform("Top", rectTransform => rectTransform.SetTop());
        }

        [MenuItem(GameObjectCreateRectTransformPath + "Bottom", false, CreatePriority + 5)]
        static void CreateRectTransformBottom()
        {
            CreateChildRectTransform("Bottom", rectTransform => rectTransform.SetBottom());
        }

        [MenuItem(GameObjectCreateRectTransformPath + "Play Top-Left", false, CreatePriority + 6)]
        static void CreateRectTransformPlayTopLeft()
        {
            CreateChildRectTransform("PlayTopLeft", rectTransform => rectTransform.SetPlayTopLeft()/*.SetIcon(LabelIcon.Blue)*/);
        }

        [MenuItem(GameObjectCreateRectTransformPath + "Play Bottom-Right", false, CreatePriority + 7)]
        static void CreateRectTransformPlayBottomRight()
        {
            CreateChildRectTransform("PlayBottomRight", rectTransform => rectTransform.SetPlayBottomRight()/*.SetIcon(LabelIcon.Blue)*/);
        }
        #endregion
    }
}
#endif
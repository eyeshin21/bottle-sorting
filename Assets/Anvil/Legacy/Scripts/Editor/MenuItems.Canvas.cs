#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        const string GameObjectCanvasPath = "GameObject/Gametamin/Canvas/";
        const string ContextCanvasSetPath = "CONTEXT/Canvas/Set/";
        const string ContextCanvasAddPath = "CONTEXT/Canvas/Add/";

        [MenuItem(GameObjectCanvasPath + "Set Screen Space - Camera (1080x1920)", false, CanvasPriority)]
        static void CanvasSetScreenSpaceCamera1080x1920()
        {
            SetSelected<Canvas>(canvas => canvas.SetScreenSpaceCamera(1080, 1920));
        }
        [MenuItem(ContextCanvasSetPath + "Screen Space - Camera (1080x1920)")]
        static void CanvasSetScreenSpaceCamera1080x1920(MenuCommand menuCommand)
        {
            menuCommand.ToCanvas().SetScreenSpaceCamera(1080, 1920);
        }

        [MenuItem(GameObjectCanvasPath + "Set Screen Space - Camera (1920x1080)", false, CanvasPriority)]
        static void CanvasSetScreenSpaceCamera1920x1080()
        {
            SetSelected<Canvas>(canvas => canvas.SetScreenSpaceCamera(1920, 1080));
        }
        [MenuItem(ContextCanvasSetPath + "Screen Space - Camera (1920x1080)")]
        static void CanvasSetScreenSpaceCamera1920x1080(MenuCommand menuCommand)
        {
            menuCommand.ToCanvas().SetScreenSpaceCamera(1920, 1080);
        }

        [MenuItem(GameObjectCanvasPath + "Add Debug", false, CanvasPriority)]
        static void CanvasAddDebug()
        {
            AddComponentToSelectedGameObject<Canvas, DebugCanvas>();
        }
        [MenuItem(ContextCanvasAddPath + "Debug")]
        static void CanvasAddDebug(MenuCommand menuCommand)
        {
            menuCommand.ToCanvas().CheckAddComponent<DebugCanvas>();
        }
    }
}
#endif
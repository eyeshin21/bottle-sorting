using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static void SetScreenSpaceCamera(this Canvas canvas, int screenWidth, int screenHeight)
        {
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = Camera.main;
                canvas.sortingLayerID = SortingLayerIDs.UI;

                var canvasScaler = canvas.GetOrAddComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = new Vector2(screenWidth, screenHeight);

                canvas.CheckAddComponent<GraphicRaycaster>();

                EditorHelper.SetDirty(canvas);
            }
        }
    }
}
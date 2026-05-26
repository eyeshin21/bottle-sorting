using Anvil.Legacy;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static void SetCanvasOverrideSortingOrder(this GameObject gameObject, int sortingOrder, string sortingLayerName = "")
        {
            if (gameObject == null)
            {
                return;
            }

            Canvas canvas = gameObject.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
            }
            gameObject.GetOrAddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = sortingLayerName != "" ? sortingLayerName : SortingLayerName.UI;
            canvas.sortingOrder = sortingOrder;
        }
    }
}

using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static RectTransform CreateRectTransform(string name, Canvas canvas)
        {
            return CreateRectTransform(name, canvas != null ? canvas.transform : null);
        }

        public static RectTransform CreateRectTransform(string name, Transform parent)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            go.layer = LayerMasks.UI;

            var rectTransform = go.AddComponent<RectTransform>();
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.localScale = Vector3.one;

            return rectTransform;
        }
    }
}
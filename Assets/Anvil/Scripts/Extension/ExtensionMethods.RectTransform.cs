#region
using UnityEngine;
#endregion

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            // This returns the world space positions of the corners in the order
            // [0] bottom left,
            // [1] top left
            // [2] top right
            // [3] bottom right
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Vector2 min = corners[0];
            Vector2 max = corners[2];
            Vector2 size = max - min;

            return new Rect(min,size);
        }
    }
}
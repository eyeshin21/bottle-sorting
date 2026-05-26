using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static void SetSize(this Image image, Vector2 size)
        {
            if (image != null)
            {
                var rectTransform = image.transform as RectTransform;
                rectTransform.SetSize(size);
            }
        }

        public static void SetSizeToFitIn(this Image image, Vector2 size)
        {
            SetSizeToFitIn(image, size.x, size.y);
        }

        public static void SetSizeToFitIn(this Image image, float width, float height)
        {
            if (image != null)
            {
                var sprite = image.sprite;
                if (sprite != null)
                {
                    sprite.GetRectSize(out float spriteWidth, out float spriteHeight);
                    var size = Helper.GetFitInSize(spriteWidth, spriteHeight, width, height);
                    SetSize(image, size);
                }
            }
        }
    }
}
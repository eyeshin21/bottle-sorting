using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static Sprite CreateAvatar(Texture2D texture, int spriteWidth, int spriteHeight)
        {
            texture = TextureHelper.RemoveCorners(texture, 20);
            spriteWidth = Mathf.Min(spriteWidth, texture.width);
            spriteHeight = Mathf.Min(spriteHeight, texture.height);
            return Sprite.Create(texture, new Rect(0, 0, spriteWidth, spriteHeight), new Vector2(0.5f, 0.5f), 100);
        }
    }
}
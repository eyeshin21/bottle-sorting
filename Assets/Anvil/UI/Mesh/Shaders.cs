using UnityEngine;

namespace Anvil
{
    public static class Shaders
    {
        static Shader _spriteDefault;
        public static Shader SpriteDefault
        {
            get
            {
                if (_spriteDefault == null)
                {
                    _spriteDefault = Shader.Find("Sprites/Default");
                }
                return _spriteDefault;
            }
        }
    }
}
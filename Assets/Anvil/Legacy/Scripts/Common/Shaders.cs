using UnityEngine;

namespace Anvil.Legacy
{
    public static class Shaders
    {
        static Shader _spriteDefault;
        public static Shader SpriteDefault => _spriteDefault ??= GetShader("Sprites/Default");

        static Shader _spriteGreyscale;
        public static Shader SpriteGreyscale => _spriteGreyscale ??= GetShader("Sprites/Greyscale");

        static Shader _spriteOutline;
        public static Shader SpriteOutline => _spriteOutline ??= GetShader("Sprites/Outline");

        static Shader _svgUnlitVector;
        public static Shader SVGUnlitVector => _svgUnlitVector ??= GetShader("Unlit/Vector");

        static Shader _svgUnlitVectorGradient;
        public static Shader SVGUnlitVectorGradient => _svgUnlitVectorGradient ??= GetShader("Unlit/VectorGradient");

        static Shader GetShader(string name)
        {
            var shader = Shader.Find(name);
#if UNITY_EDITOR || DEBUG_MODE
            if (shader == null)
            {
                LegacyLog.Warning($"{name} shader not found, did you forget to include it in the Project settings/Graphics/Preload Shaders?");
            }
#endif
            return shader;
        }
    }
}
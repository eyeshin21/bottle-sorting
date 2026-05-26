using UnityEngine;

namespace Anvil.Legacy
{
    public static class Materials
    {
        static Material _spriteDefault;
        public static Material SpriteDefault => _spriteDefault ??= new Material(Shaders.SpriteDefault);

        static Material _spriteGreyscale;
        public static Material SpriteGreyscale => _spriteGreyscale ??= CreateMaterial(Shaders.SpriteGreyscale, "Sprites-Greyscale");

        static Material _spriteOutline;
        public static Material SpriteOutline => _spriteOutline ??= CreateMaterial(Shaders.SpriteOutline, "Sprites-Outline");

        static Material _svgUnlitVector;
        public static Material SVGUnlitVector => _svgUnlitVector ??= new Material(Shaders.SVGUnlitVector);

        static Material _svgUnlitVectorGradient;
        public static Material SVGUnlitVectorGradient => _svgUnlitVectorGradient ??= new Material(Shaders.SVGUnlitVectorGradient);

        static Material CreateMaterial(Shader shader, string path)
        {
            if (shader != null)
            {
                return new Material(shader);
            }

            var material = Resources.Load<Material>(path);
#if UNITY_EDITOR || DEBUG_MODE
            if (material == null)
            {
                LegacyLog.Warning($"Can't load material \"{path}\" by Resources!");
            }
#endif
            return material;
        }
    }
}
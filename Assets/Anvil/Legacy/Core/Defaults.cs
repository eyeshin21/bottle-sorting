using UnityEngine;

namespace Anvil
{
    public static partial class Defaults
    {
        public static Vector3 Position = Vector3.zero;
        public static Vector3 Scale = Vector3.one;
        public static Quaternion Rotation = Quaternion.identity;
        public static Color Color = Color.white;
        public static SystemLanguage Language = SystemLanguage.English;

        static Material _material;
        public static Material Material
        {
            get
            {
                if (_material == null)
                {
                    var shaderName = "Sprites/Default";
                    var shader = Shader.Find(shaderName);
                    if (shader != null)
                    {
                        _material = new Material(shader);
                    }
                    else
                    {
                        LegacyLog.Warning($"Can't find shader \"{shaderName}\"!");
                    }
                }
                return _material;
            }
        }

        static object _object;
        public static object Object => _object ??= new();
        static readonly Keyframe[] DefaultKeyframes = { new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1) };

        public static AnimationCurve AnimationCurve => new AnimationCurve(DefaultKeyframes);
        public static AnimationCurve AnimationCurveEaseIn => new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 0, 0, 0.3333333f, 0.3333333f), new Keyframe(1, 1, 2, 2, 0.3333333f, 0.3333333f) });
        public static AnimationCurve AnimationCurveEaseOut => new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 2, 2, 0.3333333f, 0.3333333f), new Keyframe(1, 1, 0, 0, 0.3333333f, 0.3333333f) });

    }
}
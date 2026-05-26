using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static void SetLoop(this ParticleSystem ps, bool loop)
        {
            var main = ps.main;
            main.loop = loop;
        }

        public static void SetStartColor(this ParticleSystem ps, Color color)
        {
            var main = ps.main;
            var startColor = main.startColor;
            startColor.mode = ParticleSystemGradientMode.Color;
            startColor.color = color;
            main.startColor = startColor;
        }

        public static void SetStartColor(this ParticleSystem ps, Gradient gradient)
        {
            var main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(gradient);
        }

        public static void SetStartColorGradient(this ParticleSystem ps, Color minColor, Color maxColor)
        {
            var main = ps.main;
            var gradient = GetGradient(minColor, maxColor);
            main.startColor = new ParticleSystem.MinMaxGradient(gradient);
        }

        static Gradient _gradient;
        static GradientColorKey[] _gradientColorKeys;
        static GradientAlphaKey[] _gradientAlphaKeys;
        static Gradient GetGradient(Color minColor, Color maxColor)
        {
            if (_gradient == null)
            {
                _gradient = new Gradient();
                _gradientColorKeys = new GradientColorKey[2]
                {
                    new GradientColorKey(Color.white, 0),
                    new GradientColorKey(Color.white, 1)
                };
                _gradientAlphaKeys = new GradientAlphaKey[2]
                {
                    new GradientAlphaKey(1, 0),
                    new GradientAlphaKey(1, 1)
                };
                //_gradient.SetKeys(_gradientColorKeys, _gradientAlphaKeys);
            }

            _gradientColorKeys[0].color = minColor;
            _gradientColorKeys[1].color = maxColor;
            _gradient.SetKeys(_gradientColorKeys, _gradientAlphaKeys);

            return _gradient;
        }

        public static void Replay(this ParticleSystem ps, bool withChildren)
        {
            ps.Simulate(0, withChildren, false);
            ps.time = 0;
            ps.Play(withChildren);
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class ParticleSystemAlphaController : IAlphaController
    {
        ParticleSystem _particleSystem;

        void Construct(ParticleSystem particleSystem)
        {
            _particleSystem = particleSystem;
        }

        public GameObject GameObject => _particleSystem?.gameObject;

        public float Alpha
        {
            get => _particleSystem.main.startColor.color.a;
            set
            {
                var main = _particleSystem.main;
                var startColor = main.startColor;
                var mode = startColor.mode;
                if (mode == ParticleSystemGradientMode.Color)
                {
                    var color = startColor.color;
                    color.a = value;
                    startColor.color = color;
                }
                else if (mode == ParticleSystemGradientMode.RandomColor)
                {
                    var gradient = startColor.gradient;
                    var colorKeys = gradient.colorKeys;
                    for (int i = 0; i < colorKeys.Length; i++)
                    {
                        var color = colorKeys[i].color;
                        color.a = value;
                        colorKeys[i].color = color;
                    }
                    gradient.colorKeys = colorKeys;
                    startColor.gradient = gradient;
                }
                else
                {
                    Assert.Todo(mode);
                }
                main.startColor = startColor;
            }
        }

        public void ReturnToPool()
        {
            _particleSystem = null;
            Pool.Return(this);
        }

        static Pool<ParticleSystemAlphaController> _pool;
        static Pool<ParticleSystemAlphaController> Pool => _pool ??= new();

        public static ParticleSystemAlphaController Create(ParticleSystem particleSystem)
        {
            var controller = Pool.Get();
            controller.Construct(particleSystem);
            return controller;
        }
    }
}
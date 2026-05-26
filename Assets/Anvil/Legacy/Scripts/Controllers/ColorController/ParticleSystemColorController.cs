using UnityEngine;

namespace Anvil.Legacy
{
    public class ParticleSystemColorController : IColorController
    {
        ParticleSystem _particleSystem;

        void Construct(ParticleSystem particleSystem)
        {
            _particleSystem = particleSystem;
        }

        public GameObject GameObject => _particleSystem?.gameObject;

        public Color Color
        {
            get => _particleSystem.main.startColor.color;
            set
            {
                var main = _particleSystem.main;
                var startColor = main.startColor;
                var mode = startColor.mode;
                if (mode == ParticleSystemGradientMode.Color)
                {
                    startColor.color = value;
                }
                else if (mode == ParticleSystemGradientMode.RandomColor)
                {
                    var gradient = startColor.gradient;
                    var colorKeys = gradient.colorKeys;
                    for (int i = 0; i < colorKeys.Length; i++)
                    {
                        colorKeys[i].color = value;
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

        static Pool<ParticleSystemColorController> _pool;
        static Pool<ParticleSystemColorController> Pool => _pool ??= new();

        public static ParticleSystemColorController Create(ParticleSystem particleSystem)
        {
            var controller = Pool.Get();
            controller.Construct(particleSystem);
            return controller;
        }
    }
}
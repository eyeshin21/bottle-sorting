using UnityEngine;

namespace Anvil.Legacy
{
    public class RendererColorController : IColorController
    {
        Renderer _renderer;

        void Construct(Renderer renderer)
        {
            _renderer = renderer;
        }

        public GameObject GameObject => _renderer?.gameObject;

        public Color Color
        {
            get => _renderer.GetColor();
            set => _renderer.SetColor(value);
        }

        public void ReturnToPool()
        {
            _renderer = null;
            Pool.Return(this);
        }

        static Pool<RendererColorController> _pool;
        static Pool<RendererColorController> Pool => _pool ??= new();

        public static RendererColorController Create(Renderer renderer)
        {
            var controller = Pool.Get();
            controller.Construct(renderer);
            return controller;
        }
    }
}
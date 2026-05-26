using UnityEngine;

namespace Anvil.Legacy
{
    public class RendererAlphaController : IAlphaController
    {
        Renderer _renderer;

        void Construct(Renderer renderer)
        {
            _renderer = renderer;
        }

        public GameObject GameObject => _renderer?.gameObject;

        public float Alpha
        {
            get => _renderer.GetAlpha();
            set => _renderer.SetAlpha(value);
        }

        public void ReturnToPool()
        {
            _renderer = null;
            Pool.Return(this);
        }

        static Pool<RendererAlphaController> _pool;
        static Pool<RendererAlphaController> Pool => _pool ??= new();

        public static RendererAlphaController Create(Renderer renderer)
        {
            var controller = Pool.Get();
            controller.Construct(renderer);
            return controller;
        }
    }
}
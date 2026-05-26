using UnityEngine;

namespace Anvil.Legacy
{
    public class SpriteRendererAlphaController : IAlphaController
    {
        SpriteRenderer _spriteRenderer;

        void Construct(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }

        public GameObject GameObject => _spriteRenderer?.gameObject;

        public float Alpha
        {
            get => _spriteRenderer.color.a;
            set
            {
                var color = _spriteRenderer.color;
                color.a = value;
                _spriteRenderer.color = color;
            }
        }

        public void ReturnToPool()
        {
            _spriteRenderer = null;
            Pool.Return(this);
        }

        static Pool<SpriteRendererAlphaController> _pool;
        static Pool<SpriteRendererAlphaController> Pool => _pool ??= new();

        public static SpriteRendererAlphaController Create(SpriteRenderer spriteRenderer)
        {
            var controller = Pool.Get();
            controller.Construct(spriteRenderer);
            return controller;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class SpriteRendererColorController : IColorController
    {
        SpriteRenderer _spriteRenderer;

        void Construct(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }

        public GameObject GameObject => _spriteRenderer?.gameObject;

        public Color Color
        {
            get => _spriteRenderer.color;
            set => _spriteRenderer.color = value;
        }

        public void ReturnToPool()
        {
            _spriteRenderer = null;
            Pool.Return(this);
        }

        static Pool<SpriteRendererColorController> _pool;
        static Pool<SpriteRendererColorController> Pool => _pool ??= new();

        public static SpriteRendererColorController Create(SpriteRenderer spriteRenderer)
        {
            var controller = Pool.Get();
            controller.Construct(spriteRenderer);
            return controller;
        }
    }
}
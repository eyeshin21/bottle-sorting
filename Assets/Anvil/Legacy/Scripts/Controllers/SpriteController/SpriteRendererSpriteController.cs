using UnityEngine;

namespace Anvil.Legacy
{
    public class SpriteRendererSpriteController : ISpriteController, IColorController, IMaterialController
    {
        SpriteRenderer _spriteRenderer;
        Material _defaultMaterial;

        void Construct(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }

        public GameObject GameObject => _spriteRenderer?.gameObject;

        public Sprite Sprite
        {
            get => _spriteRenderer.sprite;
            set => _spriteRenderer.sprite = value;
        }

        public Color Color
        {
            get => _spriteRenderer.color;
            set => _spriteRenderer.color = value;
        }

        public Material Material
        {
            get => _spriteRenderer.material;
            set
            {
                if (value != null)
                {
                    if (_defaultMaterial == null)
                    {
                        _defaultMaterial = _spriteRenderer.material;
                    }
                    _spriteRenderer.material = value;
                }
                else
                {
                    if (_defaultMaterial != null)
                    {
                        _spriteRenderer.material = _defaultMaterial;
                    }
                }
            }
        }

        public bool FlipX
        {
            get => _spriteRenderer.flipX;
            set => _spriteRenderer.flipX = value;
        }

        public void ReturnToPool()
        {
            _spriteRenderer = null;
            _defaultMaterial = null;
            Pool.Return(this);
        }

        static Pool<SpriteRendererSpriteController> _pool;
        static Pool<SpriteRendererSpriteController> Pool => _pool ??= new();

        public static SpriteRendererSpriteController Create(SpriteRenderer spriteRenderer)
        {
            var controller = Pool.Get();
            controller.Construct(spriteRenderer);
            return controller;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class DefaultSpriteController : ISpriteController
    {
        Sprite _sprite;
        bool _isFlipX;

        public GameObject GameObject => default;

        public Sprite Sprite
        {
            get => _sprite;
            set => _sprite = value;
        }

        public bool FlipX
        {
            get => _isFlipX;
            set => _isFlipX = value;
        }

        public void ReturnToPool()
        {
            _sprite = null;
            Pool.Return(this);
        }

        static Pool<DefaultSpriteController> _pool;
        static Pool<DefaultSpriteController> Pool => _pool ??= new();

        public static DefaultSpriteController Create()
        {
            return Pool.Get();
        }
    }
}
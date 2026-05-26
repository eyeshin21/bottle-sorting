using UnityEngine;

namespace Anvil.Legacy
{
    public class DefaultAlphaController : IAlphaController
    {
        float _a = 1;

        public GameObject GameObject => default;

        public float Alpha
        {
            get => _a;
            set => _a = value;
        }

        public void ReturnToPool()
        {
            Pool.Return(this);
        }

        static Pool<DefaultAlphaController> _pool;
        static Pool<DefaultAlphaController> Pool => _pool ??= new();

        public static DefaultAlphaController Create()
        {
            return Pool.Get();
        }
    }
}
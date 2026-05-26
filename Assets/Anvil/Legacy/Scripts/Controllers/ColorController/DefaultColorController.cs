using UnityEngine;

namespace Anvil.Legacy
{
    public class DefaultColorController : IColorController
    {
        Color _color = Defaults.Color;

        public GameObject GameObject => default;

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public void ReturnToPool()
        {
            Pool.Return(this);
        }

        static Pool<DefaultColorController> _pool;
        static Pool<DefaultColorController> Pool => _pool ??= new();

        public static DefaultColorController Create()
        {
            return Pool.Get();
        }
    }
}
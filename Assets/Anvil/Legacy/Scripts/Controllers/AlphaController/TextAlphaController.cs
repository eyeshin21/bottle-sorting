using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class TextAlphaController : IAlphaController
    {
        Text _text;

        void Construct(Text text)
        {
            _text = text;
        }

        public GameObject GameObject => _text?.gameObject;

        public float Alpha
        {
            get => _text.color.a;
            set
            {
                var color = _text.color;
                color.a = value;
                _text.color = color;
            }
        }

        public void ReturnToPool()
        {
            _text = null;
            Pool.Return(this);
        }

        static Pool<TextAlphaController> _pool;
        static Pool<TextAlphaController> Pool => _pool ??= new();

        public static TextAlphaController Create(Text text)
        {
            var controller = Pool.Get();
            controller.Construct(text);
            return controller;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class TextColorController : IColorController
    {
        Text _text;

        void Construct(Text text)
        {
            _text = text;
        }

        public GameObject GameObject => _text?.gameObject;

        public Color Color
        {
            get => _text.color;
            set => _text.color = value;
        }

        public void ReturnToPool()
        {
            _text = null;
            Pool.Return(this);
        }

        static Pool<TextColorController> _pool;
        static Pool<TextColorController> Pool => _pool ??= new();

        public static TextColorController Create(Text text)
        {
            var controller = Pool.Get();
            controller.Construct(text);
            return controller;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class ImageAlphaController : IAlphaController
    {
        Image _image;

        void Construct(Image image)
        {
            _image = image;
        }

        public GameObject GameObject => _image?.gameObject;

        public float Alpha
        {
            get => _image.color.a;
            set
            {
                var color = _image.color;
                color.a = value;
                _image.color = color;
            }
        }

        public void ReturnToPool()
        {
            _image = null;
            Pool.Return(this);
        }

        static Pool<ImageAlphaController> _pool;
        static Pool<ImageAlphaController> Pool => _pool ??= new();

        public static ImageAlphaController Create(Image image)
        {
            var controller = Pool.Get();
            controller.Construct(image);
            return controller;
        }
    }
}
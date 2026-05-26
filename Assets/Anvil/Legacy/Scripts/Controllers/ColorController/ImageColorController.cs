using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class ImageColorController : IColorController
    {
        Image _image;

        void Construct(Image image)
        {
            _image = image;
        }

        public GameObject GameObject => _image?.gameObject;

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        public void ReturnToPool()
        {
            _image = null;
            Pool.Return(this);
        }

        static Pool<ImageColorController> _pool;
        static Pool<ImageColorController> Pool => _pool ??= new();

        public static ImageColorController Create(Image image)
        {
            var controller = Pool.Get();
            controller.Construct(image);
            return controller;
        }
    }
}
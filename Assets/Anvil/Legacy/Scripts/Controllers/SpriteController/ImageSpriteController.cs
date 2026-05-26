using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class ImageSpriteController : ISpriteController, IColorController, IMaterialController
    {
        Image _image;
        Material _defaultMaterial;

        void Construct(Image image)
        {
            _image = image;
        }

        public GameObject GameObject => _image?.gameObject;

        public Sprite Sprite
        {
            get => _image.sprite;
            set => _image.sprite = value;
        }

        public Color Color
        {
            get => _image.color;
            set => _image.color = value;
        }

        public Material Material
        {
            get => _image.material;
            set
            {
                if (value != null)
                {
                    if (_defaultMaterial == null)
                    {
                        _defaultMaterial = _image.material;
                    }
                    _image.material = value;
                }
                else
                {
                    if (_defaultMaterial != null)
                    {
                        _image.material = _defaultMaterial;
                    }
                }
            }
        }

        public bool FlipX
        {
            get => _image.transform.localScale.x < 0;
            set
            {
                var transform = _image.transform;
                var scale = transform.localScale;
                scale.x = value ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }

        public void ReturnToPool()
        {
            _image = null;
            _defaultMaterial = null;
            Pool.Return(this);
        }

        static Pool<ImageSpriteController> _pool;
        static Pool<ImageSpriteController> Pool => _pool ??= new();

        public static ImageSpriteController Create(Image image)
        {
            var controller = Pool.Get();
            controller.Construct(image);
            return controller;
        }
    }
}
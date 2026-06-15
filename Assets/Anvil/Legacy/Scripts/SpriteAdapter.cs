using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public abstract class SpriteAdapter
    {
        public abstract GameObject GameObject { get; }
        public abstract Sprite Sprite { get; set; }
        public abstract Material Material { get; set; }
        public abstract Color Color { get; set; }
        public static void SetSprite(Transform transform, Sprite sprite)
        {
            if (transform != null)
            {
                SetSprite(transform.gameObject, sprite);
            }
            else
            {
               Debug.LogWarning($"SetSprite({sprite}): Transform is null!");
            }
        }

        public static void SetSprite(GameObject go, Sprite sprite)
        {
            if (go == null)
            {
               Debug.LogWarning($"SetSprite({sprite}): GameObject is null!");
                return;
            }

            var spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;
                return;
            }

            var image = go.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.sprite = sprite;
                return;
            }
            
            CustomImage_v1 customImage = go.GetComponentInChildren<CustomImage_v1>();
            if (customImage != null)
            {
                customImage.sprite = sprite;
                return;
            }

           Debug.LogWarning($"Can't set sprite \"{sprite}\" for {go.name}!");
        }

        public static SpriteAdapter Create(SpriteRenderer spriteRenderer)
        {
            return new SpriteAdapterSpriteRenderer(spriteRenderer);
        }

        public static SpriteAdapter Create(Image image)
        {
            return new SpriteAdapterImage(image);
        }
        
        public static SpriteAdapter Create(CustomImage_v1 image)
        {
            return new SpriteAdapterCustomImage(image);
        }

        public static SpriteAdapter Create(GameObject go)
        {
            if (go == null)
            {
               Debug.LogWarning($"GameObject is null");
                return new SpriteAdapterDefault(go);
            }

            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                return new SpriteAdapterSpriteRenderer(spriteRenderer);
            }

            var image = go.GetComponent<Image>();
            if (image != null)
            {
                return new SpriteAdapterImage(image);
            }
            var customImage = go.GetComponent<CustomImage_v1>();
            if (customImage != null)
            {
                return new SpriteAdapterCustomImage(customImage);
            }

           Debug.LogWarning($"Can't create sprite adapter for {go.name}");
            return new SpriteAdapterDefault(go);
        }

        public class SpriteAdapterSpriteRenderer : SpriteAdapter
        {
            private SpriteRenderer _spriteRenderer;

            public SpriteAdapterSpriteRenderer(SpriteRenderer spriteRenderer)
            {
                _spriteRenderer = spriteRenderer;
            }

            public override GameObject GameObject => _spriteRenderer.gameObject;

            public override Sprite Sprite
            {
                get => _spriteRenderer.sprite;
                set => _spriteRenderer.sprite = value;
            }

            public override Material Material 
            {
                get => _spriteRenderer.material;
                set => _spriteRenderer.material = value;
            }
            public override Color Color
            {
                get => _spriteRenderer.color;
                set => _spriteRenderer.color = value;
            }
        }

        public class SpriteAdapterImage : SpriteAdapter
        {
            private Image _image;

            public SpriteAdapterImage(Image image)
            {
                _image = image;
            }

            public override GameObject GameObject => _image.gameObject;

            public override Sprite Sprite
            {
                get => _image.sprite;
                set => _image.sprite = value;
            }

            public override Material Material 
            {
                get => _image.material;
                set => _image.material = value;
            }
            public override Color Color
            {
                get => _image.color;
                set
                {
                    _image.color = value;
                }
            }
        }
        public class SpriteAdapterCustomImage : SpriteAdapter
        {
            private CustomImage_v1 _image;

            public SpriteAdapterCustomImage(CustomImage_v1 image)
            {
                _image = image;
            }

            public override GameObject GameObject => _image.gameObject;

            public override Sprite Sprite
            {
                get => _image.sprite;
                set => _image.sprite = value;
            }

            public override Material Material 
            {
                get => _image.material;
                set => _image.material = value;
            }
            public override Color Color
            {
                get => _image.color;
                set => _image.color = value;
            }
        }

        public class SpriteAdapterDefault : SpriteAdapter
        {
            private GameObject _gameObject;
            private Sprite _sprite;

            public SpriteAdapterDefault(GameObject gameObject)
            {
                _gameObject = gameObject;
            }

            public override GameObject GameObject => _gameObject;

            public override Sprite Sprite
            {
                get => _sprite;
                set => _sprite = value;
            }

            public override Color Color { get; set; }
            public override Material Material { get; set; }
        }
    }
}
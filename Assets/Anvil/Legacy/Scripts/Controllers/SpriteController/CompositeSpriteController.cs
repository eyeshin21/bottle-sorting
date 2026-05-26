using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class CompositeSpriteController : CompositeController<ISpriteController>, ISpriteController, IColorController, IMaterialController
    {
        Sprite _sprite;
        bool _isFlipX;
        Color _color = Defaults.Color;
        Material _material;

        public Sprite Sprite
        {
            get => _count > 0 ? _controllers[0].Sprite : _sprite;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].Sprite = value;
                }
                _sprite = value;
            }
        }

        public bool FlipX
        {
            get => _count > 0 ? _controllers[0].FlipX : _isFlipX;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].FlipX = value;
                }
                _isFlipX = value;
            }
        }

        public Color Color
        {
            get => _count > 0 ? _controllers[0].GetColor() : _color;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].SetColor(value);
                }
                _color = value;
            }
        }

        public Material Material
        {
            get => _count > 0 ? _controllers[0].GetMaterial() : _material;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].SetMaterial(value);
                }
                _material = value;
            }
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            _sprite = null;
            _color = Defaults.Color;
            _material = null;
            Pool.Return(this);
        }

        static Pool<CompositeSpriteController> _pool;
        static Pool<CompositeSpriteController> Pool => _pool ??= new();

        public static CompositeSpriteController Create(List<ISpriteController> controllers)
        {
            var controller = Pool.Get();
            controller.Construct(controllers);
            return controller;
        }
    }
}
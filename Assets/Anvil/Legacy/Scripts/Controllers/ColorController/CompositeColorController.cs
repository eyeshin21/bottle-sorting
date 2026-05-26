using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class CompositeColorController : CompositeController<IColorController>, IColorController
    {
        Color _color = Defaults.Color;

        public Color Color
        {
            get => _count > 0 ? _controllers[0].Color : _color;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].Color = value;
                }
                _color = value;
            }
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            _color = Defaults.Color;
            Pool.Return(this);
        }

        static Pool<CompositeColorController> _pool;
        static Pool<CompositeColorController> Pool => _pool ??= new();

        public static CompositeColorController Create(List<IColorController> controllers)
        {
            var controller = Pool.Get();
            controller.Construct(controllers);
            return controller;
        }
    }
}
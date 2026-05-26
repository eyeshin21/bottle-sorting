using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class CompositeAlphaController : CompositeController<IAlphaController>, IAlphaController
    {
        float _a = 1;

        public float Alpha
        {
            get => _count > 0 ? _controllers[0].Alpha : _a;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].Alpha = value;
                }
                _a = value;
            }
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            _a = 1;
            Pool.Return(this);
        }

        static Pool<CompositeAlphaController> _pool;
        static Pool<CompositeAlphaController> Pool => _pool ??= new();

        public static CompositeAlphaController Create(List<IAlphaController> controllers)
        {
            var controller = Pool.Get();
            controller.Construct(controllers);
            return controller;
        }
    }
}
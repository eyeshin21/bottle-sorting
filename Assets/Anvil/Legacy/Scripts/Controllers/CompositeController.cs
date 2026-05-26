using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class CompositeController<T> where T : IController
    {
        protected List<T> _controllers = new List<T>();
        protected int _count;

        protected virtual void Construct(List<T> controllers)
        {
            Assert.IsEmpty(_controllers);
            _controllers.AddRange(controllers);
            _count = _controllers.Count;
        }

        public virtual GameObject GameObject => _count > 0 ? _controllers[0].GameObject : default;

        public virtual void ReturnToPool()
        {
            if (_count > 0)
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].ReturnToPool();
                }
                _controllers.Clear();
                _count = 0;
            }
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    /// <summary>
    /// Stack of objects with func to create new one.
    /// </summary>
    public class PoolFunc<T> : BasePool<T>
    {
        Func<T> _newFunc;

        public PoolFunc(Func<T> newFunc)
        {
            _newFunc = newFunc;
        }

        public override T Get()
        {
            return _pool.Count > 0 ? _pool.Pop() : _newFunc();
        }
    }
}
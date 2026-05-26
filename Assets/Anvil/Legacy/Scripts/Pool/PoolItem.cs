using UnityEngine;

namespace Anvil.Legacy
{
    /// <summary>
    /// Stack of pool items (IPoolItem).
    /// </summary>
    public class PoolItem<T> : BasePool<T> where T : IPoolItem, new()
    {
        public override T Get()
        {
            return _pool.Count > 0 ? _pool.Pop() : new();
        }

        /// <summary>
        /// Calls OnReturnPool() and return item to pool.
        /// </summary>
        public override void Return(T item)
        {
            item?.OnReturnPool();
            base.Return(item);
        }
    }
}
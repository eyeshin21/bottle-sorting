using UnityEngine;

namespace Anvil.Legacy
{
    /// <summary>
    /// Stack of objects.
    /// </summary>
    public class Pool<T> : BasePool<T> where T : new()
    {
        public override T Get()
        {
            return _pool.Count > 0 ? _pool.Pop() : new();
        }
    }
}
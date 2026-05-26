#region
using UnityEngine;
#endregion

namespace Anvil.Legacy
{
    public interface IPoolData
    {
        public string PoolId { get; }
        public bool IsPoolIgnore { get; }
        public GameObject gameObject { get; }
        public void OnReturnPool();
    }
}
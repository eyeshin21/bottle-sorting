#region
using UnityEngine;
#endregion

namespace Anvil
{
    public interface IPoolData
    {
        public string PoolId { get; }
        public bool IsPoolIgnore { get; }
        public GameObject gameObject { get; }
        public void OnReturnPool();
    }

    public interface IPoolableBehaviour
    {
        void OnPoolRemove();
        void OnPoolCreate();
    }
}
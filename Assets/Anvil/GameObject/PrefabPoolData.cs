#region
using UnityEngine;
#endregion

namespace Anvil
{
    public class PrefabPoolData : MonoBehaviour,IPoolData
    {
        public GameObject prefab;
        public bool _isPoolIgnore = false;

        public string PoolId
        {
            get { return prefab.GetInstanceID().ToString(); }
        }

        public bool IsPoolIgnore=>_isPoolIgnore;

        public void OnReturnPool()
        {
        }
    }
}
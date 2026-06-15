#region
using UnityEngine;
#endregion

namespace Anvil
{
    public class StringPoolData : MonoBehaviour,IPoolData
    {
        public string poolID;
        public bool _isPoolIgnore = false;

        public string PoolId
        {
            get { return poolID; }
        }

        public bool IsPoolIgnore=>_isPoolIgnore;

        public void OnReturnPool()
        {
        }
    }
}
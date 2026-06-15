#region
using UnityEngine;
#endregion

namespace Anvil
{
    /// <summary>
    /// Storing prefab reference. similar to gameobjectReference's pool data
    /// </summary>
    public interface IPoolTable
    {
        public bool GetPrefab(string poolAddress,out GameObject prefab);
        public GameObject CreateObject(string poolAddress,Transform parent = null,bool copyParentPos = true,bool worldPositionStay = false);
    }
}
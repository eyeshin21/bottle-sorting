using System.Collections.Generic;
using UnityEngine;

namespace Anvil.Legacy
{
    [System.Serializable]
    public struct GameObjectReferenceData
    {
        public string Id;
        public GameObject Target;
    }
    public class ReferenceIDPoolTable : MonoBehaviour, IPoolTable
    {
        [SerializeField] private List<GameObjectReferenceData> _poolData = new List<GameObjectReferenceData>();
        public bool GetPrefab(string poolAddress,out GameObject prefab)
        {
            prefab = null;
            foreach (var item in _poolData)
            {
                if (item.Id == poolAddress)
                {
                    prefab = item.Target;
                    return true;
                }
            }
            return false;
        }

        public GameObject CreateObject(string poolAddress,Transform parent = null,bool copyParentPos = true,bool worldPositionStay = false)
        {
            if (!GetPrefab(poolAddress,out var prefab)) return null;
            var ret = GameObjectPool.CreateObject(parent,prefab,copyParentPos,worldPositionStay);
            return ret;
        }
    }
}

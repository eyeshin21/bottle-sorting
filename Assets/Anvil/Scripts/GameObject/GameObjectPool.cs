using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
#pragma warning disable 0162
namespace Anvil.Legacy
{
    public static class GameObjectPool
    {
        private static readonly Dictionary<string, Stack<GameObject>> _pools = new();

        // private static readonly Dictionary<GameObject, List<GameObject>> _activeDict = new();
        private static Transform _poolParent;
        //private static bool _inited = false;

        public static void Init(Transform poolParent)
        {
            Clear();
            
            _poolParent = poolParent;
            
            Cleanup();
            //_inited = true;
        }

        public static void Preload(GameObject prefab, int i)
        {
            if (_poolParent == null || prefab == null)
            {
                Debug.LogWarning("no pool parent or preloading prefab is null");
                return;
            }

            for (int j = 0; j < i; j++)
            {
                GameObject obj = Object.Instantiate(prefab, _poolParent, true);
                PrefabPoolData poolData = obj.GetOrAddComponent<PrefabPoolData>();
                poolData.prefab = prefab;


                if (!_pools.TryGetValue(poolData.PoolId, out var pool))
                {
                    pool = new Stack<GameObject>();
                    _pools.Add(poolData.PoolId, pool);
                }
                pool.Push(obj);
            }
        }

        private static void Cleanup()
        {
            // foreach (KeyValuePair<string,Stack<GameObject>> keyValuePair in _pools)
            // {
            //     Stack<GameObject> stack = keyValuePair.Value;
            // }
        }

        public static void Clear()
        {
            _pools.Clear();
            _poolParent.DestroyChildren();
        }

        public static bool TryGetObject(string poolId, out GameObject gameObject)
        {
            gameObject = null;

            if (_pools.TryGetValue(poolId, out var pool))
            {
                if (pool.Count > 0)
                {
                    gameObject = pool.Pop();
                    // if (poolId=="numberedBlock")
                    // {
                    //     Debug.Log($"[Create] object in pool: {pool.Count}");
                    // }
                    return true;
                }
            }

            return false;
        }

        public static GameObject CreateObject(GameObject prefab)
        {
            return CreateObject(null, prefab);
        }
        public static T CreateObject<T>(Transform parent, GameObject prefab, bool copyParentPos = true, bool worldPositionStays = false, bool resetScale = true, Action<GameObject> onInstantiate = null) where T : MonoBehaviour
        {
            GameObject gameObject = CreateObject(parent, prefab, copyParentPos, worldPositionStays, resetScale, onInstantiate);
            if (gameObject == null)
                return null;
            var ret = gameObject.GetComponent<T>();
            if (ret != null)
            {
                return ret;
            }
            Debug.LogError($"[GameObjectPool] Created object does not have component of type {typeof(T).Name}");
            RemoveObject(gameObject);
            return null;
        }

        public static GameObject CreateObject(Transform parent, GameObject prefab, bool copyParentPos = true, bool worldPositionStays = false, bool resetScale = true, Action<GameObject> onInstantiate = null)
        {
            if (prefab == null)
                return null;

            GameObject gameObject = null;
            var poolData = prefab.GetComponent<IPoolData>();
            string poolID = poolData != null ? poolData.PoolId : prefab.GetInstanceID().ToString(); 
            if (!TryGetObject(poolID, out gameObject))
            {
                //item = Config.Instance.CreateItem(itemType, itemParent);
                gameObject = Object.Instantiate(prefab);
                onInstantiate?.Invoke(gameObject);
            }

            poolData = gameObject.GetComponent<IPoolData>();
            if (poolData == null)
            {
                PrefabPoolData prefabData = gameObject.GetOrAddComponent<PrefabPoolData>();
                prefabData.prefab = prefab;
                poolData = prefabData;
            }

            if (parent == null)
            {
                gameObject.transform.SetParent(null, worldPositionStays);
                gameObject.transform.position = Vector3.zero;
            }
            else
            {
                gameObject.transform.SetParent(parent, worldPositionStays);
                if (copyParentPos)
                {
                    gameObject.transform.position = parent.position;
                }
            }

            if (resetScale)
            {
                gameObject.transform.localScale = Vector3.one;
            }


            return gameObject;
        }

        public static void RemoveObject(GameObject gameObject, bool removeChildren = false, bool worldPositionStay = true)
        {
            if (gameObject == null)
            {
                return;
            }
            if (_poolParent == null)
            {
#if UNITY_EDITOR
                
                if (gameObject.GetComponent<PoolIgnoreTag>() != null)
                {
                    return;
                }
                if (!Application.isPlaying)
                {
                    Object.DestroyImmediate(gameObject);
                    return;
                }
#endif
                Object.Destroy(gameObject);
                return;
            }

            if (removeChildren)
            {
                var poolableChilds = gameObject.GetComponentsInChildren<IPoolData>();
            }

            if (gameObject.GetComponent<PoolIgnoreTag>() != null)
            {
                return;
            }

            IPoolData poolData = gameObject.GetComponent<IPoolData>();
            if (poolData != null && poolData.IsPoolIgnore)
            {
                return;
            }

            if (poolData == null)
            {
                Object.Destroy(gameObject);
                return;
            }

            if (!_pools.TryGetValue(poolData.PoolId, out var pool))
            {
                pool = new Stack<GameObject>();
                _pools.Add(poolData.PoolId, pool);
            }

            poolData.OnReturnPool();
            pool.Push(gameObject);
            // if (poolData.PoolId == "numberedBlock")
            // {
            //     Debug.Log($"[remove] object in pool: {pool.Count}");
            // }
            gameObject.transform.SetParent(_poolParent, worldPositionStay);

            //TODO: check for parent disabling/enabling
        }
        public static void RemoveObjects<T>(List<T> objects) where T : MonoBehaviour
        {
            if (objects == null)
            {
                return;
            }

            foreach (var t in objects)
            {
                RemoveObject(t.gameObject);
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="parentObj"></param>
        /// <param name="originPrefab">Can be the origin prefab of this object or any object spawned from the origin prefab</param>
        public static void ClearChild(GameObject parentObj, GameObject originPrefab)
        {
            if (parentObj == null || originPrefab == null)
            {
                return;
            }

            IPoolData poolData = originPrefab.GetComponent<IPoolData>();
            string address = poolData != null ? poolData.PoolId : originPrefab.GetInstanceID().ToString();

            ClearChild(parentObj, address);
        }

        public static void ClearChild(GameObject parentObj, string address)
        {
            if (parentObj == null || string.IsNullOrEmpty(address))
            {
                return;
            }

            Transform transform = parentObj.transform;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;
                IPoolData poolData = child.GetComponent<IPoolData>();
                if (poolData != null && poolData.PoolId == address && !poolData.IsPoolIgnore)
                {
                    RemoveObject(child);
                }
            }
        }

        public static void ClearChild(GameObject parentObj)
        {
            if (parentObj == null)
            {
                return;
            }

            Transform transform = parentObj.transform;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                RemoveObject(transform.GetChild(i).gameObject);
            }
        }

        public static void ClearManagedChild(GameObject parentObj)
        {
            if (parentObj == null)
            {
                return;
            }

            Transform transform = parentObj.transform;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.GetComponent<IPoolData>() == null)
                {
                    continue;
                }
                RemoveObject(child);
            }
        }
        
        public static void ClearPool(GameObject prefab)
        {
            if (prefab == null)
            {
                return;
            }

            IPoolData poolData = prefab.GetComponent<IPoolData>();
            string address = poolData != null ? poolData.PoolId : prefab.GetInstanceID().ToString();
            if (_pools.ContainsKey(address))
            {
                Stack<GameObject> objs = _pools[address];
                for (int i = 0; i < objs.Count; i++)
                {
#if UNITY_EDITOR
                    GameObject obj = objs.Pop();
                    if (Application.IsPlaying(obj))
                    {
                        Object.Destroy(obj);
                        continue;
                    }

                    Debug.Log("destroy immediate");
                    Object.DestroyImmediate(objs.Pop());
                    return;
#endif
                    Object.Destroy(objs.Pop());
                }

                _pools.Remove(address);
            }
        }


    }
}
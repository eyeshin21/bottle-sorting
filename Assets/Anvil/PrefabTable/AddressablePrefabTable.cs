#if Addressables_ENABLED

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

    public class AddressablePrefabTable<T> : PrefabTable<T> where T : Enum
    {
        [Serializable]
        protected struct PrefabID<U> where U : Enum
        {
            public U ID;
            public AssetReference PrefabRef;
        }
        
        [SerializeField] List<PrefabID<T>> _labelMap = new List<PrefabID<T>>();
        // public void LoadPrefab(string id, Action<GameObject> resultCallback)
        // {
        //     // string label = _labelMap.TryGet(id);
        //     
        // }
        public override void LoadPrefab(T id, Action<GameObject> resultCallback)
        {
            var assetRef = _labelMap.Find(x => x.ID.Equals(id)).PrefabRef;
            if (assetRef == null)
            {
                Debug.LogError($"[Addressable PrefabTable] asset not found for id: {id}");
                return;
            }
            var handler = Addressables.LoadAssetAsync<GameObject>(assetRef);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle<GameObject> result)
            {
                if (result.Status == AsyncOperationStatus.Succeeded)
                {
                    resultCallback?.Invoke(result.Result);
                }
                else
                {
                    resultCallback?.Invoke(null);
                }
            }
            
        }

        public override void LoadPrefab(string id,Action<GameObject> resultCallback)
        {
            var assetRef = _labelMap.Find(x => x.ID.ToString().Equals(id)).PrefabRef;
            if (assetRef == null)
            {
                Debug.LogError($"[Addressable PrefabTable] asset not found for id: {id}");
                return;
            }
            var handler = Addressables.LoadAssetAsync<GameObject>(assetRef);
            handler.Completed += OnCompleted;
            void OnCompleted(AsyncOperationHandle<GameObject> result)
            {
                if (result.Status == AsyncOperationStatus.Succeeded)
                {
                    resultCallback?.Invoke(result.Result);
                }
                else
                {
                    resultCallback?.Invoke(null);
                }
            }
        }
    }
#endif

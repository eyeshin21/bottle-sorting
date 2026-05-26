using System;
using System.Collections.Generic;
using UnityEngine;
    public class PrefabTable<T> : ScriptableObject where T : Enum
    {
        public virtual void LoadPrefab(T id, Action<GameObject> resultCallback)
        {
            resultCallback?.Invoke(null);
        }
        public virtual void LoadPrefab(string id, Action<GameObject> resultCallback)
        {
            resultCallback?.Invoke(null);
        }
    }
    public class LocalPrefabTable<T> : PrefabTable<T> where T: Enum
    {
        [Serializable]
        protected struct PrefabID<U> where U : Enum
        {
            public U ID;
            public GameObject Prefab;
        }
        
        [SerializeField]List<PrefabID<T>> _sequentialPrefabs = new List<PrefabID<T>>();
        public override void LoadPrefab(T id, Action<GameObject> resultCallback)
        {
            var prefab = _sequentialPrefabs.Find(x => x.ID.Equals(id)).Prefab;
            if (prefab == null)
            {
                Debug.LogError($"[Local PrefabTable] asset not found for id: {id}");
                return;
            }
            resultCallback?.Invoke(prefab);            
        }
        
        public GameObject LoadPrefabImidiate(T id)
        {
            var prefab = _sequentialPrefabs.Find(x => x.ID.Equals(id)).Prefab;
            if (prefab == null)
            {
                Debug.LogError($"[Local PrefabTable] asset not found for id: {id}");
                return null;
            }

            return prefab;
        }
    }

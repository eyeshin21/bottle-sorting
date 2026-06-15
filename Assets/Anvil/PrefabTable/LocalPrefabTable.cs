using System;
using System.Collections.Generic;
using Anvil;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class AssetTable : ScriptableObject
{
    public virtual void LoadAsset(string id,Action<Object> resultCallback)
    {
        resultCallback?.Invoke(null);
    }
    public virtual void LoadGameObject(string id,Action<GameObject> resultCallback)
    {
        resultCallback?.Invoke(null);
    }

    public virtual void Create(string id,Transform parent,Action<GameObject> callback)
    {
        LoadGameObject(id,prefab=>
        {
            GameObject gameObject = GameObjectPool.CreateObject(parent,prefab);
            callback?.Invoke(gameObject);
        });
    }

    public virtual void Create<ComponentType>(string id,Transform parent,Action<ComponentType> callback) where ComponentType : Component
    {
        Create(id,parent,gameObject=>
        {
            if (gameObject == null)
            {
                callback?.Invoke(null);
                return;
            }

            var component = gameObject.GetComponent<ComponentType>();
            callback?.Invoke(component);
        });
    }
    public virtual void Preload()
    {
        
    }
}

public class LocalAssetTable<IDType> : AssetTable
{
    [Serializable]
    public class AssetDefinition
    {
        public IDType ID;
        public Object asset;
        public bool preload;

        public virtual Object LoadAsset()
        {
            return asset;
        } 
    }
    [FormerlySerializedAs("_sequentialPrefabs")] [SerializeField] protected List<AssetDefinition> assetDefinitions = new List<AssetDefinition>();
    //todo: Cache loaded asset in runtime
    
    public List<AssetDefinition> AssetDefinitions => assetDefinitions;
    [Button]
    public void FillWithEnum()
    {
        if (!typeof(IDType).IsEnum)
        {
            Debug.Log($"{typeof(IDType).ToString()} is not enum type");
            return;
        }
        var enumValues = Enum.GetValues(typeof(IDType));
        foreach (IDType value in enumValues)
        {
            AssetDefinition assetDefinition = new AssetDefinition
            {
                ID = value,
                asset = null
            };
            assetDefinitions.Add(assetDefinition);
        }
        
    }

    public override void Preload()
    {
        foreach (var asset in assetDefinitions)
        {
            if (!asset.preload)
            {
                continue;
            }
            if (asset.asset is GameObject go)
            {
                GameObjectPool.Preload(go, 1);
            }
        }
    }
    public override void LoadAsset(string id,Action<Object> resultCallback)
    {
        var assetDef = assetDefinitions.Find(x=>x.ID.ToString() == id);
        if (assetDef == null)
        {
            resultCallback?.Invoke(null);
            return;
        }
        resultCallback?.Invoke(assetDef.asset);
    }

    public virtual void LoadAssetID(IDType id,Action<Object> resultCallback)
    {
        var asset = assetDefinitions.Find(x=>x.ID.Equals(id));
        if (asset == null)
        {
            return;
        }

        resultCallback?.Invoke(asset.asset);
    }
    public virtual void LoadGameObjectID(IDType id,Action<GameObject> resultCallback)
    {
        LoadAssetID(id, asset =>
        {
            if (asset is GameObject prefab)
            {
                resultCallback?.Invoke(prefab);
            }
            else
            {
                Debug.LogError($"[Local PrefabTable] asset is not a GameObject for id: {id}");
                resultCallback?.Invoke(null);
            }
        });
    }
    public override void LoadGameObject(string id, Action<GameObject> resultCallback)
    {
        LoadAsset(id, asset =>
        {
            if (asset is GameObject prefab)
            {
                resultCallback?.Invoke(prefab);
            }
            else
            {
                Debug.LogError($"[Local PrefabTable] asset is not a GameObject for id: {id}");
                resultCallback?.Invoke(null);
            }
        });
    }

    public GameObject LoadPrefabImidiate(IDType id)
    {
        return LoadAssetImidiate<GameObject>(id);
    }
    public T LoadAssetImidiate<T>(IDType id) where T : Object
    {
        var asset = assetDefinitions.Find(x => x.ID.Equals(id)).asset as T;
        return asset;
    }
}
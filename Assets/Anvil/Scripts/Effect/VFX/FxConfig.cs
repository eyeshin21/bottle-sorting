#region
using System;
using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion

namespace Anvil
{
    [Serializable]
    public class FxParams
    {
        public float duration;
        public float delayBetwenSpawn;
        public float delayBetwenMove;

        // public float
        [Range(0,1)] public float delayMinDecrement;

        public FxParams()
        {
        }

        public FxParams(float duration2,float delayBetwenSpawn2,float delayBetwenMove2,float delayMinDecrement2)
        {
            duration = duration2;
            delayBetwenSpawn = delayBetwenSpawn2;
            delayBetwenMove = delayBetwenMove2;
            delayMinDecrement = delayMinDecrement2;
        }

        public FxParams GetCopy()
        {
            FxParams ret = new FxParams();
            ret.SetParam(duration,delayBetwenSpawn,delayBetwenMove,delayMinDecrement);
            return ret;
        }

        public void SetParam(float duration2,float delayBetwenSpawn2,float delayBetwenMove2,float delayMinDecrement2)
        {
            duration = duration2;
            delayBetwenSpawn = delayBetwenSpawn2;
            delayBetwenMove = delayBetwenMove2;
            delayMinDecrement = delayMinDecrement2;
        }

        public float EvaluateDelaySpawn(int index,int total)
        {
            //Linear interpolation
            float ret = delayBetwenSpawn + (delayBetwenSpawn * (delayMinDecrement - 1) * index / total);
            // Debug.Log($"eval {index}/{total}: {ret}");
            return ret;
        }
    }

    [Serializable]
    public class EffectData
    {
        public EffectType effectType;
        public GameObject prefab;
    }

    public enum CollectFxType
    {
        Default,
        DefaultUISequence,
    }

    public class FxConfig : SingletonScriptableObject<FxConfig>
    {
        [SerializeField] private LocalPrefabTable<EffectType> _fxTable;
        // [SerializeField] private List<EffectData> _effectDatas = new List<EffectData>();

        [ElementName(typeof(CollectFxType))] [SerializeField]
        private List<CollectFxConfig> _collectFxConfig = new List<CollectFxConfig>();

        [SerializeField] private GameObject _combinationFxPrefab;

        public static GameObject CombinationFxPrefab=>Instance._combinationFxPrefab;

        public static GameObject GetEffectPrefab(EffectType type)
        {
            return Instance._fxTable.LoadPrefabImidiate(type);
        }

        public static FxParams GetFxParams(CollectFxType type)
        {
            CollectFxConfig config = Instance._collectFxConfig.TryGet((int)type);
            if (config != null)
            {
                return config.Param;
            }

            return null;
        }

        [SerializeField] GameObject _defaultObjectCarrier;
       
        public static GameObject DefaultObjCarrier=>Instance._defaultObjectCarrier;

        public static AnimatedMovableObjectCarrier CreateObjectCarrier(GameObject gameObject,Vector3 targetPos,FxParams fxParams = null,MoveConfig moveConfig = null)
        {
            // if (fxParams == null)
            // {
            //     fxParams = GetFxParams(CollectFxType.Default);
            // }

            GameObject carrier = GameObjectPool.CreateObject(null,DefaultObjCarrier);
            AnimatedMovableObjectCarrier objCarrier = carrier.GetComponent<AnimatedMovableObjectCarrier>();
            objCarrier.Init(gameObject,targetPos,fxParams,moveConfig);
            return objCarrier;
        }

#if UNITY_EDITOR

        static void CheckResetPosition(GameObject prefab)
        {
            if (prefab != null)
            {
                var pos = prefab.transform.position;
                if (pos.x != 0 || pos.y != 0 || pos.z != 0)
                {
                    prefab.transform.position = Vector3.zero;
                    EditorHelper.SetDirty(prefab);
                }
            }
            else
            {
                LegacyLog.Warning("Prefab is null!");
            }
        }

        static void CheckAddPSFinishHandler(GameObject prefab,bool withChildren = true,bool finishOnDisabled = true)
        {
            if (prefab != null)
            {
                var handler = prefab.GetComponent<ParticleSystemFinishHandler>();
                if (handler == null)
                {
                    handler = prefab.AddComponent<ParticleSystemFinishHandler>();
                    handler.SetWithChildren(withChildren).SetFinishOnDisabled(finishOnDisabled);
                    EditorHelper.SetDirty(prefab);
                }
            }
            else
            {
                LegacyLog.Warning("Prefab is null!");
            }
        }

        static void CheckAddPSFinishHandler(GameObject[] prefabs,bool withChildren = true,bool finishOnDisabled = true)
        {
            int count = prefabs.GetLength();
            for (int i = 0; i < count; i++)
            {
                CheckAddPSFinishHandler(prefabs[i],withChildren,finishOnDisabled);
            }
        }

        static void CheckSetLayerID(GameObject prefab,string path,int layerID)
        {
            if (prefab != null)
            {
                var child = prefab.transform.GetChildByPath(path);
                if (child != null)
                {
                    var renderer = child.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if (renderer.sortingLayerID != layerID)
                        {
                            renderer.sortingLayerID = layerID;
                            EditorHelper.SetDirty(prefab);
                        }
                    }
                }
                else
                {
                    LegacyLog.Warning($"Can't found child \"{path}\"!");
                }
            }
            else
            {
                LegacyLog.Warning("Prefab is null!");
            }
        }

        static void CheckSetLayerEffect(GameObject prefab,string childName)
        {
            CheckSetLayerID(prefab,childName,SortingLayerIDs.Effect);
        }

        static void CheckSetLayerUI(GameObject prefab,string childName)
        {
            CheckSetLayerID(prefab,childName,SortingLayerIDs.UI);
        }

#endif
    }
}
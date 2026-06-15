using System;
using System.Collections.Generic;
using UnityEngine;
using Anvil;
using Random = UnityEngine.Random;

namespace Anvil.PrebuiltEffects
{
    public enum RewardFxType
    {
        Generic,
        Coin,
        CoinImpact,
        CoinTrail,
    }

    public class RewardFx : LocalAssetTable<RewardFxType>
    {
#region Instance

        protected static RewardFx _instance;

        public static RewardFx Instance
        {
            get
            {
                if (_instance == null)
                {
                    RewardFx instance = Resources.Load<RewardFx>($"{nameof(RewardFx)}");
                    _instance = instance;
                    if (_instance == null)
                    {
                        Debug.LogError($"Canot find instance of {nameof(RewardFx)}");
                    }
                }

                return _instance;
            }
        }

        [ElementName(typeof(RewardFxType))] [SerializeField]
        private List<TrajectoryParameterPreset> _fxTrajectoryPresets;

        [SerializeField] private int _maxParticle = 20;
        [SerializeField] private FxParams _defaultDelaySpawn;
        [SerializeField] private FxParams _defaultDelayMove;
        [SerializeField] private Rect _defaultSpawnBox;

#endregion

#region PreMadeFx

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fxType"></param>
        /// <param name="startPos"></param>
        /// <param name="target"></param>
        /// <param name="count"></param>
        /// <param name="parent"></param>
        /// <param name="onElementCreate">action(Element game object, index, total index)</param>
        /// <param name="onCollected"></param>
        /// <param name="onCompleted">action(Index, Collect value)</param>
        public static void StartRewardFx(RewardFxType fxType, Vector3 startPos, ITargetDesignator target, int count,
            Transform parent = null,
            Action<GameObject, int, int> onElementCreate = null,
            Action<int, int> onCollected = null,
            Action onCompleted = null)
        {
            RewardFxSequence(fxType, startPos, target, count
                , sequence => { sequence.ExecuteSafe(); }
                , parent, onElementCreate, onCollected, onCompleted);
        }

        public static void RewardFxSequence(RewardFxType fxType, Vector3 startPos, ITargetDesignator target, int count,
            Action<ScriptedEvent> result,
            Transform parent = null,
            Action<GameObject, int, int> onElementCreate = null,
            Action<int, int> onCollected = null,
            Action onCompleted = null)
        {
            var trajectoryPreset = Instance._fxTrajectoryPresets.ForceGet((int)fxType);
            Instance.LoadGameObjectID(fxType, prefab =>
            {
                int fxCount = Mathf.Min(count, Instance._maxParticle);
                int coinPerFx = Mathf.CeilToInt(count / (float)fxCount);
                var sequence = CollectRewardExContinuous(startPos, target, count,
                    trajectoryPreset, Instance._defaultDelaySpawn, Instance._defaultDelayMove,
                    parent, prefab, onElementCreate,
                    onCollected: (index) => { onCollected?.Invoke(index, coinPerFx); },
                    onCompleted);
                result?.Invoke(sequence);
            });
        }

#endregion

        public static ScriptedEvent CreateCollectRewardSingle(Vector3 startPos, ITargetDesignator target
            , ITrajectoryParameter<ITrajectoryCalculator> trajectoryParameter = null
            , Transform parent = null, GameObject prefab = null,
            Action<GameObject> onElementCreate = null, Action onCompleted = null)
        {
            if (prefab == null)
            {
                return new ScriptedEvent(callback =>
                {
                    onCompleted?.Invoke();
                    callback?.Invoke();
                });
            }

            ScriptedEvent ret = new ScriptedEvent();
            CreateSingleFx(parent, trajectoryParameter, startPos, prefab, onElementCreate: (fxElement, fxController) =>
            {
                // fxElement.SetImageReference(icon);
                onElementCreate?.Invoke(fxElement);
                ret.SetPrimaryAction((callback) =>
                {
                    fxController.MoveTo(target, () =>
                    {
                        onCompleted?.Invoke();
                        callback?.Invoke();
                    });
                });
            });
            // spawnFx(i);
            return ret;
        }

        /// <summary>
        /// Sequentially spawn a number of prefab object and move them to the target position
        /// </summary>
        /// <returns>Return a ScriptedEvent: wraper class for Action{Aciton} </returns>
        public static ScriptedEvent CollectRewardExSpawn(Vector3 startPos, ITargetDesignator target, int count,
            ITrajectoryParameter<ITrajectoryCalculator> moveConfig = null,
            FxParams delaySpawn = null, FxParams delayMove = null,
            Transform parent = null, GameObject prefab = null,
            Action<GameObject, int, int> onElementCreate = null,
            Action<int> onCollected = null,
            Action onCompleted = null)
        {
            // Debug.Log($"starting reward spawn fx: {collectData.total}");
            ScriptedEvent ret = new ScriptedEvent();

            var itemCount = Mathf.Min(Instance._maxParticle, count);

            Rect randomBox = Instance._defaultSpawnBox;
            if (randomBox.width == 0 && randomBox.height == 0)
            {
                if (count > 1)
                {
                    randomBox = new Rect(0, 0, 2, 2);
                }
            }

            float halfW = randomBox.width / 2;
            float halfH = randomBox.height / 2;

            List<EffectDynamic> fxControllers = new List<EffectDynamic>();
            float currentSpawnDelay = 0;
            ret.SetPrimaryAction((callback) =>
            {
                for (int i = 0; i < itemCount; i++)
                {
                    int index = i;
                    currentSpawnDelay += delaySpawn.EvaluateDelaySpawn(index, itemCount);
                    float delay = currentSpawnDelay;
                    Manager.DelayCall(currentSpawnDelay, () =>
                    {
                        CreateSingleFx(parent, moveConfig, startPos, prefab,
                            onElementCreate: (fxElement, fxController) =>
                            {
                                // VibrationType.Minimal.Vibrate();

                                fxControllers.Add(fxController);
                                Vector3 pos = startPos;
                                pos.x += Random.Range(-halfW, halfW);
                                pos.y += Random.Range(-halfH, halfH);
                                pos.z = 0;
                                fxElement.transform.position = pos;
                                onElementCreate?.Invoke(fxElement, index, itemCount - 1);
                                // fxController.PlayAnimation(AnimationNames.Start, () =>
                                // {
                                if (index == itemCount - 1)
                                {
                                    StartMoveEvent(() =>
                                    {
                                        callback?.Invoke();
                                        onCompleted?.Invoke();
                                    });
                                }
                                // });
                            });
                    });
                    // spawnFx(i);
                }
            });

            void StartMoveEvent(Action callback)
            {
                float currentMoveDelay = 0;

                for (int i = itemCount - 1; i >= 0; i--)
                {
                    int inverseIndex = itemCount - 1 - i;
                    var fxController = fxControllers[i];
                    currentMoveDelay += delayMove.EvaluateDelaySpawn(inverseIndex, itemCount);
                    Manager.DelayCall(currentMoveDelay, () =>
                    {
                        // fxController.PlayAnimation(AnimationNames.Move);
                        fxController.MoveTo(target, () =>
                        {
                            // VibrationType.Light.Vibrate();

                            onCollected?.Invoke(inverseIndex);
                            if (inverseIndex == itemCount - 1)
                            {
                                callback?.Invoke();
                            }

                            GameObjectPool.Remove(fxController, true);
                        });
                    });
                }
            }

            return ret;
        }

        /// <summary>
        /// Sequentially spawn a number of prefab object and move them to the target position
        /// </summary>
        /// <returns>Return a ScriptedEvent: wraper class for Action{Aciton} </returns>
        public static ScriptedEvent CollectRewardExContinuous(Vector3 startPos, ITargetDesignator target, int count,
            ITrajectoryParameter<ITrajectoryCalculator> moveConfig = null,
            FxParams delaySpawn = null, FxParams delayMove = null,
            Transform parent = null, GameObject prefab = null,
            Action<GameObject, int, int> onElementCreate = null,
            Action<int> onCollected = null,
            Action onCompleted = null)
        {
            // Debug.Log($"starting reward spawn fx: {collectData.total}");
            ScriptedEvent ret = new ScriptedEvent();

            var itemCount = Mathf.Min(Instance._maxParticle, count);

            Rect randomBox = Instance._defaultSpawnBox;
            if (randomBox.width == 0 && randomBox.height == 0)
            {
                if (count > 1)
                {
                    randomBox = new Rect(0, 0, 2, 2);
                }
            }

            float halfW = randomBox.width / 2;
            float halfH = randomBox.height / 2;

            List<EffectDynamic> fxControllers = new List<EffectDynamic>();
            float currentSpawnDelay = 0;
            ret.SetPrimaryAction((callback) =>
            {
                for (int i = 0; i < itemCount; i++)
                {
                    int index = i;
                    currentSpawnDelay += delaySpawn.EvaluateDelaySpawn(index, itemCount);
                    float delay = currentSpawnDelay;
                    Manager.DelayCall(currentSpawnDelay, () =>
                    {
                        CreateSingleFx(parent, moveConfig, startPos, prefab,
                            onElementCreate: (fxElement, fxController) =>
                            {
                                // VibrationType.Minimal.Vibrate();

                                fxControllers.Add(fxController);
                                Vector3 pos = startPos;
                                pos.x += Random.Range(-halfW, halfW);
                                pos.y += Random.Range(-halfH, halfH);
                                pos.z = 0;
                                fxElement.transform.position = pos;
                                onElementCreate?.Invoke(fxElement, index, itemCount - 1);
                                fxController.MoveTo(target, () =>
                                {
                                    onCollected?.Invoke(index);
                                    if (index == itemCount - 1)
                                    {
                                        callback?.Invoke();
                                        onCompleted?.Invoke();
                                    }

                                    GameObjectPool.Remove(fxController, true);
                                });

                            });
                    });
                    // spawnFx(i);
                }
            });

            return ret;
        }

        public static EffectDynamic CreateSingleFx(Transform parent,
            ITrajectoryParameter<ITrajectoryCalculator> trajectoryConfig, Vector3 startPos, GameObject prefab,
            Action<GameObject, EffectDynamic> onElementCreate = null)
        {
            // if (parent == null)
            // {
            //     parent = PoolHelper.ParentPopup.parent;
            // }

            GameObject fxElement = null;
            if (prefab == null)
            {
                return null;
            }

            fxElement = GameObjectPool.CreateObject(parent, prefab, resetScale: false, worldPositionStays: true);
            fxElement.transform.position = startPos;
            // fxElement.transform.localScale = Vector3.one;

            EffectDynamic fxDynamic = fxElement.GetComponent<EffectDynamic>();

            onElementCreate?.Invoke(fxElement, fxDynamic);
            return fxDynamic;
            // });
        }
    }
}
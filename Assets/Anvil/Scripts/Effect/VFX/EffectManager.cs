#region
using System.Collections.Generic;
using Anvil.Legacy;
using UnityEngine;
#endregion

namespace Anvil
{
    public static class EffectManager
    {
        private static readonly Dictionary<GameObject,Stack<EffectController_v0>> _particlePools = new();
        private static Transform _poolParent;

        public static void Init()
        {
            _poolParent = Manager.PoolTransform;
        }

        //public static void Init(Transform poolParent)
        //{
        //    _poolParent = poolParent;
        //}
        public static void Clear()
        {
            _particlePools.Clear();
            _poolParent.DestroyChildren();
        }

        // Pre init a minimum set ammount of object in pool before game start
        public static void PreLoadPool()
        {
        }

        public static void CreateEffectAt(Vector3 position,EffectType type)
        {
            EffectController_v0 fxControllerV0 = CreateEffect(null,type);
            if (fxControllerV0 == null)
            {
                return;
            }

            fxControllerV0.transform.position = position;
        }

        public static EffectController_v0 CreateEffect(Transform effectParent,EffectType type)
        {
            var prefab = FxConfig.GetEffectPrefab(type);
            return CreateEffect(effectParent,prefab);
            //return null;
        }

        public static void RemoveEffect(EffectController_v0 particleControllerV0)
        {
            if (particleControllerV0 == null)
            {
                return;
            }

            GameObjectPool.RemoveObject(particleControllerV0.gameObject);
        }

        public static EffectController_v0 CreateEffect(Transform effectParent,GameObject fxPrefab)
        {
            if (fxPrefab == null)
            {
                Debug.LogWarning("the effect you are trying to play is null");
                return null;
            }

            GameObject effectObj = GameObjectPool.CreateObject(effectParent,fxPrefab,false);
            EffectController_v0 particleControllerV0 = effectObj.GetComponent<EffectController_v0>();
            if (particleControllerV0 == null)
            {
                Debug.LogWarning($"Assigned Effect prefab ({fxPrefab.name}) doesnt have an effect controller. defaulting to particle system");
                particleControllerV0 = effectObj.GetOrAddComponent<EffectController_v0>();
                particleControllerV0.controllType = EffectControllType.Particle;
                particleControllerV0.Init();
            }

            if (particleControllerV0.AutoScale)
            {
                particleControllerV0.transform.localScale = particleControllerV0.DefaultScale;
            }

            return particleControllerV0;
        }

        static EffectController_v0 PlayEffect(EffectType type,Vector3 pos)
        {
            return PlayEffect(type, null, pos);
        }

        static EffectController_v0 PlayEffect(EffectType type,Transform parent,Vector3 pos)
        {
            var prefab = FxConfig.GetEffectPrefab(type);
            if (prefab != null)
            {
                var controller = CreateEffect(parent,prefab);
                if (controller != null)
                {
                    controller.transform.position = pos;
                }

                return controller;
            }

            return null;
        }

        //public static void PlayEffectRemoveItem(Item item)
        //{
        //    PlayEffect(EffectType.Break, item.CellPosition);
        //}
    }
}
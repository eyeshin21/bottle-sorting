using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class MarioWonderCoinFxParamPreset : ScriptableObject, IScriptedFxParam
    {
        private Sprite _sprite;
        Vector3 _start;
        Transform _end;
        [SerializeField]GameObject fxPrefab;
        [SerializeField]GameObject _auxilaryFxPrefab;
        [SerializeField] private MoveConfig _moveConfig;
        [SerializeField] private List<MoveConfig> _moveConfigs;
        [SerializeField] private float _delayedEnd = 0;
        public void SetParam(Vector3 start, Transform end, Sprite sprite)
        {
            _start = start;
            _end = end;
            _sprite = sprite;
        }

        private MoveConfig RandomMoveConfig()
        {
            if (_moveConfigs.IsNullOrEmpty())
            {
                return _moveConfig;
            }
            return _moveConfigs.GetRandom();
        }
        public IScriptedFX CreateFx()
        {
            if (fxPrefab == null)
            {
                return null;
            }

            if (fxPrefab == null)
            {
                return null;
            }

            GameObject rewardFxObj = GameObjectPool.CreateObject(null, fxPrefab);

            MarioWonderCoinFx rewardFx = rewardFxObj.GetComponent<MarioWonderCoinFx>();
            if (rewardFx == null)
            {
                return null;
            }
            if (_auxilaryFxPrefab != null)
            {
                GameObject auxilaryFxObj = GameObjectPool.CreateObject(rewardFxObj.transform, _auxilaryFxPrefab);
            }
            // rewardFx.Init(_clotheId, _start, _end, ClothingRewardConfig.Instance.GetSprite(_clotheId));
            rewardFx.Init(_start, new StaticTargetReference(_end.gameObject), _sprite, RandomMoveConfig(), _delayedEnd);
            return rewardFx;
        }
    }
}

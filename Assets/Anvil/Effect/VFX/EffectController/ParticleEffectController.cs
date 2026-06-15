using System;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public class ParticleEffectController : EffectController
    {
        [SerializeField] GameObject _secondaryEffect;

        [SerializeField] private Transform _container;
        private GameObject _effectInstance;
        private Transform Container
        {
            get
            {
                if (_container == null)
                {
                    _container = transform;
                }
                return _container;
            }
        }
        private void OnEnable()
        {
            _effectInstance = GameObjectPool.CreateObject(Container, _secondaryEffect);
        }

        private void OnParticleSystemStopped()
        {
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (_effectInstance  != null)
            {
                _effectInstance.SetParent(transform.parent);
            }
            if (_parentGameObject != null)
            {
                GameObjectPool.RemoveObject(_parentGameObject);
            }else GameObjectPool.RemoveObject(gameObject);
        }
    }
}
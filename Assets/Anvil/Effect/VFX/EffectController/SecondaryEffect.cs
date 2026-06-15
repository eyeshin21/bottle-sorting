using System;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public class SecondaryEffect : MonoBehaviour, IPoolableBehaviour
    {
        [SerializeField] private GameObject _fxPrefab;
        
        private GameObject _fxInstance;

        private void OnEnable()
        {
            if (_fxPrefab != null)
            {
                _fxInstance = Instantiate(_fxPrefab, transform);
            }
        }

        public void OnPoolRemove()
        {
        }

        public void OnPoolCreate()
        {
        }
    }
}
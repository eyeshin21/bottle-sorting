using System;
using UnityEngine;

namespace Anvil
{
    public class DebugDespawner : MonoBehaviour
    {
        [Header("Destroy if")] [SerializeField]
        private bool _isDebug = false;

        private void Awake()
        {
#if DEBUG_MODE
            if (_isDebug)
            {
                Destroy(gameObject);
            }
#else
            if    (!_isDebug)        
            {
                Destroy(gameObject);
            }
#endif
        }
    }
}
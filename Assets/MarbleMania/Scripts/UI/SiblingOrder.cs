using System;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public class SiblingOrder : MonoBehaviour
    {
        [SerializeField] private bool _isFirst = false;
        
        [SerializeField] private bool _isLast = false;
        private void OnEnable()
        {
            if (_isFirst)
            {
                transform.SetAsFirstSibling();
            }
            else if (_isLast)
            {
                transform.SetAsLastSibling();
            }
        }

        private void OnTransformParentChanged()
        {
            OnEnable();
        }
    }
}
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace MarbleMania.Scripts
{
    public class BufferZone : MonoBehaviour
    {
        [SerializeField] private float _pushForce;
        
        [SerializeField] private HashSet<Bottle> _content = new  HashSet<Bottle>();
        [SerializeField, ReadOnly]private Vector3 _pushDirection =  Vector3.forward;
        [SerializeField, ReadOnly]private Vector3 _pushVector;
        
        private void Awake()
        {
            CalculatePushForce();
        }

        [Button]
        private Vector3 CalculatePushForce()
        {
            return _pushVector  = _pushDirection.normalized * _pushForce;
        }

        private void Update()
        {
            foreach (Bottle bottle in _content)
            {
                if (bottle == null) continue;
                bottle.Rigidbody.AddForce(_pushVector, ForceMode.Force);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Bottle>(out Bottle bottle))
            {
                _content.Add(bottle);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Bottle>(out Bottle bottle))
                _content.Remove(bottle);
        }
        
    }
}
using System;
using UnityEngine;

namespace Anvil
{
    public class StaticTargetDesignator : MonoBehaviour , ITargetDesignator
    {
        private GameObject _targetObj;
        private Vector3 _initialPos;

        private bool _isActive = true;
        public bool IsActive 
        { 
            get => _isActive;
            set => _isActive = value;
        }

        private void OnDestroy()
        {
            _isActive = false;
        }

        public bool Validate()
        {
            return _targetObj != null;
        }

        public void SetTarget(GameObject targetObj)
        {
            if (targetObj == null)
            {
                return;
            }
            _targetObj = targetObj;
        }

        public void SetTarget(Vector3 position)
        {
            _initialPos = position;
        }

        public GameObject GetTargetObject()
        {
            return _targetObj;
        }

        public Vector3 CalculateTargetPosition()
        {
            if (_targetObj == null)
            {
                return _initialPos;
            }
            return _targetObj.transform.position;
        }

        public Vector3 GetTargetPosition()
        {
            return CalculateTargetPosition();
        }
    }
    public class StaticTargetReference : ITargetDesignator
    {
        private GameObject _targetObj;
        private Vector3 _initialPos;
        private Vector3 _offset;
        public bool IsActive
        {
            get => _targetObj != null;
            set { }
        }
        public StaticTargetReference()
        {

        }

        public StaticTargetReference(GameObject targetObj)
        {
            _targetObj = targetObj;
            _initialPos = targetObj.transform.position;
        }
        public StaticTargetReference(GameObject targetObj, Vector3 offset)
        {
            _targetObj = targetObj;
            _initialPos = targetObj.transform.position;
            _offset = offset;
        }

        public bool Validate()
        {
            return _targetObj != null && _targetObj.activeInHierarchy;
        }

        public void SetTarget(GameObject targetObj)
        {
            if (targetObj == null)
            {
                return;
            }
            _targetObj = targetObj;
        }

        public void SetTarget(Vector3 position)
        {
            _initialPos = position;
        }

        public GameObject GetTargetObject()
        {
            return _targetObj;
        }

        public Vector3 CalculateTargetPosition()
        {
            if (_targetObj == null)
            {
                return _initialPos;
            }
            return _targetObj.transform.position + _offset;
        }

        public Vector3 GetTargetPosition()
        {
            return CalculateTargetPosition();
        }
    }
}

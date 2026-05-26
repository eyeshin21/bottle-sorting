using System;
using UnityEngine;

namespace Anvil.Legacy
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
}

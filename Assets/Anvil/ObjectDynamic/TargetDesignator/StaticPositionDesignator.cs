using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public class StaticPositionReference : ITargetDesignator
    {
        private Vector3 _targetPos;

        public StaticPositionReference()
        {

        }

        public StaticPositionReference(Vector3 targetPos)
        {
            SetTarget(targetPos);
        }

        public bool IsActive
        {
            get => true;
            set{}
        }

        public void SetTarget(GameObject targetObj)
        {
            if (targetObj == null)
            {
                return;
            }
            _targetPos = targetObj.transform.position;
        }

        public void SetTarget(Vector3 position)
        {
            _targetPos = position;
        }

        public GameObject GetTargetObject()
        {
            return null;
        }

        public Vector3 CalculateTargetPosition()
        {
            return _targetPos;
        }

        public Vector3 GetTargetPosition()
        {
            return _targetPos;
        }
    }
    public class StaticPositionDesignator : MonoBehaviour , ITargetDesignator
    {
        private bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = value; }
        
        StaticPositionReference _positionReference;

        public StaticPositionDesignator()
        {
            _positionReference = new StaticPositionReference();
        }

        public StaticPositionDesignator(Vector3 targetPos)
        {
            _positionReference = new StaticPositionReference(targetPos);
        }


        public void SetTarget(GameObject targetObj)
        {
            _positionReference.SetTarget(targetObj);
        }

        public void SetTarget(Vector3 position)
        {
            _positionReference.SetTarget(position);
        }

        public GameObject GetTargetObject()
        {
            return _positionReference.GetTargetObject();
        }

        public Vector3 CalculateTargetPosition()
        {
            return _positionReference.CalculateTargetPosition();
        }

        public Vector3 GetTargetPosition()
        {
            return _positionReference.GetTargetPosition();
        }

        private void OnDestroy()
        {
            _isActive = false;
        }
    }
}

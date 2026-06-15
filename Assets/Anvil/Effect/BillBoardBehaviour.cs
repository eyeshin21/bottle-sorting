using System;
using UnityEngine;

namespace Anvil
{
    public class BillBoardBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _updateInterval = 0.2f;
        [SerializeField] private bool _invertForward = false;
        public virtual void Start()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        private float _updateTime = 0f;
        protected virtual void Update()
        {
            if (!_camera)
            {
                return;
            }
            _updateTime += Time.deltaTime;
            if (_updateTime >= _updateInterval)
            {
                _updateTime = 0f;
                Vector3 direction = _camera.transform.forward;
                // Vector3 up = _camera.transform.up;
                if (_invertForward)
                {
                    direction = -direction;
                }
                direction.y = 0; // Keep only the horizontal direction
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = targetRotation;
                }
            }
        }
    }
}
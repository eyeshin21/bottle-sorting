using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Anvil
{
    public class LocalMotionParalaxBehaviour : MonoBehaviour
    {
        Camera _mainCamera;
        [SerializeField]private float movementRatio = 0.5f; 
        [SerializeField]private bool x = true; 
        [SerializeField]private bool y = true; 
        [SerializeField]private bool z = true; 
        
        [Serializable]
        class ParallaxRig
        {
            [FormerlySerializedAs("parallaxFactor")]
            [Tooltip("The parallax factor determines how much the background moves relative to the camera. 1 means no movement(inherit all camera movement), 0 will try to stay still when camera move (in world space).")]
            [Range(0,1)] public float parallaxDistanceFactor;
            public Transform transform;
            public Vector3 originalLocalPosition;

            public void SetRoot(Vector3 position)
            {
                originalLocalPosition = position;
            }
        }
        [FormerlySerializedAs("_parallaxComponents")] [SerializeField] List<ParallaxRig> _parallaxRigs = new List<ParallaxRig>();
        private void Awake()
        {
            _mainCamera = Camera.main;
            foreach (ParallaxRig parallaxRig in _parallaxRigs)
            {
                if (parallaxRig.transform == null)
                {
                    Debug.LogWarning("Parallax Rig transform is null, please assign a valid transform.");
                    continue;
                }

                parallaxRig.SetRoot(parallaxRig.transform.localPosition);
            }
        }
        private void Update()
        {
            if (_mainCamera == null)
            {
                return;
            }

            Vector3 cameraPosition = _mainCamera.transform.position;
            Vector3 cameraForward = _mainCamera.transform.forward;
            
            Vector3 position = transform.position;
            if (transform is RectTransform rectTransform)
            {
                position = rectTransform.TransformPoint(rectTransform.rect.center);
            }
            Vector3 projectedCameraAxis = Vector3.Project(position - cameraPosition, cameraForward);
            Vector3 moveVector = position - projectedCameraAxis;
            if (!x)
            {
                moveVector.x = 0;
            }
            if (!y)
            {
                moveVector.y = 0;
            }
            if (!z)
            {
                moveVector.z = 0;
            }
            float distance = moveVector.magnitude * movementRatio;


            if (moveVector.magnitude == 0)
            {
                return;   
            }
            foreach (var parallaxRig in _parallaxRigs)
            {
                if (parallaxRig.transform == null)
                {
                    continue;
                }
                parallaxRig.transform.localPosition = parallaxRig.originalLocalPosition + moveVector * ((1 - parallaxRig.parallaxDistanceFactor) * distance);
            }
        }
    }
}
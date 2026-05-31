using System;
using System.Collections.Generic;
using System.Numerics;
using NaughtyAttributes;
using UnityEngine;
using Object = UnityEngine.Object;
using Vector3 = UnityEngine.Vector3;

namespace Anvil.Legacy
{
    public enum speedCalculationType
    {
        Default,
        Linear,
        Easing0,
        Easing1,
        Easing2,
        Custom0,
    }
    [Serializable]
    public struct DynamicProperties
    {
        // public DynamicType type;
        
        public float delay; // seconds
        public float speed;
        public float maxSpeed;
        public float acceleration;

        public DynamicProperties(float speed = 0,float acceleration = 0,float delay = 0, float maxSpeed = 0)
        {
            this.speed = speed;
            this.acceleration = acceleration;
            this.delay = delay;
            this.maxSpeed = maxSpeed;
        }
    }
    public class DiscreteObjectDynamicComponent : MonoBehaviour , IDiscreteObjectDynamic
    {
        [Serializable]
        private struct SpeedProfileConfig
        {
            public speedCalculationType speedCalculationType;
            public Object calculatorObject;
        }
        [SerializeField] private bool _useLocalCoordinates = false;

        [SerializeField] private List<SpeedProfileConfig> _speedProfiles = new List<SpeedProfileConfig>();
        
        [SerializeField] private Object trajectoryCalculatorObj;
        [SerializeField] private Object speedControllerObj;
        [SerializeField] private float _desiredMoveTime = 1f;
        
        [SerializeField] private TrajectoryParameterPreset parameterObj;
        
        private ITrajectoryCalculator _trajectoryCalculator;
        private ITargetDesignator _targetDesignator;
        private ISpeedController _speedController;
        
        public virtual float VelocityMagnitude => _speedController?.Speed ?? 0f;
        
        [SerializeField] private bool _perserveSpeed = false;
        [SerializeField] private bool _clampOvershoot = true;
        
        [SerializeField] private bool _logDebug = false;
        
        [SerializeField, ReadOnly]private bool _active = false;
        [SerializeField, ReadOnly]private bool _movable = true;
        // private bool _isMoving = false;
        [SerializeField, ReadOnly]private Vector3 _initialPosition;
        [SerializeField, ReadOnly]private float _elapsed = 0f;
        // [SerializeField, ReadOnly]private float _delayUntilMove = 0f;
        [SerializeField, ReadOnly]private DynamicProperties _dynamicProperties;
        
        [SerializeField, ReadOnly]private Action _callback;

        private void UpdateInitialPosition()
        {
            _initialPosition = CurrentPosition;
        }
        
        private Func<Vector3, Vector3> _positionConversionFunc;
        private Vector3 GetTargetPosition()
        {
            Vector3 targetPos = _targetDesignator.GetTargetPosition();
            if (!_useLocalCoordinates || _positionConversionFunc == null)
            {
                return targetPos;
            }
            return _positionConversionFunc(targetPos);
        }
        private Vector3 ConvertToMoveSpace(Vector3 worldPos)
        {
            if (!_useLocalCoordinates || transform.parent == null)
            {
                return worldPos;
            }
            return transform.parent.InverseTransformPoint(worldPos);
        }

        private Vector3 CurrentPosition 
        {
            get => _useLocalCoordinates ? transform.localPosition : transform.position;
             set
             {
                 if (_useLocalCoordinates)
                 {
                     transform.localPosition = value;
                 }
                 else
                 {
                     transform.position = value;
                 }
             }
        }
        public float DesiredMoveTime
        {
            get => _desiredMoveTime;
            set => _desiredMoveTime = value;
        }
        public bool IsActive => _active;
        
        private void Awake()
        {
            if (trajectoryCalculatorObj)
            {
                _trajectoryCalculator = trajectoryCalculatorObj as ITrajectoryCalculator;
            }
            else if(parameterObj != null)
            {
                SwapTrajectoryParam(parameterObj);
            }
            else
            {
                _trajectoryCalculator = LinearTrajectoryCalculator.Instance;
            }
            
            if (speedControllerObj)
            {
                _speedController = speedControllerObj as ISpeedController;
            }
            else
            {
                _speedController = gameObject.AddComponent<LinearSpeedCalculator>();
            }
        }

        public void SwapTrajectoryParam(TrajectoryParameterPreset trajectoryParameterPreset)
        {
            _trajectoryCalculator = trajectoryParameterPreset.CreateTrajectoryCalculator();
        }


        [SerializeField] private bool _selfUpdate = true;
        public virtual void Update()
        {
            if (_selfUpdate)
            {
                OnUpdate(Time.deltaTime);
            }
        }

        private float _moveProgress = 0f;
        public virtual void OnUpdate(float deltaTime)
        {
            if (!_movable || !_active)
            {
                return;
            }
            if (_targetDesignator == null 
                // || !_targetDesignator.IsActive 
                || _trajectoryCalculator == null)
            {
                return;
            }
            _elapsed += deltaTime;
            
            _moveProgress = _speedController.CalculateMovementProgress(_elapsed, _desiredMoveTime);
            if (_logDebug)
            {
                Debug.Log($"progress {_moveProgress}");
            }
            Vector3 calculatedPos = _trajectoryCalculator.CalculateTrajectory(_initialPosition, GetTargetPosition(), _moveProgress);
            if (_useLocalCoordinates)
                transform.localPosition = calculatedPos;
            else
                transform.position = calculatedPos;
            
            if (_elapsed >= _speedController.GetTotalTime())
            {
                OnMoveComplete();
                return;
            }
        }

#if DEBUG_MODE
        [SerializeField] private bool _drawDebug = true;
        private void OnDrawGizmos()
        {
            if (!_drawDebug)
            {
                return;
            }
            DrawDebug();
        }

        protected virtual void DrawDebug()
        {
            if (_trajectoryCalculator == null)
            {
                return;
            }
            _trajectoryCalculator?.DrawDebug();
        }
#endif
        
        private void OnMoveComplete()
        {
            if (_clampOvershoot)
            {
                CurrentPosition = GetTargetPosition();
            }
            _active = false;
            _targetDesignator = null;

            Action callback = _callback;
            _callback = null;
            callback?.Invoke();
        }

        public void SetTrajectoryCalculator(ITrajectoryCalculator trajectoryCalculator)
        {
            _trajectoryCalculator = trajectoryCalculator;
        }
        public void SetMoveTarget(Vector3 position)
        {
            _targetDesignator = new StaticPositionReference(ConvertToMoveSpace(position));
            _positionConversionFunc = null;
        }
        public void SetMoveTarget(ITargetDesignator targetDesignator)
        {
            _targetDesignator = targetDesignator;
            _positionConversionFunc = ConvertToMoveSpace;
        }

        public void ResetDynamic()
        {
            _speedController.Reset();
            // _delayUntilMove = _dynamicProperties.delay;
        }
        public void ResetTarget()
        {
            _targetDesignator = null;
            _active = false;
        }

        public void Reset()
        {
            ResetTarget();
            ResetDynamic();
        }

        private void ReinitAndActive(List<Vector3> positions)
        {
            ReInit();
            _trajectoryCalculator.ConstructTrajectory(positions);
            _speedController.Construct(_trajectoryCalculator.estimatedDistance, _desiredMoveTime);
        }

        public virtual void MoveTo(List<Vector3> positions, Action callback = null)
        {
            if (positions.IsNullOrEmpty())
            {
                callback?.Invoke();
                return;
            }else if (positions.Count == 1)
            {
                MoveTo(positions[0], callback);
                return;
            }

            _callback = callback;
            SetMoveTarget(new StaticPositionReference(positions[^1]));
            ReinitAndActive(positions);
            // _isMoving = true;
        }
        private void ReinitAndActive()
        {
            ReInit();

            _trajectoryCalculator.ConstructTrajectory(_initialPosition, GetTargetPosition());
            
            if (!_perserveSpeed)
            {
                _speedController.Reset();
            }
            _speedController.Construct(Vector3.Distance(_initialPosition, GetTargetPosition()), _desiredMoveTime);

        }

        private void ReInit()
        {
            _elapsed = 0f;
            _active = true;
            UpdateInitialPosition();
        }

        public virtual void StopMovement()
        {
            _active = false;
        }

        public virtual void MoveTo(Vector3 position)
        {
            SetMoveTarget(position);
            ReinitAndActive();
            // _isMoving = true;
        }
        public virtual void MoveTo(ITargetDesignator targetDesignator)
        {
            SetMoveTarget(targetDesignator);
            ReinitAndActive();
            // _isMoving = true;
        }
        public virtual void MoveTo(ITargetDesignator targetDesignator,Action callback)
        {
            _callback = callback;
            MoveTo(targetDesignator);
        }
        public virtual void MoveTo(Vector3 position, Action callback)
        {
            _callback = callback;
            MoveTo(position);
        }
        public void ForceCompleteMovement()
        {
        }

        [SerializeField] private GameObject testTarget;
        [Button]
        private void TestMove()
        {
            if (testTarget)
            {
                MoveTo(testTarget.transform.position);
            }
            else
            {
                Debug.LogError("No target found");
            }
        }

        public void SetEnviroment(DynamicProperties enviromentProperty)
        {
            _speedController?.SetProperty(enviromentProperty);
            _trajectoryCalculator?.SetProperty(enviromentProperty);
            // _delayUntilMove = enviromentProperty.delay;
            _dynamicProperties = enviromentProperty;
        }

        public void SwitchSpeedProfile(speedCalculationType speedType)
        {
            if (speedType == speedCalculationType.Default)
            {
                RevertToDefaultSpeedController();
                return;
            }
            
            SpeedProfileConfig? result = null;
            foreach (var config in _speedProfiles)
            {
                if (config.speedCalculationType != speedType)
                {
                    continue;
                }
                result = config;
                break;
            }
            if (result == null)
            {
                Debug.LogError($"No speed profile found for type {speedType}");
                return;
            }
            ISpeedController controller = result.Value.calculatorObject as ISpeedController;
            if (controller == null)
            {
                Debug.LogError($"Speed profile {speedType} is not a valid speed controller");
                return;
            }
            
            SetController(controller);
        }

        public void RevertToDefaultSpeedController()
        {
            ISpeedController controller = null;
            if (speedControllerObj != null)
            {
                controller = speedControllerObj as ISpeedController;   
            }

            if (controller == null)
            {
                controller = gameObject.GetComponent<ISpeedController>();
            }

            if (controller != null)
            {
                SetController(controller);
            }
            
            
        }

        public void SetController(ISpeedController controller)
        {
            _speedController = controller;
        }

        private void OnDrawGizmosSelected()
        {
            if (_useLocalCoordinates)
            {
                DrawLocalDebug();
            }
        }

        private void DrawLocalDebug()
        {
            if (_targetDesignator == null)
            {
                return;
            }
            Vector3 initialWorldPos = transform.parent.TransformPoint(_initialPosition);
            Vector3 targetWorldPos = transform.parent.TransformPoint(GetTargetPosition());
            Gizmos.DrawCube(initialWorldPos, Vector3.one * 0.2f);
            Gizmos.DrawSphere(targetWorldPos, 0.2f);
             Gizmos.DrawLine(initialWorldPos, targetWorldPos);
        }
    }
}
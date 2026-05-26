using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Anvil.Legacy
{
    public class TimebasedAccelerationCalculator : MonoBehaviour,ISpeedController
    {
        [FormerlySerializedAs("acceleration")] [SerializeField]private float _acceleration = 1;
        // [SerializeField] private float initialSpeed = 0;
        [SerializeField] private float _initialVelocity = 0;
        [SerializeField] private float _maxSpeed = 1;
        [FormerlySerializedAs("preservedInitialV")] [SerializeField] private float _preservedInitialV = 0;
        [SerializeField, ReadOnly] private float _velocity = 0;
        private float _distance = 0;
        [SerializeField] private bool logDebug = false;
        
        private float estimateMoveTime = -1;

        private void Awake()
        {
            Reset();
        }

        private float _accelerationTime = 0;
        public void Construct(float distance, float desiredMoveTime = 0)
        {
            float velocity = _velocity;
            if (velocity > _maxSpeed)
            {
                velocity = _maxSpeed;
            }
            _preservedInitialV = velocity;
            _distance = distance;
            
            float timeToMaxSpeed = (_maxSpeed - _preservedInitialV) / _acceleration;

            if (timeToMaxSpeed < 0)
            {
                timeToMaxSpeed = 0;
            }
            _accelerationTime = timeToMaxSpeed;
            if (_accelerationTime < 0)
            {
                _accelerationTime = 0;
            }
            
            float discriminant = Mathf.Sqrt(velocity * velocity + 2 * _acceleration * distance) / _acceleration;
            float t1 = -velocity/ _acceleration + discriminant;
            float t2 = -velocity / _acceleration - discriminant;
            
            // Min positive value
            if (t1 < 0 || t2 < 0)
            {
                estimateMoveTime = Mathf.Max(t1, t2);
            }
            else
            {
                estimateMoveTime = Mathf.Min(t1, t2);
            }

            if (estimateMoveTime >= timeToMaxSpeed)
            {
                estimateMoveTime = timeToMaxSpeed + (distance - (_preservedInitialV * timeToMaxSpeed + 0.5f * _acceleration * timeToMaxSpeed * timeToMaxSpeed)) / _maxSpeed;
            }
        }

        public void Reset()
        {
            _velocity = _initialVelocity;
            estimateMoveTime = -1;
        }

        public float Speed=>_velocity;
        public float Time =>estimateMoveTime;

        public float TimeBasedSpeed(float timeRatio)
        {
            float velocity = _preservedInitialV + _acceleration * timeRatio * estimateMoveTime;
            return velocity;
        }

        
        private float _lastRecordedTimeStamp = 0;
        /// <summary>
        /// Amount of distance ratio covered in the given time.
        /// </summary>
        public float CalculateMovementProgress(float elapsedTimeSec, float desiredMoveTime)
        {
            _lastRecordedTimeStamp = elapsedTimeSec;
            if (elapsedTimeSec < 0)
            {
                return 0;
            }
            
            // s = ut + 1/2 at^2
            _velocity = _preservedInitialV + _acceleration * elapsedTimeSec;
            float distanceCovered = 0;
            if (_velocity < _maxSpeed)
            {
                // still accelerating
                distanceCovered = _preservedInitialV * elapsedTimeSec + 0.5f * _acceleration * elapsedTimeSec * elapsedTimeSec;
            }
            else
            {
                // Distance covered until max speed is reached
                float distanceToMaxSpeed = _preservedInitialV * _accelerationTime +
                                           0.5f * _acceleration * _accelerationTime * _accelerationTime;
                // Remaining time at max speed
                float remainingTime = elapsedTimeSec - _accelerationTime;
                // Distance covered at max speed
                float distanceAtMaxSpeed = _maxSpeed * remainingTime;
                distanceCovered = distanceToMaxSpeed + distanceAtMaxSpeed;
            }

            return distanceCovered / _distance;
            
        }

        public float GetTotalTime()
        {
            return estimateMoveTime;
        }

        public void SetProperty(DynamicProperties enviromentProperty)
        {
                // Debug.Log($"Setting properties: {enviromentProperty}");
            _acceleration = enviromentProperty.acceleration;
            _initialVelocity = enviromentProperty.speed;
            _maxSpeed = enviromentProperty.maxSpeed;
            // Recalculate estimate move time
            // Construct(_distance);
        }

        
        [SerializeField] private int debugFontSize = 12;
        [SerializeField] private Vector3 debugOffset;
        private void OnDrawGizmos()
        {
            if (!logDebug)
            {
                return;
            }

            string text = $"Current Velocity: {_velocity}\n" +
                          $"Initial Velocity: {_initialVelocity}\n" +
                          $"Preserved Initial Velocity: {_preservedInitialV}\n" +
                          $"Estimated Move Time: {estimateMoveTime}\n" +
                          $"timestamp: {_lastRecordedTimeStamp}";
            GizmosHelper.DrawText(text, transform.position + debugOffset, Color.green, debugFontSize);
        }
    }
}
using UnityEngine;

namespace Anvil
{
    /// <summary>
    /// Receive infomation about a state of movevent and return the progress of that movement. does not contain context or additional data for any specific movement like a component based speed calculator does.
    /// </summary>
    public class SpeedProfile : ScriptableObject, ISpeedController
    {
        protected float _desiredMoveTime = 0;
        
        public virtual void Construct(float distance, float desiredMoveTime = 0)
        {
            _desiredMoveTime = desiredMoveTime;
        }

        public virtual void Reset()
        {
            Debug.Log("not supported on a speed profile");
        }

        public virtual float Speed => 0;
        public virtual float Time => 0;

        public float TimeBasedSpeed(float timeRatio)
        {
            Debug.Log("not supported on a speed profile");
            return 0;
        }

        public virtual float CalculateMovementProgress(float elapsedTimeSec, float desiredMoveTime)
        {
            _desiredMoveTime = desiredMoveTime;
            
            return elapsedTimeSec / elapsedTimeSec;
        }

        public virtual float GetTotalTime()
        {
            return _desiredMoveTime;
        }

        public void SetProperty(DynamicProperties enviromentProperty)
        {
            Debug.Log("not supported on a speed profile");
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class AnimationCurveSpeedCalculator : MonoBehaviour,ISpeedController

    {
        [SerializeField] private AnimationCurve _curve;
        
        private float averageSpeed = -1;
        private float _targetMoveTime = -1;
        public float Speed => averageSpeed;
        public void Construct(float distance, float desiredMoveTime = 0)
        {
            averageSpeed = distance / desiredMoveTime; 
            _targetMoveTime = desiredMoveTime;
        }
        public void Reset()
        {
        }
        public float CalculateMovementProgress(float elapsedTimeSec, float desiredMoveTime)
        {
            float timeFraction = elapsedTimeSec / desiredMoveTime;
            return _curve.Evaluate(timeFraction);
        }

        public float GetTotalTime()
        {
            return _targetMoveTime;
        }

        public void SetProperty(DynamicProperties enviromentProperty)
        {
        }
    }
}
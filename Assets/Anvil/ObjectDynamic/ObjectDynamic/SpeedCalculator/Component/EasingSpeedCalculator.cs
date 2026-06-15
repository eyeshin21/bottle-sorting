using System;
using UnityEngine;

namespace Anvil
{
    public class EasingSpeedCalculator : MonoBehaviour,ISpeedController
    {
        [SerializeField] EaseType easeType = EaseType.Linear;
        [SerializeField]private float moveTime = 1;
        private float averageSpeed = -1;
        Easer easer = Easers.Linear;
        public void Construct(float distance, float desiredMoveTime = 0)
        {
            averageSpeed = distance / moveTime;
            easer = Easers.GetEaser(easeType);
        }

        public void Reset()
        {
            
        }

        public float Speed=>averageSpeed;
        public float Time =>moveTime;

        public float TimeBasedSpeed(float timeRatio)
        {
            return averageSpeed;
        }

        public float CalculateMovementProgress(float elapsedTimeSec, float timeTotal)
        {
            
            // return elapsedTimeSec / moveTime;
            
            float t = elapsedTimeSec / Time;
            t = Mathf.Clamp01(t);
            return easer(t);
        }

        public float GetTotalTime()
        {
            return moveTime;
        }

        public void SetProperty(DynamicProperties enviromentProperty)
        {
            Debug.Log("not supported");
        }
    }
}
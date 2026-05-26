using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public class TimeBasedSpeedCalculator : MonoBehaviour,ISpeedController
    {
        [SerializeField]private float moveTime = 1;
        private float averageSpeed = -1;
        public void Construct(float distance, float desiredMoveTime = 0)
        {
            if (desiredMoveTime <= 0)
            {
                desiredMoveTime = moveTime;
            }
            averageSpeed = distance / desiredMoveTime;
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

        public float CalculateMovementProgress(float elapsedTimeSec, float desiredMoveTime)
        {
            return elapsedTimeSec / desiredMoveTime;
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
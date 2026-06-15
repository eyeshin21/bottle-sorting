using System;
using UnityEngine;

namespace Anvil
{
    public class LinearSpeedCalculator : MonoBehaviour,ISpeedController
    {
        private float moveTime = -1;
        public float speed = 5;

        public void Construct(float distance, float desiredMoveTime)
        {
            if (speed > 0)
            {
                moveTime = distance / speed;
            }
            else
            {
                moveTime = desiredMoveTime;
            }
            // Debug.Log($"move time: {moveTime} = {distance} / {speed}");
        }

        public void Reset()
        {
            
        }

        public float Speed=>speed;
        public float Time => moveTime;

        public float TimeBasedSpeed(float timeRatio)
        {
            return speed;
        }

        public float CalculateMovementProgress(float elapsedTimeSec, float timeTotal)
        {
            return elapsedTimeSec / moveTime;
        }

        public float GetTotalTime()
        {
            return moveTime;
        }

        public void SetProperty(DynamicProperties enviromentProperty)
        {
            speed = enviromentProperty.speed;
        }
    }
}
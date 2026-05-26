using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public interface ISpeedController
    {
        public void Construct(float distance, float desiredMoveTime = 0);
        public void Reset();
        public float Speed { get; }
        public float CalculateMovementProgress(float elapsedTimeSec, float desiredMoveTime);
        float GetTotalTime();
        void SetProperty(DynamicProperties enviromentProperty);
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Anvil.Legacy
{

    public class LinearTrajectoryCalculator : ITrajectoryCalculator
    {
        private static LinearTrajectoryCalculator _instance = new LinearTrajectoryCalculator();
        public static LinearTrajectoryCalculator Instance => _instance;
        
        private float _estimatedDistance = 0f;
        public float estimatedDistance => _estimatedDistance;
        public void ConstructTrajectory(Vector3 startPosition,Vector3 targetPosition)
        {
            _estimatedDistance = Vector3.Distance(startPosition, targetPosition);
        }

        public void ConstructTrajectory(List<Vector3> positions)
        {
            _estimatedDistance = 0f;
            for (int i = 0; i < positions.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                _estimatedDistance += Vector3.Distance(positions[i - 1], positions[i]);
            }
        }

        public Vector3 EvaluateTrajectory(float fraction)
        {
            Debug.LogError("[LinearTrajectoryController] Use CalculateTrajectory instead");
            return Vector3.zero;
        }

        public Vector3 CalculateTrajectory(Vector3 startPosition,Vector3 targetPosition,float timeRatio)
        {
            return Vector3.LerpUnclamped(startPosition,targetPosition,timeRatio);
        }

        public void DrawDebug()
        {
            
        }

        public void SetProperty(DynamicProperties enviromentProperty)
        {
            
        }

    }
}
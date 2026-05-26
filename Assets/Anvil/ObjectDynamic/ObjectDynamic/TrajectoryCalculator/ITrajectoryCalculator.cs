using System.Collections.Generic;
using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITrajectoryCalculator
    {
        public void ConstructTrajectory(Vector3 startPosition,Vector3 targetPosition);
        public void ConstructTrajectory(List<Vector3> startPosition);
        public Vector3 EvaluateTrajectory(float fraction);
        
        public Vector3 CalculateTrajectory(Vector3 startPosition,Vector3 targetPosition,float fraction);
        // public Vector3 CalculateTrajectory(ISpeedController speedController);

        public void DrawDebug();
        void SetProperty(DynamicProperties enviromentProperty);
        float estimatedDistance { get; }
    }
    public interface ITrajectoryParameter<out T> where T : ITrajectoryCalculator
    {
        public T CreateTrajectoryCalculator();
    }
    // public class TrajectoryParameter : ITrajectoryParameter<TrajectoryCalculator>
    // {
    //     public virtual TrajectoryCalculator CreateTrajectoryCalculator()
    //     {
    //         return null;
    //     }
    // }
    public abstract class TrajectoryCalculator : ITrajectoryCalculator
    {
        public virtual void ConstructTrajectory(Vector3 startPosition,Vector3 targetPosition)
        {
        }

        public void ConstructTrajectory(List<Vector3> startPosition)
        {
        }

        public virtual Vector3 EvaluateTrajectory(float fraction)
        {
            return Vector3.zero;
        }

        public virtual Vector3 CalculateTrajectory(Vector3 startPosition,Vector3 targetPosition,float fraction)
        {
            return Vector3.zero;
        }

        public virtual void DrawDebug()
        {
            
        }

        public void SetProperty(DynamicProperties enviromentProperty)
        {
            
        }

        public virtual float estimatedDistance => 0;
    }
    
    public class LinearTrajectoryParameter : ITrajectoryParameter<LinearTrajectoryCalculator>
    {
        public LinearTrajectoryCalculator CreateTrajectoryCalculator()
        {
            return LinearTrajectoryCalculator.Instance;
        }
    }
        
}
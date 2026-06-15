using UnityEngine;

namespace Anvil
{
    public abstract class TrajectoryParameter : ITrajectoryParameter<ITrajectoryCalculator>
    {
        public virtual ITrajectoryCalculator CreateTrajectoryCalculator()
        {
            return LinearTrajectoryCalculator.Instance;
        }
    }
}
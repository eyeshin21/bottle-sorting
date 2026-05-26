using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class TrajectoryParameter : ITrajectoryParameter<ITrajectoryCalculator>
    {
        public virtual ITrajectoryCalculator CreateTrajectoryCalculator()
        {
            return LinearTrajectoryCalculator.Instance;
        }
    }
}
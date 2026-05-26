using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class TrajectoryParameterPreset : ScriptableObject , ITrajectoryParameter<ITrajectoryCalculator>
    {
        public virtual ITrajectoryCalculator CreateTrajectoryCalculator()
        {
            return LinearTrajectoryCalculator.Instance;
        }
    }
}
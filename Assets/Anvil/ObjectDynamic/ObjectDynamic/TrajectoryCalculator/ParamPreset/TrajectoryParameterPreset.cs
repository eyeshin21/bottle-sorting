using UnityEngine;

namespace Anvil
{
    public abstract class TrajectoryParameterPreset : ScriptableObject , ITrajectoryParameter<ITrajectoryCalculator>
    {
        public virtual ITrajectoryCalculator CreateTrajectoryCalculator()
        {
            return LinearTrajectoryCalculator.Instance;
        }
    }
}
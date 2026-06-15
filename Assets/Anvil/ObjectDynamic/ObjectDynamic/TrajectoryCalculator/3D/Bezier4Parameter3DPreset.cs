using UnityEngine;

namespace Anvil
{
    public class Bezier4Parameter3DPreset : TrajectoryParameterPreset
    {
        [SerializeField] private Bezier4Parameter3D _value;
        
        public override ITrajectoryCalculator CreateTrajectoryCalculator()
        {
            return _value.CreateTrajectoryCalculator();
        }
    }
}
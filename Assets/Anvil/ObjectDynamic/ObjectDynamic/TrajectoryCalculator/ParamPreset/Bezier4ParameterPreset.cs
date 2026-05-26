using UnityEngine;

namespace Anvil.Legacy
{
    [CreateAssetMenu(fileName = "Bezier4Preset", menuName = "ScriptableObjects/Bezier4ParameterPreset", order = 1)]
    public class Bezier4ParameterPreset : TrajectoryParameterPreset
    {
        [SerializeField] private Bezier4Parameter _value;
        
        public override ITrajectoryCalculator CreateTrajectoryCalculator()
        {
            return _value.CreateTrajectoryCalculator();
        }
    }
}
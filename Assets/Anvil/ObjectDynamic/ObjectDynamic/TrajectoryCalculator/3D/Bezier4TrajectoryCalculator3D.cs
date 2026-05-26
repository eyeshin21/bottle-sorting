using UnityEngine;

namespace Anvil.Legacy
{
    public class Bezier4TrajectoryCalculator3D : TrajectoryCalculator
    {

        public Bezier4TrajectoryCalculator3D()
        {
            
        }
        public Bezier4TrajectoryCalculator3D(Bezier4Parameter3D config)
        {
            Config = config;
        }
        
        Bezier4Parameter3D _config;
        Vector3 _control1Value;
        Vector3 _control2Value;
        Vector3 _startPos;
        Vector3 _endPos;

        public Bezier4Parameter3D Config
        {
            get=>_config;
            set=>_config = value;
        }

        public override void ConstructTrajectory(Vector3 startPosition,Vector3 targetPosition)
        {
            _startPos = startPosition;
            _endPos = targetPosition;
            Config.GetControlsPosition(_startPos, _endPos, out _control1Value, out _control2Value);
        }

        public override Vector3 EvaluateTrajectory(float fraction)
        {
            return CalculateTrajectory(_startPos, _endPos, fraction);
        }

        public override Vector3 CalculateTrajectory(Vector3 startPosition,Vector3 targetPosition,float fraction)
        {
            // float t = _time / _duration;
            // float u = 1 - fraction;
            // float a = u * u * u;
            // float b = 3 * u * u * fraction;
            // float c = 3 * u * fraction * fraction;
            // float d = fraction * fraction * fraction;
            
            float t2 = 1 - fraction;

            float a = t2 * t2 * t2;
            float b = 3 * t2 * t2 * fraction;
            float c = 3 * t2 * fraction * fraction;
            float d = fraction * fraction * fraction;

            float x = a * startPosition.x + b * _control1Value.x + c * _control2Value.x + d * targetPosition.x;
            float y = a * startPosition.y + b * _control1Value.y + c * _control2Value.y + d * targetPosition.y;
            float z = a * startPosition.z + b * _control1Value.z + c * _control2Value.z + d * targetPosition.z;
            return new Vector3(x,y,z);
        }

        public override void DrawDebug()
        {
#if DEBUG_MODE && UNITY_EDITOR
            Config.DrawGizmos(_startPos, _endPos, Color.green);
#endif
        }
    }
}
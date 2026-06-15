using Anvil;
using UnityEngine;

namespace Anvil
{
    public class Bezier4TrajectoryCalculator : TrajectoryCalculator
    {

        public Bezier4TrajectoryCalculator()
        {
            
        }
        public Bezier4TrajectoryCalculator(Bezier4Parameter config)
        {
            Config = config;
        }

        private float _estimatedDistance = 0;
        public override float estimatedDistance => _estimatedDistance;
        Bezier4Parameter _config;
        Vector3 _controlPos;
        Vector3 _control2Pos;
        Vector3 _startPos;
        Vector3 _endPos;

        public Bezier4Parameter Config
        {
            get=>_config;
            set=>_config = value;
        }

        public override void ConstructTrajectory(Vector3 startPosition,Vector3 targetPosition)
        {
            _estimatedDistance = Vector3.Distance(startPosition, targetPosition);
            _startPos = startPosition.xy();
            _endPos = targetPosition.xy();
            Config.GetControlsPosition(_startPos, _endPos, out _controlPos, out _control2Pos);
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

            float x = a * startPosition.x + b * _controlPos.x + c * _control2Pos.x + d * targetPosition.x;
            float y = a * startPosition.y + b * _controlPos.y + c * _control2Pos.y + d * targetPosition.y;
            return new Vector3(x,y, 0);
        }

        public override void DrawDebug()
        {
#if DEBUG_MODE && UNITY_EDITOR
            Config.DrawGizmos(_startPos, _endPos, Color.green);
#endif
        }
    }
}
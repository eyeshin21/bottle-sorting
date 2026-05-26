using UnityEngine;

namespace Anvil.Legacy
{
    public class CubicBezierVector3Controller : Vector3Controller
    {
        Vector3 _start;
        Vector3 _control1;
        Vector3 _control2;
        Vector3 _end;
        float _duration;
        Evaluator _evaluator = Evaluator.Default;
        float _time;

        public Vector3 Start
        {
            get => _start;
            set => _start = value;
        }

        public Vector3 End
        {
            get => _end;
            set => _end = value;
        }

        public override float Duration => _duration;

        public Evaluator Evaluator
        {
            get => _evaluator;
            set => _evaluator = value ?? Evaluator.Default;
        }

        public CubicBezierVector3Controller()
        {

        }

        public CubicBezierVector3Controller(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float duration)
        {
            Construct(start, control1, control2, end, duration);
        }

        public void Construct(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float duration)
        {
            _start = start;
            _control1 = control1;
            _control2 = control2;
            _end = end;
            _duration = duration;

            _Construct(start);
            _time = 0;
        }

        public override bool GetDuration(out float duration)
        {
            duration = _duration;
            return true;
        }

        public override bool GetEnd(out Vector3 end)
        {
            end = _end;
            return true;
        }

        public override void SetEnd(Vector3 end)
        {
            _end = end;
        }

        public override void Reset()
        {
            base.Reset();
            _value = _start;
            _time = 0;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _time += deltaTime;
            float t;
            if (_time < _duration)
            {
                t = _time / _duration;
            }
            else
            {
                t = 1;
                _isFinished = true;
            }
            t = _evaluator.Evaluate(t);

            float t2 = 1 - t;
            float a = t2 * t2 * t2;
            float b = 3 * t2 * t2 * t;
            float c = 3 * t2 * t * t;
            float d = t * t * t;
            _value.x = a * _start.x + b * _control1.x + c * _control2.x + d * _end.x;
            _value.y = a * _start.y + b * _control1.y + c * _control2.y + d * _end.y;
            _value.z = a * _start.z + b * _control1.z + c * _control2.z + d * _end.z;
        }
    }
}
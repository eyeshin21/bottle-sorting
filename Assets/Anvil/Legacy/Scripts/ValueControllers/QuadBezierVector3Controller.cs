using UnityEngine;

namespace Anvil.Legacy
{
    public class QuadBezierVector3Controller : Vector3Controller
    {
        Vector3 _start;
        Vector3 _control;
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

        public QuadBezierVector3Controller()
        {

        }

        public QuadBezierVector3Controller(Vector3 start, Vector3 control, Vector3 end, float duration)
        {
            Construct(start, control, end, duration);
        }

        public void Construct(Vector3 start, Vector3 control, Vector3 end, float duration)
        {
            _start = start;
            _control = control;
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

            float u = 1 - t;
            float a = u * u;
            float b = 2 * u * t;
            float c = t * t;
            _value.x = a * _start.x + b * _control.x + c * _end.x;
            _value.y = a * _start.y + b * _control.y + c * _end.y;
            _value.z = a * _start.z + b * _control.z + c * _end.z;
        }
    }
}
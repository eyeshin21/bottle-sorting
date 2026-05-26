using UnityEngine;

namespace Anvil.Legacy
{
    public class LerpVector3Controller : Vector3Controller
    {
        Vector3 _start;
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

        public LerpVector3Controller()
        {

        }

        public LerpVector3Controller(Vector3 start, Vector3 end, float duration)
        {
            Construct(start, end, duration);
        }

        public void Construct(Vector3 start, Vector3 end, float duration)
        {
            _start = start;
            _end = end;
            _duration = Mathf.Max(duration, 0);

            _Construct(start);
            _time = 0;
        }

        public LerpVector3Controller SetEvaluator(EaseType easeType)
        {
            _evaluator = Evaluator.Create(easeType);
            return this;
        }

        public LerpVector3Controller SetEvaluator(Evaluator evaluator)
        {
            _evaluator = evaluator ?? Evaluator.Default;
            return this;
        }

        public override bool GetDuration(out float duration)
        {
            duration = _duration;
            return true;
        }

        public override bool GetEnd(out Vector3 value)
        {
            value = _end;
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

        public void Copy(LerpVector3Controller controller)
        {
            _start = controller._start;
            _end = controller._end;
            _duration = controller._duration;
            _evaluator = controller._evaluator;
            _time = controller._time;
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
            _value.x = _start.x + t * (_end.x - _start.x);
            _value.y = _start.y + t * (_end.y - _start.y);
            _value.z = _start.z + t * (_end.z - _start.z);
        }
    }
}
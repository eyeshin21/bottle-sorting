using UnityEngine;

namespace Anvil.Legacy
{
    public class LerpFloatController : FloatController
    {
        float _start;
        float _end;
        float _duration;
        Evaluator _evaluator = Evaluator.Default;
        float _time;

        public float Start
        {
            get => _start;
            set => _start = value;
        }

        public float End
        {
            get => _end;
            set => _end = value;
        }

        public override float Duration => _duration;

        public LerpFloatController()
        {

        }

        public LerpFloatController(float start, float end, float duration)
        {
            Construct(start, end, duration);
        }

        public void Construct(float start, float end, float duration)
        {
            _start = start;
            _end = end;
            _duration = Mathf.Max(duration, 0);

            _Construct(start);
            _time = 0;
        }

        public LerpFloatController SetEvaluator(Evaluator evaluator)
        {
            _evaluator = evaluator ?? Evaluator.Default;
            return this;
        }

        public override bool GetDuration(out float duration)
        {
            duration = _duration;
            return true;
        }

        public override bool GetEnd(out float end)
        {
            end = _end;
            return true;
        }

        public override void SetEnd(float end)
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
            _value = _start + t * (_end - _start);
        }
    }
}
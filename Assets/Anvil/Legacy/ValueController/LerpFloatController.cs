namespace Anvil
{
    public class LerpFloatController : FloatController
    {
        private float _start;
        private float _end;
        private float _duration;

        private Evaluator _evaluator = Evaluator.Default;
        private float _time;

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

        public Evaluator Evaluator
        {
            get => _evaluator;
            set => _evaluator = value ?? Evaluator.Default;
        }

        public void Construct(float start, float end, float duration)
        {
            _start = start;
            _end = end;
            _duration = duration;

            _Construct(start);
            _time = 0;
        }

        public override bool GetEnd(out float value)
        {
            value = _end;
            return true;
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
using UnityEngine;

namespace Anvil.Legacy
{
    public class AnimationCurveVector3Controller : Vector3Controller
    {
        Vector3 _start;
        Vector3 _end;
        AnimationCurve _curve;
        float _duration;
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

        public AnimationCurveVector3Controller()
        {

        }

        public AnimationCurveVector3Controller(Vector3 start, Vector3 end, AnimationCurve curve, float duration)
        {
            Construct(start, end, curve, duration);
        }

        public void Construct(Vector3 start, Vector3 end, AnimationCurve curve, float duration)
        {
            Assert.IsTrue(curve.IsTime01());
            _start = start;
            _end = end;
            _curve = curve;
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
            if (_time < _duration)
            {
                float t = _time / _duration;
                float xFactor = t;
                float yFactor = _curve.Evaluate(t);
                _value.x = _start.x + xFactor * (_end.x - _start.x);
                _value.y = _start.y + yFactor * (_end.y - _start.y);
                //TODO: _value.z
            }
            else
            {
                _value = _end;
                _isFinished = true;
            }
        }
    }
}
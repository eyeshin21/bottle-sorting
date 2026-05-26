using UnityEngine;

namespace Anvil.Legacy
{
    public class AnimationCurveFloatController : FloatController
    {
        AnimationCurve _curve;
        float _duration;
        float _endTime;
        float _time;

        public override float Duration => _duration < 0 ? _endTime : _duration;

        public AnimationCurveFloatController()
        {

        }

        public AnimationCurveFloatController(AnimationCurve curve, float duration)
        {
            Construct(curve, duration);
        }

        public void Construct(AnimationCurve curve, float duration)
        {
            _Construct(curve.Evaluate(0));

            _curve = curve;
            _duration = duration;
            if (duration < 0)
            {
                _endTime = curve.GetEndTime();
            }
            else
            {
                Assert.IsTrue(curve.IsTime01(), curve.keys.ToString2());
            }
            _time = 0;
        }

        public override bool GetDuration(out float duration)
        {
            duration = Duration;
            return true;
        }

        public override bool GetEnd(out float end)
        {
            end = _curve.GetEndValue();
            return true;
        }

        public override void Reset()
        {
            base.Reset();
            _value = _curve.Evaluate(0);
            _time = 0;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_duration < 0)
            {
                _value = _curve.Evaluate(_time);
                if (_time >= _endTime)
                {
                    _isFinished = true;
                }
            }
            else
            {
                if (_time < _duration)
                {
                    _value = _curve.Evaluate(_time / _duration);
                }
                else
                {
                    _value = _curve.Evaluate(1);
                    _isFinished = true;
                }
            }
        }
    }
}
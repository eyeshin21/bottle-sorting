using UnityEngine;

namespace Anvil.Legacy
{
    public class DiagramFloatController : FloatController
    {
        FloatDiagram _diagram;
        float _duration;
        float _endTime;
        int _segmentIndex;
        float _time;

        public override float Duration => _duration < 0 ? _endTime : _duration;

        public DiagramFloatController()
        {

        }

        public DiagramFloatController(FloatDiagram diagram, float duration)
        {
            Construct(diagram, duration);
        }

        public void Construct(FloatDiagram diagram, float duration)
        {
            _Construct(diagram.StartValue);

            _diagram = diagram;
            _duration = duration;
            if (duration < 0)
            {
                _endTime = diagram.EndTime;
            }
            else
            {
                Assert.IsTrue(diagram.IsTimeWithin01(), diagram);
            }
            _segmentIndex = 0;
            _time = 0;
        }

        public override bool GetDuration(out float duration)
        {
            duration = Duration;
            return true;
        }

        public override bool GetEnd(out float end)
        {
            end = _diagram.EndValue;
            return true;
        }

        public override void Reset()
        {
            base.Reset();
            _value = _diagram.StartValue;
            _segmentIndex = 0;
            _time = 0;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_duration < 0)
            {
                if (_time < _endTime)
                {
                    _value = _diagram.GetValue(ref _segmentIndex, _time);
                }
                else
                {
                    _value = _diagram.EndValue;
                    _isFinished = true;
                }
            }
            else
            {
                if (_time < _duration)
                {
                    _value = _diagram.GetValue(ref _segmentIndex, _time / _duration);
                }
                else
                {
                    _value = _diagram.EndValue;
                    _isFinished = true;
                }
            }
        }
    }
}
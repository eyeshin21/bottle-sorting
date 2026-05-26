using UnityEngine;

namespace Anvil.Legacy
{
    public class FloatControllerConverter : FloatController
    {
        FloatController _controller;
        Func<float, float> _converter;

        public override float Value => Convert(_controller.Value);
        public override bool Finished => _controller.Finished;
        public override float Duration => _controller.Duration;

        float Convert(float value)
        {
            return _converter != null ? _converter(value) : value;
        }

        public FloatControllerConverter()
        {

        }

        public FloatControllerConverter(FloatController controller, Func<float, float> converter)
        {
            Construct(controller, converter);
        }

        public void Construct(FloatController controller, Func<float, float> converter)
        {
            _controller = controller;
            _converter = converter;
        }

        public override bool GetDuration(out float duration)
        {
            return _controller.GetDuration(out duration);
        }

        public override bool GetEnd(out float end)
        {
            if (_controller.GetEnd(out end))
            {
                end = Convert(end);
                return true;
            }
            return false;
        }

        public override void SetEnd(float end)
        {
            Assert.NotSupported($"Set end to {end}");
        }

        public override void Stop()
        {
            _controller.Stop();
        }

        public override void Reset()
        {
            _controller.Reset();
        }

        public override float Update(float deltaTime)
        {
            if (_isFinished)
            {
                _value = Convert(_controller.Value);
            }
            else
            {
                _value = Convert(_controller.Update(deltaTime));
                _isFinished = _controller.Finished;
            }
            return _value;
        }
    }
}
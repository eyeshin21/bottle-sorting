using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Delay : ActionX
    {
        DelayGetter _delayGetter;
        float _delay;
        float _time;

        public override float Duration => _delay;

        public void Construct(float delay)
        {
            if (_delayGetter == null || !_delayGetter.Set(delay))
            {
                _delayGetter = DelayGetter.Create(delay);
            }
            Construct(_delayGetter);
        }

        public void Construct(DelayGetter delayGetter)
        {
            _delayGetter = delayGetter;
            _delayGetter.OnConstruct(ref _delay);
            _time = 0;
            _Construct();
        }

        public override void Reset()
        {
            base.Reset();
            _delayGetter.OnReset(ref _delay);
            _time = 0;
        }

        protected override bool OnPlay()
        {
            _time = 0;
            return _delay == 0;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                _time = _delay;
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            if (_delay < 0) return false;

            _time += deltaTime;
            return _time >= _delay;
        }

        public static Delay Create(float delay)
        {
            return Create(DelayGetter.Create(delay));
        }

        public static Delay Create(float minDelay, float maxDelay)
        {
            return Create(DelayGetter.Create(minDelay, maxDelay));
        }

        public static Delay Create(RangeFloat delay)
        {
            return Create(DelayGetter.Create(delay));
        }

        public static Delay Create(Func<float> func)
        {
            return Create(DelayGetter.Create(func));
        }

        public static Delay CreateInfinity()
        {
            return Create(-1);
        }

        static Delay Create(DelayGetter delayGetter)
        {
            var action = new Delay();
            action.Construct(delayGetter);
            return action;
        }
    }
}
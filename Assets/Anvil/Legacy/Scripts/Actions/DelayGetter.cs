using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class DelayGetter
    {
        public virtual void OnConstruct(ref float delay)
        {

        }

        public virtual void OnReset(ref float delay)
        {

        }

        public virtual bool Set(float delay)
        {
            return false;
        }

        #region Create
        public static DelayGetter Create(float delay)
        {
            var getter = new FixedDelayGetter();
            getter.Construct(delay);
            return getter;
        }

        public static DelayGetter Create(float minDelay, float maxDelay)
        {
            var getter = new RandomDelayGetter();
            getter.Construct(minDelay, maxDelay);
            return getter;
        }

        public static DelayGetter Create(RangeFloat delay)
        {
            return Create(delay.Min, delay.Max);
        }

        public static DelayGetter Create(Func<float> func)
        {
            var getter = new FuncDelayGetter();
            getter.Construct(func);
            return getter;
        }
        #endregion

        #region Getters
        class FixedDelayGetter : DelayGetter
        {
            float _delay;

            public void Construct(float delay)
            {
                _delay = delay;
            }

            public override void OnConstruct(ref float delay)
            {
                delay = _delay;
            }

            public override void OnReset(ref float delay)
            {
                delay = _delay;
            }

            public override bool Set(float value)
            {
                _delay = value;
                return true;
            }
        }

        class RandomDelayGetter : DelayGetter
        {
            float _minDelay;
            float _maxDelay;

            public void Construct(float minDelay, float maxDelay)
            {
                _minDelay = minDelay;
                _maxDelay = maxDelay;
            }

            public override void OnConstruct(ref float delay)
            {
                delay = Helper.GetRandomRange(_minDelay, _maxDelay);
            }

            public override void OnReset(ref float delay)
            {
                delay = Helper.GetRandomRange(_minDelay, _maxDelay);
            }
        }

        class FuncDelayGetter : DelayGetter
        {
            Func<float> _func;

            public void Construct(Func<float> func)
            {
                _func = func;
            }

            public override void OnConstruct(ref float value)
            {
                value = _func();
            }

            public override void OnReset(ref float value)
            {
                value = _func();
            }
        }
        #endregion
    }
}
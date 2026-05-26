using UnityEngine;
using System;

namespace Anvil.Legacy.Actions
{
    public class DelayCallFuncRepeat : ActionX
    {
        private float _delay;
        private float _delayFactor;
        private float _minDelay;
        private int _repeat;
        private Action<int> _callback;
        private Action _stopCallback;

        private float _currentDelay;
        private float _time;
        private int _index;

        public override float Duration
        {
            get
            {
                float duration = 0;
                float delay = _delay;
                if (delay > 0)
                {
                    if (_repeat > 1)
                    {
                        duration = delay; // i = 1
                        for (int i = 2; i < _repeat; i++)
                        {
                            delay = Mathf.Max(delay * _delayFactor, _minDelay);
                            duration += delay;
                        }
                    }
                }

                return duration;
            }
        }

        public static float GetDuration(DelayLoopData data, int repeat)
        {
            float duration = 0;
            float delay = data.Delay;
            if (delay > 0)
            {
                if (repeat > 1)
                {
                    duration = delay; // i = 1
                    for (int i = 2; i < repeat; i++)
                    {
                        delay = Mathf.Max(delay * data.Factor, data.Min);
                        duration += delay;
                    }
                }
            }

            return duration;
        }

        /// <summary>
        /// durationFunc(index, duration)
        /// </summary>
        public static float GetMaxDuration(DelayLoopData data, int repeat, Func<int, float> durationFunc)
        {
            float maxDuration = durationFunc(0);
            float delay = data.Delay;
            if (delay > 0)
            {
                if (repeat > 1)
                {
                    maxDuration = Mathf.Max(maxDuration, delay + durationFunc(1)); // i = 1
                    for (int i = 2; i < repeat; i++)
                    {
                        delay = Mathf.Max(delay * data.Factor, data.Min);
                        maxDuration = Mathf.Max(maxDuration, delay + durationFunc(i));
                    }
                }
            }

            return maxDuration;
        }

        public void Construct(float delay, float delayFactor, float minDelay, int repeat, Action<int> callback, Action stopCallback)
        {
            Assert.IsTrue(repeat > 0);
            _Construct();

            _delay = delay;
            _delayFactor = delayFactor;
            _minDelay = minDelay;
            _repeat = repeat;
            _callback = callback;
            _stopCallback = stopCallback;
        }

        protected override bool OnPlay()
        {
            _currentDelay = _delay;
            _time = 0;
            _index = 0;

            if (_delay > 0)
            {
                _callback(_index);
                _index++;
                return _index >= _repeat;
            }

            for (; _index < _repeat; _index++)
            {
                _callback(_index);
            }

            return true;
        }

        protected override void OnStop(bool forceEnd)
        {
            int index = _index;
            _index = _repeat;

            if (forceEnd)
            {
                for (; index < _repeat; index++)
                {
                    _callback(index);
                }
            }
            else
            {
                _stopCallback?.Invoke();
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time >= _currentDelay)
            {
                _callback(_index);

                _currentDelay = Mathf.Max(_currentDelay * _delayFactor, _minDelay);
                _time = 0;
                _index++;
            }

            return _index >= _repeat;
        }

        public static DelayCallFuncRepeat Create(DelayLoopData data, int repeat, Action<int> callback, Action stopCallback = null)
        {
            return Create(data.Delay, data.Factor, data.Min, repeat, callback, stopCallback);
        }

        /// <summary>
        /// callback(index)
        /// </summary>
        public static DelayCallFuncRepeat Create(float delay, float delayFactor, float minDelay, int repeat, Action<int> callback, Action stopCallback = null)
        {
            var action = new DelayCallFuncRepeat();
            action.Construct(delay, delayFactor, minDelay, repeat, callback, stopCallback);

            return action;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class ValueController<T>
    {
        protected T _value;
        protected bool _isFinished = true;

        public virtual T Value => _value;
        public virtual bool Finished => _isFinished;

        //protected virtual T DefaultValue => default;
        //[System.Obsolete("Use GetDuration(out float duration) instead.")]
        public virtual float Duration => 0;

        //protected virtual T StartValue => default;

        /// <summary>
        /// Sets start value and not finished.
        /// </summary>
        //protected void _Construct()
        //{
        //    _value = StartValue;
        //    _isFinished = false;
        //}

        /// <summary>
        /// Sets value and not finished.
        /// </summary>
        //[System.Obsolete("Use _Construct() instead.")]
        protected void _Construct(T value)
        {
            _value = value;
            _isFinished = false;
        }

        public virtual bool GetDuration(out float duration)
        {
            duration = 0;
            return false;
        }

        public virtual bool GetEnd(out T end)
        {
            end = default;
            return false;
        }

        public virtual void SetEnd(T end)
        {
            LegacyLog.NotSupported($"Set end to {end}");
        }

        public virtual void Stop()
        {
            _isFinished = true;
        }

        /// <summary>
        /// Sets not finished.
        /// Todo: Override value, time, ...
        /// </summary>
        public virtual void Reset()
        {
            _isFinished = false;
        }

        public virtual T Update(float deltaTime)
        {
            if (!_isFinished)
            {
                OnUpdate(deltaTime);
            }
            return _value;
        }

        protected virtual void OnUpdate(float deltaTime)
        {

        }
    }
}
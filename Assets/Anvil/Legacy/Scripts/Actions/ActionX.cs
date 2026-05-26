using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class ActionX
    {
        protected bool _isFinished = true;

        public virtual float Duration => 0;
        public virtual bool Finished => _isFinished;

        protected GameObject _target;
        public virtual GameObject Target
        {
            get => _target;
            set => _target = value;
        }

        /// <summary>
        /// Sets not finished.
        /// </summary>
        protected void _Construct()
        {
            _isFinished = false;
        }

        public virtual void Play()
        {
            if (!_isFinished)
            {
                _isFinished = OnPlay();
            }
        }

        /// <summary>
        /// Returns true if finished.
        /// </summary>
        protected virtual bool OnPlay()
        {
            return true;
        }

        /// <summary>
        /// Reset() and Play().
        /// </summary>
        public virtual void Replay()
        {
            Reset();
            Play();
        }

        /// <summary>
        /// Sets not finished.
        /// </summary>
        public virtual void Reset()
        {
            _isFinished = false;
        }

        /// <summary>
        /// Calls to Stop(forceEnd=false).
        /// </summary>
        public virtual void Stop()
        {
            Stop(false);
        }

        public virtual void Stop(bool forceEnd)
        {
            if (!_isFinished)
            {
                OnStop(forceEnd);
                _isFinished = true;
            }
        }

        protected virtual void OnStop(bool forceEnd)
        {

        }

        /// <summary>
        /// Returns true if finished.
        /// </summary>
        public virtual bool Update(float deltaTime)
        {
            if (!_isFinished)
            {
                _isFinished = OnUpdate(deltaTime);
            }
            return _isFinished;
        }

        protected virtual bool OnUpdate(float deltaTime)
        {
            return true;
        }
    }
}
using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class BaseMoveController : IMoveController
    {
        protected Transform _transform;
        protected bool _isLocal;
        protected Vector3 _endPos;
        protected Action _callback;

        protected bool _isFinished = true;

        public virtual Transform transform
        {
            get => _transform;
            set => _transform = value;
        }

        public virtual bool Local
        {
            get => _isLocal;
            set => _isLocal = value;
        }

        public Vector3 Current
        {
            get
            {
                if (_transform != null)
                {
                    return _isLocal ? _transform.localPosition : _transform.position;
                }
                return Vector3.zero;
            }
            set
            {
                if (_transform != null)
                {
                    if (_isLocal)
                    {
                        _transform.localPosition = value;
                    }
                    else
                    {
                        _transform.position = value;
                    }
                }
            }
        }

        public Vector3 End
        {
            get => _endPos;
            set => _endPos = value;
        }

        public bool Finished => _isFinished;

        public virtual void MoveTo(Vector3 endPos, Action callback = null)
        {
            _endPos = endPos;
            _callback = callback;
            _isFinished = false;
        }

        public virtual void Stop(bool forceEnd = false)
        {
            if (!_isFinished)
            {
                _isFinished = true;
                if (forceEnd)
                {
                    Current = _endPos;
                    if (_callback != null)
                    {
                        var callback = _callback;
                        _callback = null;
                        callback();
                    }
                }
            }
        }

        public virtual void ForceFinish()
        {
            if (!_isFinished)
            {
                _isFinished = true;
                Current = _endPos;
            }
        }

        protected virtual void OnFinish()
        {
            _isFinished = true;
            if (_callback != null)
            {
                var callback = _callback;
                _callback = null;
                callback();
            }
        }

        public abstract void Update(float deltaTime);

        public virtual void UpdateConfig(MoveConfig moveConfig)
        {

        }

        public virtual void ReturnPool()
        {
            _transform = null;
            _isLocal = false;
            _callback = null;
            _isFinished = true;
        }
    }
}
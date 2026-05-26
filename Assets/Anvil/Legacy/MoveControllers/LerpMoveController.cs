using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class LerpMoveController : BaseMoveController
    {
        protected Vector3 _startPos;
        protected float _duration = 1;
        protected float _speed;
        protected float _time;

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public override void MoveTo(Vector3 endPos, Action callback = null)
        {
            if (_speed > 0)
            {
                MoveToSpeed(endPos, _speed, callback);
            }
            else
            {
                MoveTo(endPos, _duration, callback);
            }
        }

        public void MoveTo(Vector3 endPos, float duration, Action callback = null)
        {
            base.MoveTo(endPos, callback);
            Assert.IsPositive(duration);
            _duration = duration;
            _startPos = Current;
            _time = 0;
        }

        public void MoveToSpeed(Vector3 endPos, float speed, Action callback = null)
        {
            var startPos = Current;
            float deltaX = endPos.x - startPos.x;
            float deltaY = endPos.y - startPos.y;
            float distance = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
            float duration = distance / Mathf.Max(speed, Mathf.Epsilon);
            MoveTo(endPos, duration, callback);
        }

        public override void Update(float deltaTime)
        {
            if (!_isFinished)
            {
                _time += deltaTime;
                if (_time < _duration)
                {
                    float t = GetLerp(_time / _duration);
                    float x = _startPos.x + (_endPos.x - _startPos.x) * t;
                    float y = _startPos.y + (_endPos.y - _startPos.y) * t;
                    Current = new Vector3(x, y, 0);
                }
                else
                {
                    Current = _endPos;
                    OnFinish();
                }
            }
        }

        protected virtual float GetLerp(float t)
        {
            return t;
        }

        public override void UpdateConfig(MoveConfig moveConfig)
        {
            _duration = moveConfig.Duration;
            if (moveConfig.MovementType == MovementType.Speed)
            {
                _speed = moveConfig.Speed;
            }
            else
            {
                _speed = 0;
            }
        }
        
    }
}
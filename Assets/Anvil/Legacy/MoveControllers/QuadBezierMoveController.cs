using System;
using UnityEngine;

namespace Anvil.Legacy
{
    public class QuadBezierMoveController : LerpMoveController
    {
        QuadBezierConfig2D _config2D;
        Vector3 _controlPos;

        public QuadBezierConfig2D Config2D
        {
            get => _config2D;
            set => _config2D = value;
        }

        public Vector3 ControlPos
        {
            get => _controlPos;
            set => _controlPos = value;
        }

        public override void MoveTo(Vector3 endPos, Action callback = null)
        {
            Vector3 controlPos;
            if (_config2D != null)
            {
                controlPos = _config2D.GetControlPosition(Current, endPos);
            }
            else
            {
                controlPos = _controlPos;
            }
            float duration = _duration;
            if (_speed > 0)
            {
                //TODO
                duration = (endPos - Current).magnitude / Mathf.Max(_speed, Mathf.Epsilon);
            }
            MoveTo(endPos, controlPos, duration, callback);
        }

        public void MoveTo(Vector3 endPos, Vector3 controlPos, float duration, Action callback = null)
        {
            base.MoveTo(endPos, callback);
            _controlPos = controlPos;
            _duration = duration;
            _startPos = Current;
            _time = 0;
        }

        public override void Update(float deltaTime)
        {
            if (!_isFinished)
            {
                _time += deltaTime;
                if (_time < _duration)
                {
                    float t = _time / _duration;
                    float u = 1 - t;
                    float a = u * u;
                    float b = 2 * u * t;
                    float c = t * t;
                    float x = a * _startPos.x + b * _controlPos.x + c * _endPos.x;
                    float y = a * _startPos.y + b * _controlPos.y + c * _endPos.y;
                    Current = new Vector3(x, y, 0);
                }
                else
                {
                    Current = _endPos;
                    OnFinish();
                }
            }
        }

        public override void UpdateConfig(MoveConfig moveConfig)
        {
            base.UpdateConfig(moveConfig);
            Config2D = moveConfig.QuadBezierConfig2D;
        }

        static Pool<QuadBezierMoveController> _pool;
        static Pool<QuadBezierMoveController> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new Pool<QuadBezierMoveController>();
                }
                return _pool;
            }
        }

        public static QuadBezierMoveController Create()
        {
            return Pool.Get();
        }

        public override void ReturnPool()
        {
            base.ReturnPool();
            Pool.Return(this);
        }
    }
}
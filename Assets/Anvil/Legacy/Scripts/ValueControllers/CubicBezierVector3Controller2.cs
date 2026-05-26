using UnityEngine;

namespace Anvil.Legacy
{
    /// <summary>
    /// End transform.
    /// </summary>
    public class CubicBezierVector3Controller2 : Vector3Controller
    {
        Vector3 _startPos;
        float _controlY;
        Vector3 _endPos;
        Transform _endTransform;
        float _duration;
        Evaluator _evaluator = Evaluator.Default;
        float _time;

        public Vector3 Start
        {
            get => _startPos;
            set => _startPos = value;
        }

        public Vector3 End
        {
            get => _endTransform != null ? _endTransform.position : _endPos;
            set
            {
                _endPos = value;
                if (_endTransform != null)
                {
                    _endTransform.position = value;
                }
            }
        }

        public override float Duration => _duration;

        public float BezierLength
        {
            get
            {
                var endPos = End;
                return Helper.GetCubicBezierLength(_startPos, new Vector3(_startPos.x, _controlY), new Vector3(endPos.x, _controlY), endPos);
            }
        }

        public Evaluator Evaluator
        {
            get => _evaluator;
            set => _evaluator = value ?? Evaluator.Default;
        }

        public CubicBezierVector3Controller2()
        {

        }

        public CubicBezierVector3Controller2(Vector3 start, float controlY, Transform endTransform, float duration)
        {
            Construct(start, controlY, endTransform, duration);
        }

        public void Construct(Vector3 start, float controlY, Transform endTransform, float duration)
        {
            _startPos = start;
            _controlY = controlY;
            _endPos = endTransform != null ? endTransform.position : start;
            _endTransform = endTransform;
            _duration = duration;

            _Construct(start);
            _time = 0;
        }

        public override bool GetDuration(out float duration)
        {
            duration = _duration;
            return true;
        }

        public override bool GetEnd(out Vector3 end)
        {
            end = End;
            return true;
        }

        public override void SetEnd(Vector3 end)
        {
            End = end;
        }

        public override void Reset()
        {
            base.Reset();
            _value = _startPos;
            _time = 0;
        }

        protected override void OnUpdate(float deltaTime)
        {
            _time += deltaTime;
            float t;
            if (_time < _duration)
            {
                t = _time / _duration;
            }
            else
            {
                t = 1;
                _isFinished = true;
            }
            t = _evaluator.Evaluate(t);

            float t2 = 1 - t;
            float a = t2 * t2 * t2;
            float b = 3 * t2 * t2 * t;
            float c = 3 * t2 * t * t;
            float d = t * t * t;
            var endPos = End;
            _value.x = a * _startPos.x + b * _startPos.x + c * endPos.x + d * endPos.x;
            _value.y = a * _startPos.y + b * _controlY + c * _controlY + d * endPos.y;
            //_value.z = a * _startPos.z + b * _control1.z + c * _control2.z + d * endPos.z;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class SpawnRotate : ActionX
    {
        ActionX _action;
        float _angleOffset;
        Transform _transform;
        Vector3 _prevPos;

        public override float Duration => _action.Duration;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _action.Target = value;
                _transform = value.transform;
            }
        }

        public void Construct(ActionX action, float angleOffset)
        {
            _action = action;
            _angleOffset = angleOffset;
            _transform = action.Target?.transform;
            _Construct();
        }

        public override void Play()
        {
            if (_isFinished) return;

            _action?.Play();
            _prevPos = _transform.position;

            if (_action == null || _action.Finished)
            {
                _isFinished = true;
            }
        }

        public override void Reset()
        {
            base.Reset();
            _action?.Reset();
            //TODO: Reset start angle
        }

        public override void Stop(bool forceEnd)
        {
            if (_isFinished) return;

            _action?.Stop(forceEnd);
            //if (forceEnd)
            //{

            //}
            _isFinished = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_isFinished) return true;

            if (_action != null)
            {
                _action.Update(deltaTime);
                if (_action.Finished)
                {
                    _isFinished = true;
                }
            }
            else
            {
                _isFinished = true;
            }

            var pos = _transform.position;
            float angle = Helper.GetAngle(_prevPos, pos) + _angleOffset;
            _transform.localRotation = Quaternion.Euler(0, 0, angle);
            _prevPos = pos;

            return _isFinished;
        }

        public static SpawnRotate Create(ActionX action, float angleOffset)
        {
            var action2 = new SpawnRotate();
            action2.Construct(action, angleOffset);
            return action2;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class SpeedAnimationCurve : ActionX
    {
        private ActionX _action;
        private AnimationCurve _speed;
        private float _time;

        public override float Duration => _action.Duration / _speed.Evaluate(0);

        public override GameObject Target
        {
            set
            {
                _target = value;
                _action.Target = value;
            }
        }

        public void Construct(ActionX action, AnimationCurve speed)
        {
            _Construct();

            _action = action;
            _speed = speed;
            _time = 0;
        }

        public override void Play()
        {
            if (_isFinished) return;

            _action.Play();
            if (_action.Finished)
            {
                _isFinished = true;
            }
        }

        public override void Reset()
        {
            _action.Reset();
            _time = 0;
            base.Reset();
        }

        public override void Stop(bool forceEnd)
        {
            if (_isFinished) return;

            _action.Stop(forceEnd);
            _isFinished = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_isFinished) return true;

            _time += deltaTime;
            float speed = _speed.Evaluate(_time);
            _action.Update(deltaTime * speed);
            if (_action.Finished)
            {
                _isFinished = true;
            }

            return _isFinished;
        }

        public static SpeedAnimationCurve Create(ActionX action, AnimationCurve speed)
        {
            var action2 = new SpeedAnimationCurve();
            action2.Construct(action, speed);

            return action2;
        }
    }
}

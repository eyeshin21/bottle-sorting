using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Speed : ActionX
    {
        private ActionX _action;
        private float _speed = 1;

        public override float Duration => _action.Duration / _speed;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _action.Target = value;
            }
        }

        //public float MaxSpeed
        //{
        //    get => _speed;
        //    set => _speed = value;
        //}

        public void Construct(ActionX action, float speed)
        {
            _Construct();

            _action = action;
            _speed = speed;
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

            _action.Update(deltaTime * _speed);
            if (_action.Finished)
            {
                _isFinished = true;
            }

            return _isFinished;
        }

        public static Speed Create(ActionX action, float speed)
        {
            var action2 = new Speed();
            action2.Construct(action, speed);

            return action2;
        }
    }
}

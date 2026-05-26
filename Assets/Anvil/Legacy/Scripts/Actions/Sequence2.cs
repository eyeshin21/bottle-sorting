using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Sequence2 : ActionX
    {
        private ActionX _action1;
        private ActionX _action2;
        private ActionX _currentAction;

        public override float Duration
        {
            get
            {
                float duration1 = _action1.Duration;
                if (duration1 < 0) return duration1;

                float duration2 = _action2.Duration;
                if (duration2 < 0) return duration2;

                return duration1 + duration2;
            }
        }

        public override GameObject Target
        {
            set
            {
                _target = value;
                _action1.Target = value;
                _action2.Target = value;
            }
        }

        public void Construct(ActionX action1, ActionX action2)
        {
            _Construct();

            _action1 = action1;
            _action2 = action2;
            _currentAction = _action1;
        }

        public override void Play()
        {
            if (_isFinished) return;

            _currentAction = _action1;
            _currentAction.Play();

            if (_currentAction.Finished)
            {
                _currentAction = _action2;
                _currentAction.Play();

                if (_currentAction.Finished)
                {
                    _isFinished = true;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();

            _currentAction.Reset();

            if (_currentAction == _action2)
            {
                _currentAction = _action1;
                _currentAction.Reset();
            }
        }

        public override void Stop(bool forceEnd)
        {
            if (_isFinished) return;

            _currentAction.Stop(forceEnd);

            if (forceEnd)
            {
                if (_currentAction == _action1)
                {
                    _currentAction = _action2;
                    _currentAction.Stop(true);
                }
            }

            _isFinished = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_isFinished) return true;

            _currentAction.Update(deltaTime);
            if (_currentAction.Finished)
            {
                if (_currentAction == _action1)
                {
                    _currentAction = _action2;
                    _currentAction.Play();
                    if (_currentAction.Finished)
                    {
                        _isFinished = true;
                    }
                }
                else
                {
                    _isFinished = true;
                }
            }

            return _isFinished;
        }

        public static Sequence2 Create(ActionX action1, ActionX action2)
        {
            var action = new Sequence2();
            action.Construct(action1, action2);

            return action;
        }

        public static Sequence2 Create(float delay, ActionX action)
        {
            return Create(Delay.Create(delay), action);
        }

        public static Sequence2 Create(ActionX action, float delay)
        {
            return Create(action, Delay.Create(delay));
        }
    }
}

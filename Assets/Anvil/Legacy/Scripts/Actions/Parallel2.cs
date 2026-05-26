using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Parallel2 : ActionX
    {
        private ActionX _action1;
        private ActionX _action2;
        private bool _isAny;

        public override float Duration
        {
            get
            {
                float duration1 = _action1.Duration;
                if (duration1 < 0) return duration1;

                float duration2 = _action2.Duration;
                if (duration2 < 0) return duration2;

                return _isAny ? Mathf.Min(duration1, duration2) : Mathf.Max(duration1, duration2);
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

        public void Construct(ActionX action1, ActionX action2, bool isAny)
        {
            _Construct();

            _action1 = action1;
            _action2 = action2;
            _isAny = isAny;
        }

        public override void Play()
        {
            if (_isFinished) return;

            _action1.Play();
            _action2.Play();

            if (_isAny)
            {
                if (_action1.Finished || _action2.Finished)
                {
                    _isFinished = true;
                }
            }
            else
            {
                if (_action1.Finished && _action2.Finished)
                {
                    _isFinished = true;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();

            _action2.Reset();
            _action1.Reset();
        }

        public override void Stop(bool forceEnd)
        {
            if (_isFinished) return;

            _action1.Stop(forceEnd);
            _action2.Stop(forceEnd);

            _isFinished = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_isFinished) return true;

            _action1.Update(deltaTime);
            _action2.Update(deltaTime);

            if (_isAny)
            {
                if (_action1.Finished || _action2.Finished)
                {
                    _isFinished = true;
                }
            }
            else
            {
                if (_action1.Finished && _action2.Finished)
                {
                    _isFinished = true;
                }
            }

            return _isFinished;
        }

        public static Parallel2 Create(ActionX action1, ActionX action2, bool isAny = false)
        {
            var action = new Parallel2();
            action.Construct(action1, action2, isAny);

            return action;
        }
    }
}

using UnityEngine;

namespace Anvil.Legacy.Actions
{
    public class Repeat : ActionX
    {
        private ActionX _action;
        private int _repeat = -1;
        private int _remaining;

        public override float Duration
        {
            get { return _repeat < 0 ? - 1 : _action.Duration * _repeat; }
        }

        public override GameObject Target
        {
            set
            {
                _target = value;
                _action.Target = value;
            }
        }

        public void Construct(ActionX action, int repeat)
        {
            _Construct();

            _action = action;
            _repeat = repeat;
        }

        public override void Play()
        {
            if (_isFinished) return;

            _remaining = _repeat;

            _action.Play();
            if (_action.Finished)
            {
                if (_repeat > 0)
                {
                    for (int i = 1; i < _repeat; i++)
                    {
                        _action.Replay();
                    }

                    _remaining = 0;
                    _isFinished = true;
                }
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

            _action.Update(deltaTime);
            if (_action.Finished)
            {
                if (_repeat < 0)
                {
                    _action.Replay();
                }
                else
                {
                    _remaining--;
                    if (_remaining > 0)
                    {
                        _action.Replay();
                    }
                    else
                    {
                        _isFinished = true;
                    }
                }
            }

            return _isFinished;
        }

        public static Repeat Create(ActionX action, int repeat = -1)
        {
            var action2 = new Repeat();
            action2.Construct(action, repeat);

            return action2;
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy.Actions
{
    public class Parallel : ActionX
    {
        private ActionX[] _actions;
        private int _actionCount;
        private bool _isAny; // Finish when any action finished

        public override float Duration
        {
            get
            {
                float duration = _actions[0].Duration;
                if (duration < 0) return duration;

                for (int i = 1; i < _actionCount; i++)
                {
                    float duration2 = _actions[i].Duration;
                    if (duration2 < 0) return duration2;

                    if (_isAny)
                    {
                        if (duration2 < duration) duration = duration2;
                    }
                    else
                    {
                        if (duration2 > duration) duration = duration2;
                    }
                }

                return duration;
            }
        }

        public override GameObject Target
        {
            set
            {
                _target = value;

                for (int i = 0; i < _actionCount; i++)
                {
                    _actions[i].Target = value;
                }
            }
        }

        public void Construct(ActionX[] actions, bool isAny)
        {
            _Construct();

            _actions = actions;
            _actionCount = _actions.Length;
            _isAny = isAny;
        }

        public override void Play()
        {
            if (_isFinished) return;

            if (_isAny)
            {
                bool isFinished = false;
                for (int i = 0; i < _actionCount; i++)
                {
                    var action = _actions[i];
                    action.Play();

                    if (!isFinished)
                    {
                        if (action.Finished)
                        {
                            isFinished = true;
                        }
                    }
                }

                _isFinished = isFinished;
            }
            else
            {
                bool isFinished = true;
                for (int i = 0; i < _actionCount; i++)
                {
                    var action = _actions[i];
                    action.Play();

                    if (isFinished)
                    {
                        if (!action.Finished)
                        {
                            isFinished = false;
                        }
                    }
                }

                _isFinished = isFinished;
            }
        }

        public override void Reset()
        {
            base.Reset();

            for (int i = 0; i < _actionCount; i++)
            {
                _actions[i].Reset();
            }
        }

        public override void Stop(bool forceEnd)
        {
            if (_isFinished) return;

            for (int i = 0; i < _actionCount; i++)
            {
                _actions[i].Stop(forceEnd);
            }

            _isFinished = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_isFinished) return true;

            if (_isAny)
            {
                bool isFinished = false;
                for (int i = 0; i < _actionCount; i++)
                {
                    var action = _actions[i];
                    action.Update(deltaTime);

                    if (!isFinished)
                    {
                        if (action.Finished)
                        {
                            isFinished = true;
                        }
                    }
                }

                _isFinished = isFinished;
            }
            else
            {
                bool isFinished = true;
                for (int i = 0; i < _actionCount; i++)
                {
                    var action = _actions[i];
                    action.Update(deltaTime);

                    if (isFinished)
                    {
                        if (!action.Finished)
                        {
                            isFinished = false;
                        }
                    }
                }

                _isFinished = isFinished;
            }

            return _isFinished;
        }

        public static Parallel Create(params ActionX[] actions)
        {
            var action = new Parallel();
            action.Construct(actions, false);

            return action;
        }

        public static Parallel CreateAny(params ActionX[] actions)
        {
            var action = new Parallel();
            action.Construct(actions, true);

            return action;
        }

        public static ActionX Create(List<ActionX> actions, bool isAny = false)
        {
            int count = actions.Count;
            if (count == 1) return actions[0];
            if (count == 2) return Parallel2.Create(actions[0], actions[1], isAny);

            var action = new Parallel();
            action.Construct(actions.ToArray(), isAny);

            return action;
        }

        public static ActionX Create(ActionX action1, ActionX action2, bool isAny = false)
        {
            return Parallel2.Create(action1, action2, isAny);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy.Actions
{
    public class Sequence : ActionX
    {
        ActionX[] _actions;
        int _actionCount;
        int _currentIndex;

        public void Construct(ActionX[] actions)
        {
            _actions = actions;
            _actionCount = _actions.Length;
            _currentIndex = 0;
            _Construct();
        }

        public override float Duration
        {
            get
            {
                float duration = 0;
                for (int i = 0; i < _actionCount; i++)
                {
                    float duration2 = _actions[i].Duration;
                    if (duration2 < 0) return duration2;
                    duration += duration2;
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

        public override void Play()
        {
            if (_isFinished) return;

            _currentIndex = 0;
            do
            {
                var action = _actions[_currentIndex];
                action.Play();

                if (action.Finished)
                {
                    _currentIndex++;
                    if (_currentIndex == _actionCount)
                    {
                        _isFinished = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            while (true);
        }

        public override void Reset()
        {
            for (int i = _currentIndex < _actionCount ? _currentIndex : _actionCount - 1; i >= 0; i--)
            {
                _actions[i].Reset();
            }

            base.Reset();
        }

        public override void Stop(bool forceEnd)
        {
            if (_isFinished) return;

            _actions[_currentIndex].Stop(forceEnd);

            if (forceEnd)
            {
                for (_currentIndex++; _currentIndex < _actionCount; _currentIndex++)
                {
                    _actions[_currentIndex].Stop(true);
                }
            }

            _isFinished = true;
        }

        public override bool Update(float deltaTime)
        {
            if (_isFinished) return true;

            var action = _actions[_currentIndex];
            action.Update(deltaTime);

            if (action.Finished)
            {
                do
                {
                    _currentIndex++;
                    if (_currentIndex < _actionCount)
                    {
                        action = _actions[_currentIndex];
                        action.Play();
                    }
                    else
                    {
                        _isFinished = true;
                        break;
                    }
                }
                while (action.Finished);
            }

            return _isFinished;
        }

        public static ActionX Create(float delay, params ActionX[] actions)
        {
            if (delay > 0)
            {
                int count = actions.Length;
                var actions2 = new ActionX[count + 1];
                actions2[0] = Delay.Create(delay);
                for (int i = 0; i < count; i++)
                {
                    actions2[i + 1] = actions[i];
                }
                return Create(actions2);
            }

            return Create(actions);
        }

        public static ActionX Create(params ActionX[] actions)
        {
            int count = actions.Length;
            if (count == 1) return actions[0];
            if (count == 2) return Sequence2.Create(actions[0], actions[1]);

            var action = new Sequence();
            action.Construct(actions);

            return action;
        }

        /// <summary>
        /// Check null action.
        /// </summary>
        public static ActionX SafeCreate(params ActionX[] actions)
        {
            // Check null action
            int count = actions.Length;
            for (int i = 0; i < count; i++)
            {
                if (actions[i] == null)
                {
                    //Log.Warning($"actions[{i}] is NULL!");
                    var list = new List<ActionX>(count - 1);
                    for (int j = 0; j < i; j++)
                    {
                        list.Add(actions[j]);
                    }

                    for (int j = i + 1; j < count; j++)
                    {
                        var action = actions[j];
                        if (action != null)
                        {
                            list.Add(action);
                        }
                    }

                    return list.Count > 0 ? Create(list.ToArray()) : null;
                }
            }

            return Create(actions);
        }

        public static ActionX Create(List<ActionX> actions)
        {
            return Create(actions.ToArray());
        }

        public static ActionX Create(ActionX action1, ActionX action2)
        {
            return Sequence2.Create(action1, action2);
        }

        public static ActionX Create(float delay, Callback callback)
        {
            return Create(Delay.Create(delay), CallFunc.Create(callback));
        }

        public static ActionX Create<T>(float delay, Callback<T> callback, T value)
        {
            return Create(Delay.Create(delay), CallFunc.Create(() => callback?.Invoke(value)));
        }

        public static ActionX Create(float delay, ActionX action)
        {
            return Create(Delay.Create(delay), action);
        }

        public static ActionX Create(ActionX action, Callback callback)
        {
            return callback != null ? Create(action, CallFunc.Create(callback)) : action;
        }

        public static ActionX Create(ActionX action1, ActionX action2, Callback callback)
        {
            return callback != null ? Create(action1, action2, CallFunc.Create(callback)) : Create(action1, action2);
        }

        public static ActionX Create(ActionX action1, ActionX action2, ActionX action3, Callback callback)
        {
            return callback != null ? Create(action1, action2, action3, CallFunc.Create(callback)) : Create(action1, action2, action3);
        }
    }
}
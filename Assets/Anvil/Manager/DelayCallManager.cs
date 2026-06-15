using UnityEngine;
using System;

namespace Anvil
{
    public class DelayCallController
    {
        private float _delay;
        private Action _callback;
        private int _repeat = 1; // repeat < 0 => loop
        private float _time;

        public GameObject gameObject { get; set; }

        public void Construct(float delay, Action callback, int repeat)
        {
            _delay = delay;
            _callback = callback;
            _repeat = repeat;

            _time = delay;
        }

        public void Stop()
        {
            _time = 0;
        }

        /// <summary>
        /// Returns true if finished.
        /// </summary>
        public bool Update(float deltaTime)
        {
            if (_time > 0)
            {
                _time -= deltaTime;
                if (_time <= 0)
                {
                    bool isFinished = true;

                    // Check loop
                    if (_repeat < 0)
                    {
                        _time = _delay;
                        isFinished = false;
                    }
                    else if (_repeat > 0)
                    {
                        _repeat--;
                        if (_repeat > 0)
                        {
                            _time = _delay;
                            isFinished = false;
                        }
                    }

                    _callback?.Invoke();

                    return isFinished;
                }

                return false;
            }

            return true;
        }
    }

    public class DelayCallManager
    {
        private static readonly float Epsilon = 0.001f;

        private PoolList<DelayCallController> _list = new PoolList<DelayCallController>();

        public DelayCallController DelayCall(Component component, float delay, Action callback, int repeat = 1)
        {
            var controller = DelayCall(delay, callback, repeat);
            if (controller != null)
            {
                controller.gameObject = component != null ? component.gameObject : null;
            }
            return controller;
        }

        public DelayCallController DelayCall(GameObject gameObject, float delay, Action callback, int repeat = 1)
        {
            var controller = DelayCall(delay, callback, repeat);
            if (controller != null)
            {
                controller.gameObject = gameObject;
            }
            return controller;
        }

        public DelayCallController DelayCall(float delay, Action callback, int repeat = 1)
        {
            if (delay > 0)
            {
                var node = _list.Get();
                var controller = node.Value;
                controller.Construct(delay, callback, repeat);
                _list.Add(node);

                return controller;
            }

            callback?.Invoke();
            return null;
        }

        public DelayCallController DelayCall<T>(float delay, Action<T> callback, T value, int repeat = 1)
        {
            if (delay > 0)
            {
                var node = _list.Get();
                var controller = node.Value;
                controller.Construct(delay, () => callback?.Invoke(value), repeat);
                _list.Add(node);

                return controller;
            }

            callback?.Invoke(value);
            return null;
        }

        public DelayCallController CallOnUpdate(Action callback)
        {
            return DelayCall(Epsilon, callback);
        }

        public DelayCallController CallOnUpdate<T>(Action<T> callback, T value)
        {
            return DelayCall(Epsilon, callback, value);
        }

        public void StopDelayCalls(Component component)
        {
            if (component != null)
            {
                StopDelayCalls(component.gameObject);
            }
        }

        public void StopDelayCalls(GameObject gameObject)
        {
            if (gameObject != null)
            {
                _list.Browse(controller =>
                {
                    if (controller.gameObject == gameObject)
                    {
                        controller.Stop();
                    }
                });
            }
        }

        public void OnUpdate()
        {
            if (!_list.IsEmpty())
            {
                float deltaTime = Time.deltaTime;
                _list.Browse(controller =>
                {
                    if (controller.Update(deltaTime))
                    {
                        controller.gameObject = null;
                        return true;
                    }
                    return false;
                });
            }
        }

        public void Clear()
        {
            _list.Clear();
        }
    }
}
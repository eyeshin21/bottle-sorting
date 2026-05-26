using UnityEngine;
using System;

namespace Anvil.Legacy.Actions
{
    public class Rotate2 : ActionX
    {
        private const bool DefaultLocal = true;

        private Transform _transform;
        private Action<float> _updateCallback;
        private FloatController _controller;
        private bool _isLocal;

        public override float Duration => _controller.Duration;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _transform = value.transform;
            }
        }

        public void Construct(GameObject go, Vector3ComponentType componentType, FloatController controller, bool isLocal)
        {
            _Construct();

            _target = go;
            _transform = go.transform;

            if (componentType == Vector3ComponentType.X)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        if (_isLocal)
                        {
                            var angles = _transform.localEulerAngles;
                            angles.x = value;
                            _transform.localEulerAngles = angles;
                        }
                        else
                        {
                            var angles = _transform.eulerAngles;
                            angles.x = value;
                            _transform.eulerAngles = angles;
                        }
                    }
                };
            }
            else if (componentType == Vector3ComponentType.Y)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        if (_isLocal)
                        {
                            var angles = _transform.localEulerAngles;
                            angles.y = value;
                            _transform.localEulerAngles = angles;
                        }
                        else
                        {
                            var angles = _transform.eulerAngles;
                            angles.y = value;
                            _transform.eulerAngles = angles;
                        }
                    }
                };
            }
            else if (componentType == Vector3ComponentType.Z)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        if (_isLocal)
                        {
                            var angles = _transform.localEulerAngles;
                            angles.z = value;
                            _transform.localEulerAngles = angles;
                        }
                        else
                        {
                            var angles = _transform.eulerAngles;
                            angles.z = value;
                            _transform.eulerAngles = angles;
                        }
                    }
                };
            }
            else if (componentType == Vector3ComponentType.XY)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        if (_isLocal)
                        {
                            var angles = _transform.localEulerAngles;
                            angles.x = angles.y = value;
                            _transform.localEulerAngles = angles;
                        }
                        else
                        {
                            var angles = _transform.eulerAngles;
                            angles.x = angles.y = value;
                            _transform.eulerAngles = angles;
                        }
                    }
                };
            }
            else if (componentType == Vector3ComponentType.XZ)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        if (_isLocal)
                        {
                            var angles = _transform.localEulerAngles;
                            angles.x = angles.z = value;
                            _transform.localEulerAngles = angles;
                        }
                        else
                        {
                            var angles = _transform.eulerAngles;
                            angles.x = angles.z = value;
                            _transform.eulerAngles = angles;
                        }
                    }
                };
            }
            else if (componentType == Vector3ComponentType.YZ)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        if (_isLocal)
                        {
                            var angles = _transform.localEulerAngles;
                            angles.y = angles.z = value;
                            _transform.localEulerAngles = angles;
                        }
                        else
                        {
                            var angles = _transform.eulerAngles;
                            angles.y = angles.z = value;
                            _transform.eulerAngles = angles;
                        }
                    }
                };
            }
            else
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        if (_isLocal)
                        {
                            _transform.localEulerAngles = new Vector3(value, value, value);
                        }
                        else
                        {
                            _transform.eulerAngles = new Vector3(value, value, value);
                        }
                    }
                };
            }

            _controller = controller;
            _isLocal = isLocal;
        }

        public override void Reset()
        {
            base.Reset();
            _controller.Reset();
        }

        protected override bool OnPlay()
        {
            _updateCallback(_controller.Value);
            return _controller.Finished;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                if (_controller.GetEnd(out float endAngle))
                {
                    _updateCallback(endAngle);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            float angle = _controller.Update(deltaTime);
            _updateCallback(angle);
            return _controller.Finished;
        }

        public static Rotate2 Create(GameObject go, Vector3ComponentType componentType, FloatController controller, bool isLocal = DefaultLocal)
        {
            var action = new Rotate2();
            action.Construct(go, componentType, controller, isLocal);

            return action;
        }
    }
}
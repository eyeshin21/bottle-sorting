using UnityEngine;
using System;

namespace Anvil.Legacy.Actions
{
    public class Scale2 : ActionX
    {
        private Transform _transform;
        private Action<float> _updateCallback;
        private FloatController _controller;

        public override float Duration => _controller.Duration;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _transform = value.transform;
            }
        }

        public void Construct(GameObject go, Vector3ComponentType componentType, FloatController controller)
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
                        var scale = _transform.localScale;
                        scale.x = value;
                        _transform.localScale = scale;
                    }
                };
            }
            else if (componentType == Vector3ComponentType.Y)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        var scale = _transform.localScale;
                        scale.y = value;
                        _transform.localScale = scale;
                    }
                };
            }
            else if (componentType == Vector3ComponentType.Z)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        var scale = _transform.localScale;
                        scale.z = value;
                        _transform.localScale = scale;
                    }
                };
            }
            else if (componentType == Vector3ComponentType.XY)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        var scale = _transform.localScale;
                        scale.x = scale.y = value;
                        _transform.localScale = scale;
                    }
                };
            }
            else if (componentType == Vector3ComponentType.XZ)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        var scale = _transform.localScale;
                        scale.x = scale.z = value;
                        _transform.localScale = scale;
                    }
                };
            }
            else if (componentType == Vector3ComponentType.YZ)
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        var scale = _transform.localScale;
                        scale.y = scale.z = value;
                        _transform.localScale = scale;
                    }
                };
            }
            else
            {
                _updateCallback = value =>
                {
                    if (_transform != null)
                    {
                        _transform.localScale = new Vector3(value, value, value);
                    }
                };
            }

            _controller = controller;
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
                if (_controller.GetEnd(out float scale))
                {
                    _updateCallback(scale);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            float scale = _controller.Update(deltaTime);
            _updateCallback(scale);
            return _controller.Finished;
        }

        public static Scale2 Create(GameObject go, Vector3ComponentType componentType, FloatController controller)
        {
            var action = new Scale2();
            action.Construct(go, componentType, controller);
            return action;
        }
    }
}
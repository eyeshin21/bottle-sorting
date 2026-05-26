using UnityEngine;

namespace Anvil.Legacy
{
    public enum Dimension
    {
        X,
        Y,
        Z
    }
}

namespace Anvil.Legacy.Actions
{
    public class Move1D : ActionX
    {
        private Transform _transform;
        private Dimension _dimension;
        private FloatController _controller;
        private bool _local;

        public override GameObject Target
        {
            set
            {
                _target = value;
                _transform = value.transform;
            }
        }

        public override float Duration => _controller.Duration;

        public void Construct(GameObject go, Dimension dimension, FloatController controller, bool local)
        {
            _Construct();

            _target = go;
            _transform = go.transform;
            _dimension = dimension;
            _controller = controller;
            _local = local;
        }

        public override void Reset()
        {
            base.Reset();
            _controller.Reset();
        }

        protected override bool OnPlay()
        {
            SetPosition(_controller.Value);
            return _controller.Finished;
        }

        protected override void OnStop(bool forceEnd)
        {
            if (forceEnd)
            {
                if (_controller.GetEnd(out float end))
                {
                    SetPosition(end);
                }
            }
        }

        protected override bool OnUpdate(float deltaTime)
        {
            SetPosition(_controller.Update(deltaTime));
            return _controller.Finished;
        }

        void SetPosition(float value)
        {
            var position = _local ? _transform.localPosition : _transform.position;
            if (_dimension == Dimension.X)
            {
                position.x = value;
            }
            else if (_dimension == Dimension.Y)
            {
                position.y = value;
            }
            else
            {
                position.z = value;
            }

            if (_local)
            {
                _transform.localPosition = position;
            }
            else
            {
                _transform.position = position;
            }
        }

        public static Move1D Create(GameObject go, Dimension dimension, FloatController controller, bool local = false)
        {
            var action = new Move1D();
            action.Construct(go, dimension, controller, local);

            return action;
        }

        public static Move1D CreateMoveX(GameObject go, FloatController controller, bool local = false)
        {
            return Create(go, Dimension.X, controller, local);
        }

        public static Move1D CreateMoveY(GameObject go, FloatController controller, bool local = false)
        {
            return Create(go, Dimension.Y, controller, local);
        }
    }
}

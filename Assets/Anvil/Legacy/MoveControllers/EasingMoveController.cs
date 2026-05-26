namespace Anvil.Legacy
{
    public class EasingMoveController : LerpMoveController
    {
        private Easer _easer;

        public Easer Easer
        {
            get => _easer;
            set => _easer = value;
        }

        public EaseType EaseType
        {
            get => Easers.GetEaseType(_easer);
            set => _easer = Easers.GetEaser(value);
        }

        protected override float GetLerp(float t)
        {
            return _easer != null ? _easer(t) : t;
        }

        static Pool<EasingMoveController> _pool;
        static Pool<EasingMoveController> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new Pool<EasingMoveController>();
                }
                return _pool;
            }
        }

        public static EasingMoveController Create()
        {
            return Pool.Get();
        }

        public override void UpdateConfig(MoveConfig moveConfig)
        {
            base.UpdateConfig(moveConfig);
            EaseType = moveConfig.EaseType;
        }

        public override void ReturnPool()
        {
            base.ReturnPool();
            Pool.Return(this);
        }
    }
}
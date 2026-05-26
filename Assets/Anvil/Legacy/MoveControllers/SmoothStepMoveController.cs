namespace Anvil.Legacy
{
    public class SmoothStepMoveController : LerpMoveController
    {
        protected override float GetLerp(float t)
        {
            return t * t * (3 - 2 * t);
        }

        static Pool<SmoothStepMoveController> _pool;
        static Pool<SmoothStepMoveController> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new Pool<SmoothStepMoveController>();
                }
                return _pool;
            }
        }

        public static SmoothStepMoveController Create()
        {
            return Pool.Get();
        }

        public override void ReturnPool()
        {
            base.ReturnPool();
            Pool.Return(this);
        }
    }
}
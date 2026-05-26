namespace Anvil.Legacy
{
    public class LinearMoveController : LerpMoveController
    {
        //protected override float GetLerp(float t)
        //{
        //    return t;
        //}

        static Pool<LinearMoveController> _pool;
        static Pool<LinearMoveController> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new Pool<LinearMoveController>();
                }
                return _pool;
            }
        }

        public static LinearMoveController Create()
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
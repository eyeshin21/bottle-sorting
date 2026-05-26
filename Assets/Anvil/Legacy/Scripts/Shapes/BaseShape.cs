using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class BaseShape : IShape
    {
        protected Vector3 _localPos;

        public virtual Vector3 LocalPosition
        {
            get => _localPos;
            set => _localPos = value;
        }

        public abstract bool Contains(Vector3 pos);
        public abstract void ReturnToPool();

#if DEBUG_MODE
        public abstract void DrawGizmos(Vector3 pos, Color? color);
#endif
    }
}
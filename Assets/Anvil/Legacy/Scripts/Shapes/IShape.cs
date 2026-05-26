using UnityEngine;

namespace Anvil.Legacy
{
    public interface IShape //: ILayout
    {
        Vector3 LocalPosition { get; set; }
        bool Contains(Vector3 pos);
        void ReturnToPool();

#if DEBUG_MODE
        void DrawGizmos(Vector3 pos, Color? color);
#endif
    }
}
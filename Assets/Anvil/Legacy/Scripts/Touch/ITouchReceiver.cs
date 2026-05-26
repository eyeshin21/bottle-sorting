using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITouchReceiver
    {
        void GetSize(out float width, out float height);
        void SetScale(float scale);
        bool Contains(Vector3 pos);
        void ReturnToPool();

#if DEBUG_MODE
        void DrawGizmos(Vector3 pos, Color color);
#endif
    }
}
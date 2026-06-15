using UnityEngine;

namespace Anvil
{
    public interface ITouchHandler
    {
        /// <summary>
        /// Returns true if control touch.
        /// </summary>
        bool OnTouchPressed(Vector3 pos);

        /// <summary>
        /// Returns true if control touch.
        /// </summary>
        bool OnTouchMoved(Vector3 pos);

        void OnTouchReleased(Vector3 pos);

#if DEBUG_MODE
        string DebugName { get; }
#endif
    }
}
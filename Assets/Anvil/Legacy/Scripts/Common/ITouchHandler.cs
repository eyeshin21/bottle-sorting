using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITouchHandler
    {
        /// <summary>
        /// Returns true if control touch.
        /// </summary>
        bool OnTouchPressed(Vector2 pos);

        /// <summary>
        /// Returns true if control touch.
        /// </summary>
        bool OnTouchMoved(Vector2 pos);

        void OnTouchReleased(Vector2 pos);
    }
}
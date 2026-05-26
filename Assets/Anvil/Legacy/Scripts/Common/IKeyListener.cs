using UnityEngine;

namespace Anvil.Legacy
{
    public interface IKeyListener
    {
        /// <summary>
        /// Returns true if control key.
        /// </summary>
        bool OnKeyPressed(KeyCode keyCode);

        /// <summary>
        /// Returns true if control key.
        /// </summary>
        bool UpdateKey();
    }
}
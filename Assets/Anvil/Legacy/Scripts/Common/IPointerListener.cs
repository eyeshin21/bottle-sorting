using UnityEngine;

namespace Anvil.Legacy
{
    public interface IPointerListener
    {
        /// <summary>
        /// Returns true if control pointer.
        /// </summary>
        bool OnPointerDown();
        void OnPointerClick();
        /// <summary>
        /// Returns true if control pointer.
        /// </summary>
        bool OnPointerExit();
        void OnPointerEnter();
    }
}
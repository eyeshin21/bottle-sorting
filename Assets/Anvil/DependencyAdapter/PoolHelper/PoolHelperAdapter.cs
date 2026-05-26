#undef GAMETAMIN_CORE

using UnityEngine;

namespace Anvil
{
    public class PoolHelperAdapter
    {
        private static Transform _parentPopup;

        public static Transform ParentPopup
        {
            get
            {
                if (_parentPopup == null)
                {
                    // Canvas canvas = Object.FindObjectOfType<Canvas>();
                    Canvas canvas = UICanvasTag.CanvasInstance;
                    if (canvas != null)
                    {
                        _parentPopup = canvas.transform;
                    }
                }

                return _parentPopup;
            }
        }
    }
}
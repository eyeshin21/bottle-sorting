using UnityEngine;

namespace Anvil.Legacy
{
    public partial class Manager
    {
#if DEBUG_MODE
        public static bool DebugEnabled { get; set; } = true;

        private static bool InputEnabled
        {
            get => true;
            set
            {
                Debug.Log($"InputEnabled {value}");
            }
        }
#endif
    }
}
#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public interface IDebugGUIController
    {
        //void SetLabel(string label);
        bool Contains(string search);
        void OnGUI();
    }
}
#endif
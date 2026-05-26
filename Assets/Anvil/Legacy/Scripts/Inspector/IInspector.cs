#if UNITY_EDITOR
using UnityEngine;

namespace Anvil.Legacy
{
    public interface IInspector
    {
        void OnInspectorGUI();
    }
}
#endif
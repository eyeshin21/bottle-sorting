#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public partial class AssetsWindow : WindowBehaviour<AssetsWindow>
    {
        [MenuItem("Window/Assets")]
        static void _Show()
        {
            ShowWindow();
        }

        void OnGUI()
        {
            OnGUISpriteAtlas();
        }
    }
}
#endif
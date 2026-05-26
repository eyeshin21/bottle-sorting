#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public class ColorsWindow : WindowBehaviour<ColorsWindow>
    {
        [MenuItem("Window/Colors")]
        static void _Show()
        {
            ShowWindow("Colors");
        }

        void OnGUI()
        {
            Colors.OnGUIColors();
        }
    }
}
#endif
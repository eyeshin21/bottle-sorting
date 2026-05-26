#if UNITY_EDITOR && DEBUG_MODE
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public class TestWindow : WindowBehaviour<TestWindow>
    {
        [MenuItem("Window/Test")]
        static void _Show()
        {
            ShowWindow();
        }

        TestGUIController.Test _testGUIController = new();

        void OnGUI()
        {
            TestGUIController();
        }

        void TestGUIController()
        {
            _testGUIController.OnGUI();
        }
    }
}
#endif
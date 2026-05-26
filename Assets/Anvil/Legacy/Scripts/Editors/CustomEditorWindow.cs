#if UNITY_EDITOR
using UnityEngine;

namespace Anvil.Legacy
{
    public class CustomEditorWindow : WindowBehaviour<CustomEditorWindow>
    {
        Callback _guiCallback;

        public static void Show(string title, Callback guiCallback)
        {
            var window = EditorHelper.ShowEditorWindow<CustomEditorWindow>(title);
            window._guiCallback = guiCallback;
        }

        void OnGUI()
        {
            _guiCallback?.Invoke();
        }
    }
}
#endif
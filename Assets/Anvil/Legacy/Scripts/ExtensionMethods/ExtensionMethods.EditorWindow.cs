#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static float GetWidth(this EditorWindow editorWindow)
        {
            return editorWindow.position.width;
        }

        public static Vector2 GetSize(this EditorWindow editorWindow)
        {
            return editorWindow.position.size;
        }

        public static void GetSize(this EditorWindow editorWindow, out float width, out float height)
        {
            var size = editorWindow.position.size;
            width = size.x;
            height = size.y;
        }
    }
}
#endif
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class EditorHelper
    {
#if UNITY_EDITOR
        /// <summary>
        /// Returns "Assets/..."
        /// </summary>
        public static bool GetProjectWindowSelectedPath(out string path)
        {
            var assetGUIDs = Selection.assetGUIDs;
            int count = assetGUIDs.GetLength();
            if (count > 0)
            {
                path = AssetDatabase.GUIDToAssetPath(assetGUIDs[0]);
                return true;
            }
            path = default;
            return false;
        }

        public static void SelectObject<T>(T obj) where T : Object
        {
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }
#endif
    }
}
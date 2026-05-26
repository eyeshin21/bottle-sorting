#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        static void SelectFolder(string path)
        {
            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (obj != null)
            {
                EditorUtility.FocusProjectWindow();
                var pt = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
                var ins = pt.GetField("s_LastInteractedProjectBrowser", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
                var showDirMeth = pt.GetMethod("ShowFolderContents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                showDirMeth.Invoke(ins, new object[] { obj.GetInstanceID(), true });
            }
            else
            {
                LegacyLog.Warning($"Can't load folder \"{path}\"!");
            }
        }

        static void SelectObject(string path)
        {
            SelectObject<Object>(path);
        }

        static void SelectObject<T>(string path) where T : Object
        {
            if (!path.EndsWith(".asset"))
            {
                path = $"{path}.asset";
            }

            var obj = AssetDatabase.LoadAssetAtPath<T>(path);
            if (obj != null)
            {
                EditorHelper.SelectObject(obj);
            }
            else
            {
                LegacyLog.Warning($"Can't load {typeof(T)} at path \"{path}\"!");
            }
        }

        static void SelectObject<T>() where T : Object
        {
            var assets = EditorHelper.LoadAssets<T>();
            if (!assets.IsNullOrEmpty())
            {
                EditorHelper.SelectObject(assets[0]);
            }
            else
            {
                LegacyLog.Warning($"Can't select object {typeof(T)}!");
            }
        }
    }
}
#endif
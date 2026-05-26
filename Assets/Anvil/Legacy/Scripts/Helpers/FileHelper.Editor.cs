#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class FileHelper
    {
        /// <summary>
        /// Displays the "open file" dialog and returns the selected path name.
        /// </summary>
        public static string OpenFilePanel(string title, string directory, string extension)
        {
            return EditorUtility.OpenFilePanel(title, directory, extension);
        }

        /// <summary>
        /// Displays the "save file" dialog and returns the selected path name.
        /// </summary>
        public static string SaveFilePanel(string title, string directory, string defaultName, string extension)
        {
            return EditorUtility.SaveFilePanel(title, directory, defaultName, extension);
        }

        public static void LoadTextFromFilePanel(string title, ref string lastPath, string extension, Callback<string> callback)
        {
            var path = OpenFilePanel(title, lastPath, extension);
            if (!path.IsNullOrEmpty())
            {
                lastPath = GetFolderPath(path);
                var text = LoadText(path, true);
                callback?.Invoke(text);
            }
        }
    }
}
#endif
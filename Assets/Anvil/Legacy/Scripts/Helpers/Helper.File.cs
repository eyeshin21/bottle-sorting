using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        /// <summary>
        /// Returns "{path}/{fileName}"
        /// </summary>
        public static string GetPath(string path, string fileName)
        {
            return $"{path}/{fileName}";
        }

        /// <summary>
        /// Returns "{path}/{fileName}" or "fileName"
        /// </summary>
        public static string CheckGetPath(string path, string fileName)
        {
            return string.IsNullOrWhiteSpace(path) ? fileName : $"{path}/{fileName}";
        }

        /// <summary>
        /// Returns "{path}/{fileName}.bytes"
        /// </summary>
        public static string GetPathForBinary(string path, string fileName)
        {
            return $"{path}/{fileName}.bytes";
        }

        /// <summary>
        /// Returns "{Application.dataPath}/{folderName}/Resources"
        /// </summary>
        public static string GetResourcePath(string folderName)
        {
            return $"{Application.dataPath}/{folderName}/Resources";
        }
    }
}
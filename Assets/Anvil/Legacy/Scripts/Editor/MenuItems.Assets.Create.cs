#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        const string AssetsPath = "Assets/Gametamin/";

        [MenuItem(AssetsPath + "Create Folders", false, 0)]
        static void CreateFolders()
        {
            var assetPath = SelectedAssetPath;
            if (!assetPath.Contains('.'))
            {
                var path = FileHelper.GetAbsolutePath(assetPath);
                FileHelper.CheckCreateFolder(path, "Prefabs", true);
                FileHelper.CheckCreateFolder(path, "Resources", true);
                FileHelper.CheckCreateFolder(path, "Scripts", true);
                FileHelper.CheckCreateFolder(path, "Scenes", true);
                FileHelper.CheckCreateFolder(path, "Textures", true);
                RefreshAssets();
            }
        }
    }
}
#endif
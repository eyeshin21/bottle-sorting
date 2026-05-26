#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class EditorHelper
    {
        public static List<T> LoadAssets<T>(string path, bool absolutePath = false) where T : Object
        {
            if (!absolutePath)
            {
                if (path.StartsWith("Assets/"))
                {
                    path = path.Substring(7);
                }
                path = $"{Application.dataPath}/{path}";
            }

            var files = Directory.GetFiles(path);
            if (files != null)
            {
                var assets = new List<T>();
                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i];
                    if (file.EndsWith(".DS_Store")) continue;
                    if (file.EndsWith(".meta")) continue;

                    int index = file.LastIndexOf("Assets/");
                    if (index > 0)
                    {
                        file = file.Substring(index);
                    }

                    T asset = null;
                    asset = AssetDatabase.LoadAssetAtPath<T>(file);
                    if (asset != null)
                    {
                        assets.Add(asset);
                    }
                    else
                    {
                        LegacyLog.Warning($"Can't load {typeof(T)} from \"{file}\"!");
                    }
                }
                return assets;
            }
            return null;
        }

        public static T LoadAsset<T>(string assetPath) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        /// <summary>
        /// Loads assets from files and folders ("Assets/...").
        /// </summary>
        public static List<T> LoadAssets<T>(string[] assetPaths) where T : Object
        {
            var type = typeof(T);
            var assets = new List<T>();
            var paths = FileHelper.GetAllAssetPaths(assetPaths);
            foreach (var path in paths)
            {
                //Log.Debug(path);
                T asset = null;
                asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                {
                    assets.Add(asset);
                }
                else
                {
                    //Log.Warning($"Can't load {type} from \"{path}\"!");
                }
            }
            return assets;
        }

        public static List<T> LoadAssets<T>() where T : Object
        {
            var assets = new List<T>();
            var type = typeof(T);
            var filter = type.ToString();
            var guids = AssetDatabase.FindAssets($"t:{filter}");
            if (guids.IsNullOrEmpty())
            {
                LegacyLog.Warning($"Can't load assets of type {type}!");
            }
            else
            {
                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    if (asset != null)
                    {
                        assets.Add(asset);
                    }
                    else
                    {
                        LegacyLog.Warning($"Can't load asset of type {type} at \"{assetPath}\"!");
                    }
                }
            }
            return assets;
        }
    }
}
#endif
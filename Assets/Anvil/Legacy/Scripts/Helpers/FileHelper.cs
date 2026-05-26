using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class FileHelper
    {
        static readonly string Assets = "Assets";

        public static readonly char PathSeparator = Constants.PathSeparator;
        public static readonly char DirectorySeparatorChar = Constants.PathSeparator;

        public static string AssetsPath => Application.dataPath;

        /// <summary>
        /// Returns project's path.
        /// </summary>
        public static string RootPath
        {
            get
            {
                var path = Application.dataPath;
                int index = path.LastIndexOf(PathSeparator);
                if (index > 0)
                {
                    // Remove "/Assets"
                    return path.GetSubstring(0, index - 1);
                }
                return path;
            }
        }

        public static string GetAbsolutePath(string path)
        {
            if (path.StartsWith(PathSeparator))
            {
                //Log.Warning($"Remove '{PathSeparator}' from \"{path}\"");
                path = path.Substring(1);
            }
#if UNITY_EDITOR || UNITY_STANDALONE
            var dataPath = Application.dataPath;
            if (path.StartsWith(Assets))
            {
                return path.Length == 6 ? dataPath : $"{dataPath}{path.Substring(6)}";
            }
            return $"{dataPath}/{path}";
#else
            return $"{Application.persistentDataPath}/{path}";
#endif
        }

        /// <summary>
        /// Returns project's path + fileName
        /// </summary>
        public static string GetRootPath(string fileName)
        {
            return $"{RootPath}{PathSeparator}{fileName}";
        }

        public static string GetPath(string path, string fileName)
        {
            return $"{path}{PathSeparator}{fileName}";
        }

        /// <summary>
        /// Folder1/Folder2/FileName.xxx => Folder1/Folder2
        /// </summary>
        public static string GetFolderPath(string path)
        {
            if (!path.IsNullOrEmpty())
            {
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    if (path[i] == PathSeparator)
                    {
                        return path.Substring(0, i);
                    }
                }
            }
            return path;
        }

        /// <summary>
        /// Returns ".xxx"
        /// </summary>
        public static string GetFileExtension(string fileName)
        {
            if (!fileName.IsNullOrEmpty())
            {
                for (int i = fileName.Length - 1; i >= 0; i--)
                {
                    if (fileName[i] == '.')
                    {
                        return fileName.Substring(i);
                    }
                }
            }
            return default;
        }

        static void SetAbsolutePath(ref string path, bool isAbsolutePath)
        {
            if (!isAbsolutePath)
            {
                path = GetAbsolutePath(path);
            }
        }

        static void BeginSave(ref string path, bool isAbsolutePath, bool createDirectoryIfNotExists)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (createDirectoryIfNotExists)
            {
                var path2 = Path.GetDirectoryName(path);
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }
            }
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        static void EndSave(string path, bool isOk)
        {
#if UNITY_EDITOR
            if (isOk)
            {
                //if (!Application.isPlaying)
                {
                    try
                    {
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        LegacyLog.Warning($"Save asset {path}: {e}");
                    }
                }
            }
#endif
        }

        public static string LoadText(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);

            string text = "";
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                using (var reader = fileInfo.OpenText())
                {
                    text = reader.ReadToEnd();
                    reader.Close();
                }
            }
            else
            {
                LegacyLog.Warning($"Can't load text: \"{path}\" not found!");
            }

            return text;
        }

        public static bool SaveText(string text, string path, bool isAbsolutePath = false, bool createDirectoryIfNotExists = true)
        {
            BeginSave(ref path, isAbsolutePath, createDirectoryIfNotExists);
            bool isOk = false;
            try
            {
                var writer = new StreamWriter(path);
                writer.Write(text);
                writer.Close();
                isOk = true;
            }
            catch (Exception e)
            {
                LegacyLog.Warning($"Can't save text: {e.Message}");
            }
            EndSave(path, isOk);

            return isOk;
        }

        public static T LoadBinary<T>(string path, bool isAbsolutePath = false) where T : class
        {
            Assert.EndsWith(path, ".data");
            SetAbsolutePath(ref path, isAbsolutePath);

            T data = null;
            if (File.Exists(path))
            {
                var stream = File.Open(path, FileMode.Open);
                try
                {
                    var formatter = new BinaryFormatter();
                    data = formatter.Deserialize(stream) as T;
                }
                catch (Exception e)
                {
                    LegacyLog.Warning($"Can't load binary: {e.Message}");
                }
                finally
                {
                    stream.Close();
                }
            }
            //else
            //{
            //   LegacyLog.Warning($"Can't load binary: \"{path}\" not found!");
            //}

            return data;
        }

        public static bool SaveBinary<T>(T data, string path, bool isAbsolutePath = false, bool createDirectoryIfNotExists = true) where T : class
        {
            //Assert.EndsWith(path, ".data");
            BeginSave(ref path, isAbsolutePath, createDirectoryIfNotExists);
            bool isOk = false;
            var stream = File.Create(path);
            if (stream != null)
            {
                try
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, data);
                    isOk = true;
                }
                catch (Exception e)
                {
                    LegacyLog.Warning($"Can't save binary: {e.Message}");
                }
                finally
                {
                    stream.Close();
                }
            }
            else
            {
                LegacyLog.Warning($"Can't save binary to \"{path}\"!");
            }
            EndSave(path, isOk);

            return isOk;
        }

        /// <summary>
        /// Resources/{path}.bytes
        /// </summary>
        public static T LoadResourceBinary<T>(string path) where T : class
        {
            Assert.NotContains(path, "Resources");
            Assert.NotContains(path, '.');
            var ta = Resources.Load<TextAsset>(path);
            if (ta != null)
            {
                var formatter = new BinaryFormatter();
                var stream = new MemoryStream(ta.bytes);
                if (stream.Length > 0)
                {
                    T data = formatter.Deserialize(stream) as T;
                    stream.Close();
                    return data;
                }
                stream.Close();
            }
            //else
            //{
            //   LegacyLog.Warning($"Can't load resource binary: \"{path}\" not found!");
            //}

            return default;
        }

        /// <summary>
        /// path format: ".../Resources/.../filename.bytes"
        /// </summary>
        public static bool SaveResourceBinary<T>(T data, string path) where T : class
        {
            Assert.Contains(path, "Resources");
            Assert.EndsWith(path, ".bytes");
            BeginSave(ref path, true, true);
            //Log.Debug($"Save resource binary to \"{path}\"");
            bool isOk = false;
            var stream = File.Create(path);
            if (stream != null)
            {
                try
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, data);
                    isOk = true;
                }
                catch (Exception e)
                {
                    LegacyLog.Warning($"Can't save resource binary: {e.Message}");
                }
                finally
                {
                    stream.Close();
                }
            }
            else
            {
                LegacyLog.Warning($"Can't save resource binary to \"{path}\"!");
            }
            EndSave(path, isOk);

            return isOk;
        }

        public static bool ExistsFile(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            return File.Exists(path);
        }

        public static bool ExistsFolder(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            return Directory.Exists(path);
        }

        /// <summary>
        /// Creates folder if not exist.
        /// </summary>
        public static string GetOrCreateAbsolutePath(string subPath)
        {
            var path = GetAbsolutePath(subPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static DirectoryInfo CreateFolder(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            return Directory.CreateDirectory(path);
        }

        public static void CheckCreateFolder(string path, string folderName, bool absolutePath = false)
        {
            path = Path.Combine(path, folderName);
            SetAbsolutePath(ref path, absolutePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static bool DeleteFile(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (File.Exists(path))
            {
                File.Delete(path);
#if UNITY_EDITOR
                File.Delete(path + ".meta");
#endif
                return true;
            }
            return false;
        }

        public static bool DeleteDirectory(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
#if UNITY_EDITOR
                File.Delete(path + ".meta");
#endif
                return true;
            }
            return false;
        }

        public static string[] GetFolderNames(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (Directory.Exists(path))
            {
                var folders = Directory.GetDirectories(path);
                if (folders != null)
                {
                    for (int i = 0; i < folders.Length; i++)
                    {
                        var folder = folders[i];
                        int index = folder.LastIndexOf(DirectorySeparatorChar);
                        if (index > 0)
                        {
                            folders[i] = folder.Substring(index + 1);
                        }
                    }
                }

                return folders;
            }

            return null;
        }

        public static int GetFileCount(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                if (files != null)
                {
                    int fileCount = 0;
                    for (int i = 0; i < files.Length; i++)
                    {
                        var file = files[i];
                        if (file.EndsWith(".DS_Store")) continue;
                        if (file.EndsWith(".meta")) continue;

                        int index = file.LastIndexOf(DirectorySeparatorChar);
                        if (index > 0)
                        {
                            fileCount++;
                        }
                    }

                    return fileCount;
                }
            }

            return 0;
        }

        public static List<string> GetFilenames(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                if (files != null)
                {
                    var filenames = new List<string>();
                    for (int i = 0; i < files.Length; i++)
                    {
                        var file = files[i];
                        if (file.EndsWith(".DS_Store")) continue;
                        if (file.EndsWith(".meta")) continue;

                        int index = file.LastIndexOf(DirectorySeparatorChar);
                        if (index > 0)
                        {
                            filenames.Add(file.Substring(index + 1));
                        }
                    }

                    return filenames;
                }
            }

            return null;
        }

        /// <summary>
        /// "Assets/..."
        /// </summary>
        public static List<string> GetAllAssetPaths(string[] assetPaths)
        {
            var paths = new List<string>();
            foreach (var assetPath in assetPaths)
            {
                Assert.IsTrue(assetPath.StartsWith(Assets));
                // File
                if (assetPath.Contains('.'))
                {
                    paths.Add(assetPath);
                }
                // Folder
                else
                {
                    AddAssetPaths(assetPath, paths);
                }
            }
            return paths;
        }

        static void AddAssetPaths(string assetFolderPath, List<string> paths, bool absolutePath = false)
        {
            var path = absolutePath ? assetFolderPath : GetAbsolutePath(assetFolderPath);
            if (Directory.Exists(path))
            {
                // Files
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    //Log.Debug(file);
                    if (file.EndsWith(".meta")) continue;

                    int index = file.LastIndexOf("/Assets/");
                    if (index > 0)
                    {
                        paths.Add(file.Substring(index + 1));
                    }
                    else
                    {
                        LegacyLog.Warning($"Invalid asset path \"{file}\"!");
                    }
                }

                // Directories
                var directories = Directory.GetDirectories(path);
                foreach (var directory in directories)
                {
                    //Log.Debug(directory);
                    AddAssetPaths(directory, paths, true);
                }
            }
            else
            {
                LegacyLog.Warning($"\"{path}\" not found!");
            }
        }

        public static List<string> GetFilenamesWithoutExt(string path, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                if (files != null)
                {
                    var filenames = new List<string>();
                    for (int i = 0; i < files.Length; i++)
                    {
                        var file = files[i];
                        if (file.EndsWith(".DS_Store")) continue;
                        if (file.EndsWith(".meta")) continue;

                        int index = file.LastIndexOf(DirectorySeparatorChar);
                        if (index > 0)
                        {
                            var filename = file.Substring(index + 1);
                            index = filename.LastIndexOf('.');
                            if (index > 0)
                            {
                                filename = filename.Substring(0, index);
                            }
                            filenames.Add(filename);
                        }
                    }

                    return filenames;
                }
            }

            return null;
        }

        public static Sprite LoadSprite(string path, int textureWidth, int textureHeight, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                var texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
                texture.filterMode = FilterMode.Trilinear;
                texture.LoadImage(bytes);

                return Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f), 100);
            }

            return null;
        }

        public static bool SaveSprite(Sprite sprite, string path, bool isAbsolutePath = false)
        {
            if (sprite == null) return false;

            SetAbsolutePath(ref path, isAbsolutePath);
            try
            {
                var bytes = sprite.texture.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                LegacyLog.Error(e);
                return false;
            }

            return true;
        }

        public static bool SaveTexture(Texture2D texture, string path, bool absolutePath = false)
        {
            if (texture == null) return false;

            SetAbsolutePath(ref path, absolutePath);

            try
            {
                var bytes = texture.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                LegacyLog.Error($"Can't save texture \"{texture.name}\": {e}");
                return false;
            }

            return true;
        }

        public static void ReadLines(string path, Action<string> callback, bool isAbsolutePath = false)
        {
            SetAbsolutePath(ref path, isAbsolutePath);
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                using (var reader = fileInfo.OpenText())
                {
                    do
                    {
                        var line = reader.ReadLine();
                        if (line == null) break;
                        callback(line);
                    }
                    while (true);

                    reader.Close();
                }
            }
            else
            {
                LegacyLog.Warning($"\"{path}\" not found!");
            }
        }

        public static void MoveFile(string sourcePath, string destPath)
        {
            try
            {
                File.Move(sourcePath, destPath);
            }
            catch (Exception e)
            {
                LegacyLog.Error(e);
            }
        }

#if UNITY_EDITOR
        public static string RemoveFilename(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                //Log.Debug($"Remove filename from \"{path}\"");
                int index = path.LastIndexOf('.');
                if (index > 0)
                {
                    index = path.LastIndexOf(DirectorySeparatorChar, index - 1);
                    if (index > 0)
                    {
                        path = path.Substring(0, index);
                        //Log.Debug($"=> \"{path}\"");
                    }
                }
            }
            return path;
        }

        public static void SaveFilePanel(string title, string fileName, string extension, Func<string, bool> func, ref string lastPath, ref string lastFileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                fileName = lastFileName;
            }
            var path = EditorUtility.SaveFilePanel(title, lastPath, fileName, extension);
            if (!path.IsNullOrEmpty())
            {
                if (func(path))
                {
                    path.SplitPath(out lastPath, out lastFileName);
                }
                else
                {
                    LegacyLog.Warning($"Can't save to \"{path}\"!");
                }
            }
        }

        /// <summary>
        /// Displays the "open folder" dialog and returns the selected path name.
        /// </summary>
        public static string OpenFolderPanel(string title, string folder, string defaultName = "")
        {
            return EditorUtility.OpenFolderPanel(title, folder, defaultName);
        }

        /// <summary>
        /// Displays the "open folder" dialog and returns the selected path name.
        /// callback(filePath)
        /// </summary>
        public static string OpenFolderPanel(string title, string folder, Action<string> callback)
        {
            var path = EditorUtility.OpenFolderPanel(title, folder, "");
            if (!string.IsNullOrEmpty(path))
            {
                var files = Directory.GetFiles(path);
                int count = files.Length;
                for (int i = 0; i < count; i++)
                {
                    var filePath = files[i];
                    if (!filePath.EndsWith(".meta"))
                    {
                        callback(filePath);
                    }
                }
            }
            return path;
        }

        public static string LoadTextFromFile(string title, string directory = "", string extension = "txt")
        {
            var path = OpenFilePanel(title, directory, extension);
            if (!string.IsNullOrEmpty(path))
            {
                return LoadText(path, true);
            }

            return null;
        }

        public static bool SaveTextToFile(string text, string title, string directory, string defaultName, string extension = "txt")
        {
            var path = SaveFilePanel(title, directory, defaultName, extension);
            if (!string.IsNullOrEmpty(path))
            {
                return SaveText(text, path, true);
            }

            return false;
        }

        public static T LoadBinaryFromFile<T>(string title, string directory, string extension = "bytes") where T : class
        {
            var path = OpenFilePanel(title, directory, extension);
            if (!string.IsNullOrEmpty(path))
            {
                return LoadBinary<T>(path, true);
            }

            return default;
        }

        public static bool SaveBinaryToFile<T>(T data, string title, string directory, string defaultName, string extension = "bytes") where T : class
        {
            var path = SaveFilePanel(title, directory, defaultName, extension);
            if (!string.IsNullOrEmpty(path))
            {
                return SaveBinary(data, path, true);
            }

            return false;
        }

        public static void WriteText(List<string> lines, string path, bool append = false)
        {
            int count = lines != null ? lines.Count : 0;
            if (count > 0)
            {
                var writer = new StreamWriter(path, append);
                for (int i = 0; i < count - 1; i++)
                {
                    writer.WriteLine(lines[i]);
                }
                writer.Write(lines[count - 1]);
                writer.Close();
            }
        }

        public static void WriteText(string text, string path, bool append = false)
        {
            var writer = new StreamWriter(path, append);
            writer.Write(text);
            writer.Close();
        }

        public static string ReadText(string path, bool absolutePath = false)
        {
            if (!absolutePath)
            {
                path = GetAbsolutePath(path);
            }

            string text = "";
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                using (var reader = fileInfo.OpenText())
                {
                    text = reader.ReadToEnd();
                    reader.Close();
                }
            }
            else
            {
                LegacyLog.Warning($"\"{path}\" not found!");
            }

            return text;
        }

        public static void WriteFile(string path, Action<StreamWriter> callback, bool absolutePath = false)
        {
            if (!absolutePath)
            {
                path = GetAbsolutePath(path);
            }

            using (var writer = new StreamWriter(path))
            {
                callback(writer);
                writer.Close();
            }

            AssetDatabase.Refresh();
        }

        public static void ReadFile(string path, Action<StreamReader> callback, bool absolutePath = false)
        {
            if (!absolutePath)
            {
                path = GetAbsolutePath(path);
            }

            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                using (var reader = fileInfo.OpenText())
                {
                    callback(reader);
                    reader.Close();
                }
            }
            else
            {
                LegacyLog.Warning($"\"{path}\" not found!");
            }
        }

        public static void ForEachFileName(string path, Callback<string> callback, bool isAbsolutePath = false)
        {
            if (!isAbsolutePath)
            {
                path = GetAbsolutePath(path);
            }

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                int count = files.Length;
                for (int i = 0; i < count; i++)
                {
                    var fileName = files[i];
                    if (!fileName.EndsWith(".meta"))
                    {
                        int index = fileName.LastIndexOf(DirectorySeparatorChar);
                        if (index > 0)
                        {
                            fileName = fileName.Substring(index + 1);
                        }
                        callback(fileName);
                    }
                }
            }
        }
#endif
    }
}
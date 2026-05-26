#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        const int SpeedPriority = 0;
        static readonly float TrimTextureAlphaThreshold = 0.1f;

        static string _textureLastPath;

        //[MenuItem("Gametamin/Log Play AABB")]
        //static void LogPlayAABB()
        //{

        //}

        [MenuItem("Gametamin/Speed/0.25x", false, SpeedPriority)]
        static void Speed0_25()
        {
            Time.timeScale = 0.25f;
        }

        [MenuItem("Gametamin/Speed/0.5x", false, SpeedPriority)]
        static void Speed0_5X()
        {
            Time.timeScale = 0.5f;
        }

        [MenuItem("Gametamin/Speed/1x", false, SpeedPriority)]
        static void Speed1X()
        {
            Time.timeScale = 1f;
        }

        [MenuItem("Gametamin/Speed/1.5x", false, SpeedPriority)]
        static void Speed1_5X()
        {
            Time.timeScale = 1.5f;
        }

        [MenuItem("Gametamin/Speed/2x", false, SpeedPriority)]
        static void Speed2X()
        {
            Time.timeScale = 2f;
        }

        [MenuItem("Gametamin/Capture screen")]
        static void CaptureScreen()
        {
            if (Application.isPlaying)
            {
                var path = Application.dataPath;
                int index = path.LastIndexOf("Assets", StringComparison.Ordinal);
                if (index > 0)
                {
                    path = path.Substring(0, index);
                }
                path = path + "Screenshots";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var dateTime = DateTime.Now;
                var filename = $"Screenshots/{dateTime.ToLongDateString()}, {dateTime.ToLongTimeString()}.png";
                filename = filename.Replace(':', '.');
                ScreenCapture.CaptureScreenshot(filename);
                LegacyLog.Debug(filename);
            }
            else
            {
                LegacyLog.Warning("Edit mode not supported!");
            }
        }

        [MenuItem("Gametamin/Calculate Text Width")]
        static void CalculateTextWidth()
        {
            string text = "";

            void OnGUICallback()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Text", GUILayout.Width(30));
                    text = GUILayout.TextField(text);
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show("Calculate Text Width", OnGUICallback, "Calculate", "Close", button =>
            {
                if (button == 1)
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        GUIHelper.ShowError("Text required!");
                    }
                    else
                    {
                        LegacyLog.Debug($"{text}: width = {GUIHelper.GetLabelWidth(new GUIContent(text))}");
                    }
                }
                else
                {
                    CustomDialog.CloseDialog();
                }
            });
        }

        [MenuItem("Gametamin/Copy Device ID")]
        static void CopyDeviceID()
        {
            var deviceID = Helper.DeviceID;
            deviceID.CopyToClipboard();
            LegacyLog.Warning($"DeviceID=\"{deviceID}\"");
        }

        [MenuItem("Gametamin/Reload Scene")]
        static void ReloadScene()
        {
            if (Application.isPlaying)
            {
                Helper.ReloadScene();
            }
        }

        [MenuItem("Gametamin/Reload Script")]
        static void ReloadScript()
        {
            EditorUtility.RequestScriptReload();
        }

        [MenuItem("Gametamin/Texture/Add Flip Horizontal")]
        static void AddTextureFlipHorizontal()
        {
            EditTexture("Add Flip Horizontal", texture => TextureHelper.AddFlipHorizontal(texture));
        }

        [MenuItem("Gametamin/Texture/Add Flip Vertical")]
        static void AddTextureFlipVertical()
        {
            EditTexture("Add Flip Vertical", texture => TextureHelper.AddFlipVertical(texture));
        }

        [MenuItem("Gametamin/Texture/Add Flip All")]
        static void AddTextureFlipAll()
        {
            EditTexture("Add Flip All", texture => TextureHelper.AddFlipAll(texture));
        }

        [MenuItem("Gametamin/Texture/Split Horizontal")]
        static void SplitTextureHorizontal()
        {
            EditTexture("Split Horizontal", texture => TextureHelper.SplitHorizontal(texture));
        }

        [MenuItem("Gametamin/Texture/Set Color")]
        static void SetTextureColor()
        {
            GUIHelper.ShowInputColor("Set Color", color =>
            {
                EditTexture("Set Color", texture => TextureHelper.SetColor(texture, color));
            });
        }

        [MenuItem("Gametamin/Texture/Trim")]
        static void TrimTexture()
        {
            EditTexture("Trim Texture", texture => TextureHelper.Trim(texture, TrimTextureAlphaThreshold));
        }

        [MenuItem("Gametamin/Texture/Trim Textures")]
        static void TrimTextures()
        {
            LoadTextures("Trim Textures", (filePath, texture) =>
            {
                texture = TextureHelper.Trim(texture, TrimTextureAlphaThreshold);
                if (!TextureHelper.SaveTexture(texture, filePath))
                {
                    LegacyLog.Warning($"Can't save texture to \"{filePath}\"!");
                }
            });
        }

        [MenuItem("Assets/Gametamin/Copy Instance ID", false, 0)]
        static void CopyInstanceID()
        {
            var assetPath = SelectedAssetPath;
            var @object = EditorHelper.LoadAsset<UnityEngine.Object>(assetPath);
            if (@object != null)
            {
                var instanceID = @object.GetInstanceID().ToString();
                instanceID.CopyToClipboard();
                LegacyLog.Warning(instanceID);
            }
            else
            {
                LegacyLog.Warning($"Can't load asset at \"{assetPath}\"!");
            }
        }

        [MenuItem("Assets/Gametamin/Log Instance IDs", false, 0)]
        static void LogInstanceIDs()
        {
            var assetPaths = SelectedAssetPaths;
            var objects = EditorHelper.LoadAssets<UnityEngine.Object>(assetPaths);
            if (!objects.IsNullOrEmpty())
            {
                foreach (var obj in objects)
                {
                    LegacyLog.Debug($"{obj.name}: {obj.GetInstanceID()}");
                }
            }
        }

        /// <summary>
        /// callback(path, texture)
        /// </summary>
        static void LoadTexture(string title, Callback<string, Texture2D> callback)
        {
            var path = FileHelper.OpenFilePanel(title, _textureLastPath, "png");
            if (!path.IsNullOrEmpty())
            {
                _textureLastPath = Helper.GetFolderPath(path);
                var texture = TextureHelper.LoadTexture(path);
                callback(path, texture);
            }
        }

        /// <summary>
        /// callback(path, texture)
        /// </summary>
        static void LoadTextures(string title, Callback<string, Texture2D> callback)
        {
            var path = FileHelper.OpenFolderPanel(title, _textureLastPath, filePath =>
            {
                if (filePath.EndsWith(".png"))
                {
                    var texture = TextureHelper.LoadTexture(filePath);
                    if (texture != null)
                    {
                        callback(filePath, texture);
                    }
                }
            });

            if (!path.IsNullOrEmpty())
            {
                _textureLastPath = Helper.GetFolderPath(path);
            }
        }

        static void EditTexture(string title, Func<Texture2D, Texture2D> func)
        {
            LoadTexture(title, (path, texture) =>
            {
                texture = func(texture);
                if (!TextureHelper.SaveTexture(texture, path))
                {
                    LegacyLog.Warning($"Can't save texture to \"{path}\"!");
                }
            });
        }
    }
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        static string _exportTexturesLastPath;

        [MenuItem("Assets/Gametamin/Export Sprite(s) to Texture(s)", false, 0)]
        static void ExportSpritesToTextures()
        {
            var assetPaths = SelectedAssetPaths;
            var sprites = EditorHelper.LoadAssets<Sprite>(assetPaths);
            if (!sprites.IsNullOrEmpty())
            {
                var path = FileHelper.OpenFolderPanel("Export Textures", _exportTexturesLastPath);
                if (!string.IsNullOrEmpty(path))
                {
                    _exportTexturesLastPath = path;
                    var textureEditors = TextureEditor.CreateTextureEditors(sprites, texture => !texture.isReadable);
                    textureEditors.ForEach2(textureEditor => textureEditor.SetReadable(true));
                    try
                    {
                        foreach (var sprite in sprites)
                        {
                            var texture = sprite.CreateTexture();
                            FileHelper.SaveTexture(texture, $"{path}/{sprite.name}.png", true);
                        }
                    }
                    catch (System.Exception e)
                    {
                        LegacyLog.Error(e);
                    }
                    textureEditors.ForEach2(textureEditor => textureEditor.SetReadable(false));
                }
            }
        }
    }
}
#endif
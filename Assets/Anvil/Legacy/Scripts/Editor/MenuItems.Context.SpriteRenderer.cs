#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("CONTEXT/SpriteRenderer/Set For Name")]
        static void SpriteRendererSetForName(MenuCommand menuCommand)
        {
            var spriteRenderer = menuCommand.To<SpriteRenderer>();
            var sprite = spriteRenderer.sprite;
            if (sprite != null)
            {
                var gameObject = spriteRenderer.gameObject;
                EditorHelper.Set(gameObject, "Set Name", () => gameObject.name = sprite.name);
            }
            else
            {
                LegacyLog.Warning("Missing sprite!");
            }
        }

        [MenuItem("CONTEXT/SpriteRenderer/Copy RGB")]
        static void SpriteRendererCopyRGB(MenuCommand menuCommand)
        {
            Helper.CopyRGB(menuCommand.To<SpriteRenderer>().color);
        }

        static string _spriteRendererExportPNGPath;
        static string _spriteRendererExportPNGFileName;
        [MenuItem("CONTEXT/SpriteRenderer/Export PNG")]
        static void SpriteRendererExportPNG(MenuCommand menuCommand)
        {
            var spriteRenderer = menuCommand.To<SpriteRenderer>();
            var sprite = spriteRenderer.sprite;
            if (sprite == null)
            {
                LegacyLog.Warning("Missing sprite!");
                return;
            }

            FileHelper.SaveFilePanel("Export PNG", sprite.name, "png", path =>
            {
                return FileHelper.SaveSprite(sprite, path, true);
            }, ref _spriteRendererExportPNGPath, ref _spriteRendererExportPNGFileName);
        }
    }
}
#endif
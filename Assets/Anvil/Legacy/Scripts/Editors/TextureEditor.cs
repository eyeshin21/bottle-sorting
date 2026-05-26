#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class TextureEditor
    {
        TextureImporter _textureImporter;

        public TextureEditor(Texture2D texture)
        {
            var path = AssetDatabase.GetAssetPath(texture);
            _textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
        }

        public void SetReadable(bool readable)
        {
            _textureImporter.isReadable = readable;
            _textureImporter.SaveAndReimport();
        }

        public void Edit(Callback<TextureImporter> callback)
        {
            callback(_textureImporter);
            _textureImporter.SaveAndReimport();
        }

        public static List<TextureEditor> CreateTextureEditors(List<Sprite> sprites, AcceptFunc<Texture2D> acceptFunc)
        {
            int count = sprites.GetCount();
            if (count > 0)
            {
                var textures = new List<Texture2D>(count);
                foreach (var sprite in sprites)
                {
                    var texture = sprite.texture;
                    if (!textures.Contains(texture) && acceptFunc(texture))
                    {
                        textures.Add(texture);
                    }
                }
                if (textures.Count > 0)
                {
                    var editors = new List<TextureEditor>();
                    foreach (var texture in textures)
                    {
                        editors.Add(new TextureEditor(texture));
                    }
                    return editors;
                }
            }
            return default;
        }
    }
}
#endif
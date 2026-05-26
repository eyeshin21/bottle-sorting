#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor.U2D;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public partial class AssetsWindow
    {
        class QuerySpriteAtlas
        {
            List<SpriteAtlas> _spriteAtlases = new();

            QueryGUIController<bool> _variantController = new(GUIController.CreateBool("Variant"));
            QueryGUIController<bool> _includeInBuildController = new(GUIController.CreateBool("Include in Build", true));
            QueryGUIController<bool> _allowRotationController = new(GUIController.CreateBool("Allow Rotation", true));
            QueryGUIController<bool> _tightPackingController = new(GUIController.CreateBool("Tight Packing", true));

            public void OnGUI()
            {
                _variantController.OnGUI();
                _includeInBuildController.OnGUI();
                _allowRotationController.OnGUI();
                _tightPackingController.OnGUI();

                GUIHelper.Line();
                if (GUIHelper.ButtonCenter("Query"))
                {
                    _spriteAtlases.Clear();

                    var spriteAtlases = EditorHelper.LoadAssets<SpriteAtlas>();
                    foreach (var spriteAtlas in spriteAtlases)
                    {
                        var packingSettings = spriteAtlas.GetPackingSettings();
                        //Log.Debug($@"{spriteAtlas.name}: variant={spriteAtlas.isVariant},
                        //        includeInBuild={spriteAtlas.IsIncludeInBuild()},
                        //        allowRotation={packingSettings.enableRotation},
                        //        tightPacking={packingSettings.enableTightPacking},
                        //        spriteCount={spriteAtlas.spriteCount}");

                        if (!_variantController.IsMatch(spriteAtlas.isVariant))
                        {
                            continue;
                        }
                        if (!_includeInBuildController.IsMatch(spriteAtlas.IsIncludeInBuild()))
                        {
                            continue;
                        }
                        if (!_allowRotationController.IsMatch(packingSettings.enableRotation))
                        {
                            continue;
                        }
                        if (!_tightPackingController.IsMatch(packingSettings.enableTightPacking))
                        {
                            continue;
                        }

                        _spriteAtlases.Add(spriteAtlas);
                    }
                }
                GUIHelper.Line();

                foreach (var spriteAtlas in _spriteAtlases)
                {
                    GUIHelper.Label(spriteAtlas.name);
                }
            }
        }

        QuerySpriteAtlas _querySpriteAtlas;

        void OnGUISpriteAtlas()
        {
            if (_querySpriteAtlas == null)
            {
                _querySpriteAtlas = new QuerySpriteAtlas();
            }
            _querySpriteAtlas.OnGUI();
        }
    }
}
#endif
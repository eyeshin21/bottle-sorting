// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections.Generic;
//
// namespace Gametamin.Utility
// {
//     /// <summary>
//     /// Extended from Sprite Adapter
//     /// </summary>
//     public interface IRenderAdapter
//     {
//         GameObject gameObject { get; }
//         Sprite Sprite { get; set; }
//         Material Material { get; set; }
//         public void SetSize(Vector2 size);
//         void SetShow(bool show);
//     }
//
//     public abstract class RenderAdapter : IRenderAdapter
//     {
//         public virtual GameObject gameObject { get; }
//         public virtual Sprite Sprite { get; set; }
//         public virtual Material Material { get; set; }
//         public virtual void SetShow(bool show);
//         public virtual void SetSize(Vector2 size) { }
//
//         public static SpriteAdapter Create(SpriteRenderer spriteRenderer)
//         {
//             return new SpriteRendererAdapter(spriteRenderer);
//         }
//
//         public static SpriteAdapter Create(Image image)
//         {
//             return new ImageAdapter(image);
//         }
//
//         public static ISpriteAdapter Create(Component component)
//         {
//             return Create(component.gameObject);
//         }
//
//         static List<ISpriteAdapter> _spriteAdapters = new List<ISpriteAdapter>();
//         public static ISpriteAdapter Create(GameObject gameObject)
//         {
//             _spriteAdapters.Clear();
//             gameObject.BrowseChildrenBFS(go =>
//             {
//                 var spriteAdapter = CreateSpriteAdapter(go, out bool continueChildren);
//                 if (spriteAdapter != null)
//                 {
//                     _spriteAdapters.Add(spriteAdapter);
//                 }
//                 return continueChildren;
//             });
//
//             int count = _spriteAdapters.Count;
//             if (count == 0)
//             {
//                LegacyLog.Warning($"Can't create sprite adapter for {gameObject}!");
//                 return new DefaultAdapter(gameObject);
//             }
//
//             if (count == 1)
//             {
//                 var spriteAdapter = _spriteAdapters[0];
//                 _spriteAdapters.Clear();
//                 return spriteAdapter;
//             }
//
//             var spriteAdapter2 = new CompositeAdapter(_spriteAdapters);
//             _spriteAdapters.Clear();
//             return spriteAdapter2;
//         }
//
//         static ISpriteAdapter CreateSpriteAdapter(GameObject gameObject, out bool continueChildren)
//         {
//             continueChildren = true;
//
//             var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
//             if (spriteRenderer != null)
//             {
//                 continueChildren = false;
//                 return Create(spriteRenderer);
//             }
//
//             var image = gameObject.GetComponent<Image>();
//             if (image != null)
//             {
//                 continueChildren = false;
//                 return Create(image);
//             }
//
//             var spriteAdapter = gameObject.GetComponent<ISpriteAdapter>();
//             if (spriteAdapter != null)
//             {
//                 continueChildren = false;
//                 return spriteAdapter;
//             }
//
//             return null;
//         }
//
//         class SpriteRendererAdapter : SpriteAdapter
//         {
//             SpriteRenderer _spriteRenderer;
//             Material _defaultMaterial;
//
//             public SpriteRendererAdapter(SpriteRenderer spriteRenderer)
//             {
//                 _spriteRenderer = spriteRenderer;
//             }
//
//             public override GameObject gameObject => _spriteRenderer.gameObject;
//
//             public override Sprite Sprite
//             {
//                 get => _spriteRenderer.sprite;
//                 set => _spriteRenderer.sprite = value;
//             }
//
//             public override Material Material
//             {
//                 get => _spriteRenderer.material;
//                 set
//                 {
//                     if (value != null)
//                     {
//                         if (_defaultMaterial == null)
//                         {
//                             _defaultMaterial = _spriteRenderer.material;
//                         }
//                         _spriteRenderer.material = value;
//                     }
//                     else
//                     {
//                         if (_defaultMaterial != null)
//                         {
//                             _spriteRenderer.material = _defaultMaterial;
//                         }
//                     }
//                 }
//             }
//
//             public override void SetShow(bool show)
//             {
//                 _spriteRenderer.enabled = show;
//             }
//         }
//
//         class ImageAdapter : SpriteAdapter
//         {
//             Image _image;
//             Material _defaultMaterial;
//
//             public ImageAdapter(Image image)
//             {
//                 _image = image;
//             }
//
//             public override GameObject gameObject => _image.gameObject;
//
//             public override Sprite Sprite
//             {
//                 get => _image.sprite;
//                 set => _image.sprite = value;
//             }
//
//
//             public override Material Material
//             {
//                 get => _image.material;
//                 set
//                 {
//                     if (value != null)
//                     {
//                         if (_defaultMaterial == null)
//                         {
//                             _defaultMaterial = _image.material;
//                         }
//                         _image.material = value;
//                     }
//                     else
//                     {
//                         if (_defaultMaterial != null)
//                         {
//                             _image.material = _defaultMaterial;
//                         }
//                     }
//                 }
//             }
//
//             public override void SetShow(bool show)
//             {
//                 _image.enabled = show;
//             }
//         }
//
//         class DefaultAdapter : SpriteAdapter
//         {
//             GameObject _gameObject;
//             Sprite _sprite;
//             Material _material;
//
//             public DefaultAdapter(GameObject gameObject)
//             {
//                 _gameObject = gameObject;
//             }
//
//             public override GameObject gameObject => _gameObject;
//
//             public override Sprite Sprite
//             {
//                 get => _sprite;
//                 set => _sprite = value;
//             }
//
//             public override Material Material
//             {
//                 get => _material;
//                 set => _material = value;
//             }
//
//             public override void SetShow(bool show)
//             {
//                 _gameObject.SetShow(show);
//             }
//         }
//
//         class CompositeAdapter : SpriteAdapter
//         {
//             List<ISpriteAdapter> _adapters;
//             int _count;
//             Sprite _sprite;
//             Material _material;
//
//             public CompositeAdapter(List<ISpriteAdapter> adapters)
//             {
//                 _count = adapters.Count;
//                 _adapters = new List<ISpriteAdapter>(_count);
//                 _adapters.AddRange(adapters);
//             }
//
//             public override GameObject gameObject => _count > 0 ? _adapters[0].gameObject : null;
//
//             public override Sprite Sprite
//             {
//                 get => _count > 0 ? _adapters[0].Sprite : _sprite;
//                 set
//                 {
//                     for (int i = 0; i < _count; i++)
//                     {
//                         _adapters[i].Sprite = value;
//                     }
//                     _sprite = value;
//                 }
//             }
//
//             public override Material Material
//             {
//                 get => _count > 0 ? _adapters[0].Material : _material;
//                 set
//                 {
//                     for (int i = 0; i < _count; i++)
//                     {
//                         _adapters[i].Material = value;
//                     }
//                     _material = value;
//                 }
//             }
//
//             public override void SetShow(bool show)
//             {
//                 for (int i = 0; i < _count; i++)
//                 {
//                     _adapters[i].SetShow(show);
//                 }
//             }
//         }
//     }
// }
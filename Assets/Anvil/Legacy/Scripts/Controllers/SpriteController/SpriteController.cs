using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class SpriteController
    {
        static List<ISpriteController> _controllers = new();

        public static SpriteRendererSpriteController Create(SpriteRenderer spriteRenderer)
        {
            return SpriteRendererSpriteController.Create(spriteRenderer);
        }

        public static ImageSpriteController Create(Image image)
        {
            return ImageSpriteController.Create(image);
        }

        public static ISpriteController Create(Component component, bool all = false)
        {
            return Create(component?.gameObject, all);
        }

        public static ISpriteController Create(IController controller, bool all = false)
        {
            return Create(controller?.GameObject, all);
        }

        public static ISpriteController Create(GameObject gameObject, bool all = false)
        {
            if (gameObject == null) return null;

            _controllers.Clear();
            gameObject.ForEachChildBFS(go =>
            {
                var controller = CreateController(go, out bool skipChildren);
                if (controller != null)
                {
                    _controllers.Add(controller);
                    if (!all)
                    {
                        return BFSContinueType.Break;
                    }
                }
                return skipChildren.ToBFSContinueType();
            });

            ISpriteController controller;
            int count = _controllers.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create sprite controller for {gameObject.GetHierarchyPath()}!");
                controller = DefaultSpriteController.Create();
            }
            else
            {
                if (count == 1)
                {
                    controller = _controllers[0];
                }
                else
                {
                    controller = CompositeSpriteController.Create(_controllers);
                }
                _controllers.Clear();
            }

            return controller;
        }

        static ISpriteController CreateController(GameObject gameObject, out bool skipChildren)
        {
            var spriteController = gameObject.GetComponent<ISpriteController>();
            if (spriteController != null)
            {
                skipChildren = true;
                return spriteController;
            }

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                skipChildren = true;
                return Create(spriteRenderer);
            }

            var image = gameObject.GetComponent<Image>();
            if (image != null)
            {
                skipChildren = true;
                return Create(image);
            }

            skipChildren = false;
            return null;
        }

        public static Sprite GetSprite(GameObject gameObject)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                var sprite = controller.Sprite;
                controller.ReturnToPool();
                return sprite;
            }

            LegacyLog.Warning($"Can't get sprite from {gameObject.GetHierarchyPath()}!");
            return null;
        }

        public static void SetSprite(GameObject gameObject, Sprite sprite)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                controller.Sprite = sprite;
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set sprite {sprite} for {gameObject.GetHierarchyPath()}!");
            }
        }
    }
}
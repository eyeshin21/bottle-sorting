using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace Anvil.Legacy
{
    public static class ColorController
    {
        static List<IColorController> _controllers = new();

        public static SpriteRendererColorController Create(SpriteRenderer spriteRenderer)
        {
            return SpriteRendererColorController.Create(spriteRenderer);
        }

        public static ParticleSystemColorController Create(ParticleSystem particleSystem)
        {
            return ParticleSystemColorController.Create(particleSystem);
        }

        public static ImageColorController Create(Image image)
        {
            return ImageColorController.Create(image);
        }

        public static TextColorController Create(Text text)
        {
            return TextColorController.Create(text);
        }

        public static TextMeshProColorController Create(TMP_Text tmptext)
        {
            return TextMeshProColorController.Create(tmptext);
        }

        public static RendererColorController Create(Renderer renderer)
        {
            return RendererColorController.Create(renderer);
        }

        public static IColorController Create(Component component, bool all = false)
        {
            return Create(component?.gameObject, all);
        }

        public static IColorController Create(IController controller, bool all = false)
        {
            return Create(controller?.GameObject, all);
        }

        public static IColorController Create(GameObject gameObject, bool all = false)
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

            IColorController controller;
            int count = _controllers.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create color controller for {gameObject.GetHierarchyPath()}!");
                controller = DefaultColorController.Create();
            }
            else
            {
                if (count == 1)
                {
                    controller = _controllers[0];
                }
                else
                {
                    controller = CompositeColorController.Create(_controllers);
                }
                _controllers.Clear();
            }

            return controller;
        }

        static IColorController CreateController(GameObject gameObject, out bool skipChildren)
        {
            var colorController = gameObject.GetComponent<IColorController>();
            if (colorController != null)
            {
                skipChildren = true;
                return colorController;
            }

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                skipChildren = true;
                return Create(spriteRenderer);
            }

            var particleSystem = gameObject.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                skipChildren = true;
                return Create(particleSystem);
            }

            var image = gameObject.GetComponent<Image>();
            if (image != null)
            {
                skipChildren = true;
                return Create(image);
            }

            var text = gameObject.GetComponent<Text>();
            if (text != null)
            {
                skipChildren = true;
                return Create(text);
            }

            var tmpText = gameObject.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                skipChildren = true;
                return Create(tmpText);
            }

            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                skipChildren = true;
                return Create(renderer);
            }

            skipChildren = false;
            return null;
        }

        public static Color GetColor(GameObject gameObject)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                var color = controller.Color;
                controller.ReturnToPool();
                return color;
            }

            LegacyLog.Warning($"Can't get color from {gameObject.GetHierarchyPath()}!");
            return Defaults.Color;
        }

        public static void SetColor(GameObject gameObject, Color color)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                controller.Color = color;
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set color for {gameObject.GetHierarchyPath()}!");
            }
        }

        public static Vector3 GetRGB(GameObject gameObject)
        {
            return GetColor(gameObject).GetRGB();
        }

        public static void SetRGB(GameObject gameObject, Vector3 rgb)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                controller.Color = controller.Color.SetRGB(rgb);
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set RGB for {gameObject.GetHierarchyPath()}!");
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace Anvil.Legacy
{
    public static class AlphaController
    {
        static List<IAlphaController> _controllers = new();

        public static SpriteRendererAlphaController Create(SpriteRenderer spriteRenderer)
        {
            return SpriteRendererAlphaController.Create(spriteRenderer);
        }

        public static ParticleSystemAlphaController Create(ParticleSystem particleSystem)
        {
            return ParticleSystemAlphaController.Create(particleSystem);
        }

        public static ImageAlphaController Create(Image image)
        {
            return ImageAlphaController.Create(image);
        }

        public static TextAlphaController Create(Text text)
        {
            return TextAlphaController.Create(text);
        }

        public static TextMeshProAlphaController Create(TMP_Text tmptext)
        {
            return TextMeshProAlphaController.Create(tmptext);
        }

        public static RendererAlphaController Create(Renderer renderer)
        {
            return RendererAlphaController.Create(renderer);
        }

        public static IAlphaController Create(Component component, bool all = false)
        {
            return Create(component?.gameObject, all);
        }

        public static IAlphaController Create(IController controller, bool all = false)
        {
            return Create(controller?.GameObject, all);
        }

        public static IAlphaController Create(GameObject gameObject, bool all = false)
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

            IAlphaController controller;
            int count = _controllers.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create color controller for {gameObject.GetHierarchyPath()}!");
                controller = DefaultAlphaController.Create();
            }
            else
            {
                if (count == 1)
                {
                    controller = _controllers[0];
                }
                else
                {
                    controller = CompositeAlphaController.Create(_controllers);
                }
                _controllers.Clear();
            }

            return controller;
        }

        static IAlphaController CreateController(GameObject gameObject, out bool skipChildren)
        {
            var alphaController = gameObject.GetComponent<IAlphaController>();
            if (alphaController != null)
            {
                skipChildren = true;
                return alphaController;
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

        public static float GetAlpha(GameObject gameObject)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                var a = controller.Alpha;
                controller.ReturnToPool();
                return a;
            }

            LegacyLog.Warning($"Can't get alpha from {gameObject.GetHierarchyPath()}!");
            return 1;
        }

        public static void SetAlpha(GameObject gameObject, float a)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                controller.Alpha = a;
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set alpha for {gameObject.GetHierarchyPath()}!");
            }
        }
    }
}
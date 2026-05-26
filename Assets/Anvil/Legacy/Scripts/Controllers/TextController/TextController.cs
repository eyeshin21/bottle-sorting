using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace Anvil.Legacy
{
    public static class TextController
    {
        static List<ITextController> _controllers = new();

        public static UITextTextController Create(Text text)
        {
            return UITextTextController.Create(text);
        }

        public static TextMeshProTextController Create(TMP_Text tmpText)
        {
            return TextMeshProTextController.Create(tmpText);
        }

        public static ITextController Create(Component component, bool all = false)
        {
            return Create(component?.gameObject, all);
        }

        public static ITextController Create(IController controller, bool all = false)
        {
            return Create(controller?.GameObject, all);
        }

        public static ITextController Create(GameObject gameObject, bool all = false)
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

            ITextController controller;
            int count = _controllers.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create text controller for {gameObject.GetHierarchyPath()}!");
                controller = DefaultTextController.Create();
            }
            else
            {
                if (count == 1)
                {
                    controller = _controllers[0];
                }
                else
                {
                    LegacyLog.Todo(_controllers.ToString2());
                    controller = _controllers[0];
                }
                _controllers.Clear();
            }

            return controller;
        }

        static ITextController CreateController(GameObject gameObject, out bool skipChildren)
        {
            var textController = gameObject.GetComponent<ITextController>();
            if (textController != null)
            {
                skipChildren = true;
                return textController;
            }

            var uiText = gameObject.GetComponent<Text>();
            if (uiText != null)
            {
                skipChildren = true;
                return Create(uiText);
            }

            var tmpText = gameObject.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                skipChildren = true;
                return Create(tmpText);
            }

            skipChildren = false;
            return null;
        }

        public static string GetText(GameObject gameObject)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                var text = controller.Text;
                controller.ReturnToPool();
                return text;
            }

            LegacyLog.Warning($"Can't get text from {gameObject.GetHierarchyPath()}!");
            return null;
        }

        public static void SetText(GameObject gameObject, object text)
        {
            SetText(gameObject, $"{text}");
        }

        public static void SetText(GameObject gameObject, string text)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                controller.Text = text;
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set text \"{text}\" for {gameObject.GetHierarchyPath()}!");
            }
        }
    }
}
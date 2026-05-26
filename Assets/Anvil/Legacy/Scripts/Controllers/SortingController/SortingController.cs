using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static class SortingController
    {
        const bool DefaultAll = false;

        static List<ISortingController> _controllers = new();

        public static SpriteRendererSortingController Create(SpriteRenderer spriteRenderer)
        {
            return SpriteRendererSortingController.Create(spriteRenderer);
        }

        public static RendererSortingController Create(Renderer renderer)
        {
            return RendererSortingController.Create(renderer);
        }

        public static SortingGroupSortingController Create(SortingGroup sortingGroup)
        {
            return SortingGroupSortingController.Create(sortingGroup);
        }

        public static CanvasSortingController Create(Canvas canvas)
        {
            return CanvasSortingController.Create(canvas);
        }

        public static ISortingController Create(Component component, bool all = DefaultAll)
        {
            return Create(component?.gameObject, all);
        }

        public static ISortingController Create(IController controller, bool all = DefaultAll)
        {
            return Create(controller?.GameObject, all);
        }

        public static ISortingController Create(GameObject gameObject, bool all = DefaultAll)
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

            ISortingController controller;
            int count = _controllers.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create sorting controller for {gameObject.GetHierarchyPath()}!");
                controller = DefaultSortingController.Create();
            }
            else
            {
                if (count == 1)
                {
                    controller = _controllers[0];
                }
                else
                {
                    controller = CompositeSortingController.Create(_controllers);
                }
                _controllers.Clear();
            }

            return controller;
        }

        static ISortingController CreateController(GameObject gameObject, out bool skipChildren)
        {
            var sortingController = gameObject.GetComponent<ISortingController>();
            if (sortingController != null)
            {
                skipChildren = true;
                return sortingController;
            }

            var sortingGroup = gameObject.GetComponent<SortingGroup>();
            if (sortingGroup != null)
            {
                skipChildren = true;
                return Create(sortingGroup);
            }

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                skipChildren = false;
                return Create(spriteRenderer);
            }

            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                skipChildren = false;
                return Create(renderer);
            }

            var canvas = gameObject.GetComponent<Canvas>();
            if (canvas != null)
            {
                skipChildren = true;
                return Create(canvas);
            }

            skipChildren = false;
            return null;
        }

        public static int GetSortingOrder(GameObject gameObject)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                int sortingOrder = controller.SortingOrder;
                controller.ReturnToPool();
                return sortingOrder;
            }

            LegacyLog.Warning($"Can't get sorting order from {gameObject.GetHierarchyPath()}!");
            return default;
        }

        public static void SetSortingLayerID(GameObject gameObject, int sortingLayerID, bool all = DefaultAll)
        {
            var controller = Create(gameObject, all);
            if (controller != null)
            {
                controller.SortingLayerID = sortingLayerID;
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set sorting layer {sortingLayerID} for {gameObject.GetHierarchyPath()}!");
            }
        }

        public static void SetSortingLayerID(GameObject gameObject, int sortingLayerID, out bool changed, bool all = DefaultAll)
        {
            changed = false;

            var controller = Create(gameObject, all);
            if (controller != null)
            {
                if (controller.SortingLayerID != sortingLayerID)
                {
                    controller.SortingLayerID = sortingLayerID;
                    changed = true;
                }
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set sorting layer {sortingLayerID} for {gameObject.GetHierarchyPath()}!");
            }
        }

        public static void SetSortingOrder(GameObject gameObject, int sortingOrder)
        {
            var controller = Create(gameObject);
            if (controller != null)
            {
                controller.SortingOrder = sortingOrder;
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't set sorting order {sortingOrder} for {gameObject.GetHierarchyPath()}!");
            }
        }

        public static void AddSortingOrder(GameObject gameObject, int deltaSortingOrder, bool all = DefaultAll)
        {
            var controller = Create(gameObject, all);
            if (controller != null)
            {
                controller.AddSortingOrder(deltaSortingOrder);
                controller.ReturnToPool();
            }
            else
            {
                LegacyLog.Warning($"Can't add sorting order {deltaSortingOrder} for {gameObject.GetHierarchyPath()}!");
            }
        }
    }
}
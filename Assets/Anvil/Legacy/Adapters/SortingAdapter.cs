using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anvil.Legacy
{
    public interface ISortingAdapter
    {
        GameObject GameObject { get; }
        int SortingLayerID { get; set; }
        int SortingOrder { get; set; }
    }

    public abstract class SortingAdapter : ISortingAdapter
    {
        public abstract GameObject GameObject { get; }
        public abstract int SortingLayerID { get; set; }
        public abstract int SortingOrder { get; set; }

        public static void SetSortingLayerID(GameObject gameObject, int sortingLayerID)
        {
            bool found = false;
            gameObject.BrowseChildrenBFS(go =>
            {
                var sortingGroup = go.GetComponent<SortingGroup>();
                if (sortingGroup != null)
                {
                    sortingGroup.sortingLayerID = sortingLayerID;
                    found = true;
                    return false;
                }

                var renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sortingLayerID = sortingLayerID;
                    found = true;
                }

                return true;
            });

            if (!found)
            {
                LegacyLog.Warning($"Can't set sorting layer ID for {gameObject}");
            }
        }

        public static void SetSortingLayerID(GameObject gameObject, int sortingLayerID, out bool changed)
        {
            bool isChanged = false;
            bool found = false;
            gameObject.BrowseChildrenBFS(go =>
            {
                var sortingGroup = go.GetComponent<SortingGroup>();
                if (sortingGroup != null)
                {
                    if (sortingGroup.sortingLayerID != sortingLayerID)
                    {
                        sortingGroup.sortingLayerID = sortingLayerID;
                        isChanged = true;
                    }
                    found = true;
                    return false;
                }

                var renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (renderer.sortingLayerID != sortingLayerID)
                    {
                        renderer.sortingLayerID = sortingLayerID;
                        isChanged = true;
                    }
                    found = true;
                }

                return true;
            });

            if (!found)
            {
                LegacyLog.Warning($"Can't set sorting layer ID for {gameObject}");
            }

            changed = isChanged;
        }

        public static void SetSortingOrder(GameObject gameObject, int sortingOrder)
        {
            bool found = false;
            gameObject.BrowseChildrenBFS(go =>
            {
                var sortingGroup = go.GetComponent<SortingGroup>();
                if (sortingGroup != null)
                {
                    sortingGroup.sortingOrder = sortingOrder;
                    found = true;
                    return false;
                }

                var renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder = sortingOrder;
                    found = true;
                }

                return true;
            });

            if (!found)
            {
                LegacyLog.Warning($"Can't set sorting order for {gameObject}");
            }
        }

        public static void AddSortingOrder(GameObject gameObject, int deltaSortingOrder)
        {
            bool found = false;
            gameObject.BrowseChildrenBFS(go =>
            {
                var sortingGroup = go.GetComponent<SortingGroup>();
                if (sortingGroup != null)
                {
                    sortingGroup.sortingOrder += deltaSortingOrder;
                    found = true;
                    return false;
                }

                var renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sortingOrder += deltaSortingOrder;
                    found = true;
                }

                return true;
            });

            if (!found)
            {
                LegacyLog.Warning($"Can't add sorting order for {gameObject}");
            }
        }

        public static SortingAdapter Create(SortingGroup sortingGroup)
        {
            return new SortingGroupAdapter(sortingGroup);
        }

        public static SortingAdapter Create(Renderer renderer)
        {
            return new RendererAdapter(renderer);
        }

        public static ISortingAdapter Create(Component component)
        {
            if (component == null)
            {
                LegacyLog.Warning($"Can't create sorting adapter: Component is null!");
                return new DefaultAdapter(null);
            }
            return Create(component.gameObject);
        }

        static List<ISortingAdapter> _sortingAdapters = new List<ISortingAdapter>();
        public static ISortingAdapter Create(GameObject gameObject)
        {
            _sortingAdapters.Clear();
            gameObject.BrowseChildrenBFS(go =>
            {
                var sortingAdapter = CreateSortingAdapter(go, out bool continueChildren);
                if (sortingAdapter != null)
                {
                    _sortingAdapters.Add(sortingAdapter);
                }
                return continueChildren;
            });

            int count = _sortingAdapters.Count;
            if (count == 0)
            {
                LegacyLog.Warning($"Can't create sorting adapter for {gameObject}!");
                return new DefaultAdapter(gameObject);
            }

            if (count == 1)
            {
                var sortingAdapter = _sortingAdapters[0];
                _sortingAdapters.Clear();
                return sortingAdapter;
            }

            var sortingAdapter2 = new CompositeAdapter(_sortingAdapters);
            _sortingAdapters.Clear();
            return sortingAdapter2;
        }

        static ISortingAdapter CreateSortingAdapter(GameObject gameObject, out bool continueChildren)
        {
            continueChildren = true;

            var sortingGroup = gameObject.GetComponent<SortingGroup>();
            if (sortingGroup != null)
            {
                continueChildren = false;
                return Create(sortingGroup);
            }

            var sortingAdapter = gameObject.GetComponent<ISortingAdapter>();
            if (sortingAdapter != null)
            {
                continueChildren = false;
                return sortingAdapter;
            }

            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                return Create(renderer);
            }

            return null;
        }

        class SortingGroupAdapter : SortingAdapter
        {
            private SortingGroup _sortingGroup;

            public SortingGroupAdapter(SortingGroup sortingGroup)
            {
                _sortingGroup = sortingGroup;
            }

            public override GameObject GameObject => _sortingGroup.gameObject;

            public override int SortingLayerID
            {
                get => _sortingGroup.sortingLayerID;
                set => _sortingGroup.sortingLayerID = value;
            }

            public override int SortingOrder
            {
                get => _sortingGroup.sortingOrder;
                set => _sortingGroup.sortingOrder = value;
            }
        }

        class RendererAdapter : SortingAdapter
        {
            private Renderer _renderer;

            public RendererAdapter(Renderer renderer)
            {
                _renderer = renderer;
            }

            public override GameObject GameObject => _renderer.gameObject;

            public override int SortingLayerID
            {
                get => _renderer.sortingLayerID;
                set => _renderer.sortingLayerID = value;
            }

            public override int SortingOrder
            {
                get => _renderer.sortingOrder;
                set => _renderer.sortingOrder = value;
            }
        }

        class DefaultAdapter : SortingAdapter
        {
            private GameObject _gameObject;
            private int _sortingLayerID;
            private int _sortingOrder;

            public DefaultAdapter(GameObject gameObject)
            {
                _gameObject = gameObject;
            }

            public override GameObject GameObject => _gameObject;

            public override int SortingLayerID
            {
                get => _sortingLayerID;
                set => _sortingLayerID = value;
            }

            public override int SortingOrder
            {
                get => _sortingOrder;
                set => _sortingOrder = value;
            }
        }

        class CompositeAdapter : SortingAdapter
        {
            private List<ISortingAdapter> _adapters;
            private int _count;
            private int _sortingLayerID;
            private int _sortingOrder;

            public CompositeAdapter(List<ISortingAdapter> adapters)
            {
                _count = adapters.Count;
                _adapters = new List<ISortingAdapter>(_count);
                _adapters.AddRange(adapters);
            }

            public override GameObject GameObject => _count > 0 ? _adapters[0].GameObject : null;

            public override int SortingLayerID
            {
                get => _count > 0 ? _adapters[0].SortingLayerID : _sortingLayerID;
                set
                {
                    for (int i = 0; i < _count; i++)
                    {
                        _adapters[i].SortingLayerID = value;
                    }
                    _sortingLayerID = value;
                }
            }

            public override int SortingOrder
            {
                get => _count > 0 ? _adapters[0].SortingOrder : _sortingOrder;
                set
                {
                    for (int i = 0; i < _count; i++)
                    {
                        _adapters[i].SortingOrder = value;
                    }
                    _sortingOrder = value;
                }
            }
        }
    }
}
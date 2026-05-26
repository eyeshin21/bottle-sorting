using UnityEngine;
using UnityEngine.Rendering;

namespace Anvil.Legacy
{
    public class SortingGroupSortingController : ISortingController
    {
        SortingGroup _sortingGroup;

        void Construct(SortingGroup sortingGroup)
        {
            _sortingGroup = sortingGroup;
        }

        public GameObject GameObject => _sortingGroup?.gameObject;

        public int SortingLayerID
        {
            get => _sortingGroup.sortingLayerID;
            set => _sortingGroup.sortingLayerID = value;
        }

        public string SortingLayerName
        {
            get => _sortingGroup.sortingLayerName;
            set => _sortingGroup.sortingLayerName = value;
        }

        public int SortingOrder
        {
            get => _sortingGroup.sortingOrder;
            set => _sortingGroup.sortingOrder = value;
        }

        public void AddSortingOrder(int deltaSortingOrder)
        {
            _sortingGroup.sortingOrder += deltaSortingOrder;
        }

        public void ReturnToPool()
        {
            _sortingGroup = null;
            Pool.Return(this);
        }

        static Pool<SortingGroupSortingController> _pool;
        static Pool<SortingGroupSortingController> Pool => _pool ??= new();

        public static SortingGroupSortingController Create(SortingGroup sortingGroup)
        {
            var controller = Pool.Get();
            controller.Construct(sortingGroup);
            return controller;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class DefaultSortingController : ISortingController
    {
        int _sortingLayerID;
        string _sortingLayerName;
        int _sortingOrder;

        public GameObject GameObject => default;

        public int SortingLayerID
        {
            get => _sortingLayerID;
            set => _sortingLayerID = value;
        }

        public string SortingLayerName
        {
            get => _sortingLayerName;
            set => _sortingLayerName = value;
        }

        public int SortingOrder
        {
            get => _sortingOrder;
            set => _sortingOrder = value;
        }

        public void AddSortingOrder(int deltaSortingOrder)
        {
            _sortingOrder += deltaSortingOrder;
        }

        public void ReturnToPool()
        {
            _sortingLayerID = 0;
            _sortingLayerName = "";
            _sortingOrder = 0;
            Pool.Return(this);
        }

        static Pool<DefaultSortingController> _pool;
        static Pool<DefaultSortingController> Pool => _pool ??= new();

        public static DefaultSortingController Create()
        {
            return Pool.Get();
        }
    }
}
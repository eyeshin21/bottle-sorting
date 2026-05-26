using UnityEngine;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public class CompositeSortingController : CompositeController<ISortingController>, ISortingController
    {
        int _sortingLayerID;
        string _sortingLayerName;
        int _sortingOrder;

        public int SortingLayerID
        {
            get => _count > 0 ? _controllers[0].SortingLayerID : _sortingLayerID;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].SortingLayerID = value;
                }
                _sortingLayerID = value;
            }
        }

        public string SortingLayerName
        {
            get => _count > 0 ? _controllers[0].SortingLayerName : _sortingLayerName;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].SortingLayerName = value;
                }
                _sortingLayerName = value;
            }
        }

        public int SortingOrder
        {
            get => _count > 0 ? _controllers[0].SortingOrder : _sortingOrder;
            set
            {
                for (int i = 0; i < _count; i++)
                {
                    _controllers[i].SortingOrder = value;
                }
                _sortingOrder = value;
            }
        }

        public void AddSortingOrder(int deltaSortingOrder)
        {
            for (int i = 0; i < _count; i++)
            {
                _controllers[i].AddSortingOrder(deltaSortingOrder);
            }
            _sortingOrder += deltaSortingOrder;
        }

        public override void ReturnToPool()
        {
            base.ReturnToPool();
            _sortingLayerID = 0;
            _sortingLayerName = "";
            _sortingOrder = 0;
            Pool.Return(this);
        }

        static Pool<CompositeSortingController> _pool;
        static Pool<CompositeSortingController> Pool => _pool ??= new();

        public static CompositeSortingController Create(List<ISortingController> controllers)
        {
            var controller = Pool.Get();
            controller.Construct(controllers);
            return controller;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class CanvasSortingController : ISortingController
    {
        Canvas _canvas;

        void Construct(Canvas canvas)
        {
            _canvas = canvas;
        }

        public GameObject GameObject => _canvas?.gameObject;

        public int SortingLayerID
        {
            get => _canvas.sortingLayerID;
            set => _canvas.sortingLayerID = value;
        }

        public string SortingLayerName
        {
            get => _canvas.sortingLayerName;
            set => _canvas.sortingLayerName = value;
        }

        public int SortingOrder
        {
            get => _canvas.sortingOrder;
            set => _canvas.sortingOrder = value;
        }

        public void AddSortingOrder(int deltaSortingOrder)
        {
            _canvas.sortingOrder += deltaSortingOrder;
        }

        public void ReturnToPool()
        {
            _canvas = null;
            Pool.Return(this);
        }

        static Pool<CanvasSortingController> _pool;
        static Pool<CanvasSortingController> Pool => _pool ??= new();

        public static CanvasSortingController Create(Canvas canvas)
        {
            var controller = Pool.Get();
            controller.Construct(canvas);
            return controller;
        }
    }
}
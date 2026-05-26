using UnityEngine;

namespace Anvil.Legacy
{
    public class RendererSortingController : ISortingController
    {
        Renderer _renderer;

        void Construct(Renderer renderer)
        {
            _renderer = renderer;
        }

        public GameObject GameObject => _renderer?.gameObject;

        public int SortingLayerID
        {
            get => _renderer.sortingLayerID;
            set => _renderer.sortingLayerID = value;
        }

        public string SortingLayerName
        {
            get => _renderer.sortingLayerName;
            set => _renderer.sortingLayerName = value;
        }

        public int SortingOrder
        {
            get => _renderer.sortingOrder;
            set => _renderer.sortingOrder = value;
        }

        public void AddSortingOrder(int deltaSortingOrder)
        {
            _renderer.sortingOrder += deltaSortingOrder;
        }

        public void ReturnToPool()
        {
            _renderer = null;
            Pool.Return(this);
        }

        static Pool<RendererSortingController> _pool;
        static Pool<RendererSortingController> Pool => _pool ??= new();

        public static RendererSortingController Create(Renderer renderer)
        {
            var controller = Pool.Get();
            controller.Construct(renderer);
            return controller;
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public class SpriteRendererSortingController : ISortingController
    {
        SpriteRenderer _spriteRenderer;

        void Construct(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }

        public GameObject GameObject => _spriteRenderer?.gameObject;

        public int SortingLayerID
        {
            get => _spriteRenderer.sortingLayerID;
            set => _spriteRenderer.sortingLayerID = value;
        }

        public string SortingLayerName
        {
            get => _spriteRenderer.sortingLayerName;
            set => _spriteRenderer.sortingLayerName = value;
        }

        public int SortingOrder
        {
            get => _spriteRenderer.sortingOrder;
            set => _spriteRenderer.sortingOrder = value;
        }

        public void AddSortingOrder(int deltaSortingOrder)
        {
            _spriteRenderer.sortingOrder += deltaSortingOrder;
        }

        public void ReturnToPool()
        {
            _spriteRenderer = null;
            Pool.Return(this);
        }

        static Pool<SpriteRendererSortingController> _pool;
        static Pool<SpriteRendererSortingController> Pool => _pool ??= new();

        public static SpriteRendererSortingController Create(SpriteRenderer spriteRenderer)
        {
            var controller = Pool.Get();
            controller.Construct(spriteRenderer);
            return controller;
        }
    }
}
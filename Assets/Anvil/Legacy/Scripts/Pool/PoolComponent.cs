using UnityEngine;

namespace Anvil.Legacy
{
    public class PoolComponent<T> : PoolObject<T> where T : Component
    {
        public PoolComponent(GameObject prefab, Transform poolTransform = null) : base(prefab, poolTransform)
        {

        }

        public PoolComponent<T> SetNewCallbackByIFinishHandler()
        {
            _SetNewCallbackByIFinishHandler();
            return this;
        }

        protected override T CreateNew(Transform parent)
        {
            return _prefab.Create<T>(parent);
        }

        protected override void OnGet(T component, Transform parent)
        {
            component.transform.SetParent(parent);
            if (_prefab != null)
            {
                component.transform.CopyLocal(_prefab.transform);
            }
            //component.gameObject.SetActive(true);
        }

        protected override void OnReturn(T component)
        {
            component.transform.SetParent(_poolTransform);
            //component.gameObject.SetActive(false);
        }

        protected override TComponent GetComponent<TComponent>(T component)
        {
            return component.GetComponent<TComponent>();
        }

        protected override void Destroy(T component)
        {
            component.gameObject.Destroy();
        }
    }

    /// <summary>
    /// IPoolItem
    /// </summary>
    public class PoolComponentItem<T> : PoolComponent<T> where T : Component, IPoolItem
    {
        public PoolComponentItem(GameObject prefab, Transform poolTransform = null) : base(prefab, poolTransform)
        {

        }

        public override void Return(T item)
        {
            item?.OnReturnPool();
            base.Return(item);
        }
    }
}
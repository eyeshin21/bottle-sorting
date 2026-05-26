using UnityEngine;

namespace Anvil.Legacy
{
    public class PoolGameObject : PoolObject<GameObject>
    {
        public PoolGameObject(GameObject prefab, Transform poolTransform = null) : base(prefab, poolTransform)
        {

        }

        public PoolGameObject SetNewCallbackByIFinishHandler()
        {
            _SetNewCallbackByIFinishHandler();
            return this;
        }

        protected override GameObject CreateNew(Transform parent)
        {
            return _prefab.Create(parent);
        }

        protected override void OnGet(GameObject gameObject, Transform parent)
        {
            gameObject.transform.SetParent(parent);
            if (_prefab != null)
            {
                gameObject.transform.CopyLocal(_prefab.transform);
            }
            //gameObject.SetActive(true);
        }

        protected override void OnReturn(GameObject gameObject)
        {
            gameObject.transform.SetParent(_poolTransform);
            //gameObject.SetActive(false);
        }

        protected override TComponent GetComponent<TComponent>(GameObject gameObject)
        {
            return gameObject.GetComponent<TComponent>();
        }

        protected override void Destroy(GameObject gameObject)
        {
            gameObject.Destroy();
        }
    }
}
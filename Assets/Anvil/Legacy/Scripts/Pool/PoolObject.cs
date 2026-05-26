using UnityEngine;

namespace Anvil.Legacy
{
    public abstract class PoolObject<T> : BasePool<T> where T : Object
    {
        protected GameObject _prefab;
        protected Transform _poolTransform;

        protected Callback<T> _newCallback;
        protected Callback<T> _getCallback;
        protected Callback<T> _returnCallback;

        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        public Transform PoolTransform
        {
            get => _poolTransform;
            set => _poolTransform = value;
        }

        public Callback<T> NewCallback
        {
            get => _newCallback;
            set => _newCallback = value;
        }

        public Callback<T> GetCallback
        {
            get => _getCallback;
            set => _getCallback = value;
        }

        public Callback<T> ReturnCallback
        {
            get => _returnCallback;
            set => _returnCallback = value;
        }

        protected abstract T CreateNew(Transform parent);
        protected abstract void OnGet(T item, Transform parent);
        protected abstract void OnReturn(T item);
        protected abstract TComponent GetComponent<TComponent>(T item);
        protected abstract void Destroy(T item);

        public PoolObject(GameObject prefab, Transform poolTransform = null)
        {
            _prefab = prefab;
            _poolTransform = poolTransform ?? Manager.PoolTransform;
        }

        public void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = CreateNew(null);
                _newCallback?.Invoke(obj);
                Return(obj);
            }
        }

        protected void _SetNewCallbackByIFinishHandler()
        {
            NewCallback = (obj) =>
            {
                var iFinishHandler = GetComponent<IFinishHandler>(obj);
                if (iFinishHandler != null)
                {
                    iFinishHandler.AddOnFinish(() =>
                    {
                        // Fix: Preload + Finish on disabled!
                        if (!_pool.Contains(obj))
                        {
                            Return(obj);
                        }
                    });
                }
            };
        }

        public override T Get()
        {
            return Get(null);
        }

        public virtual T Get(Transform parent)
        {
            T item;
            if (_pool.Count > 0)
            {
                item = _pool.Pop();
                OnGet(item, parent);
                _getCallback?.Invoke(item);
            }
            else
            {
                item = CreateNew(parent);
                _newCallback?.Invoke(item);
            }
            return item;
        }

        /// <summary>
        /// Gets item if it is null.
        /// </summary>
        public virtual void CheckGet(ref T item, Transform parent)
        {
            if (item == null)
            {
                item = Get(parent);
            }
        }

        public override void Return(T item)
        {
            if (item == null) return;
            Assert.NotContains(_pool, item);

            OnReturn(item);
            _returnCallback?.Invoke(item);
            _pool.Push(item);
        }

        /// <summary>
        /// Returns item to pool and set it to null.
        /// </summary>
        public virtual void Return(ref T item)
        {
            if (item != null)
            {
                Return(item);
                item = null;
            }
        }

        public virtual void ReturnItemsInRootChildren(Transform parent)
        {
            if (parent == null) return;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                var item = child.GetComponent<T>();
                if (item != null)
                {
                    Return(item);
                }
                //else
                //{
                //   LegacyLog.Warning($"{child.GetHierarchyPath()}: Missing component {typeof(T)}!");
                //}
            }
        }

        public override void Clear()
        {
            if (_pool.Count > 0)
            {
                foreach (var item in _pool)
                {
                    Destroy(item);
                }
                _pool.Clear();
            }
        }

#if DEBUG_MODE
        public override string ToString()
        {
            return $"[{Helper.GetClassName<T>()}]: prefab={_prefab.GetName()}, poolCount={_pool.Count}";
        }
#endif
    }
}
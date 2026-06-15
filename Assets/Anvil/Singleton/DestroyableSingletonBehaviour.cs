using UnityEngine;

namespace Anvil
{
    public class DestroyableSingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).Name).AddComponent<T>();
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                        {
                            return _instance;
                        }
#endif
                    }
                }

                return _instance;
            }
        }

        public static bool Active => _instance != null;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                OnAwake();
            }
            else if (_instance == this)
            {
                OnAwake();
            }
            else
            {
                Debug.LogWarning($"Invalid singleton behaviour \"{name}\"!");
            }
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
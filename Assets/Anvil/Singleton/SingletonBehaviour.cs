using UnityEngine;

namespace Anvil
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
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
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }

                return _instance;
            }
        }

        public static bool Active => _instance != null;

        public static void CheckActive()
        {
            if (_instance == null)
            {
                _instance = Instance;
            }
        }

        protected virtual void Awake()
        {
            OnAwake();
            if (_instance == null)
            {
#if UNITY_EDITOR
                // Check root game object
                var parent = transform.parent;
                if (parent != null)
                {
                    Debug.LogWarning($"{parent.name}/{name}");
                }
#endif
                _instance = this as T;
                DontDestroyOnLoad(_instance.gameObject);
                //OnAwake();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
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

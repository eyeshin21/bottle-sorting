#define POOLING_ENABLED

using UnityEngine;
using System;
using Anvil;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
{
    public partial class Manager : SingletonBehaviour<Manager>
    {
        private bool _isInited;
        private bool _isPaused;

        public static event Action onUpdate;
        public static event Action onApplicationPause;
        public static event Action onApplicationResume;
        public static event Action onApplicationQuit;
        private static event Action _onInputAny;
        private static event Action _onInputAnyOnce;
        private static event Action _onInputReleaseAny;

        public static void AddOnInputAny(Action action)
        {
            _onInputAny += action;
        }

        public static void AddOnInputReleaseAny(Action action)
        {
            _onInputReleaseAny += action;
        }
        public static void RemoveOnInputAny(Action action)
        {
            _onInputAny -= action;
        }

        public static void RemoveOnInputRelease(Action action)
        {
            _onInputReleaseAny -= action;
        }

        public static void AddOnInputAnyOnce(Action action)
        {
            _onInputAnyOnce += action;
        }
        protected override void OnAwake()
        {
            base.OnAwake();
#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += OnEditorApplicationPause;
#endif
        }
#if UNITY_EDITOR

        protected override void OnDestroy()
        {
            EditorApplication.pauseStateChanged -= OnEditorApplicationPause;
            base.OnDestroy();
        }

        void OnEditorApplicationPause(PauseState pauseState)
        {
            OnApplicationPause(pauseState == PauseState.Paused);
        }
#endif

        /// <summary>
        /// Called from Loading.
        /// </summary>
        public async void Init(Action callback)
        {
            if (_isInited)
            {
                callback?.Invoke();
                return;
            }
            _isInited = true;

            Input.multiTouchEnabled = true;

#if POOLING_ENABLED
            var commonPoolParent = CreatePoolParent("CommonPool");
            GameObjectPool.Init(commonPoolParent);
#endif

            TimeHelper.Init();
            // _loadStartTime = DateTime.Now;
              
            // RemoteConfig.Init();
            AudioManager.Instance.Init();
            // AudioManager.MusicEnabled = UserDataSerializer.MusicEnabled;
            // AudioManager.SoundEnabled = UserDataSerializer.SoundEnabled;
#if VIBRATION_ENABLED
            Vibration.Init();
#endif

            callback?.Invoke();
        }

        public static Transform CreatePoolParent(string name)
        {
            var child = new GameObject(name).transform;
            child.SetParent(Instance.transform);
            child.gameObject.SetActive(false);
            return child;
        }

        private static bool _hasInput = false;
        void Update()
        {
            _delayCallManager.OnUpdate();
#if DEBUG_MODE
#endif
            UpdateKey();
            onUpdate?.Invoke();
        }
        /// <summary>
        /// Called from KeyManager.
        /// Returns true if processed key.
        /// </summary>
        public static bool UpdateKey()
        {
#if DEBUG_MODE
            
          
#endif

            if (_hasInput && !Input.GetMouseButton(0) && Input.touchCount == 0)
            {
                _hasInput = false;
                _onInputReleaseAny?.Invoke();    
            }
            if (Input.anyKey || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                _hasInput = true;
                _onInputAny?.Invoke();
                _onInputAnyOnce?.Invoke();
                _onInputAnyOnce = null;
            }
            
            return false;
        }
        void OnApplicationPause(bool pauseStatus)
        {
#if USE_IRON_SOURCE
            IronSource.Agent.onApplicationPause(pauseStatus);
#endif
            //if (AdsManager.ShowingAd) return;
            //if (ProcessIAPSystem.Buying) return;
            if (pauseStatus)
            {
                onApplicationPause?.Invoke();
            }
            else
            {
                if (_isPaused)
                {
                    onApplicationResume?.Invoke();
                }
            }
            if (_isInited)
            {
                //UserData.OnApplicationPause(pauseStatus);
            }
            _isPaused = pauseStatus;
        }

        void OnApplicationQuit()
        {
            onApplicationQuit?.Invoke();
        }
    }
}
using UnityEngine;
using System.Collections;
using Anvil;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public partial class Manager : SingletonBehaviour<Manager>
    {
        bool _isInited;
        bool _isPaused;

#if UNITY_EDITOR
        protected override void OnAwake()
        {
            base.OnAwake();
            EditorApplication.pauseStateChanged += OnEditorApplicationPause;
        }

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
        public void Init(Callback callback)
        {
            if (_isInited)
            {
                callback?.Invoke();
                return;
            }
            _isInited = true;

            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;

            TimeHelper.Init();
            // RemoteConfig.Init();
            // SessionTracker.Init();
            // Vibration.Init();

            // Gametamin.Core.IAPHelper.Init();

            InternetHelper.Init(()=>
            {
              callback?.Invoke();
            });

#if DEBUG_MODE
#endif
        }

        public static void CheckActive()
        {
            if (_instance == null)
            {
                _instance = Instance;
                _instance.Init(null);
            }
        }

        #region Pool
        static Transform _poolTransform;
        public static Transform PoolTransform
        {
            get
            {
                if (_poolTransform == null)
                {
                    var pool = new GameObject("Pool");
                    pool.SetActive(false);
                    _poolTransform = pool.transform;
                    _poolTransform.SetParent(Instance.transform);
                }
                return _poolTransform;
            }
        }
        #endregion

        void Update()
        {
            _delayCallManager.OnUpdate();
#if DEBUG_MODE
            UpdateDebug();
#endif
            _onUpdate?.Invoke();
        }

        void LateUpdate()
        {
            _onLateUpdate?.Invoke();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _onApplicationPause?.Invoke();
            }
            else
            {
                if (_isPaused)
                {
                    _onApplicationResume?.Invoke();
                }
            }

            _isPaused = pauseStatus;
        }

        void OnApplicationQuit()
        {
            _onApplicationQuit?.Invoke();
        }

        public static Coroutine ExecCoroutine(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }
    }
}
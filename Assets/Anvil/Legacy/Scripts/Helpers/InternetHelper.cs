//#if DEBUG_MODE
//#define DEBUG_INTERNET_HELPER
//#endif
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

namespace Anvil.Legacy
{
    public static class InternetHelper
    {
        static readonly int CheckInternetConnectionTimeout = 3;

        static readonly float DefaultHasConnectionDelay = 3;
        static readonly float DefaultHasConnectionDelayStep = 1;
        static readonly float DefaultHasConnectionMaxDelay = 10;

        static readonly float DefaultNotHasConnectionDelay = 3;
        static readonly float DefaultNotHasConnectionDelayStep = 1;
        static readonly float DefaultNotHasConnectionMaxDelay = 15;

        static float HasConnectionDelay = DefaultHasConnectionDelay;
        static float HasConnectionDelayStep = DefaultHasConnectionDelayStep;
        static float HasConnectionMaxDelay = DefaultHasConnectionMaxDelay;

        static float NotHasConnectionDelay = DefaultNotHasConnectionDelay;
        static float NotHasConnectionDelayStep = DefaultNotHasConnectionDelayStep;
        static float NotHasConnectionMaxDelay = DefaultNotHasConnectionMaxDelay;

        static bool _hasConnection;
        static float _lastTime;
        static float _hasConnectionDelay = HasConnectionDelay;
        static float _notHasConnectionDelay = NotHasConnectionDelay;
        static float _checkDelayTime;
        static bool _isChecking = true;
#if DEBUG_MODE
        static bool _isQuickCheck;
#endif

        public static bool HasConnection
        {
            get => _hasConnection;
            set
            {
                if (value != _hasConnection)
                {
#if DEBUG_INTERNET_HELPER
                   LegacyLog.Debug($"Connection changed: {_hasConnection} => {value}, lastTime: {TimeHelper.SecondsFromStart.ToHMSString()}");
#endif
                    _hasConnection = value;
                    _lastTime = TimeHelper.SecondsFromStart;
                    onConnectionChanged?.Invoke(_hasConnection);
                }
            }
        }

        public static float LastTime => _lastTime;

        /// <summary>
        /// onConnectionChanged(hasConnection)
        /// </summary>
        public static event Callback<bool> onConnectionChanged;
        static Callback<bool> _checkConnectionCallback;

        /// <summary>
        /// Called from Manager.
        /// </summary>
        public static void Init(Callback callback, int timeout = -1)
        {
            InitTracker.Track("InternetHelper");
#if DEBUG_INTERNET_HELPER
           LegacyLog.Debug("InternetChecker.Init");
#endif
            LoadingTracker.Track("InternetHelper", ref callback);
            _isChecking = true;
            Manager.Instance.StartCoroutine(CheckConnectionCoroutine(hasConnection =>
            {
                _isChecking = false;
                HasConnection = hasConnection;
                _checkDelayTime = hasConnection ? HasConnectionDelay : NotHasConnectionDelay;
                callback?.Invoke();
            }, timeout));

            Manager.AddOnUpdate(Update);
            Manager.AddOnApplicationResume(ForceCheck);
        }

        public static void QuickCheck()
        {
            HasConnectionDelay = 1;
            HasConnectionDelayStep = 1;
#if UNITY_EDITOR
            HasConnectionMaxDelay = 2;
#else
            HasConnectionMaxDelay = 3;
#endif

            NotHasConnectionDelay = 3;
            NotHasConnectionDelayStep = 1;
#if UNITY_EDITOR
            NotHasConnectionMaxDelay = 2;
#else
            NotHasConnectionMaxDelay = 5;
#endif

#if DEBUG_MODE
            _isQuickCheck = true;
#endif
            ForceCheck();
        }

        public static void NormalCheck()
        {
            HasConnectionDelay = DefaultHasConnectionDelay;
            HasConnectionDelayStep = DefaultHasConnectionDelayStep;
            HasConnectionMaxDelay = DefaultHasConnectionMaxDelay;

            NotHasConnectionDelay = DefaultNotHasConnectionDelay;
            NotHasConnectionDelayStep = DefaultNotHasConnectionDelayStep;
            NotHasConnectionMaxDelay = DefaultNotHasConnectionMaxDelay;

#if DEBUG_MODE
            _isQuickCheck = false;
#endif
        }

        public static void CheckConnection(Callback<bool> callback)
        {
            _checkConnectionCallback += callback;
            ForceCheck();
        }

        public static void ForceCheck()
        {
#if DEBUG_INTERNET_HELPER
           LegacyLog.Debug("InternetChecker.ForceCheck");
#endif
            _checkDelayTime = 0;
            Update();
        }

        static void Update()
        {
            if (_isChecking) return;

            _checkDelayTime -= Time.deltaTime;
            if (_checkDelayTime < 0)
            {
                _isChecking = true;
                Manager.Instance.StartCoroutine(CheckConnectionCoroutine(hasConnection =>
                {
                    _isChecking = false;

                    if (_hasConnection)
                    {
                        if (hasConnection)
                        {
                            _hasConnectionDelay = Math.Min(_hasConnectionDelay + HasConnectionDelayStep, HasConnectionMaxDelay);
                            _checkDelayTime = _hasConnectionDelay;
                        }
                        else
                        {
                            _hasConnectionDelay = Mathf.Min(HasConnectionDelay, HasConnectionMaxDelay);
                            _notHasConnectionDelay = Mathf.Min(NotHasConnectionDelay, NotHasConnectionMaxDelay);
                            _checkDelayTime = _notHasConnectionDelay;

                            HasConnection = false;
                        }
                    }
                    else
                    {
                        if (hasConnection)
                        {
                            _notHasConnectionDelay = Mathf.Min(NotHasConnectionDelay, NotHasConnectionMaxDelay);
                            _hasConnectionDelay = Mathf.Min(HasConnectionDelay, HasConnectionMaxDelay);
                            _checkDelayTime = _hasConnectionDelay;

                            HasConnection = true;
                        }
                        else
                        {
                            _notHasConnectionDelay = Math.Min(_notHasConnectionDelay + NotHasConnectionDelayStep, NotHasConnectionMaxDelay);
                            _checkDelayTime = _notHasConnectionDelay;
                        }
                    }

                    if (_checkConnectionCallback != null)
                    {
                        var callback = _checkConnectionCallback;
                        _checkConnectionCallback = null;
                        callback(hasConnection);
                    }
                }));
            }
        }

        public static IEnumerator CheckConnectionCoroutine(Callback<bool> callback, int timeout = -1)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                callback(true);
                yield break;
            }

            var request = new UnityWebRequest("https://google.com");
            request.timeout = timeout > 0 ? timeout : CheckInternetConnectionTimeout;
            yield return request.SendWebRequest();

            callback(request.result == UnityWebRequest.Result.Success);
            request.Dispose();
        }

#if DEBUG_MODE
        public static void OnGUIDebug()
        {
            if (_isQuickCheck)
            {
                GUILayout.Label("Quick Check");
            }

            GUILayout.Label($"hasConnection: {_hasConnection}, lastTime: {_lastTime.ToHMSString()}");

            if (_isChecking)
            {
                GUILayout.Label("Checking...");
            }
            else
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label($"checkDelayTime: {_checkDelayTime.ToHMSString()}");
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Force Check"))
                    {
                        ForceCheck();
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
#endif
    }
}
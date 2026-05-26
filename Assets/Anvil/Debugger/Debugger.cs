using System;
using System.Text;
using NaughtyAttributes;
using UnityEngine;
using SystemInfo = UnityEngine.Device.SystemInfo;

namespace Anvil
{
    public partial class Debugger : Legacy.SingletonScriptableObject<Debugger>
    {
        [SerializeField] private GameObject _debugOverlayPrefab;
        [SerializeField] private string _storageLocation = "debug_logs";
        [SerializeField] private string _version = "unknown";
        [SerializeField] private int _logEveryRequest = 1;
        [SerializeField] private bool _lagMachineEnabled = false;
        [SerializeField] private int _lagIlterations = 1000000;
        private static readonly string DefaultSessionID = "UNINITIALIZED";
        private string _sessionID = DefaultSessionID;
        private static string _deviceID;
        private static DebugOverlay _debugUI;
        private static StringBuilder _logBuilder = new StringBuilder();
        public static string SessionId => Instance._sessionID.ToString();
        public static string StorageLocation => Instance._storageLocation;
        public static string MetaData =>
            $"SessionID: {SessionId}\n" +
            $"LocalTime: {DateTime.Now}\n" +
            $"WebTime: {TimeHelper.CurrentDateTime}\n" +
            $"Device Model: {SystemInfo.deviceModel}\n" +
            $"Device Type: {SystemInfo.deviceType}\n" +
            $"Operating System: {SystemInfo.operatingSystem}";

        public static void Initialize()
        {
            if (!Application.IsPlaying(Instance))
            {
                return;
            }
            Debug.Log("initializing debugger");
            if (Instance == null)
            {
                Debug.Log("no debugger instance in project");
                return;
            }
            //
            // if (SessionId != DefaultSessionID)
            // {
            //     return;
            // }
            MonoBehaviourMessageForwarder.RegisterCommonUpdate(Instance.OnCommonUpdate);

            _deviceID = SystemInfo.deviceUniqueIdentifier;
            DebugLoggingService.Initialize();
            string platformID = "";
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    platformID = "w";
                    break;
                case RuntimePlatform.Android:
                    platformID = "a";
                    break;
                default:
                    platformID = "u";
                    break;
            }

            string deviceIdShort = _deviceID.Length > 6 ? _deviceID.Substring(_deviceID.Length - 6) : _deviceID;
            string sessionID = System.DateTime.Now.ToString("yyyyMMddHHmmss").GetHashCode().ToString();
            sessionID = sessionID.Length > 6 ? sessionID.Substring(sessionID.Length - 6) : sessionID;
            Instance._sessionID = $"{platformID}.{deviceIdShort}.{sessionID}";
            Instance._storageLocation = $"{platformID}/{deviceIdShort}/{sessionID}";
#if !DEBUG_MODE
            return;            
#endif
            
            Application.logMessageReceived += HandleLog;
            
            Debug.Log("Session Debug ID: " + Instance._sessionID);
            if (Instance._debugOverlayPrefab != null)
            {
                _debugUI = GameObject.Instantiate(Instance._debugOverlayPrefab).GetComponent<DebugOverlay>();
                _debugUI.ShowSessionId(SessionId);
                DontDestroyOnLoad(_debugUI.gameObject);
                MonoBehaviourMessageForwarder.RegisterCommonClockUpdate(() =>
                {
                    _debugUI.ShowLogs($"{DateTime.Now.ToString()}-{Instance._version}");
                });
            }

            Debug.Log("debugger initialized");
        }

        private double _seed;
        private void OnCommonUpdate()
        {
            if (Instance._lagMachineEnabled)
            {
                if (_seed == 0)
                {
                    _seed = DateTime.Now.Ticks;
                }
                for (int i = 0; i < Instance._lagIlterations; i++)
                {
                    _seed = Math.Sin(_seed) * 10000;
                }
            }

            UpdateKeys();
        }

        private static int _requestCount = 0;
        public static void LogRequest(int priority = 1)
        {
            _requestCount += priority;
            // Debug.Log("request count: " + _requestCount);
            if (_requestCount >= Instance._logEveryRequest)
            {
                LogAndClear();
                _requestCount = 0;
            }
        }
        public static void LogAndClear()
        {
            DebugLoggingService.Log(_logBuilder.ToString());
            _logBuilder.Clear();
        }

        private static void HandleLog(string logString, string stackTrace, LogType type)
        {
            _logBuilder.AppendLine($"[{type}] {logString}");
            if (type == LogType.Exception || type == LogType.Error)
            {
                _logBuilder.AppendLine(stackTrace);
                LogRequest();
            }
        }
        
        [SerializeField] private string testLogMessage;
        [Button]
        private void TestLog()
        {
            DebugLoggingService.Log(testLogMessage);
        }

        [Button]
        private void PushLog()
        {
            LogRequest(_logEveryRequest);
        }
    }
}
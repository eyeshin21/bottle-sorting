using UnityEngine;

namespace Anvil
{
    public class LoggingServiceHook : MonoBehaviour
    {
        private static LoggingServiceHook _instance;

        public static LoggingServiceHook Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var searchRet = FindAnyObjectByType<LoggingServiceHook>();
                if (searchRet != null)
                {
                    _instance = searchRet;
                    return _instance;
                }
                GameObject go = new GameObject("LoggingServiceHook");
                if (Application.IsPlaying(go))
                {
                    DontDestroyOnLoad(go);
                }
                _instance = go.AddComponent<LoggingServiceHook>();
                return _instance;
            }
        }
    }
}
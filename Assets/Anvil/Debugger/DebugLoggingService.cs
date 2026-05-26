#define FIREBASE_LOGGING_ENABLED
using System;
using System.Collections;
using Anvil.Legacy;
using UnityEngine;
using UnityEngine.Networking;

namespace Anvil
{
    public class DebugLoggingService : SingletonScriptableObject<DebugLoggingService> 
    {
        [SerializeField] private string _logURL = "https://discord.com/api/webhooks/1479060021324025977/YWsODQ5h2JTr3Ov0427msrfKlsCkENF0FKX5D_Ue8EEcPNp7U-EWpR8sA0SDas5VMuDX";
        public static void Initialize()
        {

        }

        public static void Log(string message)
        {
            if (message.Length > 1800)
                message = message.Substring(message.Length - 1800);
            string log = $"[{Debugger.MetaData}]\n{message}\n\n";
            Debug.Log("logging message: " + log);
            LoggingServiceHook.Instance.StartCoroutine(Send(log, () =>
            {
                LoggingServiceHook.Instance.StopAllCoroutines();
            }));
        }

        static IEnumerator Send(string message, Action callback)
        {
            string log = message;

            string json = "{\"content\":\"```" + Escape(log) + "```\"}";

            var request = new UnityWebRequest(Instance._logURL, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            Debug.Log("request sent, response code: " + request.responseCode);
            callback?.Invoke();
        }
        static string Escape(string s)
        {
            return s.Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n")
                .Replace("\r", "");
        }
    }
}
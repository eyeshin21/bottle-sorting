using UnityEngine;
using System.Diagnostics;
using LogHelper = UnityEngine.Debug;

namespace Anvil
{
    public static class LegacyLog
    {
        [Conditional("DEBUG_MODE")]
        public static void Debug(object message)
        {
            LogHelper.Log(message);
        }

        [Conditional("DEBUG_MODE")]
        public static void Debug(string message)
        {
            LogHelper.Log(message);
        }

        [Conditional("DEBUG_MODE")]
        public static void Debug(string format, params object[] args)
        {
            LogHelper.LogFormat(format, args);
        }

        [Conditional("DEBUG_MODE")]
        public static void Warning(object message)
        {
            LogHelper.LogWarning(message);
        }

        [Conditional("DEBUG_MODE")]
        public static void Warning(string message)
        {
            LogHelper.LogWarning(message);
        }

        [Conditional("DEBUG_MODE")]
        public static void Warning(string format, params object[] args)
        {
            LogHelper.LogWarningFormat(format, args);
        }

        [Conditional("DEBUG_MODE")]
        public static void WarningIfNull(object obj, object message)
        {
            if (obj == null)
            {
                LogHelper.LogWarning(message);
            }
        }

        [Conditional("DEBUG_MODE")]
        public static void Error(object message)
        {
            LogHelper.LogError(message);
        }

        [Conditional("DEBUG_MODE")]
        public static void Error(string message)
        {
            LogHelper.LogError(message);
        }

        [Conditional("DEBUG_MODE")]
        public static void Error(string format, params object[] args)
        {
            LogHelper.LogErrorFormat(format, args);
        }

        [Conditional("DEBUG_MODE")]
        public static void Todo(object message)
        {
            LogHelper.LogWarning($"<b>[TODO]</b> {message}");
        }

        [Conditional("DEBUG_MODE")]
        public static void Todo(string message)
        {
            LogHelper.LogWarning($"<b>[TODO]</b> {message}");
        }

        [Conditional("DEBUG_MODE")]
        public static void Todo(string format, params object[] args)
        {
            LogHelper.LogWarning($"<b>[TODO]</b> {string.Format(format, args)}");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotSupported(object message)
        {
            LogHelper.LogWarning($"<b>[NOT SUPPORTED]</b> {message}");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotSupported(string message)
        {
            LogHelper.LogWarning($"<b>[NOT SUPPORTED]</b> {message}");
        }

        [Conditional("DEBUG_MODE")]
        public static void NotSupported(string format, params object[] args)
        {
            LogHelper.LogWarning($"<b>[NOT SUPPORTED]</b> {string.Format(format, args)}");
        }

        #region Object - Message
        [Conditional("DEBUG_MODE")]
        public static void Debug(object obj, object message)
        {
            Debug($"[{Helper.GetClassName(obj)}] {message}");
        }

        public static void Warning(object obj, object message)
        {
            Warning($"[{Helper.GetClassName(obj)}] {message}");
        }

        public static void Error(object obj, object message)
        {
            Error($"[{Helper.GetClassName(obj)}] {message}");
        }

        public static void Todo(object obj, object message)
        {
            Todo($"[{Helper.GetClassName(obj)}] {message}");
        }

        public static void NotSupported(object obj, object message)
        {
            NotSupported($"[{Helper.GetClassName(obj)}] {message}");
        }
        #endregion

        #region Method
        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string className, string methodName)
        {
            Debug($"{className}.<b>{methodName}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string className, string methodName, string paramName, object paramValue)
        {
            Debug($"{className}.<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string className, string methodName, string paramName, object paramValue, string paramName2, object paramValue2)
        {
            Debug($"{className}.<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string className, string methodName, string paramName, object paramValue, string paramName2, object paramValue2, string paramName3, object paramValue3)
        {
            Debug($"{className}.<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>, {paramName3}=<b>{GetLog(paramValue3)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string className, string methodName, string paramName, object paramValue, string paramName2, object paramValue2, string paramName3, object paramValue3, string paramName4, object paramValue4)
        {
            Debug($"{className}.<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>, {paramName3}=<b>{GetLog(paramValue3)}</b>, {paramName4}=<b>{GetLog(paramValue4)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName)
        {
            Debug($"<b>{methodName}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName, string paramName, object paramValue)
        {
            Debug($"<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName, string paramName, object paramValue, string paramName2, object paramValue2)
        {
            Debug($"<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName, string paramName, object paramValue, string paramName2, object paramValue2, string paramName3, object paramValue3)
        {
            Debug($"<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>, {paramName3}=<b>{GetLog(paramValue3)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName, string paramName, object paramValue, string paramName2, object paramValue2, string paramName3, object paramValue3, string paramName4, object paramValue4)
        {
            Debug($"<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>, {paramName3}=<b>{GetLog(paramValue3)}</b>, {paramName4}=<b>{GetLog(paramValue4)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName, string paramName, object paramValue, string paramName2, object paramValue2, string paramName3, object paramValue3, string paramName4, object paramValue4, string paramName5, object paramValue5)
        {
            Debug($"<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>, {paramName3}=<b>{GetLog(paramValue3)}</b>, {paramName4}=<b>{GetLog(paramValue4)}</b>, {paramName5}=<b>{GetLog(paramValue5)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName, string paramName, object paramValue, string paramName2, object paramValue2, string paramName3, object paramValue3, string paramName4, object paramValue4, string paramName5, object paramValue5, string paramName6, object paramValue6)
        {
            Debug($"<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>, {paramName3}=<b>{GetLog(paramValue3)}</b>, {paramName4}=<b>{GetLog(paramValue4)}</b>, {paramName5}=<b>{GetLog(paramValue5)}</b>, {paramName6}=<b>{GetLog(paramValue6)}</b>");
        }

        [Conditional("DEBUG_MODE")]
        public static void DebugMethod(string methodName, string paramName, object paramValue, string paramName2, object paramValue2, string paramName3, object paramValue3, string paramName4, object paramValue4, string paramName5, object paramValue5, string paramName6, object paramValue6, string paramName7, object paramValue7)
        {
            Debug($"<b>{methodName}</b>: {paramName}=<b>{GetLog(paramValue)}</b>, {paramName2}=<b>{GetLog(paramValue2)}</b>, {paramName3}=<b>{GetLog(paramValue3)}</b>, {paramName4}=<b>{GetLog(paramValue4)}</b>, {paramName5}=<b>{GetLog(paramValue5)}</b>, {paramName6}=<b>{GetLog(paramValue6)}</b>, {paramName7}=<b>{GetLog(paramValue7)}</b>");
        }

        static object GetLog(object value)
        {
            if (value == null) return "(null)";

            if (value is string)
            {
                string s = value as string;
                if (s.Length > 0)
                {
                    char c = s[0];
                    if (c == '{' || c == '[')
                    {
                        return value;
                    }
                }
                return $"\"{value}\"";
            }

            return value;
        }
        #endregion
    }
}
using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static MethodInfo _clearMethod;

        public static void ClearLog()
        {
#if UNITY_EDITOR
            if (_clearMethod == null)
            {
                var assembly = Assembly.GetAssembly(typeof(Editor));
                var type = assembly.GetType("UnityEditor.LogEntries");
                _clearMethod = type.GetMethod("Clear");
            }
            _clearMethod?.Invoke(Defaults.Object, null);
#endif
        }
    }
}
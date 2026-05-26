using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        public static void InfoBox(string messsage)
        {
            if (!messsage.IsNullOrEmpty())
            {
#if UNITY_EDITOR
                EditorGUILayout.HelpBox(messsage, MessageType.Info);
#endif
            }
        }

        public static void WarningBox(string messsage)
        {
            if (!messsage.IsNullOrEmpty())
            {
#if UNITY_EDITOR
                EditorGUILayout.HelpBox(messsage, MessageType.Warning);
#endif
            }
        }

        public static void ErrorBox(string messsage)
        {
            if (!messsage.IsNullOrEmpty())
            {
#if UNITY_EDITOR
                EditorGUILayout.HelpBox(messsage, MessageType.Error);
#endif
            }
        }
    }
}
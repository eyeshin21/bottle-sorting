#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static void OnInspectorGUI(this SerializedObject serializedObject)
        {
            serializedObject.Update();
            var property = serializedObject.GetIterator();

            // Script
            if (property.NextVisible(true))
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(property);
                GUI.enabled = true;
            }

            while (property.NextVisible(false))
            {
                EditorGUILayout.PropertyField(property);
            }

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public static void OnInspectorGUI(this SerializedObject serializedObject, Callback<SerializedProperty> callback)
        {
            serializedObject.Update();
            var property = serializedObject.GetIterator();

            // Script
            if (property.NextVisible(true))
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(property);
                GUI.enabled = true;
            }

            while (property.NextVisible(false))
            {
                callback(property);
            }

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif
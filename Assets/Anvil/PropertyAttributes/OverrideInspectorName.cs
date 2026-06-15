using UnityEngine;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
{
    public class OverrideInspectorName : PropertyAttribute
    {
        string _name;
        string _methodName;
        MethodInfo _methodInfo;

        public OverrideInspectorName(string s)
        {
            if (s.StartsWith("Get", System.StringComparison.OrdinalIgnoreCase))
            {
                _methodName = s;
            }
            else
            {
                _name = s;
            }
        }

#if UNITY_EDITOR
        public string GetName(SerializedProperty property)
        {
            if (!string.IsNullOrEmpty(_methodName))
            {
                if (_methodInfo == null)
                {
                    _methodInfo = property.GetType().GetMethod(_methodName);
                }

                if (_methodInfo != null)
                {
                    return _methodInfo.Invoke(property.serializedObject.targetObject, null).ToString();
                }
            }

            return _name;
        }
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OverrideInspectorName))]
    public class OverrideInspectorNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var inspectorName = attribute as OverrideInspectorName;
            EditorGUI.PropertyField(position, property, new GUIContent(inspectorName.GetName(property)), true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
#endif
}
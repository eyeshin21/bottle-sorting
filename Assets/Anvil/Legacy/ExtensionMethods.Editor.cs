#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static void ScriptField(this SerializedObject serializedObject)
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;
        }

        public static void OnInspectorGUI(this SerializedObject serializedObject, Action<SerializedProperty> callback)
        {
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();

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

        public static void OnInspectorGUI2(this SerializedObject serializedObject, Action<SerializedProperty> callback)
        {
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();

            // Script
            if (property.NextVisible(true))
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(property);
                GUI.enabled = true;
            }

            callback(property);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        public static int GetElementIndex(this SerializedProperty property)
        {
            var propertyPath = property.propertyPath;
            int endPos = propertyPath.LastIndexOf(']');
            int startPos = propertyPath.LastIndexOf('[', endPos);

            return propertyPath.Substring(startPos + 1, endPos - startPos - 1).ToInt(-1);
        }

        public static string GetElementName(this SerializedProperty property)
        {
            var propertyPath = property.propertyPath;
            string arrayName = propertyPath.Substring(0, propertyPath.IndexOf('.'));

            char[] buffer = arrayName.ToCharArray();
            int length = buffer.Length;
            int start = 0;

            // Backward
            int i = length - 1;
            for (; i >= 0; i--)
            {
                if (char.IsUpper(buffer[i]))
                {
                    start = i;
                    break;
                }
            }

            // Forward
            if (i < 0)
            {
                // Skip '_'
                while (start < length && buffer[start] == '_') start++;

                // Uppercase the first character
                char first = buffer[start];
                if (char.IsLower(first))
                {
                    buffer[start] = char.ToUpper(first);
                }
            }

            // Change "ies" to "y"
            if (arrayName.EndsWith("ies"))
            {
                length -= 2;
                buffer[length - 1] = 'y';
            }
            else
            {
                // Remove last 's'
                if (buffer[length - 1] == 's')
                {
                    length--;
                }
            }

            return $"{new string(buffer, start, length - start)} {property.GetElementIndex() + 1}";
        }

        public static SerializedProperty GetSiblingProperty(this SerializedProperty property, string siblingName)
        {
            string siblingPath = property.propertyPath.Replace(property.name, siblingName);
            return property.serializedObject.FindProperty(siblingPath);
        }

        static readonly BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        public static object GetTargetObject(this SerializedProperty property)
        {
            if (property == null) return null;

            object targetObject = property.serializedObject.targetObject;
            var field = targetObject.GetType().GetField(property.propertyPath, _bindingFlags);
            if (field != null)
            {
                return field.GetValue(targetObject);
            }

            var path = property.propertyPath.Replace(".Array.data[", "[");
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    targetObject = GetValue_Imp(targetObject, elementName, index);
                }
                else
                {
                    targetObject = GetValue_Imp(targetObject, element);
                }
            }

            return targetObject;
        }

        static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, _bindingFlags);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, _bindingFlags | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }

        public static void OnDraw(this SerializedProperty property, Rect position, GUIContent label, Action<Rect> callback)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            {
                Rect rect = EditorGUI.PrefixLabel(position, label);

                int indentLevel = EditorGUI.indentLevel;
                float labelWidth = EditorGUIUtility.labelWidth;

                EditorGUI.indentLevel = 0;
                callback(rect);
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.indentLevel = indentLevel;
            }
            EditorGUI.EndProperty();
        }
    }
}
#endif
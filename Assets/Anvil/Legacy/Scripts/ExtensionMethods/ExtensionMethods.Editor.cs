#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static RectTransform ToRectTransform(this MenuCommand menuCommand)
        {
            return menuCommand.context as RectTransform;
        }

        public static Canvas ToCanvas(this MenuCommand menuCommand)
        {
            return menuCommand.context as Canvas;
        }

        public static T To<T>(this MenuCommand menuCommand) where T : class
        {
            return menuCommand.context as T;
        }

        public static bool IsElement(this SerializedProperty property)
        {
            var propertyPath = property.propertyPath;
            return propertyPath[propertyPath.Length - 1] == ']';
        }

        public static int GetElementIndex(this SerializedProperty property)
        {
            var propertyPath = property.propertyPath;
            int length = propertyPath.Length;
            int endIndex = length - 1;
            if (propertyPath[endIndex] == ']')
            {
                int startIndex = propertyPath.LastIndexOf('[', endIndex - 1);
                if (startIndex > 0)
                {
                    return propertyPath.Substring(startIndex + 1, endIndex - startIndex - 1).ToInt(-1);
                }
            }
            return -1;
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
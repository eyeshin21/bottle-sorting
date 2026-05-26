#if UNITY_EDITOR
using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class EditorHelper
    {
        static readonly float MinSpriteHeight = 32;
        static readonly float MaxSpriteHeight = 64;
        public static readonly float PreferredSpriteHeight = 64;

        public static void Invoke(ref MethodInfo methodInfo, string methodName, SerializedProperty property)
        {
            if (methodInfo == null)
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                var targetObject = property.serializedObject.targetObject;
                methodInfo = targetObject.GetType().GetMethod(methodName, bindingFlags);
            }

            if (methodInfo != null)
            {
                methodInfo.Invoke(property.serializedObject.targetObject, null);
            }
        }

        public static float GetSpriteHeight(SerializedProperty property, GUIContent label, Func<SerializedProperty, GUIContent, float> baseFunc)
        {
            var sprite = property.objectReferenceValue as Sprite;
            if (sprite != null)
            {
                return Mathf.Clamp(sprite.GetRectHeight(), MinSpriteHeight, MaxSpriteHeight);
            }
            return baseFunc(property, label);
        }

        public static void SpriteField(Rect position, SerializedProperty property, bool allowSceneObjects = false)
        {
            property.objectReferenceValue = EditorGUI.ObjectField(position, property.objectReferenceValue, typeof(Sprite), allowSceneObjects);
        }

        public static void SpriteField(Rect position, SerializedProperty property, GUIContent label)
        {
            var sprite = property.objectReferenceValue as Sprite;
            if (sprite != null)
            {
                sprite.GetRectSize(out float width, out float height);
                if (height < MinSpriteHeight)
                {
                    width *= MinSpriteHeight / height;
                    height = MinSpriteHeight;
                }
                else if (height > MaxSpriteHeight)
                {
                    width *= MaxSpriteHeight / height;
                    height = MaxSpriteHeight;
                }
                position = EditorGUI.PrefixLabel(position, label);
                if (EditorGUI.indentLevel > 0)
                {
                    int offset = 15;
                    position.x -= offset;
                    position.width = width + offset;
                }
                else
                {
                    position.width = width;
                }
                position.height = height;
                property.objectReferenceValue = EditorGUI.ObjectField(position, property.objectReferenceValue, typeof(Sprite), false);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}
#endif
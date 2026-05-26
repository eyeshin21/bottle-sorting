#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public delegate void OverridePropertyField(Rect rect, SerializedProperty serializedProperty);

    public class BasePropertyDrawer
    {
        static Rect _tmpRect;

        protected static GUIContent GetContent(string label, out float labelWidth)
        {
            return GUIHelper.GetContent(label, out labelWidth);
        }

        protected void PropertyField(Rect contentPosition, SerializedProperty property, string relativeName, GUIContent content, float labelWidth, OverridePropertyField overridePropertyField)
        {
            var propertyRelative = property.FindPropertyRelative(relativeName);
            if (propertyRelative == null)
            {
                EditorGUI.LabelField(contentPosition, $"!{relativeName}");
                return;
            }

            if (overridePropertyField != null)
            {
                if (labelWidth > 0)
                {
                    _tmpRect.x = contentPosition.x - labelWidth;
                    _tmpRect.y = contentPosition.y;
                    _tmpRect.width = labelWidth;
                    _tmpRect.height = contentPosition.height;
                    EditorGUI.LabelField(_tmpRect, content);
                }
                overridePropertyField(contentPosition, propertyRelative);
            }
            else
            {
                EditorGUIUtility.labelWidth = labelWidth;
                EditorGUI.PropertyField(contentPosition, propertyRelative, content);
            }
        }
    }
}
#endif
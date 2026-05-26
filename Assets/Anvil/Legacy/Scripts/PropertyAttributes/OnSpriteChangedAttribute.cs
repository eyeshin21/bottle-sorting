using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class OnSpriteChangedAttribute : OnValueChangedAttribute
    {
        public OnSpriteChangedAttribute() : base("OnSpriteChanged")
        {

        }

        public OnSpriteChangedAttribute(string methodName) : base(methodName)
        {

        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OnSpriteChangedAttribute))]
    public class OnSpriteChangedAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorHelper.GetSpriteHeight(property, label, base.GetPropertyHeight);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorHelper.SpriteField(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                (attribute as OnSpriteChangedAttribute).OnValueChanged(property);
            }
        }
    }
#endif
}
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    [CustomPropertyDrawer(typeof(Sprite))]
    public class SpritePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.targetObject is UnityEngine.UI.Image)
            {
                return base.GetPropertyHeight(property, label);
            }
            return EditorHelper.GetSpriteHeight(property, label, base.GetPropertyHeight);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
                EditorHelper.SpriteField(position, property, label);
        }
    }
}
#endif
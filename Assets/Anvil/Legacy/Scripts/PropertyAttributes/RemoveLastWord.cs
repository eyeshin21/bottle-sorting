using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class RemoveLastWord : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RemoveLastWord))]
    public class RemoveLastWordDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var text = label.text;
            int index = text.LastIndexOf(' ');
            if (index > 0)
            {
                label.text = text.Substring(0, index);
            }
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
#endif
}
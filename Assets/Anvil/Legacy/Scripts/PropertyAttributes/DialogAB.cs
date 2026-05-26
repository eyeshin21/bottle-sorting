using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class DialogAB : PropertyAttribute
    {

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DialogAB))]
    public class DialogABDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int index = property.GetElementIndex();
            label.text = index % 2 == 0 ? "A" : "B";
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
#endif
}
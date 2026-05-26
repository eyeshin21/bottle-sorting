using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [System.Serializable]
    public class MinMaxFloat
    {
        [SerializeField] float _min;
        [SerializeField] float _max;

        public float Min => _min;
        public float Max => _max;

        public MinMaxFloat(float min, float max)
        {
            _min = min;
            _max = max;
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, _min, _max);
        }

        public float GetRandom()
        {
            return Helper.GetRandomRange(_min, _max);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MinMaxFloat))]
    public class MinMaxFloatDrawer : PropertyDrawer
    {
        static readonly GUIContent Min = new GUIContent("Min");
        static readonly GUIContent Max = new GUIContent("Max");
        static readonly float MinWidth = GUIHelper.GetLabelWidth(Min);
        static readonly float MaxWidth = GUIHelper.GetLabelWidth(Max);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                float width = contentPosition.width / 2;
                contentPosition.width = width;
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = MinWidth;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_min"), Min);
                contentPosition.x += width;
                EditorGUIUtility.labelWidth = MaxWidth;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_max"), Max);
                EditorGUIUtility.labelWidth = labelWidth;
            });
        }
    }
#endif
}
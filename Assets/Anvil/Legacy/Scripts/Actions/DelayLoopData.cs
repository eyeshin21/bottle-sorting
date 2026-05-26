using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [Serializable]
    public class DelayLoopData
    {
        [SerializeField] float _delay;
        [SerializeField] float _factor;
        [SerializeField] float _min;

        public float Delay => _delay;
        public float Factor => _factor;
        public float Min => _min;

        public DelayLoopData(float delay, float factor, float min)
        {
            _delay = delay;
            _factor = factor;
            _min = min;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DelayLoopData))]
    public class DelayLoopDataDrawer : PropertyDrawer
    {
        private static readonly GUIContent Delay = new GUIContent("Delay");
        private static readonly GUIContent Factor = new GUIContent("Factor");
        private static readonly GUIContent Min = new GUIContent("Min");
        private static readonly float DelayWidth = GUIHelper.GetLabelWidth(Delay);
        private static readonly float FactorWidth = GUIHelper.GetLabelWidth(Factor);
        private static readonly float MinWidth = GUIHelper.GetLabelWidth(Min);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                float width = contentPosition.width / 3;
                contentPosition.width = width;
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = DelayWidth;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_delay"), Delay);
                contentPosition.x += width;
                EditorGUIUtility.labelWidth = FactorWidth;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_factor"), Factor);
                contentPosition.x += width;
                EditorGUIUtility.labelWidth = MinWidth;
                EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_min"), Min);
                EditorGUIUtility.labelWidth = labelWidth;
            });
        }
    }
#endif
}
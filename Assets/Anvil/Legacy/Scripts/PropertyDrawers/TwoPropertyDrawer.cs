#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public class TwoPropertyDrawer : BasePropertyDrawer
    {
        string _property1, _property2;
        GUIContent _content1, _content2;
        float _labelWidth1, _labelWidth2;
        float _amount1; // [0,1]

        public OverridePropertyField OverridePropertyField1 { get; set; }
        public OverridePropertyField OverridePropertyField2 { get; set; }

        public TwoPropertyDrawer(string label1, string property1, string label2, string property2, float ratio1 = 1, float ratio2 = 1)
        {
            _property1 = property1;
            _property2 = property2;

            _content1 = GetContent(label1, out _labelWidth1);
            _content2 = GetContent(label2, out _labelWidth2);

            float totalRatio = ratio1 + ratio2;
            _amount1 = ratio1 / totalRatio;
        }

        public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                float width = contentPosition.width;
                float width1 = width * _amount1;
                float width2 = width - width1;
                var labelWidth = EditorGUIUtility.labelWidth;
                contentPosition.width = width1;
                PropertyField(contentPosition, property, _property1, _content1, _labelWidth1, OverridePropertyField1);
                contentPosition.x += width1;
                contentPosition.width = width2;
                PropertyField(contentPosition, property, _property2, _content2, _labelWidth2, OverridePropertyField2);
                EditorGUIUtility.labelWidth = labelWidth;
            });
        }
    }
}
#endif
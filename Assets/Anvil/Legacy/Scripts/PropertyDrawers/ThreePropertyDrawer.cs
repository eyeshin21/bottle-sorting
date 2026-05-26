#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public class ThreePropertyDrawer : BasePropertyDrawer
    {
        string _property1, _property2, _property3;
        GUIContent _content1, _content2, _content3;
        float _labelWidth1, _labelWidth2, _labelWidth3;
        float _amount1, _amount2; // [0,1]

        public OverridePropertyField OverridePropertyField1 { get; set; }
        public OverridePropertyField OverridePropertyField2 { get; set; }
        public OverridePropertyField OverridePropertyField3 { get; set; }

        public ThreePropertyDrawer(string label1, string property1,
                                    string label2, string property2,
                                    string label3, string property3,
                                    float ratio1 = 1, float ratio2 = 1, float ratio3 = 1)
        {
            _property1 = property1;
            _property2 = property2;
            _property3 = property3;

            _content1 = GetContent(label1, out _labelWidth1);
            _content2 = GetContent(label2, out _labelWidth2);
            _content3 = GetContent(label3, out _labelWidth3);

            float totalRatio = ratio1 + ratio2 + ratio3;
            _amount1 = ratio1 / totalRatio;
            _amount2 = ratio2 / totalRatio;
        }

        public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                float width = contentPosition.width;
                float width1 = width * _amount1;
                float width2 = width * _amount2;
                float width3 = width - width1 - width2;
                var labelWidth = EditorGUIUtility.labelWidth;
                contentPosition.width = width1;
                PropertyField(contentPosition, property, _property1, _content1, _labelWidth1, OverridePropertyField1);
                contentPosition.x += width1;
                contentPosition.width = width2;
                PropertyField(contentPosition, property, _property2, _content2, _labelWidth2, OverridePropertyField2);
                contentPosition.x += width2;
                contentPosition.width = width3;
                PropertyField(contentPosition, property, _property3, _content3, _labelWidth3, OverridePropertyField3);
                EditorGUIUtility.labelWidth = labelWidth;
            });
        }
    }
}
#endif
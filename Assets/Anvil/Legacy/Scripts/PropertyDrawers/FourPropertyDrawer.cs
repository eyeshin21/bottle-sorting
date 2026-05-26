#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public class FourPropertyDrawer : BasePropertyDrawer
    {
        string _property1, _property2, _property3, _property4;
        GUIContent _content1, _content2, _content3, _content4;
        float _labelWidth1, _labelWidth2, _labelWidth3, _labelWidth4;
        float _amount1, _amount2, _amount3; // [0,1]

        public OverridePropertyField OverridePropertyField1 { get; set; }
        public OverridePropertyField OverridePropertyField2 { get; set; }
        public OverridePropertyField OverridePropertyField3 { get; set; }
        public OverridePropertyField OverridePropertyField4 { get; set; }

        public FourPropertyDrawer(string label1, string property1,
                                    string label2, string property2,
                                    string label3, string property3,
                                    string label4, string property4,
                                    float ratio1 = 1, float ratio2 = 1, float ratio3 = 1, float ratio4 = 1)
        {
            _property1 = property1;
            _property2 = property2;
            _property3 = property3;
            _property4 = property4;

            _content1 = GetContent(label1, out _labelWidth1);
            _content2 = GetContent(label2, out _labelWidth2);
            _content3 = GetContent(label3, out _labelWidth3);
            _content4 = GetContent(label4, out _labelWidth4);

            float totalRatio = ratio1 + ratio2 + ratio3 + ratio4;
            _amount1 = ratio1 / totalRatio;
            _amount2 = ratio2 / totalRatio;
            _amount3 = ratio3 / totalRatio;
        }

        public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.OnDraw(position, label, (contentPosition) =>
            {
                float width = contentPosition.width;
                float width1 = width * _amount1;
                float width2 = width * _amount2;
                float width3 = width * _amount3;
                float width4 = width - width1 - width2 - width3;
                var labelWidth = EditorGUIUtility.labelWidth;
                contentPosition.width = width1;
                PropertyField(contentPosition, property, _property1, _content1, _labelWidth1, OverridePropertyField1);
                contentPosition.x += width1;
                contentPosition.width = width2;
                PropertyField(contentPosition, property, _property2, _content2, _labelWidth2, OverridePropertyField2);
                contentPosition.x += width2;
                contentPosition.width = width3;
                PropertyField(contentPosition, property, _property3, _content3, _labelWidth3, OverridePropertyField3);
                contentPosition.x += width3;
                contentPosition.width = width4;
                PropertyField(contentPosition, property, _property4, _content4, _labelWidth4, OverridePropertyField4);
                EditorGUIUtility.labelWidth = labelWidth;
            });
        }
    }
}
#endif
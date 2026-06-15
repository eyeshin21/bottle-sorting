#pragma warning disable 0414
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
{
    public class ConditionalShow : PropertyAttribute
    {
        enum ParameterType
        {
            None,
            Bool,
            Int,
            Float,
            String,
        }

        private string _sourceField;
        private bool _boolValue;
        private int _intValue;
        private float _floatValue;
        private string _stringValue;
        private int _indent;
        private ParameterType _parameterType;

        public ConditionalShow(string sourceField)
        {
            _sourceField = sourceField;

            _parameterType = ParameterType.None;
        }

        public ConditionalShow(string sourceField, bool value)
        {
            _sourceField = sourceField;
            _boolValue = value;

            _parameterType = ParameterType.Bool;
        }

        public ConditionalShow(string sourceField, int value)
        {
            _sourceField = sourceField;
            _intValue = value;

            _parameterType = ParameterType.Int;
        }

        public ConditionalShow(string sourceField, float value)
        {
            _sourceField = sourceField;
            _floatValue = value;

            _parameterType = ParameterType.Float;
        }

        public ConditionalShow(string sourceField, string value, int indent = 1)
        {
            _sourceField = sourceField;
            _stringValue = value;
            _indent = indent;

            _parameterType = ParameterType.String;
        }

#if UNITY_EDITOR
        public int Indent { get { return _indent; } }

        public bool IsShow(SerializedProperty property)
        {
            string sourcePath = property.propertyPath.Replace(property.name, _sourceField);
            var sourceProperty = property.serializedObject.FindProperty(sourcePath);

            if (sourceProperty != null)
            {
                var sourcePropertyType = sourceProperty.propertyType;

                if (sourcePropertyType == SerializedPropertyType.Boolean)
                {
                    if (_parameterType == ParameterType.None) return sourceProperty.boolValue;
                    if (_parameterType == ParameterType.Bool) return sourceProperty.boolValue == _boolValue;
                }
                else if (sourcePropertyType == SerializedPropertyType.Enum)
                {
                    if (_parameterType == ParameterType.Int) return sourceProperty.intValue == _intValue;
                    if (_parameterType == ParameterType.String) return sourceProperty.enumNames[sourceProperty.enumValueIndex] == _stringValue;
                }
                else if (sourcePropertyType == SerializedPropertyType.Float)
                {
                    if (_parameterType == ParameterType.Float) return sourceProperty.floatValue >= _floatValue;
                }
                else if (sourcePropertyType == SerializedPropertyType.ObjectReference)
                {
                    if (_parameterType == ParameterType.None) return sourceProperty.objectReferenceValue != null;
                    if (_parameterType == ParameterType.Bool) return (sourceProperty.objectReferenceValue != null) == _boolValue;
                }
            }
            else
            {
                Debug.LogWarning("ConditionalShow: Field not found " + _sourceField);
            }

            return false;
        }
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ConditionalShow))]
    public class ConditionalShowDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var conditionalShow = attribute as ConditionalShow;
            if (conditionalShow.IsShow(property))
            {
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = indentLevel + conditionalShow.Indent;
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.indentLevel = indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var conditionalShow = attribute as ConditionalShow;
            if (conditionalShow.IsShow(property))
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }
#endif
}
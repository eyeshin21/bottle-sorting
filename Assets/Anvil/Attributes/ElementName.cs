using UnityEngine;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
{
#pragma warning disable 0414
    public class ElementName : PropertyAttribute
    {
        enum NameType
        {
            ArrayName,
            EnumName,
            Prefix,
            Func,
        }

        private string _prefix;
        private Type _enumType;
        private int _enumStartIndex;
        private NameType _nameType;

        private string _changedMethodName;
        private MethodInfo _changedMethodInfo;
        private object[] _parameters;
        private string _funcMethodName;
        private MethodInfo _funcMethodInfo;
        bool _isInited = false;

        public ElementName()
        {
            _nameType = NameType.ArrayName;
        }

        public ElementName(Type enumType, int startIndex = 0)
        {
            _enumType = enumType;
            _enumStartIndex = startIndex;
            _nameType = NameType.EnumName;
        }

        public ElementName(string prefix, string changedMethodName)
        {
            _prefix = prefix;
            _nameType = NameType.Prefix;
            _changedMethodName = changedMethodName;
        }

        public ElementName(string funcMethodName)
        {
            _funcMethodName = funcMethodName;
            _nameType = NameType.Func;
        }

#if UNITY_EDITOR
        void Init(SerializedProperty property)
        {
            if (!string.IsNullOrEmpty(_changedMethodName))
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                var targetObject = property.serializedObject.targetObject;
                _changedMethodInfo = targetObject.GetType().GetMethod(_changedMethodName, bindingFlags);
                //_parameters = new object[1];
            }

            if (!string.IsNullOrEmpty(_funcMethodName))
            {
                var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                var targetObject = property.serializedObject.targetObject;
                _funcMethodInfo = targetObject.GetType().GetMethod(_funcMethodName, bindingFlags);
                //Assert.IsTrue(_funcMethodInfo != null && _funcMethodInfo.ReturnType.Equals(typeof(string)));
            }
        }

        string GetName(SerializedProperty property)
        {
            if (_nameType == NameType.ArrayName)
            {
                return property.GetElementName();
            }

            int index = property.GetElementIndex();
            if (_nameType == NameType.EnumName)
            {
                return Enum.GetName(_enumType, _enumStartIndex + index);
            }

            if (_nameType == NameType.Prefix)
            {
                if (string.IsNullOrEmpty(_prefix)) return (index + 1).ToString();
                return string.Format("{0} {1}", _prefix, index + 1);
            }

            if (_nameType == NameType.Func)
            {
                if (_funcMethodInfo != null)
                {
                    if (_parameters == null)
                    {
                        _parameters = new object[1];
                    }
                    _parameters[0] = index;
                    return _funcMethodInfo.Invoke(property.serializedObject.targetObject, _parameters).ToString();
                }
            }

            return string.Format("Element {0}", index + 1);
        }

        public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_isInited)
            {
                _isInited = true;
                Init(property);
            }

            label.text = GetName(property);

            if (_changedMethodInfo != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUI.PropertyField(position, property, label, true);
                if (EditorGUI.EndChangeCheck())
                {
                    if (_parameters == null)
                    {
                        _parameters = new object[1];
                    }
                    _parameters[0] = property.GetElementIndex();
                    _changedMethodInfo.Invoke(property.serializedObject.targetObject, _parameters);
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ElementName))]
    public class ElementNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var elementName = attribute as ElementName;
            elementName.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
#endif
}
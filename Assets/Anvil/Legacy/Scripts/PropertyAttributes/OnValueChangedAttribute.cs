using UnityEngine;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class OnValueChangedAttribute : PropertyAttribute
    {
        string _methodName;
#if UNITY_EDITOR
        MethodInfo _methodInfo;
#endif
        public OnValueChangedAttribute(string methodName)
        {
            _methodName = methodName;
        }

#if UNITY_EDITOR
        public void OnValueChanged(SerializedProperty property)
        {
            EditorHelper.Invoke(ref _methodInfo, _methodName, property);
        }

        class TestOnValueChanged : MonoBehaviour
        {
            [SerializeField, OnValueChanged("OnBoolChanged")] bool _bool;
            [SerializeField, OnValueChanged("OnIntChanged")] int _int;
            [SerializeField, OnValueChanged("OnDirChanged")] Direction4 _dir;
            [SerializeField, OnValueChanged("OnSpriteChanged")] Sprite _sprite;
            [SerializeField, OnSpriteChanged("OnSprite2Changed")] Sprite _sprite2;
            [SerializeField, OnValueChanged("OnStringChanged")] string _string;

            void OnBoolChanged()
            {
                LegacyLog.Debug($"OnBoolChanged: {_bool}");
            }

            void OnIntChanged()
            {
                LegacyLog.Debug($"OnIntChanged: {_int}");
            }

            void OnDirChanged()
            {
                LegacyLog.Debug($"OnDirChanged: {_dir}");
            }

            void OnSpriteChanged()
            {
                LegacyLog.Debug($"OnSpriteChanged: {_sprite}");
            }

            void OnSprite2Changed()
            {
                LegacyLog.Debug($"OnSprite2Changed: {_sprite2}");
            }

            void OnStringChanged()
            {
                LegacyLog.Debug($"OnStringChanged: {_string}");
            }

            //[MenuItem("Gametamin/Test/OnValueChanged")]
            static void Test()
            {
                new GameObject("TestOnValueChanged", typeof(TestOnValueChanged));
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
    public class OnValueChangedAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
                (attribute as OnValueChangedAttribute).OnValueChanged(property);
            }
        }
    }
#endif
}
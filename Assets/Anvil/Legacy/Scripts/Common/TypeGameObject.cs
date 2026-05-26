using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [Serializable]
    public class TypeGameObject<T> where T : Enum
    {
        [SerializeField] T _type;
        [SerializeField] GameObject _object;

        public T Type => _type;
        public GameObject GameObject => _object;

        public TypeGameObject(T type, GameObject @object)
        {
            _type = type;
            _object = @object;
        }

        public void Get(out T type, out GameObject @object)
        {
            type = _type;
            @object = _object;
        }

        public override string ToString()
        {
            return $"{_type}:{_object}";
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeGameObject<>))]
    public class TypeGameObjectDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new TwoPropertyDrawer("", "_type", "", "_object", 1, 1);
                _propertyDrawer.OverridePropertyField2 = (rect, prop) =>
                {
                    prop.objectReferenceValue = EditorGUI.ObjectField(rect, prop.objectReferenceValue, typeof(GameObject), false);
                };
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
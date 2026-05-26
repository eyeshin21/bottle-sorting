using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [Serializable]
    public class TypeAmount<T> where T : Enum
    {
        [SerializeField] T _type;
        [SerializeField] int _amount;

        public T Type => _type;
        public int Amount => _amount;

        public TypeAmount(T type, int amount)
        {
            _type = type;
            _amount = amount;
        }

        public void Get(out T type, out int amount)
        {
            type = _type;
            amount = _amount;
        }

        public override string ToString()
        {
            return $"{_type}:{_amount}";
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeAmount<>))]
    public class TypeAmountDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new TwoPropertyDrawer("", "_type", "", "_amount", 5, 1);
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
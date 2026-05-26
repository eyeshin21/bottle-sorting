using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [Serializable]
    public class TypeSprite<T> where T : Enum
    {
        [SerializeField] T _type;
        [SerializeField] Sprite _sprite;

        public T Type => _type;
        public Sprite Sprite => _sprite;

        public TypeSprite(T type, Sprite sprite)
        {
            _type = type;
            _sprite = sprite;
        }

        public void Get(out T type, out Sprite sprite)
        {
            type = _type;
            sprite = _sprite;
        }

        public override string ToString()
        {
            return $"{_type}:{_sprite}";
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeSprite<>))]
    public class TypeSpriteDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorHelper.PreferredSpriteHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new TwoPropertyDrawer("", "_type", "", "_sprite");
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
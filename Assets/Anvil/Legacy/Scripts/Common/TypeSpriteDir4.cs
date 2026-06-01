using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    /// <summary>
    /// Left, Up, Right, Down
    /// </summary>
    [Serializable]
    public class TypeSpriteDir4<T> where T : Enum
    {
        [SerializeField] T _type;
        [SerializeField] Sprite _sprite1;
        [SerializeField] Sprite _sprite2;
        [SerializeField] Sprite _sprite3;
        [SerializeField] Sprite _sprite4;

        public T Type => _type;
        public Sprite Sprite1 => _sprite1;
        public Sprite Sprite2 => _sprite2;
        public Sprite Sprite3 => _sprite3;
        public Sprite Sprite4 => _sprite4;

        public Sprite Left => _sprite1;
        public Sprite Up => _sprite2;
        public Sprite Right => _sprite3;
        public Sprite Down => _sprite4;

        /// <summary>
        /// Left, Up, Right, Down
        /// </summary>
        public Sprite GetSprite(Direction4 direction)
        {
            if (direction == Direction4.Left) return _sprite1;
            if (direction == Direction4.Up) return _sprite2;
            if (direction == Direction4.Right) return _sprite3;
            if (direction == Direction4.Down) return _sprite4;

            return _sprite1;
        }

        public Sprite GetSprite(int index)
        {
            if (index <= 0) return _sprite1;
            if (index == 1) return _sprite2;
            if (index == 2) return _sprite3;
            return _sprite4;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeSpriteDir4<>))]
    public class TypeSpriteDir4Drawer : PropertyDrawer
    {
        static FivePropertyDrawer _propertyDrawer;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorHelper.PreferredSpriteHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new FivePropertyDrawer("", "_type", "", "_sprite1", "", "_sprite2", "", "_sprite3", "", "_sprite4");
            }
            label.tooltip = "Left-Up-Right-Down";
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
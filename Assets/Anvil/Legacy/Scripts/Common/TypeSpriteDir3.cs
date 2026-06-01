using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    /// <summary>
    /// Down, Right, Up (Alphabet)
    /// </summary>
    [System.Serializable]
    public class TypeSpriteDir3<T> where T : System.Enum
    {
        [SerializeField] T _type;
        [SerializeField] Sprite _sprite1;
        [SerializeField] Sprite _sprite2;
        [SerializeField] Sprite _sprite3;

        public T Type => _type;
        public Sprite Sprite1 => _sprite1;
        public Sprite Sprite2 => _sprite2;
        public Sprite Sprite3 => _sprite3;

        public Sprite Down => _sprite1;
        public Sprite Right => _sprite2;
        public Sprite Up => _sprite3;

        public Sprite GetSprite(Direction4 direction)
        {
            if (direction == Direction4.Left) return _sprite2;
            if (direction == Direction4.Up) return _sprite3;
            if (direction == Direction4.Right) return _sprite2;
            if (direction == Direction4.Down) return _sprite1;

            return _sprite1;
        }

        public Sprite GetSprite(int index)
        {
            if (index <= 0) return _sprite1;
            if (index == 1) return _sprite2;
            return _sprite3;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeSpriteDir3<>))]
    public class TypeSpriteDir3Drawer : PropertyDrawer
    {
        static FourPropertyDrawer _propertyDrawer;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorHelper.PreferredSpriteHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new FourPropertyDrawer("", "_type", "", "_sprite1", "", "_sprite2", "", "_sprite3");
            }
            label.tooltip = "Down-Right-Up";
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
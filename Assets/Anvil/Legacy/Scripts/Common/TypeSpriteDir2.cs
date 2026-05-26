using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    /// <summary>
    /// Horizontal, Vertical
    /// </summary>
    [System.Serializable]
    public class TypeSpriteDir2<T> where T : System.Enum
    {
        [SerializeField] T _type;
        [SerializeField] Sprite _sprite1;
        [SerializeField] Sprite _sprite2;

        public T Type => _type;
        public Sprite Sprite1 => _sprite1;
        public Sprite Sprite2 => _sprite2;

        public Sprite Horizontal => _sprite1;
        public Sprite Vertical => _sprite2;

        public Sprite GetSprite(Direction direction)
        {
            if (direction == Direction.Left) return _sprite1;
            if (direction == Direction.Up) return _sprite2;
            if (direction == Direction.Right) return _sprite1;
            if (direction == Direction.Down) return _sprite2;

            return _sprite1;
        }

        public Sprite GetSprite(int index)
        {
            return index <= 0 ? _sprite1 : _sprite2;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeSpriteDir2<>))]
    public class TypeSpriteDir2Drawer : PropertyDrawer
    {
        static ThreePropertyDrawer _propertyDrawer;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorHelper.PreferredSpriteHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new ThreePropertyDrawer("", "_type", "", "_sprite1", "", "_sprite2");
            }
            label.tooltip = "Horizontal-Vertical";
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
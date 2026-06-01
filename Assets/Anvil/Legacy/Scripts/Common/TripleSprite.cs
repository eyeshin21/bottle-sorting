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
    public class TripleSprite
    {
        [SerializeField] Sprite _sprite1;
        [SerializeField] Sprite _sprite2;
        [SerializeField] Sprite _sprite3;

        public Sprite Sprite1 => _sprite1;
        public Sprite Sprite2 => _sprite2;
        public Sprite Sprite3 => _sprite3;

        public Sprite Get(Direction4 direction)
        {
            if (direction == Direction4.Left) return _sprite2;
            if (direction == Direction4.Up) return _sprite3;
            if (direction == Direction4.Right) return _sprite2;
            if (direction == Direction4.Down) return _sprite1;
            return _sprite1;
        }

        public Sprite Get(int index)
        {
            if (index <= 0) return _sprite1;
            if (index == 1) return _sprite2;
            return _sprite3;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TripleSprite))]
    public class TripleSpriteDrawer : PropertyDrawer
    {
        static ThreePropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new ThreePropertyDrawer("", "_sprite1", "", "_sprite2", "", "_sprite3");
                _propertyDrawer.OverridePropertyField2 = (rect, prop) => EditorHelper.SpriteField(rect, prop);
                _propertyDrawer.OverridePropertyField3 = (rect, prop) => EditorHelper.SpriteField(rect, prop);
            }
            label.tooltip = "Down-Right-Up";
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
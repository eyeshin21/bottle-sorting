using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    /// <summary>
    /// False, True
    /// </summary>
    [System.Serializable]
    public class TypeSpriteBool<T> where T : System.Enum
    {
        [SerializeField] T _type;
        [SerializeField] Sprite _sprite1;
        [SerializeField] Sprite _sprite2;

        public T Type => _type;
        public Sprite Sprite1 => _sprite1;
        public Sprite Sprite2 => _sprite2;

        public Sprite False => _sprite1;
        public Sprite True => _sprite2;

        /// <summary>
        /// False=1, True=2
        /// </summary>
        public Sprite GetSprite(bool value)
        {
            return value ? _sprite2 : _sprite1;
        }

        public Sprite GetSprite(int index)
        {
            return index <= 0 ? _sprite1 : _sprite2;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeSpriteBool<>))]
    public class TypeSpriteBoolDrawer : PropertyDrawer
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
            label.tooltip = "False-True";
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
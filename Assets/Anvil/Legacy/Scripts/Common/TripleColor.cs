using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [System.Serializable]
    public class TripleColor
    {
        [SerializeField] Color _color1;
        [SerializeField] Color _color2;
        [SerializeField] Color _color3;

        public Color Color1 => _color1;
        public Color Color2 => _color2;
        public Color Color3 => _color3;

        public void Get(out Color color1, out Color color2, out Color color3)
        {
            color1 = _color1;
            color2 = _color2;
            color3 = _color3;
        }

        public Color Get(int index)
        {
            if (index <= 0) return _color1;
            if (index == 1) return _color2;
            return _color3;
        }

        public bool IsClear()
        {
            return _color1 == Color.clear && _color2 == Color.clear && _color3 == Color.clear;
        }

        public void Reset()
        {
            _color1 = _color2 = _color3 = Defaults.Color;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TripleColor))]
    public class TripleColorDrawer : PropertyDrawer
    {
        static ThreePropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new ThreePropertyDrawer("", "_color1", "", "_color2", "", "_color3");
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [System.Serializable]
    public class DoubleColor
    {
        [SerializeField] Color _color1;
        [SerializeField] Color _color2;

        public Color Color1 => _color1;
        public Color Color2 => _color2;

        public void Get(out Color color1, out Color color2)
        {
            color1 = _color1;
            color2 = _color2;
        }

        public Color Get(int index)
        {
            if (index <= 0) return _color1;
            return _color2;
        }

        public bool IsClear()
        {
            return _color1 == Color.clear && _color2 == Color.clear;
        }

        public void Reset()
        {
            _color1 = _color2 = Defaults.Color;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DoubleColor))]
    public class DoubleColorDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new TwoPropertyDrawer("", "_color1", "", "_color2");
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
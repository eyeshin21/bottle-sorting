using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [System.Serializable]
    public class Padding
    {
        [SerializeField] float _left;
        [SerializeField] float _right;
        [SerializeField] float _top;
        [SerializeField] float _bottom;

        public float Left => _left;
        public float Right => _right;
        public float Top => _top;
        public float Bottom => _bottom;

        public Padding()
        {

        }

        public Padding(float padding)
        {
            _left = _right = _top = _bottom = padding;
        }

        public void Update(ref float left, ref float top, ref float right, ref float bottom)
        {
            left += _left;
            top -= _top;
            right -= _right;
            bottom += _bottom;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Padding))]
    public class PaddingDrawer : PropertyDrawer
    {
        static FourPropertyDrawer _propertyDrawer = new("Left", "_left", "Right", "_right", "Top", "_top", "Bottom", "_bottom");
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
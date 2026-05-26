using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [System.Serializable]
    public class RangeInt
    {
        [SerializeField] int _min;
        [SerializeField] int _max;

        public int Min => _min;
        public int Max => _max;
        public int Delta => _max - _min;

        public RangeInt(int min, int max)
        {
            if (min < max)
            {
                _min = min;
                _max = max;
            }
            else
            {
                _min = max;
                _max = min;
            }
        }

        public int Clamp(int value)
        {
            Assert.IsLessThanOrEquals(_min, _max);
            if (value < _min) return _min;
            if (value > _max) return _max;
            //if (_min < _max)
            //{
            //    if (value < _min) return _min;
            //    if (value > _max) return _max;
            //}
            //else
            //{
            //    if (value < _max) return _max;
            //    if (value > _min) return _min;
            //}
            return value;
        }

        public int GetRandom()
        {
            return Helper.GetRandomRange(_min, _max);
        }

        public override string ToString()
        {
            return $"[{_min},{_max}]";
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(RangeInt))]
    public class RangeIntDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new("Min", "_min", "Max", "_max");
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
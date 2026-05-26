using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    /// <summary>
    /// AnimationCurve + Time (float)
    /// </summary>
    [System.Serializable]
    public class AnimationCurveTime
    {
        [SerializeField] AnimationCurve _curve;
        [SerializeField] float _time = 1;

        public AnimationCurveTime()
        {

        }

        public AnimationCurveTime(AnimationCurve curve, float time)
        {
            _curve = curve;
            _time = time;
        }

        public AnimationCurve Curve => _curve;
        public float Time => _time;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AnimationCurveTime))]
    public class AnimationCurveTimeDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer = new("", "_curve", "Time", "_time", 3, 1);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
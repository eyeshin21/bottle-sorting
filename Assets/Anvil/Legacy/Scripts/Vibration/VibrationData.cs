using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public enum iOSVibrationType
    {
        [InspectorName("0")]
        None,
        [InspectorName("50")]
        Pop,
        [InspectorName("100")]
        Peek,
        [InspectorName("50,50,50")]
        Nope,
    }

    [System.Serializable]
    public class VibrationData
    {
        [SerializeField] string _android = "50";
        [SerializeField] iOSVibrationType _iOS = iOSVibrationType.Pop;

        public string Android => _android;
        public iOSVibrationType iOS => _iOS;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(VibrationData))]
    public class VibrationDataDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new TwoPropertyDrawer("Android", "_android", "iOS", "_iOS");
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
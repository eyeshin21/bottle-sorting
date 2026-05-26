using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [System.Serializable]
    public class NameGameObject
    {
        [SerializeField] string _name;
        [SerializeField] GameObject _gameObject;

        public string Name => _name;
        public GameObject GameObject => _gameObject;

        public void Get(out string name, out GameObject gameObject)
        {
            name = _name;
            gameObject = _gameObject;
        }

        public override string ToString()
        {
            return $"{_name}:{_gameObject}";
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(NameGameObject))]
    public class NameGameObjectDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new TwoPropertyDrawer("", "_name", "", "_gameObject");
            }
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    /// <summary>
    /// Left, Up, Right, Down
    /// </summary>
    [Serializable]
    public class TypeGameObjectDir4<T> where T : Enum
    {
        [SerializeField] T _type;
        [SerializeField] GameObject _gameObject1;
        [SerializeField] GameObject _gameObject2;
        [SerializeField] GameObject _gameObject3;
        [SerializeField] GameObject _gameObject4;

        public T Type => _type;
        public GameObject GameObject1 => _gameObject1;
        public GameObject GameObject2 => _gameObject2;
        public GameObject GameObject3 => _gameObject3;
        public GameObject GameObject4 => _gameObject4;

        public GameObject Left => _gameObject1;
        public GameObject Up => _gameObject2;
        public GameObject Right => _gameObject3;
        public GameObject Down => _gameObject4;

        /// <summary>
        /// Left, Up, Right, Down
        /// </summary>
        public GameObject GetGameObject(Direction direction)
        {
            if (direction == Direction.Left) return _gameObject1;
            if (direction == Direction.Up) return _gameObject2;
            if (direction == Direction.Right) return _gameObject3;
            if (direction == Direction.Down) return _gameObject4;

            return _gameObject1;
        }

        public GameObject GetGameObject(int index)
        {
            if (index <= 0) return _gameObject1;
            if (index == 1) return _gameObject2;
            if (index == 2) return _gameObject3;
            return _gameObject4;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TypeGameObjectDir4<>))]
    public class TypeGameObjectDir4Drawer : PropertyDrawer
    {
        static FivePropertyDrawer _propertyDrawer;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_propertyDrawer == null)
            {
                _propertyDrawer = new FivePropertyDrawer("", "_type", "", "_gameObject1", "", "_gameObject2", "", "_gameObject3", "", "_gameObject4");
            }
            label.tooltip = "Left-Up-Right-Down";
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}
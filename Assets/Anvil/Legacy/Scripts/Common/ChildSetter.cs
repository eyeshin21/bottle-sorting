using UnityEngine;

namespace Anvil.Legacy
{
    public class ChildSetter : MonoBehaviour
    {
        [SerializeField, ElementName("Child")] NameGameObject[] _children;

        public GameObject GetChild(string name)
        {
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    if (child.Name == name)
                    {
                        return child.GameObject;
                    }
                }
            }
            return null;
        }
    }
}
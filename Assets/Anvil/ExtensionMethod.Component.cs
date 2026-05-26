
using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        public static T TryGetComponent<T>(this GameObject root) where T : Component
        {
            if (root == null)
            {
                return null;
            }
            var component = root.GetComponent<T>();
            return component;
        }
        public static T GetOrAddComponentSafe<T>(this GameObject root) where T : Component
        {
            if (root == null)
            {
                return null;
            }
            var component = root.GetComponent<T>();
            if (component == null)
            {
                component = root.AddComponent<T>();
            }
            return component;
        }
    }
}

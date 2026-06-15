using UnityEngine;
using Component = System.ComponentModel.Component;

namespace Anvil.Extension
{
    public static class ComponentExtension
    {
        public static T GetComponentSafe<T>(this GameObject obj) where T : MonoBehaviour
        {
            if (obj == null)
            {
                return null;
            }

            return obj.GetComponent<T>();
        }
    }
}
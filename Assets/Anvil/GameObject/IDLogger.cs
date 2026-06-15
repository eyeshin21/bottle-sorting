using NaughtyAttributes;
using UnityEngine;

namespace Anvil
{
    public class IDLogger : MonoBehaviour
    {
        [Button]
        private void LogID()
        {
            Debug.Log($"ID: {GetInstanceID()}");
        }
    }
}
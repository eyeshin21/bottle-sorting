using UnityEngine;

namespace Anvil.Legacy
{
    public class Tag : MonoBehaviour
    {
        [SerializeField] string _name;

        public string Name => _name;
    }
}
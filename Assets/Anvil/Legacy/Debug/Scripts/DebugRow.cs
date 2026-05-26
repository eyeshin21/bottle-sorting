#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public class DebugRow : MonoBehaviour
    {
        [SerializeField] GameObject _labelText;

        public void SetLabel(string label)
        {
            _labelText.SetText(label);
        }
    }
}
#endif
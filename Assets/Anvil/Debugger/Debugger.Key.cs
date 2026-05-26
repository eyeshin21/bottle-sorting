using UnityEngine;

namespace Anvil
{
    public partial class Debugger
    {
        private void UpdateKeys()
        {
            if (Input.GetKey(KeyCode.F2))
            {
                F2();
            }
        }

        private void F2()
        {
        }
    }
}
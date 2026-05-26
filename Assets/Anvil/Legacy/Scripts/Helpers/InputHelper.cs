using UnityEngine;

namespace Anvil.Legacy
{
    public static class InputHelper
    {
        public static bool IsDecrease(KeyCode keyCode)
        {
            return keyCode == KeyCode.Minus || keyCode == KeyCode.KeypadMinus;
        }

        public static bool IsIncrease(KeyCode keyCode)
        {
            return keyCode == KeyCode.Plus || keyCode == KeyCode.KeypadPlus || keyCode == KeyCode.Equals || keyCode == KeyCode.KeypadEquals;
        }

        public static bool IsKeyDownDecrease()
        {
            return Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus);
        }

        public static bool IsKeyDownIncrease()
        {
            return Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadEquals);
        }

        public static bool GetKeyDownNumber(out int number)
        {
            for (var keyCode = KeyCode.Alpha0; keyCode <= KeyCode.Alpha9; keyCode++)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    number = keyCode - KeyCode.Alpha0;
                    return true;
                }
            }

            number = 0;
            return false;
        }
    }
}
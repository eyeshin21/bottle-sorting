using UnityEngine;

namespace Anvil.Legacy
{
    public partial class UIMeshBehaviour
    {
        protected static bool Set(ref float currentValue, float newValue)
        {
            if (currentValue != newValue)
            {
                currentValue = newValue;
                return true;
            }
            return false;
        }

        protected static bool SetColor(ref Color currentValue, Color newValue)
        {
            if (currentValue.r != newValue.r || currentValue.g != newValue.g || currentValue.b != newValue.b || currentValue.a != newValue.a)
            {
                currentValue = newValue;
                return true;
            }
            return false;
        }

        protected static bool Set<T>(ref T currentValue, T newValue) where T : class
        {
            if (currentValue == null)
            {
                if (newValue == null)
                {
                    return false;
                }
            }
            else
            {
                if (newValue != null && currentValue.Equals(newValue))
                {
                    return false;
                }
            }

            currentValue = newValue;
            return true;
        }
    }
}
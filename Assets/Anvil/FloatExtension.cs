namespace Anvil
{
    public static class FloatExtension
    {
        public static bool IsInRange(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }
    }
}
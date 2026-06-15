namespace Anvil
{
    public static class FloatExtension
    {
        public static bool IsInRange(this float value, float min, float max)
        {
            if (min >= max)
            {
                (min, max) = (max, min);
            }
            return value >= min && value <= max;
        }
    }
}
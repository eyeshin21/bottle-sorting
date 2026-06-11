namespace Anvil
{
    public static class FloatExtension
    {
        public static bool IsInRange(this float value, float min, float max)
        {
            if (min > max)
            {
                float t = min;
                min = max;
                max = t;
            }
            return value >= min && value <= max;
        }
    }
}
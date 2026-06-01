public static class FlagExtensions
{
    public static bool HasFlag(this Directions flag, Directions other)
    {
        return flag.HasFlag(other);
    }

    public static Directions SetFlag(this Directions flag, Directions other)
    {
        if ((flag & other) != 0)
        {
            flag |= other;
        }

        return flag;
    }

    public static bool HasMultipleFlag(this Directions flag)
    {
        int value = (int)flag;
        return value != 0 && (value & (value - 1)) != 0;
    }
}
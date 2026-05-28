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
}
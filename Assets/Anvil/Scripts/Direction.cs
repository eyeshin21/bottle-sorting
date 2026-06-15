public enum Direction
{
    None = -1,
    Left,
    Up,
    Right,
    Down,
}
public enum Direction8
{
    None = -1,
    Left,
    UpLeft,
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
}
public enum Axis
{
    None = -1,
    Horizontal,
    Vertical,
}
public enum FacingDirection
{
    Forward,
    Backward,
}

public static partial class ExtensionMethods
{
    public static Axis ConvertToAxis(this Direction direction)
    {
        if (direction.IsHorizontal()) return Axis.Horizontal;
        if (direction.IsVertical()) return Axis.Vertical;
        return Axis.None;
    }
    
    public static Direction ToDirection(this bool horizontal)
    {
        return horizontal ? Direction.Right : Direction.Down;
    }

    public static bool IsNone(this Direction direction)
    {
        return direction == Direction.None;
    }

    public static bool IsLeft(this Direction direction)
    {
        return direction == Direction.Left;
    }

    public static bool IsUp(this Direction direction)
    {
        return direction == Direction.Up;
    }

    public static bool IsRight(this Direction direction)
    {
        return direction == Direction.Right;
    }

    public static bool IsDown(this Direction direction)
    {
        return direction == Direction.Down;
    }

    public static bool IsHorizontal(this Direction direction)
    {
        return direction == Direction.Left || direction == Direction.Right;
    }

    public static bool IsVertical(this Direction direction)
    {
        return direction == Direction.Up || direction == Direction.Down;
    }

    public static bool IsHorizontalOrVertical(this Direction direction, Direction other)
    {
        if (direction == Direction.Left || direction == Direction.Right)
        {
            return other == Direction.Left || other == Direction.Right;
        }

        if (direction == Direction.Up || direction == Direction.Down)
        {
            return other == Direction.Up || other == Direction.Down;
        }

        return false;
    }

    public static bool IsSame(this Direction direction, Direction other)
    {
        if (direction == Direction.None) return other == Direction.None;
        return direction.IsHorizontal() == other.IsHorizontal();
    }

    public static Direction Reverse(this Direction direction)
    {
        if (direction == Direction.Left) return Direction.Right;
        if (direction == Direction.Right) return Direction.Left;
        if (direction == Direction.Up) return Direction.Down;
        if (direction == Direction.Down) return Direction.Up;

        return Direction.None;
    }

    public static Direction RotateLeft(this Direction direction)
    {
        if (direction > Direction.Left)
        {
            return (Direction)((int)direction - 1);
        }
        return Direction.Down;
    }

    public static Direction RotateRight(this Direction direction)
    {
        if (direction >= Direction.Left && direction < Direction.Down)
        {
            return (Direction)((int)direction + 1);
        }
        return Direction.Left;
    }

    public static Direction Rotate(this Direction direction, bool rotateLeft)
    {
        return rotateLeft ? RotateLeft(direction) : RotateRight(direction);
    }
}
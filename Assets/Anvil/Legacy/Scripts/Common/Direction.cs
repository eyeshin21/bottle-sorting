using UnityEngine;

namespace Anvil.Legacy
{
    /// <summary>
    /// None, Left, Up, Right, Down
    /// </summary>
    public enum Direction
    {
        None,
        Left,
        Up,
        Right,
        Down
    }

    public static partial class ExtensionMethods
    {
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

        public static Direction Reverse(this Direction direction)
        {
            if (direction == Direction.Left) return Direction.Right;
            if (direction == Direction.Up) return Direction.Down;
            if (direction == Direction.Right) return Direction.Left;
            if (direction == Direction.Down) return Direction.Up;

            return direction;
        }

        public static Direction RotateLeft(this Direction direction)
        {
            if (direction == Direction.Left) return Direction.Down;
            if (direction == Direction.Up) return Direction.Left;
            if (direction == Direction.Right) return Direction.Up;
            if (direction == Direction.Down) return Direction.Right;

            return direction;
        }

        public static Direction RotateRight(this Direction direction)
        {
            if (direction == Direction.Left) return Direction.Up;
            if (direction == Direction.Up) return Direction.Right;
            if (direction == Direction.Right) return Direction.Down;
            if (direction == Direction.Down) return Direction.Left;

            return direction;
        }

        public static Vector3 GetDeltaPosition(this Direction direction, float offset)
        {
            Assert.IsPositive(offset);
            if (direction == Direction.Left) return new Vector3(-offset, 0, 0);
            if (direction == Direction.Up) return new Vector3(0, offset, 0);
            if (direction == Direction.Right) return new Vector3(offset, 0, 0);
            if (direction == Direction.Down) return new Vector3(0, -offset, 0);

            return Vector3.zero;
        }

        static float[] _angles;
        static float[] Angles
        {
            get
            {
                if (_angles == null)
                {
                    _angles = new float[]
                    {
                        0,  // None
                        180,// Left
                        90, // Up
                        0,  // Right
                        270 // Down
                    };
                }
                return _angles;
            }
        }

        static Quaternion[] _rotations;
        static Quaternion[] Rotations
        {
            get
            {
                if (_rotations == null)
                {
                    _rotations = new Quaternion[]
                    {
                        Quaternion.identity,        // None
                        Quaternion.Euler(0, 0, 180),// Left
                        Quaternion.Euler(0, 0, 90), // Up
                        Quaternion.identity,        // Right
                        Quaternion.Euler(0, 0, 270) // Down
                    };
                }
                return _rotations;
            }
        }

        /// <summary>
        /// Returns angle between [0,360].
        /// </summary>
        public static float GetAngle(this Direction direction)
        {
            return Angles[(int)direction];
        }

        public static Quaternion GetRotation(this Direction direction)
        {
            return Rotations[(int)direction];
        }

        public static int ToInt(this Direction direction)
        {
            return (int)direction;
        }

        public static Direction ToDirection(this int value)
        {
            return (Direction)value;
        }

        public static Direction ToDirection(this float angle)
        {
            angle = Helper.ClampAngle(angle);
            if (angle > 45)
            {
                if (angle < 135) return Direction.Up;
                if (angle < 225) return Direction.Left;
                if (angle < 315) return Direction.Down;
            }
            return Direction.Right;
        }

        public static void Shift(this Direction direction, ref int row, ref int column)
        {
            if (direction == Direction.Left) column--;
            else if (direction == Direction.Right) column++;
            else if (direction == Direction.Up) row--;
            else if (direction == Direction.Down) row++;
        }

        public static CornerType GetCornerType(this Direction direction, Direction toDirection)
        {
            if (direction == Direction.Left)
            {
                if (toDirection == Direction.Up) return CornerType.BottomLeft;
                if (toDirection == Direction.Down) return CornerType.TopLeft;
            }
            else if (direction == Direction.Right)
            {
                if (toDirection == Direction.Up) return CornerType.BottomRight;
                if (toDirection == Direction.Down) return CornerType.TopRight;
            }
            else if (direction == Direction.Up)
            {
                if (toDirection == Direction.Left) return CornerType.TopRight;
                if (toDirection == Direction.Right) return CornerType.TopLeft;
            }
            else if (direction == Direction.Down)
            {
                if (toDirection == Direction.Left) return CornerType.BottomRight;
                if (toDirection == Direction.Right) return CornerType.BottomLeft;
            }

            //Assert.Todo($"{direction} -> {toDirection}");
            return CornerType.None;
        }
    }
}
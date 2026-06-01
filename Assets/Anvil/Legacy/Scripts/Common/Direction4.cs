using UnityEngine;

namespace Anvil.Legacy
{
    /// <summary>
    /// None, Left, Up, Right, Down
    /// </summary>
    public enum Direction4
    {
        None,
        Left,
        Up,
        Right,
        Down
    }

    public static partial class ExtensionMethods
    {
        public static bool IsNone(this Direction4 direction)
        {
            return direction == Direction4.None;
        }

        public static bool IsLeft(this Direction4 direction)
        {
            return direction == Direction4.Left;
        }

        public static bool IsUp(this Direction4 direction)
        {
            return direction == Direction4.Up;
        }

        public static bool IsRight(this Direction4 direction)
        {
            return direction == Direction4.Right;
        }

        public static bool IsDown(this Direction4 direction)
        {
            return direction == Direction4.Down;
        }

        public static bool IsHorizontal(this Direction4 direction)
        {
            return direction == Direction4.Left || direction == Direction4.Right;
        }

        public static bool IsVertical(this Direction4 direction)
        {
            return direction == Direction4.Up || direction == Direction4.Down;
        }

        public static Direction4 Reverse(this Direction4 direction)
        {
            if (direction == Direction4.Left) return Direction4.Right;
            if (direction == Direction4.Up) return Direction4.Down;
            if (direction == Direction4.Right) return Direction4.Left;
            if (direction == Direction4.Down) return Direction4.Up;

            return direction;
        }

        public static Direction4 RotateLeft(this Direction4 direction)
        {
            if (direction == Direction4.Left) return Direction4.Down;
            if (direction == Direction4.Up) return Direction4.Left;
            if (direction == Direction4.Right) return Direction4.Up;
            if (direction == Direction4.Down) return Direction4.Right;

            return direction;
        }

        public static Direction4 RotateRight(this Direction4 direction)
        {
            if (direction == Direction4.Left) return Direction4.Up;
            if (direction == Direction4.Up) return Direction4.Right;
            if (direction == Direction4.Right) return Direction4.Down;
            if (direction == Direction4.Down) return Direction4.Left;

            return direction;
        }

        public static Vector3 GetDeltaPosition(this Direction4 direction, float offset)
        {
            Assert.IsPositive(offset);
            if (direction == Direction4.Left) return new Vector3(-offset, 0, 0);
            if (direction == Direction4.Up) return new Vector3(0, offset, 0);
            if (direction == Direction4.Right) return new Vector3(offset, 0, 0);
            if (direction == Direction4.Down) return new Vector3(0, -offset, 0);

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
        public static float GetAngle(this Direction4 direction)
        {
            return Angles[(int)direction];
        }

        public static Quaternion GetRotation(this Direction4 direction)
        {
            return Rotations[(int)direction];
        }

        public static int ToInt(this Direction4 direction)
        {
            return (int)direction;
        }

        public static Direction4 ToDirection(this int value)
        {
            return (Direction4)value;
        }

        public static Direction4 ToDirection(this float angle)
        {
            angle = Helper.ClampAngle(angle);
            if (angle > 45)
            {
                if (angle < 135) return Direction4.Up;
                if (angle < 225) return Direction4.Left;
                if (angle < 315) return Direction4.Down;
            }
            return Direction4.Right;
        }

        public static void Shift(this Direction4 direction, ref int row, ref int column)
        {
            if (direction == Direction4.Left) column--;
            else if (direction == Direction4.Right) column++;
            else if (direction == Direction4.Up) row--;
            else if (direction == Direction4.Down) row++;
        }

        public static CornerType GetCornerType(this Direction4 direction, Direction4 toDirection)
        {
            if (direction == Direction4.Left)
            {
                if (toDirection == Direction4.Up) return CornerType.BottomLeft;
                if (toDirection == Direction4.Down) return CornerType.TopLeft;
            }
            else if (direction == Direction4.Right)
            {
                if (toDirection == Direction4.Up) return CornerType.BottomRight;
                if (toDirection == Direction4.Down) return CornerType.TopRight;
            }
            else if (direction == Direction4.Up)
            {
                if (toDirection == Direction4.Left) return CornerType.TopRight;
                if (toDirection == Direction4.Right) return CornerType.TopLeft;
            }
            else if (direction == Direction4.Down)
            {
                if (toDirection == Direction4.Left) return CornerType.BottomRight;
                if (toDirection == Direction4.Right) return CornerType.BottomLeft;
            }

            //Assert.Todo($"{direction} -> {toDirection}");
            return CornerType.None;
        }
    }
}
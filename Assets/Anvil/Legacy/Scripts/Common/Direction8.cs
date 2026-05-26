using UnityEngine;

namespace Anvil.Legacy
{
    public enum Direction8
    {
        None,

        Left,
        Up,
        Right,
        Down,

        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
    }

    public static partial class ExtensionMethods
    {
        public static bool IsNone(this Direction8 direction)
        {
            return direction == Direction8.None;
        }

        public static bool IsLeft(this Direction8 direction)
        {
            return direction == Direction8.Left;
        }

        public static bool IsUp(this Direction8 direction)
        {
            return direction == Direction8.Up;
        }

        public static bool IsRight(this Direction8 direction)
        {
            return direction == Direction8.Right;
        }

        public static bool IsDown(this Direction8 direction)
        {
            return direction == Direction8.Down;
        }

        public static bool IsHorizontal(this Direction8 direction)
        {
            return direction == Direction8.Left || direction == Direction8.Right;
        }

        public static bool IsVertical(this Direction8 direction)
        {
            return direction == Direction8.Up || direction == Direction8.Down;
        }

        public static Direction8 Reverse(this Direction8 direction)
        {
            if (direction == Direction8.Left) return Direction8.Right;
            if (direction == Direction8.Up) return Direction8.Down;
            if (direction == Direction8.Right) return Direction8.Left;
            if (direction == Direction8.Down) return Direction8.Up;

            if (direction == Direction8.UpLeft) return Direction8.DownRight;
            if (direction == Direction8.UpRight) return Direction8.DownLeft;
            if (direction == Direction8.DownLeft) return Direction8.UpRight;
            if (direction == Direction8.DownRight) return Direction8.UpLeft;

            return direction;
        }

        public static Direction8 RotateLeft(this Direction8 direction)
        {
            if (direction == Direction8.Left) return Direction8.DownLeft;
            if (direction == Direction8.Up) return Direction8.UpLeft;
            if (direction == Direction8.Right) return Direction8.UpRight;
            if (direction == Direction8.Down) return Direction8.DownRight;

            if (direction == Direction8.UpLeft) return Direction8.Left;
            if (direction == Direction8.UpRight) return Direction8.Up;
            if (direction == Direction8.DownLeft) return Direction8.Down;
            if (direction == Direction8.DownRight) return Direction8.Right;

            return direction;
        }

        public static Direction8 RotateRight(this Direction8 direction)
        {
            if (direction == Direction8.Left) return Direction8.UpLeft;
            if (direction == Direction8.Up) return Direction8.UpRight;
            if (direction == Direction8.Right) return Direction8.DownRight;
            if (direction == Direction8.Down) return Direction8.DownLeft;

            if (direction == Direction8.UpLeft) return Direction8.Up;
            if (direction == Direction8.UpRight) return Direction8.Right;
            if (direction == Direction8.DownLeft) return Direction8.Left;
            if (direction == Direction8.DownRight) return Direction8.Down;

            return direction;
        }

        public static int ToInt(this Direction8 direction)
        {
            return (int)direction;
        }

        public static Direction8 ToDirection8(this int value)
        {
            return (Direction8)value;
        }

        public static Direction8 ToDirection8(this float angle, float minAngle)
        {
            angle = Helper.ClampAngle(angle);
            if (angle > minAngle)
            {
                if (angle < 90 - minAngle) return Direction8.UpRight;
                if (angle < 90 + minAngle) return Direction8.Up;
                if (angle < 180 - minAngle) return Direction8.UpLeft;
                if (angle < 180 + minAngle) return Direction8.Left;
                if (angle < 270 - minAngle) return Direction8.DownLeft;
                if (angle < 270 + minAngle) return Direction8.Down;
                if (angle < 360 - minAngle) return Direction8.DownRight;
            }
            return Direction8.Right;
        }
    }
}
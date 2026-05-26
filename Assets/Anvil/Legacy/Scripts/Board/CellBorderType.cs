using UnityEngine;

namespace Anvil.Legacy
{
    [System.Flags]
    public enum CellBorderType
    {
        /*
         * ....
         * .  .
         * ....
         */
        None,

        /*
         * ---
         * ...
         */
        Top = 1,
        /*
         * ...
         * |
         * ...
         */
        Left = 2,
        /*
         * ...
         *   |
         * ...
         */
        Right = 4,
        /*
         * ...
         * ---
         */
        Bottom = 8,

        /*
         * ---
         * 
         * ---
         */
        TopBottom = Top | Bottom,
        /*
         * ....
         * |  |
         * ....
         */
        LeftRight = Left | Right,

        /*
         * +--
         * |
         */
        TopLeft = Top | Left,
        /*
         * --+
         *   |
         */
        TopRight = Top | Right,
        /*
         * +--+
         * |  |
         */
        TopLeftRight = Top | LeftRight,

        /*
         * +--
         * |
         * +--
         */
        LeftTopBottom = Left | TopBottom,
        /*
         * --+
         *   |
         * --+
         */
        RightTopBottom = Right | TopBottom,


        /*
         * |
         * +--
         */
        BottomLeft = Bottom | Left,
        /*
         *   |
         * --+
         */
        BottomRight = Bottom | Right,
        /*
         * |  |
         * +--+
         */
        BottomLeftRight = Bottom | LeftRight,

        /*
         * +--+
         * |  |
         * +--+
         */
        All = TopBottom | LeftRight,
    }

    public static partial class ExtensionMethods
    {
        public static bool HasBottom(this CellBorderType borderType)
        {
            return FlagHelper.IsOn(borderType, CellBorderType.Bottom);
        }

        public static CellBorderType AddBottom(this CellBorderType borderType)
        {
            return FlagHelper.Add(borderType, CellBorderType.Bottom);
        }

        public static CellBorderType RemoveTop(this CellBorderType borderType)
        {
            return FlagHelper.Remove(borderType, CellBorderType.Top);
        }

        public static CellBorderType RemoveLeft(this CellBorderType borderType)
        {
            return FlagHelper.Remove(borderType, CellBorderType.Left);
        }

        public static CellBorderType RemoveRight(this CellBorderType borderType)
        {
            return FlagHelper.Remove(borderType, CellBorderType.Right);
        }
    }
}
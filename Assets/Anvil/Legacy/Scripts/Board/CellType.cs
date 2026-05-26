using UnityEngine;

namespace Anvil.Legacy
{
    [System.Flags]
    public enum CellType
    {
        None = 0,

        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,

        LeftTop = Left | Top,
        LeftBottom = Left | Bottom,
        RightTop = Right | Top,
        RightBottom = Right | Bottom,
        LeftRight = Left | Right,
        TopBottom = Top | Bottom,

        LeftTopBottom = Left | TopBottom,
        RightTopBottom = Right | TopBottom,
        TopLeftRight = Top | LeftRight,
        BottomLeftRight = Bottom | LeftRight,

        All = LeftRight | TopBottom,
    }
}
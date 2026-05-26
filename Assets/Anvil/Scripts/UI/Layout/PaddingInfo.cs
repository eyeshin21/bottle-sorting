using System;

namespace Anvil
{
    [Serializable]
    public struct PaddingInfo
    {
        public int _top;
        public int _bottom;
        public int _left;
        public int _right;
        public int _spacing;

        public PaddingInfo(int space = 0, int top = 0, int bottom = 0, int left = 0, int right = 0)
        {
            _spacing = space;
            _top = top;
            _bottom = bottom;
            _left = left;
            _right = right;
        }

        public static PaddingInfo Default = new PaddingInfo();
    }
}

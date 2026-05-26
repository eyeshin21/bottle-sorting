using UnityEngine;

namespace Anvil.Legacy
{
    public struct SizeInt
    {
        public int width;
        public int height;

        public SizeInt(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Get(out int rowCount, out int columnCount)
        {
            rowCount = height;
            columnCount = width;
        }

        public override string ToString()
        {
            return $"{width}x{height}";
        }
    }
}
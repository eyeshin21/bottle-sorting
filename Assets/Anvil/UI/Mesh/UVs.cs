using UnityEngine;

namespace Anvil
{
    /*
     *  L   X      Z   R
     *  ^   ^      ^   ^
     *  |   |      |   |
     *  +---+------+---+---> T
     *  | 0 |  1   | 2 |
     *  +---+------+---+---> W
     *  |   |      |   |
     *  | 3 |  4   | 5 |
     *  |   |      |   |
     *  +---+------+---+---> Y
     *  | 6 |  7   | 8 |
     *  +---+------+---+---> B
     */
    public class UVs
    {
        private float _left;
        private float _top;
        private float _right;
        private float _bottom;
        private float _x;
        private float _y;
        private float _z;
        private float _w;

        public float Left => _left;
        public float Top => _top;
        public float Right => _right;
        public float Bottom => _bottom;
        public float X => _x;
        public float Y => _y;
        public float Z => _z;
        public float W => _w;

        public UVs()
        {

        }

        public UVs(Sprite sprite)
        {
            Construct(sprite);
        }

        public void Construct(Sprite sprite)
        {
            sprite.GetUVs(out _left, out _top, out _right, out _bottom, out _x, out _y, out _z, out _w);
        }

        public void Get3x3UVs(int id, out float uvLeft, out float uvTop, out float uvRight, out float uvBottom)
        {
            if (id == 0)
            {
                uvLeft = _left;
                uvTop = _top;
                uvRight = _x;
                uvBottom = _w;
            }
            else if (id == 1)
            {
                uvLeft = _x;
                uvTop = _top;
                uvRight = _z;
                uvBottom = _w;
            }
            else if (id == 2)
            {
                uvLeft = _z;
                uvTop = _top;
                uvRight = _right;
                uvBottom = _w;
            }
            else if (id == 3)
            {
                uvLeft = _left;
                uvTop = _w;
                uvRight = _x;
                uvBottom = _y;
            }
            else if (id == 4)
            {
                uvLeft = _x;
                uvTop = _w;
                uvRight = _z;
                uvBottom = _y;
            }
            else if (id == 5)
            {
                uvLeft = _z;
                uvTop = _w;
                uvRight = _right;
                uvBottom = _y;
            }
            else if (id == 6)
            {
                uvLeft = _left;
                uvTop = _y;
                uvRight = _x;
                uvBottom = _bottom;
            }
            else if (id == 7)
            {
                uvLeft = _x;
                uvTop = _y;
                uvRight = _z;
                uvBottom = _bottom;
            }
            else if (id == 8)
            {
                uvLeft = _z;
                uvTop = _y;
                uvRight = _right;
                uvBottom = _bottom;
            }
            else
            {
                uvLeft = _left;
                uvTop = _top;
                uvRight = _right;
                uvBottom = _bottom;
            }
        }

        public void GetLeftBottomUVs(Sprite sprite, float subWidth, float subHeight, out float uvLeft, out float uvTop, out float uvRight, out float uvBottom)
        {
            if (sprite != null)
            {
                var texture = sprite.texture;
                uvLeft = _left;
                uvBottom = _bottom;
                uvRight = uvLeft + subWidth / texture.width;
                uvTop = uvBottom + subHeight / texture.height;
            }
            else
            {
                uvLeft = uvTop = uvRight = uvBottom = 0;
            }
        }
    }
}
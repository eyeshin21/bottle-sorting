using UnityEngine;

namespace Anvil.Legacy
{
    public enum Pivot
    {
        Center,
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }

    public static partial class ExtensionMethods
    {
        static readonly float[] AnchorXs =
        {
            0.5f,
            0.0f, 0.5f, 1.0f,
            0.0f, 1.0f,
            0.0f, 0.5f, 1.0f
        };

        static readonly float[] AnchorYs =
        {
            0.5f,
            1.0f, 1.0f, 1.0f,
            0.5f, 0.5f,
            0.0f, 0.0f, 0.0f
        };

        public static float GetAnchorX(this Pivot pivot)
        {
            return AnchorXs[(int)pivot];
        }

        public static float GetAnchorY(this Pivot pivot)
        {
            return AnchorYs[(int)pivot];
        }

        public static void GetAnchors(this Pivot pivot, out float anchorX, out float anchorY)
        {
            int index = (int)pivot;
            anchorX = AnchorXs[index];
            anchorY = AnchorYs[index];
        }

        public static void SetOffset(this Pivot pivot, BoxCollider2D collider)
        {
            Vector2 size = collider.size;
            int index = (int)pivot;
            float x = size.x * (0.5f - AnchorXs[index]);
            float y = size.y * (0.5f - AnchorYs[index]);
            collider.offset = new Vector2(x, y);
        }

        public static void GetVertices(this Pivot pivot, float width, float height, out float left, out float top, out float right, out float bottom)
        {
            int index = (int)pivot;
            left = -width * AnchorXs[index];
            bottom = -height * AnchorYs[index];
            right = left + width;
            top = bottom + height;
        }

        public static void GetVertices(this Pivot pivot, Vector2 size, out float left, out float top, out float right, out float bottom)
        {
            int index = (int)pivot;
            left = -size.x * AnchorXs[index];
            bottom = -size.y * AnchorYs[index];
            right = left + size.x;
            top = bottom + size.y;
        }

        public static float GetLeft(this Pivot pivot, float width)
        {
            return -width * AnchorXs[(int)pivot];
        }

        public static float GetTop(this Pivot pivot, float height)
        {
            return height * (1f - AnchorYs[(int)pivot]);
        }

        public static void GetTopLeft(this Pivot pivot, float width, float height, out float top, out float left)
        {
            int index = (int)pivot;
            left = -width * AnchorXs[index];
            top = height * (1f - AnchorYs[index]);
        }

        public static Pivot ToHorizontal(this Pivot pivot)
        {
            if (pivot == Pivot.Top || pivot == Pivot.Bottom) return Pivot.Center;
            if (pivot == Pivot.TopLeft || pivot == Pivot.BottomLeft) return Pivot.Left;
            if (pivot == Pivot.TopRight || pivot == Pivot.BottomRight) return Pivot.Right;
            return pivot;
        }

        public static Pivot ToVertical(this Pivot pivot)
        {
            if (pivot == Pivot.Left || pivot == Pivot.Right) return Pivot.Center;
            if (pivot == Pivot.TopLeft || pivot == Pivot.TopRight) return Pivot.Top;
            if (pivot == Pivot.BottomLeft || pivot == Pivot.BottomRight) return Pivot.Bottom;
            return pivot;
        }
    }
}
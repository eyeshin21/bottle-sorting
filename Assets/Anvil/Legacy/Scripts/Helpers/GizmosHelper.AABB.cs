using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class GizmosHelper
    {
        static void _DrawAABB(float left, float top, float right, float bottom)
        {
            var pos1 = new Vector3(left, top);
            var pos2 = new Vector3(right, top);
            var pos3 = new Vector3(right, bottom);
            var pos4 = new Vector3(left, bottom);
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawLine(pos2, pos3);
            Gizmos.DrawLine(pos3, pos4);
            Gizmos.DrawLine(pos4, pos1);
        }

        static void _DrawCrossAABB(float left, float top, float right, float bottom)
        {
            var pos1 = new Vector3(left, top);
            var pos2 = new Vector3(right, top);
            var pos3 = new Vector3(right, bottom);
            var pos4 = new Vector3(left, bottom);
            // Rect
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawLine(pos2, pos3);
            Gizmos.DrawLine(pos3, pos4);
            Gizmos.DrawLine(pos4, pos1);
            // Cross
            Gizmos.DrawLine(pos1, pos3);
            Gizmos.DrawLine(pos2, pos4);
        }


        public static void DrawAABB(RectTransform rectTransform, Color? color = null)
        {
            if (rectTransform != null)
            {
                rectTransform.GetAABB(out float left, out float top, out float right, out float bottom);
                DrawAABB(left, top, right, bottom, color);
            }
        }

        public static void DrawAABB(float left, float top, float right, float bottom, Color? color = null)
        {
            Draw(color, () => _DrawAABB(left, top, right, bottom));
        }

        public static void DrawCrossAABB(float left, float top, float right, float bottom, Color? color = null)
        {
            Draw(color, () => _DrawCrossAABB(left, top, right, bottom));
        }

        public static void DrawAABB(float left, float top, float right, float bottom, Padding padding, Color? color = null)
        {
            DrawAABB(left + padding.Left, top - padding.Top, right - padding.Right, bottom + padding.Bottom, color);
        }

        public static void FillAABB(float left, float top, float right, float bottom, Color? color = null)
        {
            Draw(color, () =>
            {
                var pos1 = new Vector3(left, top);
                float width = right - left;
                float height = top - bottom;

                if (width < height)
                {
                    var pos2 = new Vector3(left, bottom);
                    int count = Mathf.RoundToInt(width / LineStep);
                    float lineStep = width / count;
                    for (int i = 0; i < count; i++)
                    {
                        Gizmos.DrawLine(pos1, pos2);
                        pos1.x += lineStep;
                        pos2.x = pos1.x;
                    }
                    pos1.x = pos2.x = right;
                    Gizmos.DrawLine(pos1, pos2);
                }
                else
                {
                    var pos2 = new Vector3(right, top);
                    int count = Mathf.RoundToInt(height / LineStep);
                    float lineStep = height / count;
                    for (int i = 0; i < count; i++)
                    {
                        Gizmos.DrawLine(pos1, pos2);
                        pos1.y -= lineStep;
                        pos2.y = pos1.y;
                    }
                    pos1.y = pos2.y = bottom;
                    Gizmos.DrawLine(pos1, pos2);
                }
            });
        }

        public static void DrawAABB(Collider2D collider, Color? color = null)
        {
            var bounds = collider.bounds;
            var size = bounds.size;
            DrawRect(bounds.center, size.x, size.y, color);
        }

        public static void DrawSize(Vector3 pos, float width, float height, Color? color = null)
        {
            var color2 = color.HasValue ? color.Value : Color.blue;
            float left = pos.x - width * 0.5f;
            float right = left + width;
            float bottom = pos.y - height * 0.5f;
            float top = bottom + height;
            FillAABB(left, top, right, bottom, color2.GetColor(color2.a * 0.1f));
            DrawAABB(left, top, right, bottom, color2);
        }
    }
}
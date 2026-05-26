using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static Vector3 GetCenterAndSize(Transform playTopLeft, Transform playBottomRight, out float width, out float height)
        {
            var topLeft = playTopLeft != null ? playTopLeft.position : Context.MainCamera.GetTopLeft();
            var bottomRight = playBottomRight != null ? playBottomRight.position : Context.MainCamera.GetBottomRight();
            float left = topLeft.x;
            float top = topLeft.y;
            float right = bottomRight.x;
            float bottom = bottomRight.y;
            width = Mathf.Abs(right - left);
            height = Mathf.Abs(top - bottom);
            return new Vector3((left + right) * 0.5f, (bottom + top) * 0.5f);
        }

        public static float GetFitInScale(float width, float height, float targetWidth, float targetHeight)
        {
            float scaleX = targetWidth / width;
            float scaleY = targetHeight / height;
            return Mathf.Min(scaleX, scaleY);
        }

        public static Vector2 GetFitInSize(float width, float height, float targetWidth, float targetHeight)
        {
            float scaleX = targetWidth / width;
            float scaleY = targetHeight / height;
            float scale = Mathf.Min(scaleX, scaleY);
            return new Vector2(width * scale, height * scale);
        }

        /// <summary>
        /// Angle in degrees.
        /// </summary>
        public static void GetAABBSize(float width, float height, float angle, out float outWidth, out float outHeight)
        {
            angle *= Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * width * 0.5f;
            float y = Mathf.Sin(angle) * width * 0.5f;
            angle += Mathf.PI * 0.5f;
            float deltaX = Mathf.Cos(angle) * height * 0.5f;
            float deltaY = Mathf.Sin(angle) * height * 0.5f;
            float x1 = -x + deltaX;
            float y1 = -y + deltaY;
            float x2 = x + deltaX;
            float y2 = y + deltaY;
            float x3 = x - deltaX;
            float y3 = y - deltaY;
            float x4 = -x - deltaX;
            float y4 = -y - deltaY;
            float left = Mathf.Min(x1, Mathf.Min(x2, Mathf.Min(x3, x4)));
            float right = Mathf.Max(x1, Mathf.Max(x2, Mathf.Max(x3, x4)));
            float bottom = Mathf.Min(y1, Mathf.Min(y2, Mathf.Min(y3, y4)));
            float top = Mathf.Max(y1, Mathf.Max(y2, Mathf.Max(y3, y4)));
            outWidth = right - left;
            outHeight = top - bottom;
        }
    }
}
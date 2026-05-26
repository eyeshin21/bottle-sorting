#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class DebugHelper
    {
        public static string ToString(float value)
        {
            if (value > 0)
            {
                int integer = Mathf.FloorToInt(value);
                int @decimal = Mathf.RoundToInt((value - integer) * 100);
                if (@decimal == 0)
                {
                    return integer.ToString();
                }
                if (@decimal < 10)
                {
                    return $"{integer}.0{@decimal}";
                }
                if (@decimal % 10 == 0)
                {
                    return $"{integer}.{@decimal / 10}";
                }
                return $"{integer}.{@decimal}";
            }

            if (value < 0)
            {
                return $"-{ToString(-value)}";
            }

            return "0";
        }

        public static string ToStringPosition(float x, float y)
        {
            return $"({ToString(x)},{ToString(y)})";
        }

        public static string ToStringSize(float width, float height)
        {
            return $"{ToString(width)}x{ToString(height)}";
        }

        public static void LogAABB(float left, float top, float right, float bottom)
        {
            float centerX = (left + right) * 0.5f;
            float centerY = (bottom + top) * 0.5f;
            string aabb = $"({left:0.00}f, {top:0.00}f, {right:0.00}f, {bottom:0.00}f)";
            aabb.CopyToClipboard();
            LegacyLog.Debug($"{aabb} ({centerX:0.00}, {centerY:0.00})");
        }

        public static void LogPlayAABB(Transform playTopLeft, Transform playBottomRight)
        {
            if (playTopLeft != null)
            {
                if (playBottomRight != null)
                {
                    var topLeft = playTopLeft.position;
                    var bottomRight = playBottomRight.position;
                    float left = topLeft.x;
                    float top = topLeft.y;
                    float right = bottomRight.x;
                    float bottom = bottomRight.y;
                    LogAABB(left, top, right, bottom);
                    //float centerX = (left + right) * 0.5f;
                    //float centerY = (bottom + top) * 0.5f;
                    //float width = right - left;
                    //float height = top - bottom;
                    //Log.Debug($"center={ToStringPosition(centerX, centerY)}, size={ToStringSize(width, height)}");
                }
                else
                {
                    LegacyLog.Warning("Play bottom-right required!");
                }
            }
            else
            {
                LegacyLog.Warning("Play top-left required!");
            }
        }
    }
}
#endif
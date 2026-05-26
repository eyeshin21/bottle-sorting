#if DEBUG_MODE
#define LOG_NULL
#endif
using UnityEngine;
using System.Diagnostics;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        static readonly string StringNull = "(null)";
        static readonly string StringArrayEmpty = "[]";
        static readonly string StringListEmpty = "[]";

        public static string GetString(this object obj)
        {
            return obj != null ? obj.ToString() : "";
        }

        [Conditional("LOG_NULL")]
        static void LogNullComponent()
        {
            LegacyLog.Warning($"Component is null!");
        }

        [Conditional("LOG_NULL")]
        static void LogNull<T>() where T : Component
        {
            LegacyLog.Warning($"Component <b>{typeof(T).Name}</b> is null!");
        }

        public static void GetSize(this Rect rect, out int width, out int height)
        {
            var size = rect.size;
            width = Helper.RoundToInt(size.x);
            height = Helper.RoundToInt(size.y);
        }

        public static void Get(this Rect rect, out int x, out int y, out int width, out int height)
        {
            var size = rect.size;
            x = Helper.RoundToInt(rect.x);
            y = Helper.RoundToInt(rect.y);
            width = Helper.RoundToInt(size.x);
            height = Helper.RoundToInt(size.y);
        }

        public static Rect AddMargins(this Rect rect, float margins)
        {
            rect.x -= margins;
            rect.y -= margins;
            rect.width += margins * 2;
            rect.height += margins * 2;
            return rect;
        }
    }
}
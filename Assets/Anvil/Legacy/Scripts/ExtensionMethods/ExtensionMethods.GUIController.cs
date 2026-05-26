using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// (width, height) or (columnCount, rowCount)
        /// </summary>
        public static void SetSize(this IGUIController<SizeInt> controller, int width, int height)
        {
            if (controller != null)
            {
                controller.Value = new SizeInt(width, height);
            }
            else
            {
                LegacyLog.Warning($"Can't set size {width}x{height}!");
            }
        }
    }
}
using UnityEngine;

namespace Anvil.Legacy
{
    public interface IColor
    {
        Color Color { get; set; }
    }

    public static partial class ExtensionMethods
    {
        public static Color GetColor(this IColor iColor)
        {
            if (iColor != null)
            {
                return iColor.Color;
            }
            LegacyLog.Warning("Can't get color: IColor is null!");
            return Defaults.Color;
        }

        public static void SetColor(this IColor iColor, Color color)
        {
            if (iColor != null)
            {
                iColor.Color = color;
            }
            else
            {
                LegacyLog.Warning("Can't set color: IColor is null!");
            }
        }
    }
}
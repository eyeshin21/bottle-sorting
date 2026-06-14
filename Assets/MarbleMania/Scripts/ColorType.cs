using UnityEngine;

public enum ColorType
{
    Red,
    Green,
    Blue,
}

public static class ColorTypeExtension
{
    public static Color ToColor(this ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Red:
                return Color.red;
            case ColorType.Green:
                return Color.green;
            case ColorType.Blue:
                return Color.blue;
            default:
                return Color.white;
        }
    }
}
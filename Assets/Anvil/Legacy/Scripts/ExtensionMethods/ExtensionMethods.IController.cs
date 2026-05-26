using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static void SetAlpha(this IColorController colorController, float a)
        {
            var color = colorController.Color;
            color.a = a;
            colorController.Color = color;
        }

        public static Color GetColor(this IController controller)
        {
            if (controller != null)
            {
                var colorController = controller as IColorController;
                if (colorController != null)
                {
                    return colorController.Color;
                }
                LegacyLog.Warning($"Can't get color from {controller}!");
            }
            return Defaults.Color;
        }

        public static void SetColor(this IController controller, Color color)
        {
            if (controller != null)
            {
                var colorController = controller as IColorController;
                if (colorController != null)
                {
                    colorController.Color = color;
                }
                else
                {
                    LegacyLog.Warning($"Can't set color for {controller}!");
                }
            }
        }

        public static Material GetMaterial(this IController controller)
        {
            if (controller != null)
            {
                var materialController = controller as IMaterialController;
                if (materialController != null)
                {
                    return materialController.Material;
                }
                LegacyLog.Warning($"Can't get material from {controller}!");
            }
            return Defaults.Material;
        }

        public static void SetMaterial(this IController controller, Material material)
        {
            if (controller != null)
            {
                var materialController = controller as IMaterialController;
                if (materialController != null)
                {
                    materialController.Material = material;
                }
                else
                {
                    LegacyLog.Warning($"Can't set material for {controller}!");
                }
            }
        }
    }
}
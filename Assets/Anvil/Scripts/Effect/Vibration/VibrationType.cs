using System.Collections.Generic;
using Anvil.Legacy;

    public enum VibrationType
    {
        // In-game
        Tap,
        
        Minimal,
        VeryLight,
        Light,
        Medium,
        Heavy,
        VeryHeavy,
    }

    public static partial class ExtensionMethods
    {
        static Dictionary<VibrationType, VibrationController> _vibrationControllers = new Dictionary<VibrationType, VibrationController>();
        static VibrationController GetVibrationController(VibrationType vibrationType)
        {
            if (!_vibrationControllers.TryGetValue(vibrationType, out VibrationController controller))
            {
                var data = VibrationConfig.Instance.GetVibrationData(vibrationType);
                controller = VibrationController.Create(data);
                _vibrationControllers.Add(vibrationType, controller);
            }
            return controller;
        }
        public static void Vibrate(this VibrationType vibrationType)
        {
// #if DEBUG_MODE
//                    LegacyLog.Debug(vibrationType);
// #endif
            var controller = GetVibrationController(vibrationType);
            if (controller != null)
            {
                controller.Vibrate();
            }
        }
    }

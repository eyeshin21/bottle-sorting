#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class DebugManager
    {
        static DebugBoolGUIController _showFPS = CreateBoolController("Show FPS", "_showFPS");
        public static bool ShowFPS => _showFPS.Value;

        static DebugBoolGUIController _skipSyncCloud = CreateBoolController("Skip sync cloud", "_skipSyncCloud");
        public static bool SkipSyncCloud => _skipSyncCloud.Value;

        static DebugBoolGUIController _forceTutorial = CreateBoolController("Force tutorial", "_forceTutorial");
        public static bool ForceTutorial => _forceTutorial.Value;

        static DebugBoolGUIController _skipTutorial = CreateBoolController("Skip tutorial", "_skipTutorial");
        public static bool SkipTutorial => _skipTutorial.Value;

        static DebugBoolGUIController _logFirebaseEvents = CreateBoolController("Log Firebase events", "_logFirebaseEvents");
        public static bool LogFirebaseEvents => _logFirebaseEvents.Value;

        static DebugBoolGUIController _logAppsFlyerEvents = CreateBoolController("Log AppsFlyer events", "_logAppsFlyerEvents");
        public static bool LogAppsFlyerEvents => _logAppsFlyerEvents.Value;

        static DebugBoolGUIController _logGametaminEvents = CreateBoolController("Log Gametamin events", "_logGametaminEvents");
        public static bool LogGametaminEvents => _logGametaminEvents.Value;

        static DebugBoolGUIController _useDebugAds = CreateBoolController("Use debug Ads", "_useDebugAds", true);
        public static bool UseDebugAds => _useDebugAds.Value;

        static DebugBoolGUIController _logPlaySound = CreateBoolController("Log play sound", "_logPlaySound");
        public static bool LogPlaySound => _logPlaySound.Value;

        static DebugBoolGUIController _logVibration = CreateBoolController("Log Vibration", "_logVibration");
        public static bool LogVibration => _logVibration.Value;
    }
}
#endif
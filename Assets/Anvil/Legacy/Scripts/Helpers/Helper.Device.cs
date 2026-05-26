using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static string _deviceID;
        public static string DeviceID
        {
            get
            {
                if (string.IsNullOrEmpty(_deviceID))
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
                    AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
                    _deviceID = secure.CallStatic<string>("getString", contentResolver, "android_id");
#else
                    _deviceID = SystemInfo.deviceUniqueIdentifier;
#endif
                }
                return _deviceID;
            }
        }
    }
}
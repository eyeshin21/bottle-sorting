#if UNITY_IOS
#if !USE_MAX_SDK
//#define CUSTOM_ATTRACKING
#endif
#if CUSTOM_ATTRACKING
using UnityEngine;
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;
#endif
#endif

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static void RequestAuthorizationTracking(Callback callback)
        {
#if CUSTOM_ATTRACKING && !UNITY_EDITOR
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            Version currentVersion = new Version(Device.systemVersion); 
            Version ios14 = new Version("14.5"); 
           
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && currentVersion >= ios14)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking(status =>
                {
                    callback?.Invoke();
                });
            }
            else
            {
                callback?.Invoke();
            }
#else
            callback?.Invoke();
#endif
        }
    }
}
#if UNITY_ANDROID
// #define APPSFLYER_ANDROID
#endif
#if UNITY_IOS
#define APPSFLYER_IOS
#endif
using UnityEngine;
#if APPSFLYER_ENABLED
using AppsFlyerSDK;
#else
using _AppsFlyerSDK;
#endif
#if APPSFLYER_IOS && APPSFLYER_ENABLED
using Unity.Notifications.iOS;
#endif
#pragma warning disable 0414
namespace Anvil.Legacy
{
    public static partial class AppsFlyerHelper
    {
        //static bool _FirstOpen
        //{
        //    get => PlayerPrefs.GetInt("_af_first_open_", 0) == 0;
        //    set => PlayerPrefs.SetInt("_af_first_open_", 1);
        //}
        /// <summary>
        /// Called from Manager.
        /// </summary>
        static bool _initialized;
        public static void Init()
        {
            LoadingTracker.StartTrack("AppsFlyerHelper");
            AppsFlyer.initSDK("cfgYFoj97okaEbzqp8q5w5", "");
            AppsFlyer.setCustomerUserId(Helper.DeviceID);
            AppsFlyer.startSDK();
            LoadingTracker.EndTrack("AppsFlyerHelper");
            _initialized = true;
            LogPendingEvents();
            //if (_FirstOpen)
            //{
            //    _FirstOpen = false;
            //    LogEvent("ft_first_open");
            //}
            LogEvent("app_launched", "duration", TimeHelper.SecondsFromStartInt);
#if APPSFLYER_ANDROID
            FirebaseHelper.AddTokenReceivedEvent((token) =>
            {
                AppsFlyer.updateServerUninstallToken(token);
            });
#endif

#if APPSFLYER_IOS && APPSFLYER_ENABLED
            Manager.Instance.StartCoroutine(RequestAuthorization());
#endif
        }

#if APPSFLYER_IOS && APPSFLYER_ENABLED
        static System.Collections.IEnumerator RequestAuthorization()
        {
            using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                }
                if (req.Granted && !string.IsNullOrEmpty(req.DeviceToken))
                {
                    AppsFlyer.registerUninstall(System.Text.Encoding.UTF8.GetBytes(req.DeviceToken));
                }
            }
        }
#endif
    }
}
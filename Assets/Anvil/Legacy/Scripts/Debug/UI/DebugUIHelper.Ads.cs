// #if DEBUG_MODE
// using UnityEngine;
//
// namespace Gametamin
// {
//     public static partial class DebugUIHelper
//     {
//         public static void ShowInterstitial(Callback<InterstitialResult> callback)
//         {
//             ShowMessage("Ads", "Interstitial", () => callback?.Invoke(InterstitialResult.Closed));
//         }
//
//         /// <summary>
//         /// callback(rewarded)
//         /// </summary>
//         public static void ShowRewardedAd(Callback<RewardedAdResult> callback)
//         {
//             ShowOption("Ads", "Rewarded ad", "Reward", "Cancel", (button) =>
//             {
//                 callback?.Invoke(button == 1 ? RewardedAdResult.Rewarded : RewardedAdResult.Closed);
//             });
//         }
//
//         /// <summary>
//         /// callback(rewarded)
//         /// </summary>
//         public static void ShowRewardedAd(Callback<bool> callback)
//         {
//             ShowOption("Ads", "Rewarded ad", "Reward", "Cancel", (button) =>
//             {
//                 callback?.Invoke(button == 1);
//             });
//         }
//
//         static GameObject _banner;
//         public static void ShowBanner()
//         {
//             if (_banner != null)
//             {
//                 _banner.SetActive(true);
//             }
//             else
//             {
//                 //TODO
//                 //_banner = Config.Banner.CreateUI();
//             }
//         }
//
//         public static void HideBanner()
//         {
//             if (_banner != null)
//             {
//                 _banner.SetActive(false);
//             }
//         }
//     }
// }
// #endif
using UnityEngine;
using System.Collections.Generic;
#if APPSFLYER_ENABLED
using AppsFlyerSDK;
#else
using _AppsFlyerSDK;
#endif

namespace Anvil.Legacy
{
    public static partial class AppsFlyerHelper
    {
        static bool _LogEventEnable => true;
        #region Login
        public static void LogLogin(int errorCode)
        {
            if (_LogEventEnable)
                LogEvent(AFInAppEvents.LOGIN, AFInAppEvents.CUSTOMER_USER_ID, Helper.DeviceID);
        }

        public static void LogCompleteRegistration(string method)
        {
            if (_LogEventEnable)
                LogEvent(AFInAppEvents.COMPLETE_REGISTRATION, AFInAppEvents.REGSITRATION_METHOD, method);
        }
        #endregion

        #region Level
        public static void LogPlayLevel(int level)
        {
            if (_LogEventEnable)
                LogEvent(AFInAppEvents.LEVEL, AFInAppEvents.LEVEL, level);
        }

        public static void LogLevelAchieved(int level, int score)
        {
            if (_LogEventEnable)
                LogEvent(AFInAppEvents.LEVEL_ACHIEVED, AFInAppEvents.LEVEL, level, AFInAppEvents.SCORE, score);
        }

        public static void LogCompletedLevel(int level)
        {
            if (_LogEventEnable)
                LogEvent($"completed_level", AFInAppEvents.LEVEL, level);
        }

        public static void LogLevelFail(int level)
        {
            if (_LogEventEnable)
                LogEvent("level_fail", AFInAppEvents.LEVEL, level);
        }

        public static void LogTutorialCompletion(string tutorialId, bool success)
        {
            if (_LogEventEnable)
                LogEvent(AFInAppEvents.TUTORIAL_COMPLETION, dict =>
                {
                    dict.Add(AFInAppEvents.CONTENT_ID, tutorialId);
                    dict.Add("af_content", tutorialId);
                    dict.Add(AFInAppEvents.SUCCESS, success ? "true" : "false");
                });
        }
        #endregion

        #region Ads


        public static void LogAdRevenueIronSource(string adNetwork, double revenue, string country, string instanceId, string adUnit, string placement, string encryptedCPM)
        {
            var additionalParams = new Dictionary<string, string>
            {
                { AdRevenueScheme.COUNTRY, country },
                { AdRevenueScheme.AD_UNIT, adUnit },
                { AdRevenueScheme.AD_TYPE, instanceId },
                { AdRevenueScheme.PLACEMENT, placement },
            };
            var logRevenue = new AFAdRevenueData(adNetwork, MediationNetwork.IronSource, "USD", revenue);
            AppsFlyer.logAdRevenue(logRevenue, additionalParams);
        }
        public static void LogAdRevenueAppLovin(string adNetwork, double revenue, string country, string format, string adUnit, string placement)
        {
            var additionalParams = new Dictionary<string, string>
            {
                { AdRevenueScheme.COUNTRY, country },
                { AdRevenueScheme.AD_UNIT, adUnit },
                { AdRevenueScheme.AD_TYPE, format },
                { AdRevenueScheme.PLACEMENT, placement },
            };
            var logRevenue = new AFAdRevenueData(adNetwork, MediationNetwork.ApplovinMax, "USD", revenue);
            AppsFlyer.logAdRevenue(logRevenue, additionalParams);
        }
        #endregion

        #region IAP
        static string GetAppsFlyerRevenue(decimal localizedPrice)
        {
            var revenue = decimal.Multiply(localizedPrice, 0.63m);
            return revenue.ToString();
        }

        public static void LogPurchase(string contentId, decimal localizedPrice, string currency, string orderID)
        {
            LogEvent(AFInAppEvents.PURCHASE, dict =>
            {
                dict.Add(AFInAppEvents.CONTENT_ID, contentId);
                dict.Add(AFInAppEvents.REVENUE, GetAppsFlyerRevenue(localizedPrice));
                dict.Add(AFInAppEvents.CURRENCY, currency);
                dict.Add(AFInAppEvents.ORDER_ID, orderID);
                dict.Add(AFInAppEvents.RECEIPT_ID, orderID);
            });
        }
        #endregion
    }
}
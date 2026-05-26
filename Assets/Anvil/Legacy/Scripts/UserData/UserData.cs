// using Gametamin.Server;
// using UnityEngine;
//
// namespace Gametamin
// {
//     public static partial class UserData
//     {
//         static bool _isProcessingCloud;
//
//         static string _userID;
//         public static string UserID
//         {
//             get => _userID;
//             private set
//             {
//                 _userID = value;
// #if DEBUG_MODE
//                 _txtUserID = value;
// #endif
//                 LocalPrefs.LastUserID = value;
//                 //SetGroupFolderName(value);//TODO
//             }
//         }
//
//         public static void Init(string cloudUserID, Callback<SyncCloudResult> callback)
//         {
//             //TODO
//             AnalyticsHelper.LogLogin((int)ErrorCode.OK);
//             UserID = cloudUserID;
//             callback?.Invoke(SyncCloudResult.None);
//
// #if DEBUG_MODE
//             InitDebug();
// #endif
//         }
//
//         public static void DeleteAccount(Callback callback)
//         {
//             StartCloud();
//             ServerHelper.RemoveCurrentUserAndAddNewUser(() =>
//             {
//                 EndCloud();
//                 UserPrefs.OnUserDataChanged();
//                 callback?.Invoke();
//             });
//         }
//
//         static void StartCloud()
//         {
//             //ShowWaiting();
//             _isProcessingCloud = true;
//         }
//
//         static void EndCloud()
//         {
//             //HideWaiting();
//             _isProcessingCloud = false;
//         }
//     }
// }
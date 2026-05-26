// #if DEBUG_MODE
// using UnityEngine;
//
// namespace Gametamin
// {
//     public static partial class UserData
//     {
//         static string _txtUserID;
//         //static bool _isWaiting;
//         static bool _isInitedDebug;
//
//         //static void ShowWaiting() => _isWaiting = true;
//         //static void HideWaiting() => _isWaiting = false;
//
//         static void InitDebug()
//         {
//             if (_isInitedDebug) return;
//             _isInitedDebug = true;
//             Manager.AddDebugGUI("UserData", OnGUIDebug);
//         }
//
//         public static void OnGUIDebug()
//         {
//             bool guiEnabled = GUI.enabled;
//             GUI.enabled = guiEnabled && !_isProcessingCloud;
//
//             GUIHelper.LayoutLeft(() =>
//             {
//                 if (Button("Delete Account"))
//                 {
//                     Manager.HideDebug();
//                     DeleteAccount(() => Helper.LoadScene("Loading"));
//                 }
//             });
//
//             GUI.enabled = guiEnabled;
//         }
//
//         static bool Button(string label)
//         {
//             return GUILayout.Button(label);
//         }
//     }
// }
// #endif
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class LocalPrefs
    {
        // static LoginData _lastLoginData;
        //
        // public static LoginData LastLoginData
        // {
        //     get
        //     {
        //         if (_lastLoginData == null)
        //         {
        //             _lastLoginData = LoginData.Deserialize(GetString(LocalKeys.LastLoginData));
        //         }
        //         return _lastLoginData;
        //     }
        //     set
        //     {
        //         _lastLoginData = value;
        //
        //         var json = "";
        //         if (value != null)
        //         {
        //             json = value.Serialize();
        //             LastUserID = value.UserID;
        //         }
        //         SetString(LocalKeys.LastLoginData, json);
        //     }
        // }

        public static string LastUserID
        {
            get => GetString(LocalKeys.LastUserID);
            set
            {
                //Log.Debug($"Last userID: \"{LastUserID}\" => \"{value}\"");
                SetString(LocalKeys.LastUserID, value);
            }
        }
    }
}
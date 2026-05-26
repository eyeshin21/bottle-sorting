using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class LocalKeys
    {
        public static readonly string LastLoginData = AddKey("gb", "LastLoginData");
        public static readonly string SetDeviceInfo = AddKey("gc", "SetDeviceInfo");
        public static readonly string Location = AddKey("gd", "Location");
        public static readonly string LastUserID = AddKey("gf", "LastUserID");
        public static readonly string NewDayFlags = AddKey("ndl", "NewDayFlags");

#if DEBUG_MODE
        public static readonly string _HackSeconds = AddKey("_hackSeconds", "HackSeconds");
#endif
    }
}
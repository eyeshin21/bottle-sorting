namespace Anvil
{
    public static partial class AnimationNames
    {
        public static readonly string Default = "Default";
        public static readonly string Show = "Show";
        public static readonly string Hide = "Hide";
        public static readonly string Active = "Active";
        public static readonly string InActive = "InActive";
        public static readonly string ShowActive = "ShowActive";
        public static readonly string Spawn = "Spawn";
        public static readonly string Spawn2 = "Spawn2";
        public static readonly string EndSpawn = "EndSpawn";
        public static readonly string Collect = "Collect";
        public static readonly string Appear = "Appear";
        public static readonly string Disappear = "Disappear";
        public static readonly string Idle = "Idle";
        public static readonly string Unlock = "Unlock";
        public static readonly string Lock = "Lock";
        public static readonly string On = "On";
        public static readonly string Off = "Off";
        public static readonly string Bounce = "Bounce";
        public static readonly string Press = "Press";
        public static readonly string Drag = "Drag";
        public static readonly string Tap = "Tap";
        public static readonly string Enter = "Enter";
        public static readonly string Entry = "Entry";
        public static readonly string Exit = "Exit";
        public static readonly string Break = "Break";
        public static readonly string Move = "Move";
        public static readonly string Stop = "Stop";

        //Character animation
        public static readonly string Still = "Still";
        public static readonly string JumpLight = "JumpLight";
        public static readonly string Jump = "Jump";
        public static readonly string Shake = "Shake";
        public static readonly string ShakeLight = "ShakeLight";
        public static readonly string RunLeft = "RunLeft";
        public static readonly string RunRight = "RunRight";
    
        public static readonly string End = "End";
        public static readonly string Start = "Start";
        
        public static readonly string Select = "Select";
        public static readonly string Deselect = "Deselect";
        public static readonly string Hidden = "Hidden";
        public static readonly string Complete = "Complete";
        public static readonly string ButtonPress = "Press";

    }
    public static partial class RectTransformExtension
    {
        public static string AddPrefix(this string name, string prefix)
        {
            return $"{prefix}_{name}";
        }
    }
}


namespace Anvil
{
    public class GameObjectReferenceIDAdapter
    {
#if !GAMETAMIN_CORE
        
        public static string Root = "Root";
        public static string Bottom = "Bottom";
        public static string Top = "Top";
        public static string Text = "Text";
#else
        public static string Root = GameObjectReferenceID.Root;
        public static string Bottom = GameObjectReferenceID.Bottom;
        public static string Top = GameObjectReferenceID.Top;
        public static string Text = GameObjectReferenceID.Text;
#endif
    }
}
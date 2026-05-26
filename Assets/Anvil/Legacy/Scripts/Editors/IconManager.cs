#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Anvil.Legacy
{
    public enum LabelIcon
    {
        Gray,
        Blue,
        Teal,
        Green,
        Yellow,
        Orange,
        Red,
        Purple
    }

    public enum ShapeIcon
    {
        CircleGray,
        CircleBlue,
        CircleTeal,
        CircleGreen,
        CircleYellow,
        CircleOrange,
        CircleRed,
        CirclePurple,
        DiamondGray,
        DiamondBlue,
        DiamondTeal,
        DiamondGreen,
        DiamondYellow,
        DiamondOrange,
        DiamondRed,
        DiamondPurple
    }

    public static class IconManager
    {
#if !UNITY_2021_2_OR_NEWER
        private static MethodInfo setIconForObjectMethodInfo;
#endif

        public static void SetIcon(Component component, LabelIcon labelIcon)
        {
            SetIcon(component.gameObject, labelIcon);
        }

        public static void SetIcon(GameObject gameObject, LabelIcon labelIcon)
        {
            SetIcon(gameObject, $"sv_label_{(int)labelIcon}");
        }

        public static void SetIcon(Component component, ShapeIcon shapeIcon)
        {
            SetIcon(component.gameObject, shapeIcon);
        }

        public static void SetIcon(GameObject gameObject, ShapeIcon shapeIcon)
        {
            SetIcon(gameObject, $"sv_icon_dot{(int)shapeIcon}_pix16_gizmo");
        }

        private static void SetIcon(GameObject gameObject, string contentName)
        {
            GUIContent iconContent = EditorGUIUtility.IconContent(contentName);
            SetIconForObject(gameObject, (Texture2D)iconContent.image);
        }

        public static void RemoveIcon(GameObject gameObject)
        {
            SetIconForObject(gameObject, null);
        }

        public static void SetIconForObject(GameObject obj, Texture2D icon)
        {
#if UNITY_2021_2_OR_NEWER
            EditorGUIUtility.SetIconForObject(obj, icon);
#else
            if (setIconForObjectMethodInfo == null)
            {
                var type = typeof(EditorGUIUtility);
                setIconForObjectMethodInfo = type.GetMethod("SetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);
            }
            setIconForObjectMethodInfo.Invoke(null, new object[] {obj, icon});
#endif
        }
    }
}
#endif
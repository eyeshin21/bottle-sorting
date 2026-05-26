using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        public static GUIStyle CreateLabelStyle() => new GUIStyle(GUI.skin.label);
        public static GUIStyle CreateLabelStyle(Color color) => CreateLabelStyle().SetTextColor(color);
        public static GUIStyle CreateLabelStyle(GUIStyle other) => new GUIStyle(other);
        public static GUIStyle CreateTextFieldStyle() => new GUIStyle(GUI.skin.textField);
        public static GUIStyle CreateButtonStyle() => new GUIStyle(GUI.skin.button);
        public static GUIStyle CreateToggleStyle() => new GUIStyle(GUI.skin.toggle);
        public static GUIStyle CreateToggleStyle(Color color) => CreateToggleStyle().SetTextColor(color);
        public static GUIStyle CreateFoldoutStyle()
        {
#if UNITY_EDITOR
            return new GUIStyle(EditorStyles.foldout);
#else
            return null;
#endif
        }
        public static GUIStyle CreateBoxStyle() => new GUIStyle(GUI.skin.box);
        public static GUIStyle CreateGroupStyle() => new GUIStyle();
        public static GUIStyle CreateGroupStyle(GUIStyle other) => new GUIStyle(other);
        public static GUIStyle CreateGroupStyle(Color backgroundColor) => new GUIStyle().SetBackgroundColor(backgroundColor);

        static GUIStyle _labelMiddleLeftStyle;
        static GUIStyle LabelMiddleLeftStyle => _labelMiddleLeftStyle ??= CreateLabelStyle().SetAlignment(TextAnchor.MiddleLeft);

        static GUIStyle _defaultGroupStyle;
        static GUIStyle DefaultGroupStyle
        {
            get
            {
                if (_defaultGroupStyle == null)
                {
                    var style = new GUIStyle();
                    //var rectOffset = new RectOffset();
                    //style.border = rectOffset;
                    //style.margin = rectOffset;
                    //style.padding = rectOffset;
                    _defaultGroupStyle = style;
                }
                return _defaultGroupStyle;
            }
        }

        static GUIStyle _iconStyle;
        public static GUIStyle IconStyle => _iconStyle ??= CreateLabelStyle().SetPaddingHorizontal(0).SetMarginRight(0);

        static GUIStyle _selectedFlagButtonStyle;
        public static GUIStyle SelectedFlagButtonStyle => _selectedFlagButtonStyle ??= CreateButtonStyle().SetTextColor(Color.yellow).SetBold();

        #region Scene
        static GUIStyle _sceneLabelStyle;
        public static GUIStyle SceneLabelStyle => _sceneLabelStyle ??= CreateLabelStyle().SetTextColor(Color.white);

        static GUIStyle _sceneToggleStyle;
        public static GUIStyle SceneToggleStyle => _sceneToggleStyle ??= CreateToggleStyle().SetTextColor(Color.white);

        static GUIStyle _sceneFoldoutStyle;
        public static GUIStyle SceneFoldoutStyle => _sceneFoldoutStyle ??= CreateFoldoutStyle().SetTextColor(Color.white);
        #endregion
    }
}
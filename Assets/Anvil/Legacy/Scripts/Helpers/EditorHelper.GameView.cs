#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class EditorHelper
    {
        static class GameViewUtils
        {
            static object _instance;

            static Type _gameViewType;
            static MethodInfo _sizeSelectionCallback;

            static Type _gameViewSizesType;
            static MethodInfo _getGroup;

            static GameViewUtils()
            {
                _gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
                _sizeSelectionCallback = _gameViewType.GetMethod("SizeSelectionCallback", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                _gameViewSizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
                _getGroup = _gameViewSizesType.GetMethod("GetGroup");

                var singleType = typeof(ScriptableSingleton<>).MakeGenericType(_gameViewSizesType);
                var instance = singleType.GetProperty("instance");
                _instance = instance.GetValue(null, null);
            }

            static GameViewSizeGroupType GetCurrentGroupType()
            {
#if UNITY_STANDALONE
                return GameViewSizeGroupType.Standalone;
#elif UNITY_IOS
                return GameViewSizeGroupType.iOS;
#elif UNITY_ANDROID
                return GameViewSizeGroupType.Android;
#endif
                // Add your own
            }

            public static bool TrySet1080x1920()
            {
                return TrySetSize("1920x1080 Portrait");
            }

            public static bool TrySet1920x1080()
            {
                return TrySetSize("1920x1080 Landscape");
            }

            public static bool TrySetSize(string size)
            {
                return TrySetSize(size, GetCurrentGroupType());
            }

            public static bool TrySetSize(string size, GameViewSizeGroupType groupType = GameViewSizeGroupType.Standalone)
            {
                int index = FindSize(groupType, size);
                if (index < 0)
                {
                    LegacyLog.Error($"Size {size} was not found in game view settings");
                    return false;
                }
                SetSizeIndex(index);
                return true;
            }

            public static void SetSizeIndex(int index)
            {
                // Calling GameView.SizeSelectionCallback will also auto focus game view,
                // We will restore focus if it is something else
                EditorWindow currentWindow = EditorWindow.focusedWindow;
                SceneView lastSceneView = SceneView.lastActiveSceneView;

                EditorWindow gv = EditorWindow.GetWindow(_gameViewType);
                _sizeSelectionCallback.Invoke(gv, new object[] { index, null });

                // Hack, will mock re-active scene view, in case it was active,
                // Because EditorWindow.focusedWindow could now be inspector
                // If scene view and game view were in same docking group,
                // SizeSelectionCallback will switch to game view without knowing if user left scene view visible or not.
                // - If last active was actually game view, it should be corrected by currentWindow.Focus, no problem
                // - If last active is something else, like console for inspector, this will bring up scene view, should be no harm.
                // Remove this out if you do not want this behavior
                if (lastSceneView != null)
                    lastSceneView.Focus();

                if (currentWindow != null)
                    currentWindow.Focus();
            }

            /// <summary>
            /// Finding text could be fixed resoluation as WxH "1280x720"
            /// or ratio like W:H "16:9"
            /// </summary>
            public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
            {
                var group = GetGroup(sizeGroupType); // class GameViewSizeGroup
                var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
                var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
                for (int i = 0; i < displayTexts.Length; i++)
                {
                    if (displayTexts[i].Contains(text))
                    {
                        return i;
                    }
                }
                return -1;
            }

            static object GetGroup(GameViewSizeGroupType type)
            {
                return _getGroup.Invoke(_instance, new object[] { (int)type });
            }
        }

        public static void SetGameView1080x1920()
        {
            GameViewUtils.TrySet1080x1920();
        }

        public static void SetGameView1920x1080()
        {
            GameViewUtils.TrySet1920x1080();
        }
    }
}
#endif
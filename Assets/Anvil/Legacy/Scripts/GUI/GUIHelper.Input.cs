#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEditor;

namespace Anvil.Legacy
{
	public static partial class GUIHelper
	{
        public static void ShowSelectLanguage(Action<SystemLanguage> continueCallback, Action cancelCallback = null)
        {
            var language = SystemLanguage.English;

            void OnGUI()
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Language");
                    language = (SystemLanguage)EditorGUILayout.EnumPopup(language, GUILayout.Width(150));
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }

            CustomDialog.Show("Select language", OnGUI, 1, "Continue", "Cancel", button =>
            {
                if (button == 1)
                {
                    continueCallback?.Invoke(language);
                }
                else
                {
                    cancelCallback?.Invoke();
                }
            }, true);
        }
    }
}
#endif
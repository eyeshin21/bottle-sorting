#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("CONTEXT/TextMeshProUGUI/Log Line Count")]
        static void TextMeshProUGUILogLineCount(MenuCommand menuCommand)
        {
            var tmpUGUI = menuCommand.To<TextMeshProUGUI>();
            LegacyLog.Debug($"lineCount={tmpUGUI.textInfo.lineCount}");
        }
    }
}
#endif
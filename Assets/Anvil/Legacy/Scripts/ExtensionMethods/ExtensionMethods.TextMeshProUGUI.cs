using UnityEngine;
using TMPro;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static float GetCharacterLeft(this TextMeshProUGUI textUGUI, int characterIndex)
        {
            var characterInfos = textUGUI.textInfo.characterInfo;
            int length = characterInfos.Length;
            if (characterIndex >= length)
            {
                LegacyLog.Warning($"Out of range: length={length}, characterIndex={characterIndex}");
                return 0;
            }
            return characterInfos[Mathf.Max(characterIndex, 0)].topLeft.x;
        }

        public static bool GetAABB(this TextMeshProUGUI textUGUI, string text, out float left, out float top, out float right, out float bottom)
        {
            if (!string.IsNullOrEmpty(text))
            {
                int startIndex = textUGUI.text.IndexOf(text);
                if (startIndex >= 0)
                {
                    GetAABB(textUGUI, startIndex, startIndex + text.Length - 1, out left, out top, out right, out bottom);
                    return true;
                }
            }

            left = top = right = bottom = 0;
            return false;
        }

        public static void GetAABB(this TextMeshProUGUI textUGUI, int characterStartIndex, int characterEndIndex, out float left, out float top, out float right, out float bottom)
        {
            var characterInfos = textUGUI.textInfo.characterInfo;
            int length = characterInfos.Length;
            if (characterStartIndex >= length)
            {
                LegacyLog.Warning($"Out of range: length={length}, startIndex={characterStartIndex}");
                left = top = right = bottom = 0;
                return;
            }
            if (characterEndIndex >= length)
            {
                LegacyLog.Warning($"Out of range: length={length}, endIndex={characterEndIndex}");
                left = top = right = bottom = 0;
                return;
            }
            //for (int i = 0; i < length; i++)
            //         {
            //	Log.Debug($"[{i}]: {characterInfos[i].character}");
            //         }
            //Log.Debug($"[{characterStartIndex}]: {characterInfos[characterStartIndex].character} => [{characterEndIndex}]: {characterInfos[characterEndIndex].character}");

            var topLeft = characterInfos[characterStartIndex].topLeft;
            var bottomRight = characterInfos[characterEndIndex].bottomRight;
            for (int i = characterStartIndex + 1; i <= characterEndIndex; i++)
            {
                var charTopLeft = characterInfos[i].topLeft;
                topLeft.x = Mathf.Min(topLeft.x, charTopLeft.x);
                topLeft.y = Mathf.Max(topLeft.y, charTopLeft.y);
            }
            for (int i = characterStartIndex; i < characterEndIndex; i++)
            {
                var charBottomRight = characterInfos[i].bottomRight;
                bottomRight.x = Mathf.Max(bottomRight.x, charBottomRight.x);
                bottomRight.y = Mathf.Min(bottomRight.y, charBottomRight.y);
            }
            var textTransform = textUGUI.transform;
            topLeft = textTransform.TransformPoint(topLeft);
            bottomRight = textTransform.TransformPoint(bottomRight);
            left = topLeft.x;
            top = topLeft.y;
            right = bottomRight.x;
            bottom = bottomRight.y;
        }
    }
}
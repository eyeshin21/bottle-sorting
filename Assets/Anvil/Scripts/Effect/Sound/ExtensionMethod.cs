using System;
using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethod
    {
        public static string GetAudioPath(this AudioClipName clipName2)
        {
            return $"{AudioManager.AudioPath}{clipName2.ToString()}";
        }

        public static string GetAudioPath(this string clipName)
        {
            return $"{AudioManager.AudioPath}{clipName}";
        }

        public static void PlaySound(this AudioClipName clipName, float volume = 100)
        {
// #if DEBUG_MODE
//                    LegacyLog.Debug($"Play sound {clipName}");
// #endif
            AudioManager.Instance.PlayEffect(clipName.ToString(), null, volume / 100);
        }

        public static void PlaySound(this AudioClipName clipName,Action onPlayDone)
        {
// #if DEBUG_MODE
//                    LegacyLog.Debug($"Play sound {clipName}");
// #endif
            AudioManager.Instance.PlayEffect(clipName,onPlayDone);
            return;
        }

        public static AudioSource PlaySoundEx(this AudioClipName clipName)
        {
// #if DEBUG_MODE
//                    LegacyLog.Debug($"Play sound {clipName}");
// #endif
            return AudioManager.Instance.PlayEffect(clipName);
        }
    }
}
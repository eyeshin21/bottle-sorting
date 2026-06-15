using System;
using UnityEngine;

namespace Anvil
{
    public static partial class AudioExtensionMethod
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
//                     Log.Debug($"Play sound {clipName}");
// #endif
            if (clipName == AudioClipName.None)
            {
                return;
            }
            AudioManager.Instance.PlayEffect(clipName.ToString(), null, volume / 100);
        }
        public static void PlaySound30(this AudioClipName clipName, float volume = 50)
        {
            clipName.PlaySound(volume: 30);
        }
        public static void PlaySound50(this AudioClipName clipName, float volume = 50)
        {
            clipName.PlaySound(volume: 50);
        }

        public static void PlaySound(this AudioClipName clipName,Action onPlayDone)
        {
// #if DEBUG_MODE
//                     Log.Debug($"Play sound {clipName}");
// #endif
            AudioManager.Instance.PlayEffect(clipName,onPlayDone);
            return;
        }

        public static AudioPlayer PlaySoundEx(this AudioClipName clipName)
        {
// #if DEBUG_MODE
//                     Log.Debug($"Play sound {clipName}");
// #endif
            return AudioManager.Instance.PlayEffect(clipName);
        }
        public static AudioSource PlaySoundEx2(this AudioClipName clipName)
        {
// #if DEBUG_MODE
//                     Log.Debug($"Play sound {clipName}");
// #endif
            var audioPlayer = AudioManager.Instance.PlayEffect(clipName);
            if (audioPlayer == null)
            {
                return null;
            }
            return audioPlayer.audioSource;
        }
    }
}